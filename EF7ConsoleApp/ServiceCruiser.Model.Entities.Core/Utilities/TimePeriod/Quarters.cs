using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public sealed class Quarters : QuarterTimeRange
	{

		// ----------------------------------------------------------------------
		public Quarters( DateTime moment, YearQuarter startYearQuarter, int count ) :
			this( moment, startYearQuarter, count, new TimeCalendar() )
		{
		} // Quarters

		// ----------------------------------------------------------------------
		public Quarters( DateTime moment, YearQuarter startYearQuarter, int count, ITimeCalendar calendar ) :
			this( TimeTool.GetYearOf( calendar.YearBaseMonth, calendar.GetYear( moment ), calendar.GetMonth( moment ) ),
			startYearQuarter, count, calendar )
		{
		} // Quarters

		// ----------------------------------------------------------------------
		public Quarters( int startYear, YearQuarter startYearQuarter, int quarterCount ) :
			this( startYear, startYearQuarter, quarterCount, new TimeCalendar() )
		{
		} // Quarters

		// ----------------------------------------------------------------------
		public Quarters( int startYear, YearQuarter startYearQuarter, int quarterCount, ITimeCalendar calendar ) :
			base( startYear, startYearQuarter, quarterCount, calendar )
		{
		} // Quarters

		// ----------------------------------------------------------------------
		public ITimePeriodCollection GetQuarters()
		{
			TimePeriodCollection quarters = new TimePeriodCollection();
			for ( int i = 0; i < QuarterCount; i++ )
			{
				int year;
				YearQuarter quarter;
				TimeTool.AddQuarter( BaseYear, StartQuarter, i, out year, out quarter );
				quarters.Add( new Quarter( year, quarter, Calendar ) );
			}
			return quarters;
		} // GetQuarters

		// ----------------------------------------------------------------------
		protected override string Format( ITimeFormatter formatter )
		{
			return formatter.GetCalendarPeriod( StartQuarterOfYearName, EndQuarterOfYearName,
				formatter.GetShortDate( Start ), formatter.GetShortDate( End ), Duration );
		} // Format

	} // class Quarters

} // namespace TimePeriod

