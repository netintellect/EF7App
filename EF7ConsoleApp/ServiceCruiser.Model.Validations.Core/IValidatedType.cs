using System.Collections.Generic;
using System.Reflection;

namespace ServiceCruiser.Model.Validations.Core
{
    public interface IValidatedType : IValidatedElement
    {
        IEnumerable<IValidatedElement> GetValidatedProperties();

        IEnumerable<IValidatedElement> GetValidatedFields();

        IEnumerable<IValidatedElement> GetValidatedMethods();

        IEnumerable<MethodInfo> GetSelfValidationMethods();
    }
}
