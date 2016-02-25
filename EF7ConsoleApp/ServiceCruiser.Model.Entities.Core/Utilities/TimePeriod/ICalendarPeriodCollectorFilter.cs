using System.Collections.Generic;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public interface ICalendarPeriodCollectorFilter : ICalendarVisitorFilter
	{

		// ----------------------------------------------------------------------
		IList<MonthRange> CollectingMonths { get; }

		// ----------------------------------------------------------------------
		IList<DayRange> CollectingDays { get; }

		// ----------------------------------------------------------------------
		IList<HourRange> CollectingHours { get; }

	} // interface ICalendarPeriodCollectorFilter

} // namespace TimePeriod

