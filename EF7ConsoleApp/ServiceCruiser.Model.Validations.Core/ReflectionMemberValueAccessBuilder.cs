using System.Reflection;
using ServiceCruiser.Model.Validations.Core.Validators;

namespace ServiceCruiser.Model.Validations.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class ReflectionMemberValueAccessBuilder : MemberValueAccessBuilder
    {
        protected override ValueAccess DoGetFieldValueAccess(FieldInfo fieldInfo)
        {
            return new FieldValueAccess(fieldInfo);
        }

        protected override ValueAccess DoGetMethodValueAccess(MethodInfo methodInfo)
        {
            return new MethodValueAccess(methodInfo);
        }
        
        protected override ValueAccess DoGetPropertyValueAccess(PropertyInfo propertyInfo)
        {
            return new PropertyValueAccess(propertyInfo);
        }
    }
}
