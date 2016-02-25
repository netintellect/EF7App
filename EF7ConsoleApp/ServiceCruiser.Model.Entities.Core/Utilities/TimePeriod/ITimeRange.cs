using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public interface ITimeRange : ITimePeriod
	{

		// ----------------------------------------------------------------------
		new DateTime Start { get; set; }

		// ----------------------------------------------------------------------
		new DateTime End { get; set; }

		// ----------------------------------------------------------------------
		new TimeSpan Duration { get; set; }

		// ----------------------------------------------------------------------
		void Move( TimeSpan offset );

		// ----------------------------------------------------------------------
		void ExpandStartTo( DateTime moment );

		// ----------------------------------------------------------------------
		void ExpandEndTo( DateTime moment );

		// ----------------------------------------------------------------------
		void ExpandTo( DateTime moment );

		// ----------------------------------------------------------------------
		void ExpandTo( ITimePeriod period );

		// ----------------------------------------------------------------------
		void ShrinkStartTo( DateTime moment );

		// ----------------------------------------------------------------------
		void ShrinkEndTo( DateTime moment );

		// ----------------------------------------------------------------------
		void ShrinkTo( ITimePeriod period );

		// ----------------------------------------------------------------------
		ITimeRange Copy( TimeSpan offset );

		// ----------------------------------------------------------------------
		ITimeRange GetIntersection( ITimePeriod period );

	} // interface ITimeRange

} // namespace TimePeriod

