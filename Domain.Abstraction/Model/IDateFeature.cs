using System;

namespace Domain.Abstraction.Model
{
    public interface IDateFeature
	{
		double GetValue(DateTime date);
		bool ItOccured(DateTime date);
	}
}
