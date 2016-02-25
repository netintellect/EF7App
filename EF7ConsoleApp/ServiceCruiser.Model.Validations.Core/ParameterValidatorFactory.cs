using System.Reflection;

namespace ServiceCruiser.Model.Validations.Core
{
    public static class ParameterValidatorFactory
    {
        public static Validator CreateValidator(ParameterInfo paramInfo)
        {
            var parameterElement = new MetadataValidatedParameterElement();
            parameterElement.UpdateFlyweight(paramInfo);
            var compositeBuilder = new CompositeValidatorBuilder(parameterElement);
            foreach (IValidatorDescriptor descriptor in parameterElement.GetValidatorDescriptors())
            {
                compositeBuilder.AddValueValidator(descriptor.CreateValidator(paramInfo.ParameterType,
                                                                              null, null,
                                                                              ValidationFactory.DefaultCompositeValidatorFactory));
            }
            return compositeBuilder.GetValidator();
        }
    }
}
