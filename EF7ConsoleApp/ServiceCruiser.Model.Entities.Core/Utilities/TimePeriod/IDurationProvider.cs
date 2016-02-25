using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public interface IDurationProvider
	{

		// ----------------------------------------------------------------------
		TimeSpan GetDuration( DateTime start, DateTime end );

	} // interface IDurationProvider

} // namespace TimePeriod

