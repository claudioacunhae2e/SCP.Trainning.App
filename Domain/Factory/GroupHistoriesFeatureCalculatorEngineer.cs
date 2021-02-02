using Domain.Abstraction.Factory;
using Domain.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using Old._42.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Factory
{
    public class GroupHistoriesFeatureCalculatorEngineer : IInputFeatureEngineer
	{
		public GroupHistoriesFeatureCalculatorEngineer(ILocationHistoryOpen pastLocationSchedule)
		{
			PastLocationSchedule = pastLocationSchedule;
			Dates = new Dictionary<DateTime, IDictionary<long, double>>();
		}

		private const double _AjustToAvoidDistortions = 1d;

		private readonly ILocationHistoryOpen PastLocationSchedule;
		private readonly IDictionary<DateTime, IDictionary<long, double>> Dates;

		private IDictionary<string, Func<DateTime, double>> BuildFunctions()
		{
			return new Dictionary<string, Func<DateTime, double>>
			{
				{ "LogOfOpenStores", (DateTime date) => Math.Log(PastLocationSchedule.OpenLocationAmount(date) + _AjustToAvoidDistortions) },
				{ "OpenStores", (DateTime date) => PastLocationSchedule.OpenLocationAmount(date) },
			};
		}

		public IInputFeatureEngineer Load(IEnumerable<IFeature> features, ITimeScope scope)
		{
			var functions = BuildFunctions();
			///////////////////////////////////////////////////////////////////////////////////////////////////
			//                                                                                               //
			//   O Tolist está servindo para transforma a lista e não dar erro de thread, favor não apagar   //
			//                                                                                               //
			///////////////////////////////////////////////////////////////////////////////////////////////////
			var daysForward = scope.DaysForward().ToList();

			foreach (var date in daysForward)
			{
				var featuresFiltered = features.Where(f => f.Rule.IsNotNull() && functions.ContainsKey(f.Rule));

				foreach (var feature in featuresFiltered)
				{
					Dates.AddIfNotExistsAndGet(date, new Dictionary<long, double>())
						 .Add(feature.ID, functions[feature.Rule](date));
				}
			}


			return this;
		}

		public void Transform(IEnumerable<IInput> data)
		{
			if (Dates.Any())
			{
				foreach (var input in data)
				{
					var values = Dates[input.Date];

					foreach (var feature in values)
					{
						if (input.Observation.Xs.ContainsKey(feature.Key))
						{
							input.Observation.Xs[feature.Key] = values[feature.Key];
						}
					}
				}
			}
		}

		#region Not implemented
		public IDictionary<long, double> GetFeatures(DateTime date) => throw new NotImplementedException();

		public IDictionary<DateTime, IDictionary<long, double>> GetFeatures(DateTime start, DateTime end) => throw new NotImplementedException();
		#endregion
	}
}
