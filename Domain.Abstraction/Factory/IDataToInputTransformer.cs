﻿using Domain.Abstraction.Model;
using System.Collections.Generic;

namespace Domain.Abstraction.Factory
{
    /// <summary>
    /// Allows you transform a given object into an input for the experiment
    /// </summary>
    /// <typeparam name="T">The type of object that will be transformed</typeparam>
    public interface IDataToInputTransformer<T>
	{
		/// <summary>
		/// Transforms the list of objects into a list of inputs to the experiment
		/// </summary>
		/// <param name="data">List of objects to be transformed</param>
		/// <returns>Inputs based on objects sent</returns>
		IEnumerable<IRegressionModelInput> Transform(IEnumerable<T> data);
	}
}
