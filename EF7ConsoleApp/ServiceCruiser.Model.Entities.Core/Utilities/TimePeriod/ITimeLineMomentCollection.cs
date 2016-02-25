using System;
using System.Collections.Generic;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public interface ITimeLineMomentCollection : IEnumerable<ITimeLineMoment>
	{

		// ----------------------------------------------------------------------
		int Count { get; }

		// ----------------------------------------------------------------------
		bool IsEmpty { get; }

		// ----------------------------------------------------------------------
		ITimeLineMoment Min { get; }

		// ----------------------------------------------------------------------
		ITimeLineMoment Max { get; }

		// ----------------------------------------------------------------------
		ITimeLineMoment this[ int index ] { get; }

		// ----------------------------------------------------------------------
		ITimeLineMoment this[ DateTime moment ] { get; }

		// ----------------------------------------------------------------------
		void Add( ITimePeriod period );

		// ----------------------------------------------------------------------
		void AddAll( IEnumerable<ITimePeriod> periods );

		// ----------------------------------------------------------------------
		void Remove( ITimePeriod period );

		// ----------------------------------------------------------------------
		ITimeLineMoment Find( DateTime moment );

		// ----------------------------------------------------------------------
		bool Contains( DateTime moment );

		// ----------------------------------------------------------------------
		bool HasOverlaps();

		// ----------------------------------------------------------------------
		bool HasGaps();

	} // interface ITimeLineMomentCollection

} // namespace TimePeriod

