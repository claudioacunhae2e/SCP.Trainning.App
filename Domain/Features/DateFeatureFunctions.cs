using Domain.Abstraction.Features;
using System;
using System.Collections.Generic;

namespace Domain.Features
{
    public abstract class DateFeatureFunctions : IDateFeatureFunctions
	{
		public IDictionary<string, Func<DateTime, double>> Functions { get; } = new Dictionary<string, Func<DateTime, double>>();

		public double this[string key, DateTime date] => Functions[key](date);

		public DateFeatureFunctions Add(string key, Func<DateTime, double> value)
		{
			Functions.Add(key, value);
			return this;
		}
	}
}
