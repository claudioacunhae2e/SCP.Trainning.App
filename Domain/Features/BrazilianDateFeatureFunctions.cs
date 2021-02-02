using Old._42.Util.Extensions;
using System;

namespace Domain.Features
{
	public class BrazilianDateFeatureFunctions : DateFeatureFunctions
	{
		public BrazilianDateFeatureFunctions()
		{
			Add("Brazil_IsChristmasDay24", IsChristmasDay24);
			Add("Brazil_IsChristmasDay23", IsChristmasDay23);
			Add("Brazil_IsChristmasDaySaturday", IsChristmasSaturday);
			Add("Brazil_IsChristmasWeek", IsChristmasWeek);
			Add("Brazil_IsOneChristmasWeek", IsOneChristmasWeek);
			Add("Brazil_IsTwoChristmasWeek", IsTwoChristmasWeek);
			Add("Brazil_IsThreeChristmasWeek", IsThreeChristmasWeek);
			Add("Brazil_IsFourChristmasWeek", IsFourChristmasWeek);
			Add("Brazil_IsChristmasGeneralEffect", IsChristmasGeneralEffect);
			Add("Brazil_IsChristmasOverhang", IsChristmasOverhang);
			Add("Brazil_IsChildrenDayWeek", IsChildrenDayWeek);
			Add("Brazil_IsValentinesDay", IsValentinesDay);
			Add("Brazil_IsOneValentinesDay", IsOneValentinesWeek);
			Add("Brazil_IsTwoValentinesDay", IsTwoValentinesWeek);
			Add("Brazil_IsThreeValentinesDay", IsThreeValentinesWeek);
			Add("Brazil_IsFourValentinesDay", IsFourValentinesWeek);
			Add("Brazil_IsFatherDaySaturday", IsFatherDaySaturday);
			Add("Brazil_IsFatherDayWeek", IsFatherDayWeek);
			Add("Brazil_IsOneFatherDayWeek", IsOneFatherDayWeek);
			Add("Brazil_IsMotherDaySaturday", IsMotherDaySaturday);
			Add("Brazil_IsMotherDayWeek", IsMotherDayWeek);
			Add("Brazil_IsOneMotherDayWeek", IsOneMotherDayWeek);
			Add("Brazil_IsTwoMotherDayWeek", IsTwoMotherDayWeek);
			Add("Brazil_IsThreeMotherDayWeek", IsThreeMotherDayWeek);
			Add("Brazil_IsFourMotherDayWeek", IsFourMotherDayWeek);
			Add("Brazil_IsPromoBlackFridayWeek", IsPromoBlackFriday);
			Add("Brazil_IsPromoBlackFridayMainDay", IsPromoBlackFridayMainDay);
			Add("Brazil_IsCovid19", IsCovid19);

		}

		/// <summary>
		/// Identify if the date is the day before Christmas (24/12)
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is 24 of December, Zero if isn't</returns>
		private double IsChristmasDay24(DateTime date) => date.IsDay(24, 12).ToDouble();

		/// <summary>
		/// Identify if the date is two days before Christmas (23/12)
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is 23 of December, Zero if isn't</returns>
		private double IsChristmasDay23(DateTime date) => date.IsDay(23, 12).ToDouble();

		/// <summary>
		/// Identify if the date is one of the latest 4 Saturdays before Christmas
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is one of the latest 4 Saturdays before Christmas, Zero if isn't</returns>
		private double IsChristmasSaturday(DateTime date) => (date.IsBetween(new DateTime(date.Year, 12, 25).GetPreviousWeeks(4), new DateTime(date.Year, 12, 25)) && date.Is(DayOfWeek.Saturday)).ToDouble();

		/// <summary>
		/// Identify if the date is a week before Christmas
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is Christmas week Zero if isn't</returns>
		private double IsChristmasWeek(DateTime date) => date.IsBetween(new DateTime(date.Year, 12, 18), new DateTime(date.Year, 12, 24)).ToDouble();

		/// <summary>
		/// Identify if the date is on one week before Christmas
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if the date is into one weeks from Christmas, Zero if isn't</returns>
		private double IsOneChristmasWeek(DateTime date) => date.IsBetween(new DateTime(date.Year, 12, 11), new DateTime(date.Year, 12, 17)).ToDouble();

