using System;
using System.Reflection;

namespace ServiceCruiser.Model.Validations.Core
{
    public class MemberAccessValidatorBuilderFactory
    {
        internal static readonly MemberAccessValidatorBuilderFactory Default = new MemberAccessValidatorBuilderFactory();

        private readonly MemberValueAccessBuilder _valueAccessBuilder;
        
        public MemberAccessValidatorBuilderFactory() : this(new ReflectionMemberValueAccessBuilder())
        { }
        
        public MemberAccessValidatorBuilderFactory(MemberValueAccessBuilder valueAccessBuilder)
        {
            _valueAccessBuilder = valueAccessBuilder;
        }

        
        public virtual ValueAccessValidatorBuilder GetPropertyValueAccessValidatorBuilder(PropertyInfo propertyInfo, IValidatedElement validatedElement)
        {
            return new ValueAccessValidatorBuilder(_valueAccessBuilder.GetPropertyValueAccess(propertyInfo), validatedElement);
        }
        
        public virtual ValueAccessValidatorBuilder GetFieldValueAccessValidatorBuilder(FieldInfo fieldInfo, IValidatedElement validatedElement)
        {
            return new ValueAccessValidatorBuilder(_valueAccessBuilder.GetFieldValueAccess(fieldInfo), validatedElement);
        }
        
        public virtual ValueAccessValidatorBuilder GetMethodValueAccessValidatorBuilder(MethodInfo methodInfo, IValidatedElement validatedElement)
        {
            return new ValueAccessValidatorBuilder(this._valueAccessBuilder.GetMethodValueAccess(methodInfo), validatedElement);
        }

        public virtual CompositeValidatorBuilder GetTypeValidatorBuilder(Type type, IValidatedElement validatedElement)
        {
            if (null == type)
                throw new ArgumentNullException("type");

            return new CompositeValidatorBuilder(validatedElement);
        }

        /// <summary>
        /// 
        /// </summary>
        public MemberValueAccessBuilder MemberValueAccessBuilder
        {
            get { return _valueAccessBuilder; }
        }
    }
}
