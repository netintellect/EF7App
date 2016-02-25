using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public class TimePeriodIntersector<T> where T : ITimePeriod, new()
	{

		// ----------------------------------------------------------------------
		public TimePeriodIntersector() :
			this( null )
		{
		} // TimePeriodIntersector

		// ----------------------------------------------------------------------
		public TimePeriodIntersector( ITimePeriodMapper periodMapper )
		{
			this.periodMapper = periodMapper;
		} // TimePeriodIntersector

		// ----------------------------------------------------------------------
		public ITimePeriodMapper PeriodMapper
		{
			get { return periodMapper; }
		} // PeriodMapper

		// ----------------------------------------------------------------------
		public virtual ITimePeriodCollection IntersectPeriods( ITimePeriodContainer periods, bool combinePeriods = true )
		{
			if ( periods == null )
			{
				throw new ArgumentNullException( "periods" );
			}
			TimeLine<T> timeLine = new TimeLine<T>( periods, periodMapper );
			return timeLine.IntersectPeriods( combinePeriods );
		} // IntersectPeriods

		// ----------------------------------------------------------------------
		// members
		private readonly ITimePeriodMapper periodMapper;

	} // class TimePeriodIntersector

} // namespace TimePeriod

