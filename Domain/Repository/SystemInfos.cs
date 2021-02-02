using Dapper;
using Domain.Abstraction.ExternalService;
using Domain.Abstraction.Model.Entitys;
using Domain.Abstraction.Repository;
using Domain.Model;
using Domain.Model.Entitys;
using E2E.Generic.Extension;
using Microsoft.Extensions.Logging;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using Old._42.SCP.Domain.Bases.Entitys;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public class SystemInfos : RepositoryBase<SystemInfo, ISystemInfo>, ISystemInfoRepository
    {
        public SystemInfos(
            IDataBaseAccess dataBaseAccess,
            ILogger<SystemInfos> logger,
            ISystemConfigScpParamsExternalService systemConfigScpParamsExternalService)
            : base("SystemInfo", dataBaseAccess)
        {
            _Logger = logger;
            _SystemConfigScpParamsExternalService = systemConfigScpParamsExternalService;
        }

        private readonly ILogger<SystemInfos> _Logger;
        private readonly ISystemConfigScpParamsExternalService _SystemConfigScpParamsExternalService;

        private const string _QueryBase = @"
				SELECT [ID]														[ID]
					  ,[DistributionRegressionLevel]							[DistributionRegressionLevelID]
					  ,[NormalizationRegressionLevel]							[NormalizationRegressionLevelID]
					  ,[DistributionMaxDaysAhead]								[DistributionMaxDaysAhead]
					  ,[LastETL]												[LastETL]
					  ,[LastTraining]											[LastTraining]
					  ,[LastLambdaCalculation]									[LastLambdaCalculation]
					  ,[SourceHistoryMaxDate]									[SourceHistoryMaxDate]
					  ,[ProductLocationHistoryMaxDate]							[ProductLocationHistoryMaxDate]
					  ,[DataStart]												[DataStart]
					  ,[TrainingMaxDegreeOfParallelism]							[TrainingMaxDegreeOfParallelism]
					  ,[DaysToCalculateAverageSalesOnLocation]					[DaysToCalculateAverageSalesOnLocation]
					  ,[InitialDistribution_LeadTime]							[InitialDistribution_LeadTime]
					  ,[InitialDistribution_SLA]								[InitialDistribution_SLA]
					  ,[InitialDistribution_PercentageOfStockOnStoreLimit]		[InitialDistribution_PercentageOfStockOnStoreLimit]
					  ,[LeadTimePreparationAdicionalDays]						[LeadTimePreparationAdicionalDays]
					  ,[MinDaysWithSalesToGroupBeTrained]						[MinDaysWithSalesToGroupBeTrained]
					  ,[MinDaysWithSalesOverSalesAgeToGroupBeTrained]			[MinDaysWithSalesOverSalesAgeToGroupBeTrained]
					  ,[GroupHistoryLimitDays]								    [GroupHistoryLimitDays]
					  ,[HistoryStabilityLimit]									[HistoryStabilityLimitDateLastLambdaCalculation]
					  ,[Lambda_RectificationLevel]								[RectificationLevelID]
					  ,[Lambda_MinBeta]											[MinBeta]
					  ,[Lambda_RectificationMinDataPoints]						[RectificationMinDataPoints]
					  ,[Lambda_RectificationPercentil]							[RectificationPercentil]
					  ,[Lambda_Default]											[Default]
					  ,[Lambda_AlfaOutlier]										[AlfaOutlier]
					  ,[Lambda_MinDataPointsToUseLocationVariationConstraints]	[MinDataPointsToUseLocationVariationConstraints]
					  ,[Lambda_MaxLocationVariation]							[MaxLocationVariation]
					  ,[Lambda_MinLocationVariation]							[MinLocationVariation]
					  ,[PromotionKeyAttribute]									[PromotionKeyAttribute]
				  FROM [dbo].[SystemInfo]	";

        protected override string BaseSelectQuery() =>
            _QueryBase;

        public void UpdateLastLambdaCalculation()
        {
            var param = new
            {
                value = DateTime.Now.NowBrasiliaTimeZone(),
            };

            Query("\tUPDATE SystemInfo SET LastLambdaCalculation = @value\t", param);
        }

        public void UpdateLastTraining()
        {
            _Logger.LogInformation("Updating system info");

            var param = new
            {
                value = DateTime.Now.NowBrasiliaTimeZone(),
            };

            Query("\tUPDATE SystemInfo SET LastTraining = @value\t", param);

            _Logger.LogInformation("Systeminfo updated");
        }

        public void UpdateLastDistribution()
        {
            var param = new
            {
                value = DateTime.Now.NowBrasiliaTimeZone(),
            };

            Query("\tUPDATE SystemInfo SET LastDistribution = @value\t", param);
        }


        public void UpdateLastNormalizationStabilityLimit(DateTime value)
        {
            var param = new
            {
                value
            };

            Query("\tUPDATE SystemInfo SET HistoryStabilityLimit = @value\t", param);
        }

        public async Task<ISystemInfo> Get(string scope = null)
        {
            SystemInfoDTO result;

            var connection = (string.IsNullOrEmpty(scope))
                                ? await DataBaseAccess.GetAsync()
                                : await DataBaseAccess.GetByScopeAsync(scope);

            using (connection)
            {
                _Logger.LogInformation("Consultando SystemInfo");
                result = await connection.QueryFirstAsync<SystemInfoDTO>(_QueryBase, commandTimeout: 0);
                _Logger.LogInformation("Consulta SystemInfo OK");
            }

            _Logger.LogInformation("Consultando SystemConfig");
            var systemConfig = await _SystemConfigScpParamsExternalService.Get();//TODO: remover este comentário para evitar chamar a API new Infra.DTO.SystemConfigDTO { Lambda = 0.03, MinBeta = 15, Percentil = 0.65 };
            _Logger.LogInformation("Consulta SystemConfig OK");

            return result.ToSystemInfo(systemConfig);
        }
    }
}
