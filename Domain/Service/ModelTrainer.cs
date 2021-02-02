using Domain.Abstraction.Factory;
using Domain.Abstraction.Features;
using Domain.Abstraction.Model;
using Domain.Abstraction.Model.Entitys;
using Domain.Abstraction.Repository;
using Domain.Abstraction.Service;
using Domain.Factory;
using Domain.Model;
using E2E.SCP.RegressionModel.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using E2E.SCP.RegressionModel.Model.Entitys;
using E2E.SCP.RegressionModel.Standard.Enum;
using Microsoft.Extensions.Logging;
using Old._42.Util.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Service
{
    public class ModelTrainer : IModelTrainer
	{
		public ModelTrainer(
			ILogger<ModelTrainer> logger,
			IGroupHistoryRepository histories,
			IModelTrainerNormalizer normalizer,
			IRegressionModelRepository regressionModels,
			IRegressionLevelRepository regressionLevels,
			IPastLocationScheduleBuilder pastLocationScheduleBuilder,
			IStockToRegressionModelInputTransformer transformer,
			IExperimentFactory experimentFactory,
			IFeatureEngineerBuilder dateFeatureEngineerBuilder
			)
		{
			_Logger = logger;
			_Histories = histories;
			_Normalizer = normalizer;
			_RegressionModels = regressionModels;
			_RegressionLevels = regressionLevels;
			_PastLocationScheduleBuilder = pastLocationScheduleBuilder;
			_Transformer = transformer;
			_ExperimentFactory = experimentFactory;
			_DateFeatureEngineerBuilder = dateFeatureEngineerBuilder;
		}

		private readonly ILogger<ModelTrainer> _Logger;
		private readonly IGroupHistoryRepository _Histories;
		private readonly IModelTrainerNormalizer _Normalizer;
		private readonly IRegressionModelRepository _RegressionModels;
		private readonly IRegressionLevelRepository _RegressionLevels;
		private readonly IPastLocationScheduleBuilder _PastLocationScheduleBuilder;
		private readonly IStockToRegressionModelInputTransformer _Transformer;
		private readonly IExperimentFactory _ExperimentFactory;
		private readonly IFeatureEngineerBuilder _DateFeatureEngineerBuilder;

		public ModelTrainerAux Start(ISystemInfo SystemInfo)
		{
			var result = new ModelTrainerAux();

			_Logger.LogInformation("Loading configurations");

			result.SystemInfo = SystemInfo;
			result.Level = _RegressionLevels.ByID(SystemInfo.NormalizationRegressionLevelID);
			result.DistributionLevel = _RegressionLevels.ByID(SystemInfo.DistributionRegressionLevelID);

			var features = (SystemInfo.NormalizationRegressionLevelID == result.DistributionLevel.ID)
								? result.Level.Features
								: result.DistributionLevel.Features;

			SetSchedule(result);

			result.LastSessionModels = GetLastSessionModels();
			result.DateFeatureEngineer = _DateFeatureEngineerBuilder.Default(features, SystemInfo.DataTimeScope);

			SetIsNormalizeInfos(SystemInfo, result, features);

			_Logger.LogInformation("Configurations loaded");

			return result;
		}

		public TrainedInfosModel Train(IParentName name, ModelTrainerAux aux, bool normalizeProcessCheck)
		{
			var group = _Histories.By(aux.Level.ID, name.Name);

			if (group == null)
			{
				group = new Group(aux.Level.ID, new GroupNameDTO(name));
			}

			var schedule = aux.GetSchedule(name.Name);
			var model = Train(group, schedule, aux);

			IEnumerable<IProductLocation> productLocations = null;

			if (normalizeProcessCheck)
			{
				productLocations = aux.Normalize
									? _Normalizer.Calculate(aux.Level, group.Name, model.Model, aux.Factory, aux.DistributionLevel.Features, aux.SystemInfo)
									: null;
			}

			return new TrainedInfosModel(model, productLocations);
		}

		private void SetSchedule(ModelTrainerAux aux)
		{
			if (aux.Level.LocationGroupers.NotEmpty())
			{
				aux.ScopedSchedules = _PastLocationScheduleBuilder.BuildLevel(aux.Level);
				aux.GetSchedule = (string name) =>
				{
					aux.ScopedSchedules.TryGetValue(name, out var schedule);
					return schedule;
				};
			}
			else
			{
				aux.Schedule = _PastLocationScheduleBuilder.Build();
				aux.GetSchedule = (string name) => aux.Schedule;
			}
		}

		private void SetIsNormalizeInfos(ISystemInfo SystemInfo, ModelTrainerAux result, IList<IFeature> features)
		{
			if (result.Level.ID == SystemInfo.NormalizationRegressionLevelID)
			{
				var featureEngineer = _DateFeatureEngineerBuilder.Default(features, SystemInfo.DataTimeScope);

				result.Factory = new DateFeatureEngineerFactory(featureEngineer);
				result.Normalize = true;
			}
			else
			{
				result.Factory = null;
				result.Normalize = false;
			}
		}

		private IDictionary<long, IDictionary<string, IRegressionModel>> GetLastSessionModels()
		{
			var result = new Dictionary<long, IDictionary<string, IRegressionModel>>();
			var models = _RegressionModels.AllIgnored();

			if (models.NotEmpty())
			{
				var modelGroupBy = models.GroupBy(r => r.RegressionLevel);

				foreach (var model in modelGroupBy)
				{
					var dict = model.ToDictionary(l => l.Name, l => l);
					result.Add(model.Key, dict);
				}
			}

			return result;
		}

		private IRegressionModel Train(IGroup group, ILocationHistoryOpen schedule, ModelTrainerAux aux)
		{
			IRegressionModel model;
			var prior = GetPrior(group, aux);

			if (group.IsMature(aux.SystemInfo.MinDaysWithSalesToGroupBeTrained, aux.SystemInfo.MinDaysWithSalesOverSalesAgeToGroupBeTrained))
			{
				var locationEngineer = schedule.IsNotNull()
														? new GroupHistoriesFeatureCalculatorEngineer(schedule).Load(aux.Level.Features, aux.SystemInfo.DataTimeScope)
														: null;

				var input = _Transformer.Transform(aux.Level.Features, group, group.Name, group.ParentName, null);
				var productLocationGroupEngineer = new ProductLocationGroupFeatureEngineer(group.Histories, aux.Level.Features);

				model = GetTrainFactory(input, locationEngineer, productLocationGroupEngineer, aux, prior);
			}
			else
			{
				model = new RegressionModel(group.Name, prior, aux.Level.ID, false);
			}

			return model;
		}

		private IRegressionModel GetTrainFactory(
			IRegressionModelInput input,
			IInputFeatureEngineer locationFeatureEngineer,
			IInputFeatureEngineer productLocationFeatureEngineer,
			ModelTrainerAux aux,
			IModel prior)
		{
			var factory = _ExperimentFactory.Bayesian(aux.Level, input, prior, aux.DateFeatureEngineer, locationFeatureEngineer, productLocationFeatureEngineer);
			var factoryTrain = factory.Train();

			return factoryTrain.FirstOrDefault();
		}

		private IModel GetPrior(IGroup group, ModelTrainerAux aux)
		{
			var prior = GetPriorMain(group, aux);

			return prior.IsNull()
					? GetPriorIfIsNull(aux)
					: prior;
		}

		private IModel GetPriorIfIsNull(ModelTrainerAux aux)
		{
			var coefficients = aux.Level.Features.ToDictionary(f => f.ID, f => f.DeafultCoefficient());
			var constant = aux.Level.Features.FirstOrDefault(f => f.Type.Equals(FeatureType.Constant));

			return new Models(coefficients, constant.ID);
		}

		private IModel GetPriorMain(IGroup group, ModelTrainerAux aux)
		{
			var prior = FromPriorsSameRegressionLevel(group, aux);

			if (aux.Level.AscendantID.IsNotNull() && prior.IsNull())
			{
				prior = FromPriorsAscendantRegressionLevel(group, aux);

				if (prior.IsNotNull())
				{
					prior = aux.Level.FromAnotherLevelModel(prior);
				}
			}

			return prior;
		}

		private IModel FromPriorsSameRegressionLevel(IGroup group, ModelTrainerAux aux) =>
			aux.LastSessionModels.GetIfExists(aux.Level.ID)?.GetIfExists(group.Name)?.Model;

		private IModel FromPriorsAscendantRegressionLevel(IGroup group, ModelTrainerAux aux) =>
			aux.LastSessionModels.GetIfExists((long)aux.Level.AscendantID)?.GetIfExists(group.ParentName)?.Model;
	}
}
