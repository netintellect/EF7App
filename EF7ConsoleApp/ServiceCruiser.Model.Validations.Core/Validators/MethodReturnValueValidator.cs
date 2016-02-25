namespace ServiceCruiser.Model.Validations.Core.Validators
{
	public class MethodReturnValueValidator<T> : MemberAccessValidator<T>
	{
		public MethodReturnValueValidator(string methodName, Validator methodValueValidator)
			: base(GetMethodValueAccess(methodName), methodValueValidator)
		{ }

		private static ValueAccess GetMethodValueAccess(string methodName)
		{
			return new MethodValueAccess(ValidationReflectionHelper.GetMethod(typeof(T), methodName, true));
		}
	}
}
