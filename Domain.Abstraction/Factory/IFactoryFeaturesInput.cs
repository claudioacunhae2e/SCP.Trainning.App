using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstraction.Factory
{
    public interface IFactoryFeaturesInput
    {
        IFactoryFeaturesInput Add(IList<IFeature> features);
        IFactoryFeatureEngineer Next();
    }
}
