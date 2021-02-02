using Domain.Abstraction.Model;
using Domain.Abstraction.Repository;
using Domain.Abstraction.Service;
using Domain.Model;
using Domain.Model.QualityComparer;
using E2E.Generic.Helpful;
using E2E.Generic.Helpful.ThreadSafe;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using E2E.SCP.RegressionModel.Model.Entitys;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Service
{
    public class TrainingAppService : IProcessService
    {
        private const string _MsgDefaultProcess = " - {0} item {1} to processed {2}/{3}. Total time elapsed: {4}.";
        private readonly ISystemInfoRepository _SystemInfoRepository;
        private readonly IProductLocationStatsRepository _ProductLocationNormalizerRepository;
        private readonly ILogger<TrainingAppService> _Logger;
        private readonly IModelTrainer _ModelTrainer;
        private readonly IGroupHistoryRepository _GroupHistories;
        private readonly IRegressionModelRepository _RegresssionModels;
        private readonly IRegressionLevelRepository _RegressionLevelRepository;
        private readonly IRegressionModelItens _RegressionModelItensRepository;
        private readonly ParallelOptions _ParalelOption;

        public TrainingAppService(
            IModelTrainer modelTrainer,
            ISystemInfoRepository systemInfoRepository,
            IGroupHistoryRepository groupHistories,
            IRegressionModelRepository regresssionModels,
            IProductLocationStatsRepository ProductLocationNormalizerRepository,
            ILogger<TrainingAppService> logger,
            IRegressionLevelRepository regressionLevelRepository,
            IRegressionModelItens regressionItensRepository)
        {
            _ModelTrainer = modelTrainer;
            _SystemInfoRepository = systemInfoRepository;
            _GroupHistories = groupHistories;
            _RegresssionModels = regresssionModels;
            _ProductLocationNormalizerRepository = ProductLocationNormalizerRepository;
            _Logger = logger;
            _ParalelOption = new ParallelOptions
            {
                MaxDegreeOfParallelism = 20
            };
            _RegressionLevelRepository = regressionLevelRepository;
            _RegressionModelItensRepository = regressionItensRepository;
        }

        public async Task Init()
        {
            await Init(new ProcessServiceDecisions());
        }

        public async Task Init(IProcessServiceDecisions decisions)
        {
            _ParalelOption.MaxDegreeOfParallelism = decisions.MaxDegreesOfParallelism;

            try
            {
                var startModel = await StartTraining(decisions);

                Training(startModel, decisions);

                if (decisions.MergeDatabaseAndUpdateSystemInfo)
                {
                    await EndTraining();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<TrainingAppServiceStartModel> StartTraining(IProcessServiceDecisions decisions)
        {
            if (decisions.MergeDatabaseAndUpdateSystemInfo)
            {
                var cleanTempTableTask = _ProductLocationNormalizerRepository.ClearTempTable();
                await cleanTempTableTask;
            }

            var systemInfo = await _SystemInfoRepository.Get();
            var modelTrainerTask = Task.Run(() => _ModelTrainer.Start(systemInfo));
            var itens = (await GetRegressionModelsForTrainning(systemInfo.NormalizationRegressionLevelID));

            var modelTrainer = await modelTrainerTask;

            return new TrainingAppServiceStartModel(itens, modelTrainer);
        }

        private async Task<IEnumerable<IParentName>> GetRegressionModelsForTrainning(long normalizationRegressionLevelID)
        {
            _Logger.LogInformation("Seaching itens to train");

            var itensFromGroupHistory = await _GroupHistories.GetGroupNamesToTrain(normalizationRegressionLevelID);

            var regressionLevel = _RegressionLevelRepository.ByID(normalizationRegressionLevelID);

            var itensFromProductLocation = await _RegressionModelItensRepository.GetRegressionModelsToTrain(regressionLevel);

            var concat = itensFromGroupHistory.Concat(itensFromProductLocation);
            var result = concat.Distinct(new ParentNameQualityComparer());

            _Logger.LogInformation(string.Concat(result.Count(), " found"));

            return result;
        }

        private void Training(TrainingAppServiceStartModel modelStart, IProcessServiceDecisions decisions)
        {
            var watch = new StopwatchThreadSafe();
            watch.Start();

            var counter = new IncrementThreadSafe();
            var itensCounter = modelStart.Itens.Count();

            Parallel.ForEach(
                modelStart.Itens,
                _ParalelOption,
                item => TrainingProcess(new TrainingAppServiceModel(itensCounter, item, modelStart.ModelTrainer, watch), counter, decisions).GetAwaiter().GetResult());

            watch.Stop();
        }

        private async Task TrainingProcess(TrainingAppServiceModel model, IncrementThreadSafe counter, IProcessServiceDecisions decisions)
        {
            var count = counter.IncrementAndGetCount();

            string GetMsgDeafultProcess(string startText)
            {
                return string.Format(_MsgDefaultProcess, startText, model.Item.Name, count, model.ItensCounter, model.Watch.Elapsed);
            }

            _Logger.LogInformation(GetMsgDeafultProcess("START"));

            try
            {
                await TrainingProcessTry(model, decisions);
                _Logger.LogInformation(GetMsgDeafultProcess("FINISH"));
            }
            catch (Exception ex)
            {
                _Logger.LogError(string.Concat(GetMsgDeafultProcess("ERROR"), " Error: ", ex));
                throw ex;
            }
        }

        private async Task TrainingProcessTry(TrainingAppServiceModel model, IProcessServiceDecisions decisions)
        {
            var trained = _ModelTrainer.Train(model.Item, model.ModelTrainer, decisions.Normalization);

            _Logger.LogInformation(string.Concat("item ", model.Item.Name, " processed. Time elapsed ", model.Watch.Elapsed));

            if (decisions.MergeDatabaseAndUpdateSystemInfo)
            {
                await UpdateRegressionModel(trained.RegressionModel, model);
            }

            if (decisions.Normalization)
            {
                await InsertProductLocations(trained);
            }
        }

        private async Task UpdateRegressionModel(IRegressionModel regressionModel, TrainingAppServiceModel model)
        {
            if (regressionModel != null)
            {
                var concrete = (RegressionModel)regressionModel;

                await _RegresssionModels.Update(concrete);
            }
            else
            {
                _Logger.LogInformation(string.Concat("No model of item : ", model.Item.Name));
            }
        }

        private async Task InsertProductLocations(TrainedInfosModel trained)
        {
            var Stats = trained.ProductLocations.Select(e => new ProductLocationWithStatsDTO(e));
            await _ProductLocationNormalizerRepository.Insert(Stats);
        }

        private async Task EndTraining()
        {
            await _ProductLocationNormalizerRepository.UpdateProductLocationStats();
            _SystemInfoRepository.UpdateLastTraining();
        }
    }
}
