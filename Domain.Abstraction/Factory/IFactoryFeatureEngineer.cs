namespace Domain.Abstraction.Factory
{
    public interface IFactoryFeatureEngineer
    {
        IFactoryFeatureEngineer AddFeatureEngineer(IInputFeatureEngineer featureEngineer);
        IFactoryModelInitializer Next();
    }
}
