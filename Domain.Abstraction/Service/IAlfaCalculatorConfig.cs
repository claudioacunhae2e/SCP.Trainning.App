using E2E.SCP.RegressionModel.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System.Collections.Generic;

namespace Domain.Abstraction.Service
{
    public interface IAlfaCalculatorConfig
    {
        IAlfaCalculator Build(IModel model, IEnumerable<IFeature> features, double? alfaOutlier);
    }
}
