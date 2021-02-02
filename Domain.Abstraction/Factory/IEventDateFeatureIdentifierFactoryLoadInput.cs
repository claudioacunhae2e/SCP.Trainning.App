using Domain.Abstraction.Model.Entitys;
using System.Collections.Generic;

namespace Domain.Abstraction.Factory
{
    public interface IEventDateFeatureIdentifierFactoryLoadInput
	{
		IEventDateFeatureIdentifierFactoryLoadInput Add(IEventDateFeature feature);

		IEventDateFeatureIdentifierFactoryLoadInput Add(IEnumerable<IEventDateFeature> features);

		IEventDateFeatureIdentifierFactoryLoadInput Add(params IEventDateFeature[] features);

		IFeatureIdentifierFunctionLoader NextStep();
	}
}
