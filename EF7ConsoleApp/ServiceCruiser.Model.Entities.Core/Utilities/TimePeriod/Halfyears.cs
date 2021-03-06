using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public sealed class Halfyears : HalfyearTimeRange
	{

		// ----------------------------------------------------------------------
		public Halfyears( DateTime moment, YearHalfyear startHalfyear, int count ) :
			this( moment, startHalfyear, count, new TimeCalendar() )
		{
		} // Halfyears

		// ----------------------------------------------------------------------
		public Halfyears( DateTime moment, YearHalfyear startHalfyear, int count, ITimeCalendar calendar ) :
			this( TimeTool.GetYearOf( calendar.YearBaseMonth, calendar.GetYear( moment ), calendar.GetMonth( moment ) ),
			startHalfyear, count, calendar )
		{
		} // Halfyears

		// ----------------------------------------------------------------------
		public Halfyears( int startYear, YearHalfyear startHalfyear, int halfyearCount ) :
			this( startYear, startHalfyear, halfyearCount, new TimeCalendar() )
		{
		} // Halfyears

		// ----------------------------------------------------------------------
		public Halfyears( int startYear, YearHalfyear startHalfyear, int halfyearCount, ITimeCalendar calendar ) :
			base( startYear, startHalfyear, halfyearCount, calendar )
		{
		} // Halfyears

		// ----------------------------------------------------------------------
		public ITimePeriodCollection GetHalfyears()
		{
			TimePeriodCollection halfyears = new TimePeriodCollection();
			for ( int i = 0; i < HalfyearCount; i++ )
			{
				int year;
				YearHalfyear halfyear;
				TimeTool.AddHalfyear( BaseYear, StartHalfyear, i, out year, out halfyear );
				halfyears.Add( new Halfyear( year, halfyear, Calendar ) );
			}
			return halfyears;
		} // GetHalfyears

		// ----------------------------------------------------------------------
		protected override string Format( ITimeFormatter formatter )
		{
			return formatter.GetCalendarPeriod( StartHalfyearOfYearName, EndHalfyearOfYearName,
				formatter.GetShortDate( Start ), formatter.GetShortDate( End ), Duration );
		} // Format

	} // class Halfyears

} // namespace TimePeriod

