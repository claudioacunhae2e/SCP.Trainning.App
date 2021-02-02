using Domain.Abstraction.Model.Entitys;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using Old._42.SCP.Domain.Bases.Entitys;
using System;

namespace Domain.Model.Entitys
{
    public class EventDateFeature : Entity, IEventDateFeature
    {
        public virtual DateTime Start { get; set; }
        public virtual DateTime? End { get; set; }
        public virtual double OcurrenceValue { get; set; }
        public virtual long FeatureID { get; set; }
        public virtual IFeature Feature { get; set; }
        public virtual IRegressionLevel RegressionLevel { get; set; }
        public virtual string Rule { get; set; }
        public virtual Func<DateTime, double> RuleFunction { get; set; }
        public virtual double GetValue(DateTime date) => ItOccured(date) ? OcurrenceValue : Feature.NotOcurenceValue;
        public virtual bool ItOccured(DateTime date) => End.HasValue ? date >= Start && date <= End : date == Start;

        public EventDateFeature()
        {
        }
    }
}
