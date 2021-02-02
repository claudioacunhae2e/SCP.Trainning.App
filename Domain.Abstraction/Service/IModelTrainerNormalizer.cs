using Domain.Abstraction.Factory;
using Domain.Abstraction.Model.Entitys;
using E2E.SCP.RegressionModel.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System.Collections.Generic;

namespace Domain.Abstraction.Service
{
    public interface IModelTrainerNormalizer
    {
        IEnumerable<IProductLocation> Calculate(IRegressionLevel level,
                                                string item,
                                                IModel model,
                                                IFeatureEngineerFactory factory,
                                                IList<IFeature> normalizationFeatures,
                                                ISystemInfo systemInfo);
    }
}
