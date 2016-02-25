namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public static class ClockProxy
	{

		// ----------------------------------------------------------------------
		public static IClock Clock
		{
			get
			{
				if ( clock == null )
				{
					lock ( mutex )
					{
						if ( clock == null )
						{
							clock = new SystemClock();
						}
					}
				}
				return clock;
			}
			set
			{
				lock ( mutex )
				{
					clock = value;
				}
			}
		} // Clock

		// ----------------------------------------------------------------------
		// members
		private static readonly object mutex = new object();
		private static volatile IClock clock;

	} // class ClockProxy

} // namespace TimePeriod

