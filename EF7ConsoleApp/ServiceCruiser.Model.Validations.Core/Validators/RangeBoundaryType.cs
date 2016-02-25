﻿namespace ServiceCruiser.Model.Validations.Core.Validators
{
	/// <summary>
	/// Indicates how to interpret a range boundary.
	/// </summary>
	public enum RangeBoundaryType
	{
		/// <summary>
		/// Ignore the range boundary.
		/// </summary>
		Ignore = 0,

		/// <summary>
		/// Allow values equal to the boundary.
		/// </summary>
		Inclusive = 1,

		/// <summary>
		/// Reject values equal to the boundary.
		/// </summary>
		Exclusive = 2
	}
}
