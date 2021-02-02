using Domain.Standard.Enums;
using System;
using System.Collections.Generic;

namespace Domain.Abstraction.Model
{
    public interface IPromotion
	{
		DateTime Start { get; }
		DateTime End { get; }
		long Location { get; }
		PromotionIntensity Intensity { get; }
		HashSet<string> Keys { get; }

		bool Match(DateTime date, string key);
		bool Match(DateTime date);
		bool Match(string key);
	}
}
