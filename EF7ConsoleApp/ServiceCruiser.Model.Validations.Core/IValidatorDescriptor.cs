using System;

namespace ServiceCruiser.Model.Validations.Core
{
    public interface IValidatorDescriptor
    {
        Validator CreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder, ValidatorFactory validatorFactory);
    }
}
