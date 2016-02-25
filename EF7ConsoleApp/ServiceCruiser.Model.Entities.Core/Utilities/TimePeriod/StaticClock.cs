using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public class StaticClock : IClock
	{

		// ----------------------------------------------------------------------
		public StaticClock( DateTime now )
		{
			this.now = now;
		} // StaticClock

		// ----------------------------------------------------------------------
		public DateTime Now
		{
			get { return now; }
		} // Now

		// ----------------------------------------------------------------------
		// members
		private readonly DateTime now;

	} // class StaticClock

} // namespace TimePeriod

