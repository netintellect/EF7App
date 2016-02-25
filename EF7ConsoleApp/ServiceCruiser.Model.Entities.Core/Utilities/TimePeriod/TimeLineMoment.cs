using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public class TimeLineMoment : ITimeLineMoment
	{

		// ----------------------------------------------------------------------
		public TimeLineMoment( DateTime moment )
		{
			this.moment = moment;
		} // TimeLineMoment

		// ----------------------------------------------------------------------
		public DateTime Moment
		{
			get { return moment; }
		} // Moment

		// ----------------------------------------------------------------------
		public int BalanceCount
		{
			get { return startCount - endCount; }
		} // BalanceCount

		// ----------------------------------------------------------------------
		public int StartCount
		{
			get { return startCount; }
		} // StartCount

		// ----------------------------------------------------------------------
		public int EndCount
		{
			get { return endCount; }
		} // EndCount

		// ----------------------------------------------------------------------
		public bool IsEmpty
		{
			get { return startCount == 0 && endCount == 0; }
		} // IsEmpty

		// ----------------------------------------------------------------------
		public void AddStart()
		{
			startCount++;
		} // AddStart

		// ----------------------------------------------------------------------
		public void RemoveStart()
		{
			if ( startCount == 0 )
			{
				throw new InvalidOperationException();
			}
			startCount--;
		} // RemoveStart

		// ----------------------------------------------------------------------
		public void AddEnd()
		{
			endCount++;
		} // AddEnd

		// ----------------------------------------------------------------------
		public void RemoveEnd()
		{
			if ( endCount == 0 )
			{
				throw new InvalidOperationException();
			}
			endCount--;
		} // RemoveEnd

		// ----------------------------------------------------------------------
		public override string ToString()
		{
			return string.Format( "{0} [{1}/{2}]", Moment, StartCount, EndCount );
		} // ToString

		// ----------------------------------------------------------------------
		// members
		private readonly DateTime moment;
		private int startCount;
		private int endCount;

	} // class TimeLineMoment

} // namespace TimePeriod

