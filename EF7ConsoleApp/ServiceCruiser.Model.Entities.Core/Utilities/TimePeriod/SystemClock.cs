using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public class SystemClock : IClock
	{

		// ----------------------------------------------------------------------
		internal SystemClock()
		{
		} // SystemClock

		// ----------------------------------------------------------------------
		public DateTime Now
		{
			get { return DateTime.Now; }
		} // Now

	} // class SystemClock

} // namespace TimePeriod

