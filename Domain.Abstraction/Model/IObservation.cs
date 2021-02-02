using System.Collections.Generic;

namespace Domain.Abstraction.Model
{
    public interface IObservation
	{
		/// <summary>
		/// Depedent Variable
		/// </summary>
		double Y { get; }
		/// <summary>
		/// Indenpendent variable
		/// </summary>
		IDictionary<long, double> Xs { get; }
	}
}
