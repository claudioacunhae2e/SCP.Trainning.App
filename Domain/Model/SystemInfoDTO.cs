using Domain.Abstraction.Model;
using Domain.Model.Entitys;
using System;

namespace Domain.Model
{
    public class SystemInfoDTO
    {
        public virtual long ID { get; set; }
        public virtual int TrainingMaxDegreeOfParallelism { get; }
        public virtual long DistributionRegressionLevelID { get; set; }
        public virtual long NormalizationRegressionLevelID { get; set; }
        public virtual int DistributionMaxDaysAhead { get; set; }
        public virtual DateTime LastETL { get; set; }
        public virtual DateTime HistoryStabilityLimitDateLastLambdaCalculation { get; set; }
        public virtual DateTime HistoryStabilityLimitDateNewLambdaCalculation { get; set; }
        public virtual DateTime? LastTraining { get; set; }
        public virtual DateTime? LastLambdaCalculation { get; set; }
        public virtual DateTime DataStart { get; set; }
        public virtual int DaysToCalculateAverageSalesOnLocation { get; set; }
        public virtual int InitialDistribution_LeadTime { get; set; }
        public virtual double InitialDistribution_SLA { get; set; }
        public virtual double InitialDistribution_PercentageOfStockOnStoreLimit { get; set; }
        public virtual int LeadTimePreparationAdicionalDays { get; set; }
        public virtual DateTime ProductLocationHistoryMaxDate { get; }
        public virtual DateTime SourceHistoryMaxDate { get; }
        public virtual int MinDaysWithSalesToGroupBeTrained { get; set; }
        public virtual double MinDaysWithSalesOverSalesAgeToGroupBeTrained { get; set; }
        public virtual int GroupHistoryLimitDays { get; set; }
        public int MinBeta { get; set; }
        public int RectificationMinDataPoints { get; set; }
        public double RectificationPercentil { get; set; }
        public long RectificationLevelID { get; set; }
        public double Default { get; set; }
        public int MinDataPointsToUseLocationVariationConstraints { get; set; }
        public double MaxLocationVariation { get; set; }
        public double MinLocationVariation { get; set; }
        public virtual double? AlfaOutlier { get; set; }
        public virtual string PromotionKeyAttribute { get; set; }

        public SystemInfo ToSystemInfo(ISystemConfigDTO SystemConfigDto)
        {
            var lambda = ToSystemInfoLambda(SystemConfigDto);

            return new SystemInfo()
            {
                ID = ID,
                TrainingMaxDegreeOfParallelism = TrainingMaxDegreeOfParallelism,
                DistributionRegressionLevelID = DistributionRegressionLevelID,
                NormalizationRegressionLevelID = NormalizationRegressionLevelID,
                DistributionMaxDaysAhead = DistributionMaxDaysAhead,
                LastETL = LastETL,
                HistoryStabilityLimitDateLastLambdaCalculation = HistoryStabilityLimitDateLastLambdaCalculation,
                HistoryStabilityLimitDateNewLambdaCalculation = SourceHistoryMaxDate.AddDays(-GroupHistoryLimitDays),
                LastTraining = LastTraining,
                LastLambdaCalculation = LastLambdaCalculation,
                DataStart = DataStart,
                DaysToCalculateAverageSalesOnLocation = DaysToCalculateAverageSalesOnLocation,
                InitialDistribution_LeadTime = InitialDistribution_LeadTime,
                InitialDistribution_SLA = InitialDistribution_SLA,
                InitialDistribution_PercentageOfStockOnStoreLimit = InitialDistribution_PercentageOfStockOnStoreLimit,
                LeadTimePreparationAdicionalDays = LeadTimePreparationAdicionalDays,
                ProductLocationHistoryMaxDate = ProductLocationHistoryMaxDate,
                SourceHistoryMaxDate = SourceHistoryMaxDate,
                MinDaysWithSalesToGroupBeTrained = MinDaysWithSalesToGroupBeTrained,
                MinDaysWithSalesOverSalesAgeToGroupBeTrained = MinDaysWithSalesOverSalesAgeToGroupBeTrained,
                GroupHistoryLimitDays = GroupHistoryLimitDays,
                PromotionKeyAttribute = PromotionKeyAttribute,
                Lambda = lambda,
            };
        }

        private SystemInfoLambda ToSystemInfoLambda(ISystemConfigDTO SystemConfigDto)
        {
            return new SystemInfoLambda()
            {
                MinBeta = SystemConfigDto.MinBeta,
                RectificationPercentil = SystemConfigDto.Percentil,
                Default = SystemConfigDto.Lambda,
                RectificationMinDataPoints = RectificationMinDataPoints,
                RectificationLevelID = RectificationLevelID,
                MinDataPointsToUseLocationVariationConstraints = MinDataPointsToUseLocationVariationConstraints,
                MaxLocationVariation = MaxLocationVariation,
                MinLocationVariation = MinLocationVariation,
                AlfaOutlier = AlfaOutlier,
            };
        }
    }
}
