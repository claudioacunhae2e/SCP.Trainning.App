using Domain.Abstraction.Factory;
using Domain.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System.Collections.Generic;

namespace Domain.Abstraction.Features
{
    public interface IFeatureEngineerBuilder
	{
		IFeatureIdentifierFactoryStarted Start();
		IInputFeatureEngineer Default(IEnumerable<IFeature> features);
		IInputFeatureEngineer Default(IEnumerable<IFeature> features, ITimeScope timeScope, string scope = "");
	}
}
