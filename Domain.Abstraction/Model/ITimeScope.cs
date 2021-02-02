using System;
using System.Collections.Generic;

namespace Domain.Abstraction.Model
{
    public interface ITimeScope
	{
		DateTime Start { get; }
		DateTime End { get; }
		IEnumerable<DateTime> DaysForward();
		IEnumerable<DateTime> DaysBackward();
		bool IsValid();
		bool IsIn(DateTime date);
	}
}
