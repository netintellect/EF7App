using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public class DayHourRange : HourRange
	{

		// ----------------------------------------------------------------------
		public DayHourRange( DayOfWeek day, int hour ) :
			base( hour, hour )
		{
			this.day = day;
		} // DayHourRange

		// ----------------------------------------------------------------------
		public DayHourRange( DayOfWeek day, int startHour, int endHour ) :
			base( new Time( startHour ), new Time( endHour ) )
		{
			this.day = day;
		} // DayHourRange

		// ----------------------------------------------------------------------
		public DayHourRange( DayOfWeek day, Time start, Time end ) :
			base( start, end )
		{
			this.day = day;
		} // DayHourRange

		// ----------------------------------------------------------------------
		public DayOfWeek Day
		{
			get { return day; }
		} // Day

		// ----------------------------------------------------------------------
		public override string ToString()
		{
			return Day + ": " + base.ToString();
		} // ToString

		// ----------------------------------------------------------------------
		// members
		private readonly DayOfWeek day;

	} // class DayHourRange

} // namespace TimePeriod

