namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public interface ITimeLine
	{

		// ----------------------------------------------------------------------
		ITimePeriodContainer Periods { get; }

		// ----------------------------------------------------------------------
		ITimePeriod Limits { get; }

		// ----------------------------------------------------------------------
		ITimePeriodMapper PeriodMapper { get; }

		// ----------------------------------------------------------------------
		bool HasOverlaps();

		// ----------------------------------------------------------------------
		bool HasGaps();

		// ----------------------------------------------------------------------
		ITimePeriodCollection CombinePeriods();

		// ----------------------------------------------------------------------
		ITimePeriodCollection IntersectPeriods( bool combinePeriods );

		// ----------------------------------------------------------------------
		ITimePeriodCollection CalculateGaps();

	} // interface ITimeLine

} // namespace TimePeriod

