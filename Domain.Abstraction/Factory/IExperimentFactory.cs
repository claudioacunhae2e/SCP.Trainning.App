using Domain.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;

namespace Domain.Abstraction.Factory
{
    public interface IExperimentFactory
    {
        IFactoryConfigured Bayesian(IRegressionLevel level, IRegressionModelInput input, IModel prior, params IInputFeatureEngineer[] engineer);
    }
}
