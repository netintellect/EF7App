using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	public interface ITimePeriodChain : ITimePeriodContainer
	{

		// ----------------------------------------------------------------------
		new DateTime Start { get; set; }

		// ----------------------------------------------------------------------
		new DateTime End { get; set; }

		// ----------------------------------------------------------------------
		ITimePeriod First { get; }

		// ----------------------------------------------------------------------
		ITimePeriod Last { get; }

	} // interface ITimePeriodChain

} // namespace TimePeriod

