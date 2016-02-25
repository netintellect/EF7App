namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public sealed class TimePeriodStartComparer : ITimePeriodComparer
	{

		// ----------------------------------------------------------------------
		public static ITimePeriodComparer Comparer = new TimePeriodStartComparer();
		public static ITimePeriodComparer ReverseComparer = new TimePeriodReversComparer( new TimePeriodStartComparer() );

		// ----------------------------------------------------------------------
		public int Compare( ITimePeriod left, ITimePeriod right )
		{
			return left.Start.CompareTo( right.Start );
		} // Compare

	} // class TimePeriodStartComparer

} // namespace TimePeriod

