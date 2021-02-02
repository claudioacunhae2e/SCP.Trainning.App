using Domain.Abstraction.Model.Entitys;
using System;

namespace Domain.Features
{
    public class InputedDateFeatureFunctions : InputedFeatureFunctions
	{
		public InputedDateFeatureFunctions()
		{
			Add("DaysUntilTheEnd", DaysUntilTheEnd);
			Add("DaysSinceTheStart", DaysSinceTheStart);
		}

		private double DaysUntilTheEnd(IEventDateFeature inputedFeature, DateTime date)
		{
			return date > inputedFeature.Start && date < inputedFeature.End ? (inputedFeature.End.Value - date).TotalDays : inputedFeature.Feature.NotOcurenceValue;
		}

		private double DaysSinceTheStart(IEventDateFeature inputedFeature, DateTime date)
		{
			return date > inputedFeature.Start && date < inputedFeature.End ? (date - inputedFeature.Start).TotalDays : inputedFeature.Feature.NotOcurenceValue;
		}

	}
}
