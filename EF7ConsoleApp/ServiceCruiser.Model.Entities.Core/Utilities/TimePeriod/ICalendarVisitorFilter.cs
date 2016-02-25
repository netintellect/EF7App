using System;
using System.Collections.Generic;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public interface ICalendarVisitorFilter
	{

		// ----------------------------------------------------------------------
		ITimePeriodCollection ExcludePeriods { get; }

		// ----------------------------------------------------------------------
		IList<int> Years { get; }

		// ----------------------------------------------------------------------
		IList<YearMonth> Months { get; }

		// ----------------------------------------------------------------------
		IList<int> Days { get; }

		// ----------------------------------------------------------------------
		IList<DayOfWeek> WeekDays { get; }

		// ----------------------------------------------------------------------
		IList<int> Hours { get; }

		// ----------------------------------------------------------------------
		void AddWorkingWeekDays();

		// ----------------------------------------------------------------------
		void AddWeekendWeekDays();

		// ----------------------------------------------------------------------
		void Clear();

	} // interface ICalendarVisitorFilter

} // namespace TimePeriod

