using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System.Collections.Generic;

namespace Domain.Abstraction.Factory
{
    public interface IFeatureIdentifierFactoryStarted
	{
		IFeatureIdentifierFactoryStarted Add(IFeature feature);

		IFeatureIdentifierFactoryStarted Add(IEnumerable<IFeature> features);

		IFeatureIdentifierFactoryStarted Add(params IFeature[] features);

		IEventDateFeatureIdentifierFactoryLoadInput NextStep();
	}
}
