using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{
    // ------------------------------------------------------------------------
	public class DurationProvider : IDurationProvider
	{

		// ----------------------------------------------------------------------
		public virtual TimeSpan GetDuration( DateTime start, DateTime end )
		{
			return end.Subtract( start );
		} // GetDuration

	} // class DurationProvider

} // namespace TimePeriod

