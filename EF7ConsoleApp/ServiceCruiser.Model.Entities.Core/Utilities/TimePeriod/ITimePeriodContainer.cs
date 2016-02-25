using System;
using System.Collections.Generic;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public interface ITimePeriodContainer : IList<ITimePeriod>, ITimePeriod
	{

		// ----------------------------------------------------------------------
		new bool IsReadOnly { get; }

		// ----------------------------------------------------------------------
		bool ContainsPeriod( ITimePeriod test );

		// ----------------------------------------------------------------------
		void AddAll( IEnumerable<ITimePeriod> periods );

		// ----------------------------------------------------------------------
		void Move( TimeSpan delta );

	} // class ITimePeriodContainer

} // namespace TimePeriod

