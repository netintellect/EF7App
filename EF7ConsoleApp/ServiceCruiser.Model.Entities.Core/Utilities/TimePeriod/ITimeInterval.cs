using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public interface ITimeInterval : ITimePeriod
	{

		// ----------------------------------------------------------------------
		bool IsStartOpen { get; }

		// ----------------------------------------------------------------------
		bool IsEndOpen { get; }

		// ----------------------------------------------------------------------
		bool IsOpen { get; }

		// ----------------------------------------------------------------------
		bool IsStartClosed { get; }

		// ----------------------------------------------------------------------
		bool IsEndClosed { get; }

		// ----------------------------------------------------------------------
		bool IsClosed { get; }

		// ----------------------------------------------------------------------
		bool IsEmpty { get; }

		// ----------------------------------------------------------------------
		bool IsDegenerate { get; }

		// ----------------------------------------------------------------------
		bool IsIntervalEnabled { get; }

		// ----------------------------------------------------------------------
		DateTime StartInterval { get; set; }

		// ----------------------------------------------------------------------
		DateTime EndInterval { get; set; }

		// ----------------------------------------------------------------------
		IntervalEdge StartEdge { get; set; }

		// ----------------------------------------------------------------------
		IntervalEdge EndEdge { get; set; }

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
		ITimeInterval Copy( TimeSpan offset );

	} // interface ITimeInterval

} // namespace TimePeriod

