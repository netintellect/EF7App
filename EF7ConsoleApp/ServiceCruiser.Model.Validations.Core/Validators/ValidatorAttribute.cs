using System;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | 
                    AttributeTargets.Class | AttributeTargets.Parameter, AllowMultiple = true, Inherited = false)]
    public abstract class ValidatorAttribute : BaseValidationAttribute, IValidatorDescriptor
    {
        Validator IValidatorDescriptor.CreateValidator(Type targetType, Type ownerType,
                                                       MemberValueAccessBuilder memberValueAccessBuilder, ValidatorFactory validatorFactory)
        {
            return CreateValidator(targetType, ownerType, memberValueAccessBuilder, validatorFactory);
        }

        protected Validator CreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder,
                                            ValidatorFactory validatorFactory)
        {
            Validator validator = DoCreateValidator(targetType, ownerType, memberValueAccessBuilder, validatorFactory);
            validator.Tag = Tag;
            validator.MessageTemplate = GetMessageTemplate();

            return validator;
        }

        protected virtual Validator DoCreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder,
                                                      ValidatorFactory validatorFactory)
        {
            return DoCreateValidator(targetType);
        }
        
        protected abstract Validator DoCreateValidator(Type targetType);

    }
}
