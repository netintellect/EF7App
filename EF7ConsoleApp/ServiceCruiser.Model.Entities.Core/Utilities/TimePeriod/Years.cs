using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public sealed class Years : YearTimeRange
	{

		// ----------------------------------------------------------------------
		public Years( DateTime moment, int count ) :
			this( moment, count, new TimeCalendar() )
		{
		} // Years

		// ----------------------------------------------------------------------
		public Years( DateTime moment, int count, ITimeCalendar calendar ) :
			this( TimeTool.GetYearOf( calendar.YearBaseMonth, calendar.GetYear( moment ), calendar.GetMonth( moment ) ),
			count, calendar )
		{
		} // Years

		// ----------------------------------------------------------------------
		public Years( int year, int count ) :
			this( year, count, new TimeCalendar() )
		{
		} // Years

		// ----------------------------------------------------------------------
		public Years( int year, int count, ITimeCalendar calendar ) :
			base( year, count, calendar )
		{
		} // Years

		// ----------------------------------------------------------------------
		public ITimePeriodCollection GetYears()
		{
			TimePeriodCollection years = new TimePeriodCollection();
			for ( int i = 0; i < YearCount; i++ )
			{
				years.Add( new Year( BaseYear + i, Calendar ) );
			}
			return years;
		} // GetYears

		// ----------------------------------------------------------------------
		protected override string Format( ITimeFormatter formatter )
		{
			return formatter.GetCalendarPeriod( StartYearName, EndYearName,
				formatter.GetShortDate( Start ), formatter.GetShortDate( End ), Duration );
		} // Format

	} // class Years

} // namespace TimePeriod

