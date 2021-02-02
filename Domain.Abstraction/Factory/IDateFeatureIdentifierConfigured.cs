using System;

namespace Domain.Abstraction.Factory
{
    public interface IDateFeatureIdentifierConfigured
	{
		IInputFeatureEngineer Load(DateTime start, DateTime end);
	}
}
