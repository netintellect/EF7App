using System;
using System.Collections.Generic;
using System.Reflection;
using ServiceCruiser.Model.Validations.Core.Validators;

namespace ServiceCruiser.Model.Validations.Core
{
    public interface IValidatedElement
    {
        IEnumerable<IValidatorDescriptor> GetValidatorDescriptors();

        CompositionType CompositionType { get; }

        string CompositionMessageTemplate { get; }

        string CompositionTag { get; }

        bool IgnoreNulls { get; }

        string IgnoreNullsMessageTemplate { get; }

        string IgnoreNullsTag { get; }
        MemberInfo MemberInfo { get; }
        Type TargetType { get; }
    }
}
