using System;
using System.Collections.Generic;
using System.Resources;
using ServiceCruiser.Model.Entities.Core.Resources;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public sealed class Week : WeekTimeRange
	{
        private static readonly List<KeyValuePair<DayOfWeek, string>> _weekDays = new List<KeyValuePair<DayOfWeek, string>>();
        public static List<KeyValuePair<DayOfWeek, string>> WeekDays
	    {
            get { return _weekDays; }    
	    }

        private static readonly List<KeyValuePair<DayOfWeek, string>> _workingWeekDays = new List<KeyValuePair<DayOfWeek, string>>();
        public static List<KeyValuePair<DayOfWeek, string>> WorkingWeekDays
        {
            get { return _workingWeekDays; }
        }

        private static readonly List<KeyValuePair<DayOfWeek, string>> _weekendDays = new List<KeyValuePair<DayOfWeek, string>>();
        public static List<KeyValuePair<DayOfWeek, string>> WeekendDays
        {
            get { return _weekendDays; }
        }

        static Week()
        {
            // working work week
            _workingWeekDays.Add(new KeyValuePair<DayOfWeek, string>(DayOfWeek.Monday,
                                 Translations.ResourceManager.GetString(DayOfWeek.Monday.ToString())));
            _workingWeekDays.Add(new KeyValuePair<DayOfWeek, string>(DayOfWeek.Tuesday,
                                 Translations.ResourceManager.GetString(DayOfWeek.Tuesday.ToString())));
            _workingWeekDays.Add(new KeyValuePair<DayOfWeek, string>(DayOfWeek.Wednesday,
                                 Translations.ResourceManager.GetString(DayOfWeek.Wednesday.ToString())));
            _workingWeekDays.Add(new KeyValuePair<DayOfWeek, string>(DayOfWeek.Thursday,
                                 Translations.ResourceManager.GetString(DayOfWeek.Thursday.ToString())));
            _workingWeekDays.Add(new KeyValuePair<DayOfWeek, string>(DayOfWeek.Friday,
                                Translations.ResourceManager.GetString(DayOfWeek.Friday.ToString())));

            // weekend days
            _weekendDays.Add(new KeyValuePair<DayOfWeek, string>(DayOfWeek.Saturday,
                          Translations.ResourceManager.GetString(DayOfWeek.Saturday.ToString())));
            _weekendDays.Add(new KeyValuePair<DayOfWeek, string>(DayOfWeek.Sunday,
                          Translations.ResourceManager.GetString(DayOfWeek.Sunday.ToString())));
            
            // week days
            WeekDays.AddRange(_workingWeekDays);
            WeekDays.AddRange(_weekendDays);
	    }

	    public static Week Current
	    {
            get { return new Week(DateTime.Today);}
	    }

		// ----------------------------------------------------------------------
		public Week() :
			this( new TimeCalendar() )
		{
		} // Week

		// ----------------------------------------------------------------------
		public Week( ITimeCalendar calendar ) :
			this( ClockProxy.Clock.Now, calendar )
		{
		} // Week

		// ----------------------------------------------------------------------
		public Week( DateTime moment ) :
			this( moment, new TimeCalendar() )
		{
		} // Week

		// ----------------------------------------------------------------------
		public Week( DateTime moment, ITimeCalendar calendar ) :
			base( moment, 1, calendar )
		{
		} // Week

		// ----------------------------------------------------------------------
		public Week( int year, int weekOfYear ) :
			this( year, weekOfYear, new TimeCalendar() )
		{
		} // Week

		// ----------------------------------------------------------------------
		public Week( int year, int weekOfYear, ITimeCalendar calendar ) :
			base( year, weekOfYear, 1, calendar )
		{
		} // Week

		// ----------------------------------------------------------------------
		public int WeekOfYear
		{
			get { return StartWeek; }
		} // WeekOfYear

		// ----------------------------------------------------------------------
		public string WeekOfYearName
		{
			get { return StartWeekOfYearName; }
		} // WeekOfYearName

		// ----------------------------------------------------------------------
		public DateTime FirstDayOfWeek
		{
			get { return Start; }
		} // FirstDayOfWeek

		// ----------------------------------------------------------------------
		public DateTime LastDayOfWeek
		{
			get { return FirstDayOfWeek.AddDays( TimeSpec.DaysPerWeek - 1 ); }
		} // LastDayOfWeek

		// ----------------------------------------------------------------------
		public bool MultipleCalendarYears
		{
			get { return FirstDayOfWeek.Year != LastDayOfWeek.Year; }
		} // IsCalendarHalfyear

		// ----------------------------------------------------------------------
		public Week GetPreviousWeek()
		{
			return AddWeeks( -1 );
		} // GetPreviousWeek

		// ----------------------------------------------------------------------
		public Week GetNextWeek()
		{
			return AddWeeks( 1 );
		} // GetNextWeek

		// ----------------------------------------------------------------------
		public Week AddWeeks( int weeks )
		{
			DateTime startDate = TimeTool.GetStartOfYearWeek( Year, StartWeek, Calendar.Culture, Calendar.YearWeekType );
			return new Week( startDate.AddDays( weeks * TimeSpec.DaysPerWeek ), Calendar );
		} // AddWeeks

		// ----------------------------------------------------------------------
		protected override string Format( ITimeFormatter formatter )
		{
			return formatter.GetCalendarPeriod( WeekOfYearName, 
				formatter.GetShortDate( Start ), formatter.GetShortDate( End ), Duration );
		} // Format



	} // class Week

} // namespace TimePeriod

