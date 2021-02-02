using Domain.Abstraction.Model;
using Domain.Abstraction.Model.Entitys;
using Old._42.SCP.Domain.Bases.Entitys;
using System;
using System.Runtime.Serialization;

namespace Domain.Model.Entitys
{
    [DataContract]
    [KnownType("GetKnownTypes")]
    public class SystemInfo : Entity, ISystemInfo
    {
        [DataMember]
        public virtual int TrainingMaxDegreeOfParallelism { get; set; }

        [DataMember]
        public virtual long DistributionRegressionLevelID { get; set; }

        [DataMember]
        public virtual long NormalizationRegressionLevelID { get; set; }

        [DataMember]
        public virtual int DistributionMaxDaysAhead { get; set; }

        [DataMember]
        public virtual DateTime LastETL { get; set; }

        [DataMember]
        public virtual DateTime HistoryStabilityLimitDateLastLambdaCalculation { get; set; }

        [DataMember]
        public virtual DateTime HistoryStabilityLimitDateNewLambdaCalculation { get; set; }

        [DataMember]
        public virtual DateTime? LastTraining { get; set; }

        [DataMember]
        public virtual DateTime? LastLambdaCalculation { get; set; }

        [DataMember]
        public virtual DateTime DataStart { get; set; }

        [DataMember]
        public virtual int DaysToCalculateAverageSalesOnLocation { get; set; }

        [DataMember]
        public virtual int InitialDistribution_LeadTime { get; set; }

        [DataMember]
        public virtual double InitialDistribution_SLA { get; set; }

        [DataMember]
        public virtual double InitialDistribution_PercentageOfStockOnStoreLimit { get; set; }

        [DataMember]
        public virtual ISystemInfoLambda Lambda { get; set; }

        [DataMember]
        public virtual int LeadTimePreparationAdicionalDays { get; set; }

        [DataMember]
        public virtual DateTime ProductLocationHistoryMaxDate { get; set; }

        [DataMember]
        public virtual DateTime SourceHistoryMaxDate { get; set; }

        [DataMember]
        public virtual double MinDaysWithSalesOverSalesAgeToGroupBeTrained { get; set; }

        [DataMember]
        public virtual string PromotionKeyAttribute { get; set; }

        [DataMember]
        public virtual int GroupHistoryLimitDays { get; set; }

        [DataMember]
        public virtual int MinDaysWithSalesToGroupBeTrained { get; set; }


        [DataMember]
        public virtual ITimeScope DataTimeScope
        {
            get
            {
                if (_DataTimeScope == null)
                {
                    SetDataTimeScope();
                }

                return _DataTimeScope;
            }
        }
        public virtual DateTime StabilityDate
        {
            get
            {
                return HistoryStabilityLimitDateLastLambdaCalculation;
            }
        }

        private ITimeScope _DataTimeScope { get; set; } = null;

        private void SetDataTimeScope() => _DataTimeScope = new TimeScope(DataStart, LastETL.AddDays(-1));
    }
}
