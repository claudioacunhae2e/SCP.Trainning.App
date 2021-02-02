using Domain.Abstraction.Factory;
using Domain.Abstraction.Model.Entitys;
using Domain.Abstraction.Service;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System;
using System.Collections.Generic;

namespace Domain.Abstraction.Model
{
    public interface IProductLocationInfo : IProductLocation<IHistoryInfo>
    {
        IProductLocation ProductLocation { get; }
        IProduct Product { get; }
        ILocation Location { get; }
        IHistoryInfo AddHistory(DateTime date, double movements, double revenue, double salesQuantity);
        string GetGroupName(IEnumerable<string> productGroupers, IEnumerable<string> locationGroupers);
        string GetGroupName(IRegressionLevel regressionLevel);
        void CalculateLambda(IAlfaCalculator normalizer, IFeatureEngineerFactory factory, int minBeta, DateTime stability);
    }
}
