using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public sealed class Month : MonthTimeRange
	{

		// ----------------------------------------------------------------------
		public Month() :
			this( new TimeCalendar() )
		{
		} // Month

		// ----------------------------------------------------------------------
		public Month( ITimeCalendar calendar ) :
			this( ClockProxy.Clock.Now, calendar )
		{
		} // Month

		// ----------------------------------------------------------------------
		public Month( DateTime moment ) :
			this( moment, new TimeCalendar() )
		{
		} // Month

		// ----------------------------------------------------------------------
		public Month( DateTime moment, ITimeCalendar calendar ) :
			this( calendar.GetYear( moment ), (YearMonth)calendar.GetMonth( moment ), calendar )
		{
		} // Month

		// ----------------------------------------------------------------------
		public Month( int year, YearMonth yearMonth ) :
			this( year, yearMonth, new TimeCalendar() )
		{
		} // Month

		// ----------------------------------------------------------------------
		public Month( int year, YearMonth yearMonth, ITimeCalendar calendar ) :
			base( year, yearMonth, 1, calendar )
		{
		} // Month

		// ----------------------------------------------------------------------
		public int Year
		{
			get { return StartYear; }
		} // Year

		// ----------------------------------------------------------------------
		public YearMonth YearMonth
		{
			get { return StartMonth; }
		} // YearMonth

		// ----------------------------------------------------------------------
		public int MonthValue
		{
			get { return (int)StartMonth; }
		} // MonthValue

		// ----------------------------------------------------------------------
		public string MonthName
		{
			get { return StartMonthName; }
		} // MonthName

		// ----------------------------------------------------------------------
		public string MonthOfYearName
		{
			get { return StartMonthOfYearName; }
		} // MonthOfYearName

		// ----------------------------------------------------------------------
		public int DaysInMonth
		{
			get { return TimeTool.GetDaysInMonth( StartYear, (int)StartMonth ); }
		} // DaysInMonth

		// ----------------------------------------------------------------------
		public Month GetPreviousMonth()
		{
			return AddMonths( -1 );
		} // GetPreviousMonth

		// ----------------------------------------------------------------------
		public Month GetNextMonth()
		{
			return AddMonths( 1 );
		} // GetNextMonth

		// ----------------------------------------------------------------------
		public Month AddMonths( int months )
		{
			DateTime startDate = new DateTime( StartYear, (int)StartMonth, 1 );
			return new Month( startDate.AddMonths( months ), Calendar );
		} // AddMonths

		// ----------------------------------------------------------------------
		protected override string Format( ITimeFormatter formatter )
		{
			return formatter.GetCalendarPeriod( MonthOfYearName, 
				formatter.GetShortDate( Start ), formatter.GetShortDate( End ), Duration );
		} // Format

	} // class Month

} // namespace TimePeriod

