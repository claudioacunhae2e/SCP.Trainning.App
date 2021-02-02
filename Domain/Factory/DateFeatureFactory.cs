using Domain.Abstraction.Factory;

namespace Domain.Factory
{
    public class DateFeatureFactory : IFeatureIdentifierFactory
	{
		public IFeatureIdentifierFactoryStarted Start() => new DateFeatureEngineer();
	}
}
