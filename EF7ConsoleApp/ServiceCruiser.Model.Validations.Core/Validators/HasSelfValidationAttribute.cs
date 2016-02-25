using System;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
	/// <summary>
	/// Indicates the target type defines self validation methods.
	/// </summary>
	/// <remarks>
	/// Types without this attribute will not be scanned for self validation methods.
	/// </remarks>
	/// <seealso cref="SelfValidationValidator"/>
	/// <seealso cref="SelfValidationAttribute"/>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class HasSelfValidationAttribute : Attribute
	{ }
}