		/// <summary>
		/// Identify if the date is into two weeks before Christmas
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if the date is into two weeks from Christmas, Zero if isn't</returns>
		private double IsTwoChristmasWeek(DateTime date) => date.IsBetween(new DateTime(date.Year, 12, 4), new DateTime(date.Year, 12, 10)).ToDouble();

		/// <summary>
		///Identify if the date is into three weeks before Christmas
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if the date is into three weeks from Christmas, Zero if isn't</returns>
		private double IsThreeChristmasWeek(DateTime date) => date.IsBetween(new DateTime(date.Year, 11, 27), new DateTime(date.Year, 12, 3)).ToDouble();

		/// <summary>
		/// Identify if the date is into four weeks before Christmas
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if the date is into four weeks from Christmas, Zero if isn't</returns>
		private double IsFourChristmasWeek(DateTime date) => date.IsBetween(new DateTime(date.Year, 11, 20), new DateTime(date.Year, 11, 26)).ToDouble();

		/// <summary>
		/// Identify if Christmas is coming
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>The closer to Christmas, the greater the effect Zero if isn't</returns>
		private double IsChristmasGeneralEffect(DateTime date) => date.IsBetween(new DateTime(date.Year, 11, 25), new DateTime(date.Year, 12, 22)) ? 28 - date.GetDaysDiff(new DateTime(date.Year, 12, 22)) : 0;

		/// <summary>
		/// Identify if it's days after Christmas
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if it is one of the 5 days after Christmas Zero if isn't</returns>
		private double IsChristmasOverhang(DateTime date) => date.IsBetween(new DateTime(date.Year, 12, 26), new DateTime(date.Year, 12, 30)).ToDouble();

		/// <summary>
		/// Identify if it's Children's day week
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One to eight value, where 8 is the children`s day and 1 the week before </returns>
		private double ChildrenDayWeek(DateTime date) => date.IsBetween(new DateTime(date.Year, 10, 5), new DateTime(date.Year, 10, 12)) ? 8 - date.GetDaysDiff(new DateTime(date.Year, 10, 12)) : 0;

		/// <summary>
		/// Identify if it's Children's day week
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if it is children`s day week, zero if not </returns>
		private double IsChildrenDayWeek(DateTime date) => date.IsBetween(new DateTime(date.Year, 10, 5), new DateTime(date.Year, 10, 12)) ? 1 : 0;

		/// <summary>
		/// Identify if Valentine's Day
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if It's Valentine's Day Zero if isn't</returns>
		private double IsValentinesDay(DateTime date) => date.IsBetween(new DateTime(date.Year, 06, 08), new DateTime(date.Year, 06, 12)).ToDouble();

		/// <summary>
		/// Identify if it's a week before Valentine's Day
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if a week before Valentine's Day Zero if isn't</returns>
		private double IsOneValentinesWeek(DateTime date) => date.IsBetween(new DateTime(date.Year, 06, 01), new DateTime(date.Year, 06, 07)).ToDouble();

		/// <summary>
		/// Identify if you are two weeks early Valentine's Day
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One is two weeks before Valentine's Day Zero if isn't</returns>
		private double IsTwoValentinesWeek(DateTime date) => date.IsBetween(new DateTime(date.Year, 05, 25), new DateTime(date.Year, 05, 31)).ToDouble();

		/// <summary>
		/// Identify if you are three weeks early Valentine's Day
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One is three weeks before Valentine's Day Zero if isn't</returns>
		private double IsThreeValentinesWeek(DateTime date) => date.IsBetween(new DateTime(date.Year, 05, 18), new DateTime(date.Year, 05, 24)).ToDouble();

		/// <summary>
		/// Identify if you are four weeks early Valentine's Day
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One is four weeks before Valentine's Day Zero if isn't</returns>
		private double IsFourValentinesWeek(DateTime date) => date.IsBetween(new DateTime(date.Year, 05, 11), new DateTime(date.Year, 05, 17)).ToDouble();

		/// <summary>
		/// Identify if it's Saturday from the father's day
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if it's Saturday from Father's Day Zero if isn't</returns>
		private double IsFatherDaySaturday(DateTime date) => (date == new DateTime(date.Year, 08, 1).GetOccurrenceOfDayOfWeek(2, DayOfWeek.Saturday)).ToDouble();

