namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public interface ICalendarTimeRange : ITimeRange
	{

		// ----------------------------------------------------------------------
		ITimeCalendar Calendar { get; }

	} // interface ICalendarTimeRange

} // namespace TimePeriod

