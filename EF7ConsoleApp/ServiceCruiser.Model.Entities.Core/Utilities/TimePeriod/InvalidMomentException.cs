using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public class InvalidMomentException : Exception
	{

		// ----------------------------------------------------------------------
		public InvalidMomentException( DateTime moment )
		{
			this.moment = moment;
		} // InvalidMomentException

		// ----------------------------------------------------------------------
		public InvalidMomentException( DateTime moment, string message ) :
			base( message )
		{
			this.moment = moment;
		} // InvalidMomentException

		// ----------------------------------------------------------------------
		public InvalidMomentException( DateTime moment, Exception cause ) :
			base( cause.Message, cause )
		{
			this.moment = moment;
		} // InvalidMomentException

		// ----------------------------------------------------------------------
		public InvalidMomentException( DateTime moment, string message, Exception cause ) :
			base( message, cause )
		{
			this.moment = moment;
		} // InvalidMomentException


		// ----------------------------------------------------------------------
		public DateTime Moment
		{
			get { return moment; }
		} // Moment

		// ----------------------------------------------------------------------
		// members
		private readonly DateTime moment;

	} // class InvalidMomentException

} // namespace TimePeriod

