using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using System;

namespace Domain.Abstraction.Model.Entitys
{
    public interface ISystemInfo : IEntity
	{
		int TrainingMaxDegreeOfParallelism { get; }
		long DistributionRegressionLevelID { get; set; }
		long NormalizationRegressionLevelID { get; set; }
		ITimeScope DataTimeScope { get; }
		DateTime? LastTraining { get; }
		DateTime LastETL { get; }
		DateTime HistoryStabilityLimitDateLastLambdaCalculation { get; }
		DateTime HistoryStabilityLimitDateNewLambdaCalculation { get; }
		DateTime? LastLambdaCalculation { get; }
		DateTime DataStart { get; }
		int DaysToCalculateAverageSalesOnLocation { get; }
		ISystemInfoLambda Lambda { get; set; }
		int LeadTimePreparationAdicionalDays { get; }
		DateTime ProductLocationHistoryMaxDate { get; }
		DateTime SourceHistoryMaxDate { get; }
		int MinDaysWithSalesToGroupBeTrained { get; set; }
		double MinDaysWithSalesOverSalesAgeToGroupBeTrained { get; set; }
		int GroupHistoryLimitDays { get; set; }
		int DistributionMaxDaysAhead { get; set; }
		string PromotionKeyAttribute { get; set; }
	}
}
