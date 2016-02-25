using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public class DaySeekerContext : ICalendarVisitorContext
	{

		// ----------------------------------------------------------------------
		public DaySeekerContext( Day startDay, int dayCount )
		{
			if ( startDay == null )
			{
				throw new ArgumentNullException( "startDay" );
			}
			StartDay = startDay;
			DayCount = Math.Abs( dayCount );
			RemaingDays = DayCount;
		} // DaySeekerContext

		// ----------------------------------------------------------------------
		public int DayCount { get; private set; }

		// ----------------------------------------------------------------------
		public int RemaingDays { get; private set; }

		// ----------------------------------------------------------------------
		public Day StartDay { get; private set; }

		// ----------------------------------------------------------------------
		public Day FoundDay { get; private set; }

		// ----------------------------------------------------------------------
		public bool IsFinished
		{
			get { return RemaingDays == 0; }
		} // IsFinished

		// ----------------------------------------------------------------------
		public void ProcessDay( Day day )
		{
			if ( IsFinished )
			{
				return;
			}

			RemaingDays -= 1;
			if ( IsFinished )
			{
				FoundDay = day;
			}
		} // ProcessDay

	} // class DaySeekerContext

} // namespace TimePeriod

