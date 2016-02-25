using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public interface ITimePeriodMapper
	{

		// ----------------------------------------------------------------------
		DateTime MapStart( DateTime moment );

		// ----------------------------------------------------------------------
		DateTime MapEnd( DateTime moment );

		// ----------------------------------------------------------------------
		DateTime UnmapStart( DateTime moment );

		// ----------------------------------------------------------------------
		DateTime UnmapEnd( DateTime moment );

	} // interface ITimePeriodMapper

} // namespace TimePeriod

