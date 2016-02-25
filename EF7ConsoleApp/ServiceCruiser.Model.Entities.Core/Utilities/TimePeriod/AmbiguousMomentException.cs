using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public class AmbiguousMomentException : Exception
	{

		// ----------------------------------------------------------------------
		public AmbiguousMomentException( DateTime moment )
		{
			this.moment = moment;
		} // AmbiguousMomentException

		// ----------------------------------------------------------------------
		public AmbiguousMomentException( DateTime moment, string message ) :
			base( message )
		{
			this.moment = moment;
		} // AmbiguousMomentException

		// ----------------------------------------------------------------------
		public AmbiguousMomentException( DateTime moment, Exception cause ) :
			base( cause.Message, cause )
		{
			this.moment = moment;
		} // AmbiguousMomentException

		// ----------------------------------------------------------------------
		public AmbiguousMomentException( DateTime moment, string message, Exception cause ) :
			base( message, cause )
		{
			this.moment = moment;
		} // AmbiguousMomentException

		// ----------------------------------------------------------------------
		public DateTime Moment
		{
			get { return moment; }
		} // Moment

		// ----------------------------------------------------------------------
		// members
		private readonly DateTime moment;

	} // class AmbiguousMomentException

} // namespace TimePeriod
