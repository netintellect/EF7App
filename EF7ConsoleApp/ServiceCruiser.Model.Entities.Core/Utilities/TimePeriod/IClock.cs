using System;

namespace ServiceCruiser.Model.Entities.Core.Utilities.TimePeriod
{

	// ------------------------------------------------------------------------
	// see http://stackoverflow.com/questions/43711/whats-a-good-way-to-overwrite-datetime-now-during-testing
	public interface IClock
	{

		// ----------------------------------------------------------------------
		DateTime Now { get; }

	} // interface IClock

} // namespace TimePeriod

