namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public sealed class TimePeriodReversComparer : ITimePeriodComparer
	{

		// ----------------------------------------------------------------------
		public TimePeriodReversComparer( ITimePeriodComparer baseComparer )
		{
			this.baseComparer = baseComparer;
		} // TimePeriodReversComparer

		// ----------------------------------------------------------------------
		public ITimePeriodComparer BaseComparer
		{
			get { return baseComparer; }
		} // BaseComparer

		// ----------------------------------------------------------------------
		public int Compare( ITimePeriod left, ITimePeriod right )
		{
			return -baseComparer.Compare( left, right );
		} // Compare

		// ----------------------------------------------------------------------
		// members
		private readonly ITimePeriodComparer baseComparer;

	} // class TimePeriodReversComparer

} // namespace TimePeriod

