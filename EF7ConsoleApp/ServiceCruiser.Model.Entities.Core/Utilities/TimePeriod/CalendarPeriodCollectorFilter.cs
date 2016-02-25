using System.Collections.Generic;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public class CalendarPeriodCollectorFilter : CalendarVisitorFilter, ICalendarPeriodCollectorFilter
	{

		// ----------------------------------------------------------------------
		public override void Clear()
		{
			base.Clear();
			collectingMonths.Clear();
			collectingDays.Clear();
			collectingHours.Clear();
		} // Clear

		// ----------------------------------------------------------------------
		public IList<MonthRange> CollectingMonths
		{
			get { return collectingMonths; }
		} // CollectingMonths

		// ----------------------------------------------------------------------
		public IList<DayRange> CollectingDays
		{
			get { return collectingDays; }
		} // CollectingDays

		// ----------------------------------------------------------------------
		public IList<HourRange> CollectingHours
		{
			get { return collectingHours; }
		} // CollectingHours

		// ----------------------------------------------------------------------
		public IList<DayHourRange> CollectingDayHours
		{
			get { return collectingDayHours; }
		} // CollectingDayHours

		// ----------------------------------------------------------------------
		// members
		private readonly List<MonthRange> collectingMonths = new List<MonthRange>();
		private readonly List<DayRange> collectingDays = new List<DayRange>();
		private readonly List<HourRange> collectingHours = new List<HourRange>();
		private readonly List<DayHourRange> collectingDayHours = new List<DayHourRange>();

	} // class CalendarPeriodCollectorFilter

} // namespace TimePeriod

