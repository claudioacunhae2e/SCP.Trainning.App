using Domain.Abstraction.Model;
using System.Collections.Generic;

namespace Domain.Abstraction.Factory
{
    /// <summary>
    /// Allows you to filter the given inputs
    /// </summary>
    public interface IInputFilter
	{

		/// <summary>
		/// Transforms the list of objects into a list of observations to the experiment
		/// </summary>
		/// <param name="data">List of objects to be transformed</param>
		/// <returns>Observations based on objects sent</returns>
		IList<IInput> Filter(IList<IInput> inputs);
	}
}
