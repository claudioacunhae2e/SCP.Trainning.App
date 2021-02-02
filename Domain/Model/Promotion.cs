using Domain.Abstraction.Model;
using Domain.Standard.Enums;
using Old._42.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Model
{
	public class Promotion : IPromotion
	{
		public DateTime Start { get; set; }
		public DateTime End { get; set; }
		public long Location { get; set; }
		public HashSet<string> Keys { get; set; }
		public PromotionIntensity Intensity { get; set; }

		public Promotion(DateTime Start, DateTime End, long location, IEnumerable<string> keys, PromotionIntensity intensity)
		{
			this.Start = Start;
			this.End = End;
			Location = location;
			Intensity = intensity;
			Keys = new HashSet<string>(keys);
		}
		public bool Match(DateTime date, string key)
		{
			return Keys.Contains(key) && date.IsBetween(Start, End);
		}
		public bool Match(DateTime date)
		{
			return date.IsBetween(Start, End);
		}
		public bool Match(string key)
		{
			return Keys.Contains(key);
		}
	}
}
