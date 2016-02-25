using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public interface ITimeBlock : ITimePeriod
	{

		// ----------------------------------------------------------------------
		new DateTime Start { get; set; }

		// ----------------------------------------------------------------------
		new DateTime End { get; set; }

		// ----------------------------------------------------------------------
		new TimeSpan Duration { get; set; }

		// ----------------------------------------------------------------------
		void Setup( DateTime newStart, TimeSpan newDuration );

		// ----------------------------------------------------------------------
		void Move( TimeSpan delta );

		// ----------------------------------------------------------------------
		void DurationFromStart( TimeSpan newDuration );

		// ----------------------------------------------------------------------
		void DurationFromEnd( TimeSpan newDuration );

		// ----------------------------------------------------------------------
		ITimeBlock Copy( TimeSpan delta );

		// ----------------------------------------------------------------------
		ITimeBlock GetPreviousPeriod( TimeSpan offset );

		// ----------------------------------------------------------------------
		ITimeBlock GetNextPeriod( TimeSpan offset );

		// ----------------------------------------------------------------------
		ITimeBlock GetIntersection( ITimePeriod period );

	} // class ITimeBlock

} // namespace TimePeriod

