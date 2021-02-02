using Domain.Abstraction.Model;
using E2E.SCP.RegressionModel.Abstraction.Model.Entitys;
using System;

namespace Domain.Model
{
    public class Input : IInput
	{
		public Input(DateTime date, IObservation observation)
		{
			Date = date;
			Observation = observation;
		}

		public double this[IFeature key]
		{
			get => this[key.ID];
			set => this[key.ID] = value;
		}
		public double this[long key]
		{
			get => Observation.Xs[key];
			set => Observation.Xs[key] = value;
		}

		public DateTime Date { get; }

		public IObservation Observation { get; }
	}
}
