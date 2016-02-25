using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public sealed class Months : MonthTimeRange
	{

		// ----------------------------------------------------------------------
		public Months( DateTime moment, YearMonth startMonth, int count ) :
			this( moment, startMonth, count, new TimeCalendar() )
		{
		} // Months

		// ----------------------------------------------------------------------
		public Months( DateTime moment, YearMonth startMonth, int count, ITimeCalendar calendar ) :
			this( calendar.GetYear( moment ), startMonth, count, calendar )
		{
		} // Months

		// ----------------------------------------------------------------------
		public Months( int startYear, YearMonth startMonth, int monthCounth ) :
			this( startYear, startMonth, monthCounth, new TimeCalendar() )
		{
		} // Months

		// ----------------------------------------------------------------------
		public Months( int startYear, YearMonth startMonth, int monthCount, ITimeCalendar calendar ) :
			base( startYear, startMonth, monthCount, calendar )
		{
		} // Months

		// ----------------------------------------------------------------------
		public ITimePeriodCollection GetMonths()
		{
			TimePeriodCollection months = new TimePeriodCollection();
			for ( int i = 0; i < MonthCount; i++ )
			{
				int year;
				YearMonth month;
				TimeTool.AddMonth( StartYear, StartMonth, i, out year, out month );
				months.Add( new Month( year, month, Calendar ) );
			}
			return months;
		} // GetMonths

		// ----------------------------------------------------------------------
		protected override string Format( ITimeFormatter formatter )
		{
			return formatter.GetCalendarPeriod( StartMonthOfYearName, EndMonthOfYearName, 
				formatter.GetShortDate( Start ), formatter.GetShortDate( End ), Duration );
		} // Format

	} // class Months

} // namespace TimePeriod

