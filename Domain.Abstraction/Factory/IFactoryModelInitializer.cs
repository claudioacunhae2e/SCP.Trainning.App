using E2E.SCP.RegressionModel.Abstraction.Model;

namespace Domain.Abstraction.Factory
{
    public interface IFactoryModelInitializer
    {
        IFactoryModelRefiner AddModelInitializer(IModel model);
    }
}
