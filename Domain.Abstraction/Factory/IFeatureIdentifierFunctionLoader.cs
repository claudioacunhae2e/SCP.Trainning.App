using Domain.Abstraction.Features;

namespace Domain.Abstraction.Factory
{
    public interface IFeatureIdentifierFunctionLoader
	{
		IFeatureIdentifierFunctionLoader AddCalculableFunctions<T>() where T : IDateFeatureFunctions, new();
		IFeatureIdentifierFunctionLoader AddInputedFunctions<T>() where T : IInputedFeatureFunctions, new();

		IDateFeatureIdentifierConfigured NextStep();
	}
}
