using Old._42.Util.Extensions;
using System;

namespace Domain.Features
{
    public class GlobalDateFeatureFunctions : DateFeatureFunctions
	{
		public GlobalDateFeatureFunctions()
		{
			Add("Global_IsSunday", IsSunday);
			Add("Global_IsMonday", IsMonday);
			Add("Global_IsTuesday", IsTuesday);
			Add("Global_IsWednesday", IsWednesday);
			Add("Global_IsThursday", IsThursday);
			Add("Global_IsFriday", IsFriday);
			Add("Global_IsSaturday", IsSaturday);
			Add("Global_IsJanuary", IsJanuary);
			Add("Global_IsFebruary", IsFebruary);
			Add("Global_IsMarch", IsMarch);
			Add("Global_IsApril", IsApril);
			Add("Global_IsMay", IsMay);
			Add("Global_IsJune", IsJune);
			Add("Global_IsJuly", IsJuly);
			Add("Global_IsAugust", IsAugust);
			Add("Global_IsSeptember", IsSeptember);
			Add("Global_IsOctober", IsOctober);
			Add("Global_IsNovember", IsNovember);
			Add("Global_IsDecember", IsDecember);
			Add("Global_IsBetweenDay1To7", IsBetweenDay1To7);
			Add("Global_IsBetweenDay8To14", IsBetweenDay8To14);
			Add("Global_IsBetweenDay15To21", IsBetweenDay15To21);
			Add("Global_IsFirstDaysOfMonth", IsFirstDaysOfMonth);
			Add("Global_DaysUntilTheEndOfTheMonth", GetDaysToEndMonth);
		}

		/// <summary>
		/// Identifies if the date is a Sunday
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is Sunday Zero if isn't</returns>
		private double IsSunday(DateTime date) => date.Is(DayOfWeek.Sunday).ToDouble();

		/// <summary>
		/// Identifies if the date is a Monday
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is Monday Zero if isn't</returns>
		private double IsMonday(DateTime date) => date.Is(DayOfWeek.Monday).ToDouble();

		/// <summary>
		/// Identifies if the date is a Tuesday
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is Tuesday Zero if isn't</returns>
		private double IsTuesday(DateTime date) => date.Is(DayOfWeek.Tuesday).ToDouble();

		/// <summary>
		/// Identifies if the date is a Wednesday
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is Wednesday Zero if isn't</returns>
		private double IsWednesday(DateTime date) => date.Is(DayOfWeek.Wednesday).ToDouble();

		/// <summary>
		/// Identifies if the date is a Thursday
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is Thursday Zero if isn't</returns>
		private double IsThursday(DateTime date) => date.Is(DayOfWeek.Thursday).ToDouble();

		/// <summary>
		/// Identifies if the date is a Friday
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is Friday Zero if isn't</returns>
		private double IsFriday(DateTime date) => date.Is(DayOfWeek.Friday).ToDouble();

		/// <summary>
		/// Identifies if the date is a Saturday
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is Saturday Zero if isn't</returns>
		private double IsSaturday(DateTime date) => date.Is(DayOfWeek.Saturday).ToDouble();

		/// <summary>
		/// Identify if the date is a January
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is January Zero if isn't</returns>
		private double IsJanuary(DateTime date) => date.InMonth(1).ToDouble();

		/// <summary>
		/// Identify if the date is a February
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is February Zero if isn't</returns>
		private double IsFebruary(DateTime date) => date.InMonth(2).ToDouble();

		/// <summary>
		/// Identify if the date is a March
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is March Zero if isn't</returns>
		private double IsMarch(DateTime date) => date.InMonth(3).ToDouble();

		/// <summary>
		/// Identify if the date is a April
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is April Zero if isn't</returns>
		private double IsApril(DateTime date) => date.InMonth(4).ToDouble();

		/// <summary>
		/// Identify if the date is a May
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is May Zero if isn't</returns>
		private double IsMay(DateTime date) => date.InMonth(5).ToDouble();

		/// <summary>
		/// Identify if the date is a June
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is June Zero if isn't</returns>
		private double IsJune(DateTime date) => date.InMonth(6).ToDouble();

		/// <summary>
		/// Identify if the date is a July
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is July Zero if isn't</returns>
		private double IsJuly(DateTime date) => date.InMonth(7).ToDouble();

		/// <summary>
		/// Identify if the date is a August
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is August Zero if isn't</returns>
		private double IsAugust(DateTime date) => date.InMonth(8).ToDouble();

		/// <summary>
		/// Identify if the date is a September
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is September Zero if isn't</returns>
		private double IsSeptember(DateTime date) => date.InMonth(9).ToDouble();

		/// <summary>
		/// Identify if the date is a October
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is October Zero if isn't</returns>
		private double IsOctober(DateTime date) => date.InMonth(10).ToDouble();

		/// <summary>
		/// Identify if the date is a November
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is November Zero if isn't</returns>
		private double IsNovember(DateTime date) => date.InMonth(11).ToDouble();

		/// <summary>
		/// Identify if the date is a December
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is December Zero if isn't</returns>
		private double IsDecember(DateTime date) => date.InMonth(12).ToDouble();

		/// <summary>
		///  Identifies whether a date is between the 1th and 7st day of the month
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One is between zero if it isn't</returns>
		private double IsBetweenDay1To7(DateTime date) => date.IsBetweenDays(1, 7).ToDouble();

		/// <summary>
		///  Identifies whether a date is between the 8th and 14st day of the month
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One is between zero if it isn't</returns>
		private double IsBetweenDay8To14(DateTime date) => date.IsBetweenDays(8, 14).ToDouble();

		/// <summary>
		/// Identifies whether a date is between the 15th and 21st day of the month
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One is between zero if it isn't</returns>
		private double IsBetweenDay15To21(DateTime date) => date.IsBetweenDays(15, 21).ToDouble();

		/// <summary>
		/// Identify if the date is from the first days of the month
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>One if is December Zero if isn't</returns>
		private double IsFirstDaysOfMonth(DateTime date) => date.IsFirstWorkingDaysOfTheMonth(3).ToDouble();

		/// <summary>
		/// Identify the amount of days to the end of the month 
		/// </summary>
		/// <param name="date">Date to check</param>
		/// <returns>Amount of days to end of month Zero if it is the first days</returns>
		private double GetDaysToEndMonth(DateTime date)
		{
			if (!date.IsFirstWorkingDaysOfTheMonth(3))
				return (DateTime.DaysInMonth(date.Year, date.Month) - date.Day) + 1;

			return 0;
		}
	}
}
