using System;
using System.Globalization;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public struct TimeCalendarConfig
	{

		// ----------------------------------------------------------------------
		public CultureInfo Culture { get; set; }

		// ----------------------------------------------------------------------
		public YearType? YearType { get; set; }

		// ----------------------------------------------------------------------
		public TimeSpan? StartOffset { get; set; }

		// ----------------------------------------------------------------------
		public TimeSpan? EndOffset { get; set; }

		// ----------------------------------------------------------------------
		public YearMonth? YearBaseMonth { get; set; }

		// ----------------------------------------------------------------------
		public YearMonth? FiscalYearBaseMonth { get; set; }

		// ----------------------------------------------------------------------
		public DayOfWeek? FiscalFirstDayOfYear { get; set; }

		// ----------------------------------------------------------------------
		public FiscalYearAlignment? FiscalYearAlignment { get; set; }

		// ----------------------------------------------------------------------
		public FiscalQuarterGrouping? FiscalQuarterGrouping { get; set; }

		// ----------------------------------------------------------------------
		public YearWeekType? YearWeekType { get; set; }

		// ----------------------------------------------------------------------
		public CalendarNameType? DayNameType { get; set; }

		// ----------------------------------------------------------------------
		public CalendarNameType? MonthNameType { get; set; }

	} // struct TimeCalendarConfig

} // namespace TimePeriod

