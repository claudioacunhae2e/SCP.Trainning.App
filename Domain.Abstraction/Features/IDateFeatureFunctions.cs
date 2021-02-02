using System;
using System.Collections.Generic;

namespace Domain.Abstraction.Features
{
    public interface IDateFeatureFunctions
	{
		IDictionary<string, Func<DateTime, double>> Functions { get; }
	}
}
