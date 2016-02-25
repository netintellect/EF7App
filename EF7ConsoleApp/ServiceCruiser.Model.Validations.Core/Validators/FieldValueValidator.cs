namespace ServiceCruiser.Model.Validations.Core.Validators
{
	public class FieldValueValidator<T> : MemberAccessValidator<T>
	{
		public FieldValueValidator(string fieldName, Validator fieldValueValidator)
			: base(GetFieldValueAccess(fieldName), fieldValueValidator)
		{
		}

		private static ValueAccess GetFieldValueAccess(string fieldName)
		{
			return new FieldValueAccess(ValidationReflectionHelper.GetField(typeof(T), fieldName, true));
		}
	}
}
