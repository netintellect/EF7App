using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public interface ITimePeriod
	{

		// ----------------------------------------------------------------------
		bool HasStart { get; }

		// ----------------------------------------------------------------------
		DateTime Start { get; }

		// ----------------------------------------------------------------------
		bool HasEnd { get; }

		// ----------------------------------------------------------------------
		DateTime End { get; }

		// ----------------------------------------------------------------------
		TimeSpan Duration { get; }

		// ----------------------------------------------------------------------
		string DurationDescription { get; }

		// ----------------------------------------------------------------------
		bool IsMoment { get; }

		// ----------------------------------------------------------------------
		bool IsAnytime { get; }

		// ----------------------------------------------------------------------
		bool IsReadOnly { get; }

		// ----------------------------------------------------------------------
		TimeSpan GetDuration( IDurationProvider provider );

		// ----------------------------------------------------------------------
		void Setup( DateTime newStart, DateTime newEnd );

		// ----------------------------------------------------------------------
		bool IsSamePeriod( ITimePeriod test );

		// ----------------------------------------------------------------------
		bool HasInside( DateTime test );

		// ----------------------------------------------------------------------
		bool HasInside( ITimePeriod test );

		// ----------------------------------------------------------------------
		bool IntersectsWith( ITimePeriod test );

		// ----------------------------------------------------------------------
		bool OverlapsWith( ITimePeriod test );

		// ----------------------------------------------------------------------
		PeriodRelation GetRelation( ITimePeriod test );

		// ----------------------------------------------------------------------
		int CompareTo( ITimePeriod other, ITimePeriodComparer comparer );

		// ----------------------------------------------------------------------
		string GetDescription( ITimeFormatter formatter );

	} // interface ITimePeriod

} // namespace TimePeriod

