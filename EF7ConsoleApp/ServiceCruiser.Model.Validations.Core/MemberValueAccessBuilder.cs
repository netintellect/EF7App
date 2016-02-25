using System;
using System.Reflection;
using ServiceCruiser.Model.Validations.Core.Resources;
using ServiceCruiser.Model.Validations.Core.Validators;

namespace ServiceCruiser.Model.Validations.Core
{
	public abstract class MemberValueAccessBuilder
	{
		public ValueAccess GetFieldValueAccess(FieldInfo fieldInfo)
		{
			if (null == fieldInfo)
				throw new ArgumentNullException("fieldInfo");

			return DoGetFieldValueAccess(fieldInfo);
		}

		protected abstract ValueAccess DoGetFieldValueAccess(FieldInfo fieldInfo);

		public ValueAccess GetMethodValueAccess(MethodInfo methodInfo)
		{
			if (null == methodInfo)
				throw new ArgumentNullException("methodInfo");
			if (typeof(void) == methodInfo.ReturnType)
				throw new ArgumentException(Translations.ExceptionMethodHasNoReturnValue, "methodInfo");
			if (0 < methodInfo.GetParameters().Length)
				throw new ArgumentException(Translations.ExceptionMethodHasParameters, "methodInfo");

			return DoGetMethodValueAccess(methodInfo);
		}

		protected abstract ValueAccess DoGetMethodValueAccess(MethodInfo methodInfo);

		public ValueAccess GetPropertyValueAccess(PropertyInfo propertyInfo)
		{
			if (null == propertyInfo)
				throw new ArgumentNullException("propertyInfo");

			return DoGetPropertyValueAccess(propertyInfo);
		}

		protected abstract ValueAccess DoGetPropertyValueAccess(PropertyInfo propertyInfo);
	}
}
