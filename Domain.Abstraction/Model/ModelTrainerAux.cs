using Domain.Abstraction.Factory;
using Domain.Abstraction.Model.Entitys;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System;
using System.Collections.Generic;

namespace Domain.Abstraction.Model
{
    public class ModelTrainerAux
    {
        public bool Normalize { get; set; }
        public ISystemInfo SystemInfo { get; set; }
        public IRegressionLevel Level { get; set; }
        public IRegressionLevel DistributionLevel { get; set; }
        public ILocationHistoryOpen Schedule { get; set; }
        public IDictionary<string, ILocationHistoryOpen> ScopedSchedules { get; set; }
        public IInputFeatureEngineer DateFeatureEngineer { get; set; }
        public IFeatureEngineerFactory Factory { get; set; }
        public Func<string, ILocationHistoryOpen> GetSchedule { get; set; }
        public IDictionary<long, IDictionary<string, IRegressionModel>> LastSessionModels { get; set; }
    }
}
