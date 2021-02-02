using Domain.Abstraction.Service;
using E2E.SCP.RegressionModel.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System.Collections.Generic;

namespace Domain.Service
{
    public class AlfaCalculatorConfig : IAlfaCalculatorConfig
    {
        public IAlfaCalculator Build(IModel model, IEnumerable<IFeature> features, double? alfaOutlier) =>
            new AlfaCalculator(model, features, alfaOutlier);
    }
}
