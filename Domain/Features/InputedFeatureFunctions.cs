using Domain.Abstraction.Features;
using Domain.Abstraction.Model.Entitys;
using System;
using System.Collections.Generic;

namespace Domain.Features
{
    public abstract class InputedFeatureFunctions : IInputedFeatureFunctions
	{
		public IDictionary<string, Func<IEventDateFeature, DateTime, double>> Functions { get; } = new Dictionary<string, Func<IEventDateFeature, DateTime, double>>();


		public InputedFeatureFunctions Add(string key, Func<IEventDateFeature, DateTime, double> value)
		{
			Functions.Add(key, value);
			return this;
		}
	}
}
