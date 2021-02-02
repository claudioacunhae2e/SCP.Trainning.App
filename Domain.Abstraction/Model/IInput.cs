using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System;

namespace Domain.Abstraction.Model
{
    public interface IInput
	{
		DateTime Date { get; }
		IObservation Observation { get; }
		double this[IFeature key] { get; set; }
		double this[long key] { get; set; }
	}
}
