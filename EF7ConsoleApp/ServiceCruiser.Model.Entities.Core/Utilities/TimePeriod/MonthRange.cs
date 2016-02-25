using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public struct MonthRange
	{

		// ----------------------------------------------------------------------
		public MonthRange( YearMonth month ) :
			this( month, month )
		{
		} // MonthRange

		// ----------------------------------------------------------------------
		public MonthRange( YearMonth min, YearMonth max )
		{
			if ( max < min )
			{
				throw new ArgumentOutOfRangeException( "max" );
			}
			this.min = min;
			this.max = max;
		} // MonthRange

		// ----------------------------------------------------------------------
		public YearMonth Min
		{
			get { return min; }
		} // Min

		// ----------------------------------------------------------------------
		public YearMonth Max
		{
			get { return max; }
		} // Max

		// ----------------------------------------------------------------------
		public bool IsSingleMonth
		{
			get { return min == max; }
		} // IsSingleMonth

		// ----------------------------------------------------------------------
		public bool HasInside( YearMonth test )
		{
			return test >= min && test <= max;
		} // HasInside

		// ----------------------------------------------------------------------
		// members
		private readonly YearMonth min;
		private readonly YearMonth max;

	} // struct MonthRange

} // namespace TimePeriod

