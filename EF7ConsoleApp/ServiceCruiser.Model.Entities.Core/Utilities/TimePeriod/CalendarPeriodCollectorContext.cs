namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public class CalendarPeriodCollectorContext : ICalendarVisitorContext
	{

		// ----------------------------------------------------------------------
		public enum CollectType
		{
			Year,
			Month,
			Day,
			Hour,
		} // enum CollectType

		// ----------------------------------------------------------------------
		public CollectType Scope { get; set; }

	} // class CalendarPeriodCollectorContext

} // namespace TimePeriod

