using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using Old._42.SCP.Domain.Abstractions.Bases.Entitys;
using System;

namespace Domain.Abstraction.Model.Entitys
{
    public interface IEventDateFeature : IDateFeature, IEntity
	{
		DateTime Start { get; set; }
		DateTime? End { get; set; }
		double OcurrenceValue { get; set; }
		string Rule { get; set; }
		long FeatureID { get; set; }
		IFeature Feature { get; set; }
		IRegressionLevel RegressionLevel { get; set; }
	}
}
