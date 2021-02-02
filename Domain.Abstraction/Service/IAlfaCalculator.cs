using Domain.Abstraction.Factory;
using System;

namespace Domain.Abstraction.Service
{
    public interface IAlfaCalculator
	{
		double Normalize(double salesQuantity, DateTime date, IInputFeatureEngineer engineer);
	}
}