		/// <summary>
		/// Identify if it is in the week of the father's day
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One is in the week of the father's day Zero if isn't</returns>
		private double IsFatherDayWeek(DateTime date) => (date.InWeek(new DateTime(date.Year, 08, 1).GetOccurrenceOfDayOfWeek(2, DayOfWeek.Saturday), -1)).ToDouble();

		/// <summary>
		/// Identify if one is a week ago from father's Day
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if there is one in the week of the father's day Zero if isn't</returns>
		private double IsOneFatherDayWeek(DateTime date) => (date.InWeekExcludingEnd(new DateTime(date.Year, 08, 1).GetOccurrenceOfDayOfWeek(2, DayOfWeek.Saturday), -2)).ToDouble();

		/// <summary>
		/// Identify If is Mothers Day
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		private double IsMothersDay(DateTime date) => (date.InMonth(5) && date == date.GetOccurrenceOfDayOfWeek(2, DayOfWeek.Sunday)).ToDouble();

		/// <summary>
		/// Identify if it's Saturday from the mother's day
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if it's Saturday from mother's Day Zero if isn't</returns>
		private double IsMotherDaySaturday(DateTime date) => IsMothersDay(date.AddDays(1));

		/// <summary>
		/// Identify if there is a week of mothers days
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One is in the week of Mother's Day Zero if isn't</returns>
		private double IsMotherDayWeek(DateTime date) => (date.InWeek(new DateTime(date.Year, 05, 1).GetOccurrenceOfDayOfWeek(2, DayOfWeek.Saturday), -1)).ToDouble();

		/// <summary>
		///  Identify if it's a week before mother's Day
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if a week before Mother's Day Zero if isn't</returns>
		private double IsOneMotherDayWeek(DateTime date) => (date.InWeekExcludingEnd(new DateTime(date.Year, 05, 1).GetOccurrenceOfDayOfWeek(2, DayOfWeek.Saturday), -2)).ToDouble();

		/// <summary>
		/// Identify if you are two weeks away from the days of the mothers
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One is two weeks before Mother's Day Zero if isn't</returns>
		private double IsTwoMotherDayWeek(DateTime date) => (date.InWeekExcludingEnd(new DateTime(date.Year, 05, 1).GetOccurrenceOfDayOfWeek(2, DayOfWeek.Saturday), -3)).ToDouble();

		/// <summary>
		/// Identify if you are three weeks away from the days of the mothers
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One is three weeks before Mother's Day Zero if isn't</returns>
		private double IsThreeMotherDayWeek(DateTime date) => (date.InWeekExcludingEnd(new DateTime(date.Year, 05, 1).GetOccurrenceOfDayOfWeek(2, DayOfWeek.Saturday), -4)).ToDouble();

		/// <summary>
		/// Identify if you are four weeks away from the days of the mothers
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One is four weeks before Mother's Day Zero if isn't</returns>
		private double IsFourMotherDayWeek(DateTime date) => (date.InWeekExcludingEnd(new DateTime(date.Year, 05, 1).GetOccurrenceOfDayOfWeek(2, DayOfWeek.Saturday), -5)).ToDouble();

		/// <summary>
		/// Identify if it's black Friday week
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One is in the week of black friday Zero if isn't</returns>
		private double IsPromoBlackFriday(DateTime date) => (date.InWeekExcludingStart(new DateTime(date.Year, 11, 1).GetOccurrenceOfDayOfWeek(4, DayOfWeek.Thursday).AddDays(1), -1, days: 1)).ToDouble();

		/// <summary>
		/// Identify if it's black friday
		/// </summary>
		/// <param name="date"></param>
		/// <returns>One if is black friday Zero if isn't</returns>
		private double IsPromoBlackFridayMainDay(DateTime date) => (date == new DateTime(date.Year, 11, 1).GetOccurrenceOfDayOfWeek(4, DayOfWeek.Thursday).AddDays(1)).ToDouble();

		/// <summary>
		/// Identify if the date is into Brazilian Pandemic period
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if the date is into Brazilian Pandemic period, Zero if isn't</returns>
		private double IsCovid19(DateTime date) => date.IsBetween(new DateTime(2020, 3, 15), new DateTime(2020, 11, 15)).ToDouble();

	}
}
