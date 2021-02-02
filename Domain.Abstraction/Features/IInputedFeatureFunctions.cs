using Domain.Abstraction.Model.Entitys;
using System;
using System.Collections.Generic;

namespace Domain.Abstraction.Features
{
    public interface IInputedFeatureFunctions
	{
		IDictionary<string, Func<IEventDateFeature, DateTime, double>> Functions { get; }
	}
}
