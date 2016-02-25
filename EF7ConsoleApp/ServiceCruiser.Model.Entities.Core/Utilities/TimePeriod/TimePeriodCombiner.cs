using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public class TimePeriodCombiner<T> where T : ITimePeriod, new()
	{

		// ----------------------------------------------------------------------
		public TimePeriodCombiner() :
			this( null )
		{
		} // TimePeriodCombiner

		// ----------------------------------------------------------------------
		public TimePeriodCombiner( ITimePeriodMapper periodMapper )
		{
			this.periodMapper = periodMapper;
		} // TimePeriodCombiner

		// ----------------------------------------------------------------------
		public ITimePeriodMapper PeriodMapper
		{
			get { return periodMapper; }
		} // PeriodMapper

		// ----------------------------------------------------------------------
		public virtual ITimePeriodCollection CombinePeriods( ITimePeriodContainer periods )
		{
			if ( periods == null )
			{
				throw new ArgumentNullException( "periods" );
			}
			TimeLine<T> timeLine = new TimeLine<T>( periods, periodMapper );
			return timeLine.CombinePeriods();
		} // CombinePeriods

		// ----------------------------------------------------------------------
		// members
		private readonly ITimePeriodMapper periodMapper;

	} // class TimePeriodCombiner

} // namespace TimePeriod

