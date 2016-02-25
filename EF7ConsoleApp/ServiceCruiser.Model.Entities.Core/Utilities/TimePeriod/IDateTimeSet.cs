using System;
using System.Collections.Generic;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public interface IDateTimeSet : ICollection<DateTime>
	{

		// ----------------------------------------------------------------------
		DateTime this[ int index ] { get; }

		// ----------------------------------------------------------------------
		DateTime? Min { get; }

		// ----------------------------------------------------------------------
		DateTime? Max { get; }

		// ----------------------------------------------------------------------
		TimeSpan? Duration { get; }

		// ----------------------------------------------------------------------
		bool IsEmpty { get; }

		// ----------------------------------------------------------------------
		bool IsMoment { get; }

		// ----------------------------------------------------------------------
		bool IsAnytime { get; }

		// ----------------------------------------------------------------------
		int IndexOf( DateTime moment );
		
		// ----------------------------------------------------------------------
		new bool Add( DateTime moment );

		// ----------------------------------------------------------------------
		void AddAll( IEnumerable<DateTime> moments );

		// ----------------------------------------------------------------------
		IList<TimeSpan> GetDurations( int startIndex, int count );

		// ----------------------------------------------------------------------
		DateTime? FindPrevious( DateTime moment );

		// ----------------------------------------------------------------------
		DateTime? FindNext( DateTime moment );

	} // class IDateTimeSet

} // namespace TimePeriod

