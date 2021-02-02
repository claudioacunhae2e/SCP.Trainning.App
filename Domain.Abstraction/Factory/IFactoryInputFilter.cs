namespace Domain.Abstraction.Factory
{
    public interface IFactoryInputFilter
	{
		IFactoryInputFilter Add(IInputFilter filter);
		IFactoryFeaturesInput Next();
	}
}
