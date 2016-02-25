namespace ServiceCruiser.Model.Validations.Core.Validators
{
	/// <summary>
	/// Represents the logic of how to access values from a source object.
	/// </summary>
	public abstract class ValueAccess
	{
		public abstract bool GetValue(object source, out object value, out string valueAccessFailureMessage);

		/// <summary>
		/// Gets a hint of the location of the value relative to the object where it was retrieved from.
		/// </summary>
		public abstract string Key { get; }
	}
}
