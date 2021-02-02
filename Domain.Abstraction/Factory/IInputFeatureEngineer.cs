using Domain.Abstraction.Model;
using System;
using System.Collections.Generic;

namespace Domain.Abstraction.Factory
{
    /// <summary>
    /// Get the features of a given input
    /// </summary>
    public interface IInputFeatureEngineer
	{
		/// <summary>
		/// Get the features of a given input
		/// </summary>
		/// <param name="data">The list of inputs to get the features</param>
		/// <returns>A list of inputs and features</returns>
		void Transform(IEnumerable<IInput> data);
		IDictionary<long, double> GetFeatures(DateTime date);
		IDictionary<DateTime, IDictionary<long, double>> GetFeatures(DateTime start, DateTime end);
	}
}
