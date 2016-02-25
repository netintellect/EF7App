using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public class TimeGapCalculator<T> where T : ITimePeriod, new()
	{

		// ----------------------------------------------------------------------
		public TimeGapCalculator() :
			this( null )
		{
		} // TimeGapCalculator

		// ----------------------------------------------------------------------
		public TimeGapCalculator( ITimePeriodMapper periodMapper )
		{
			this.periodMapper = periodMapper;
		} // TimeGapCalculator

		// ----------------------------------------------------------------------
		public ITimePeriodMapper PeriodMapper
		{
			get { return periodMapper; }
		} // PeriodMapper

		// ----------------------------------------------------------------------
		public virtual ITimePeriodCollection GetGaps( ITimePeriodContainer periods, ITimePeriod limits = null ) 
		{
			if ( periods == null )
			{
				throw new ArgumentNullException( "periods" );
			}
			TimeLine<T> timeLine = new TimeLine<T>( periods, limits, periodMapper );
			return timeLine.CalculateGaps();
		} // GetGaps

		// ----------------------------------------------------------------------
		// members
		private readonly ITimePeriodMapper periodMapper;

	} // class TimeGapCalculator

} // namespace TimePeriod

