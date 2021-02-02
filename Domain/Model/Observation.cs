using Domain.Abstraction.Model;
using System.Collections.Generic;

namespace Domain.Model
{
    public class Observation : IObservation
	{
		public Observation(double y, IDictionary<long, double> xs)
		{
			Y = y;
			Xs = xs;
		}

		public double Y { get; }
		public IDictionary<long, double> Xs { get; }
	}
}
