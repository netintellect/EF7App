using System;
using System.Collections.Generic;
using System.Linq;
using ServiceCruiser.Model.Validations.Core.Instrumentation;

namespace ServiceCruiser.Model.Validations.Core
{
    public static class ValidationFactory
    {
        public static void ResetCaches()
        {
            DefaultCompositeValidatorFactory.ResetCache();
        }

        public static Validator<T> CreateValidator<T>()
        {
            return DefaultCompositeValidatorFactory.CreateValidator<T>();
        }
        public static Validator<T> CreateValidator<T>(string ruleset)
        {
            return DefaultCompositeValidatorFactory.CreateValidator<T>(ruleset);
        }
        
        public static Validator CreateValidator(Type targetType)
        {
            return DefaultCompositeValidatorFactory.CreateValidator(targetType);
        }

        public static Validator CreateValidator(Type targetType, string ruleset)
        {
            return DefaultCompositeValidatorFactory.CreateValidator(targetType, ruleset);
        }
        
        public static Validator<T> CreateValidator<T>(ValidationSpecificationSource source)
        {
            return GetValidatorFactory(source).CreateValidator<T>();
        }

        public static Validator<T> CreateValidator<T>(string ruleset, ValidationSpecificationSource source)
        {
            return GetValidatorFactory(source).CreateValidator<T>(ruleset);
        }

        public static Validator CreateValidator(Type targetType, ValidationSpecificationSource source)
        {
            return GetValidatorFactory(source).CreateValidator(targetType);
        }

        public static Validator CreateValidator(Type targetType, string ruleset, ValidationSpecificationSource source)
        {
            return GetValidatorFactory(source).CreateValidator(targetType, ruleset);
        }

        public static Validator<T> CreateValidatorFromAttributes<T>()
        {
            return DefaultAttributeValidatorFactory.CreateValidator<T>();
        }

        public static Validator<T> CreateValidatorFromAttributes<T>(string ruleset)
        {
            return DefaultAttributeValidatorFactory.CreateValidator<T>(ruleset);
        }

        public static Validator CreateValidatorFromAttributes(Type targetType, string ruleset)
        {
            return DefaultAttributeValidatorFactory.CreateValidator(targetType, ruleset);
        }


        private static CompositeValidatorFactory CreateCompositeValidatorFactory(IValidationInstrumentationProvider instrumentationProvider,
                                                                                 IEnumerable<ValidatorFactory> validatorFactories)
        {
            return new CompositeValidatorFactory(instrumentationProvider, validatorFactories.ToArray());
        }

        private static ValidatorFactory GetValidatorFactory(ValidationSpecificationSource source)
        {
            if (source == ValidationSpecificationSource.All || source == ValidationSpecificationSource.Both)
            {
                return DefaultCompositeValidatorFactory;
            }

            var factories = new List<ValidatorFactory>();
            if (source.IsSet(ValidationSpecificationSource.Attributes))
            {
                factories.Add(DefaultAttributeValidatorFactory);
            }
            if (source.IsSet(ValidationSpecificationSource.DataAnnotations))
            {
                factories.Add(DefaultValidationAttributeValidatorFactory);
            }

            if (factories.Count == 1)
            {
                return factories[0];
            }

            return CreateCompositeValidatorFactory(new NullValidationInstrumentationProvider(), 
                                                   factories);
        }


        private static AttributeValidatorFactory DefaultAttributeValidatorFactory
        {
            get
            {
                return new AttributeValidatorFactory(new NullValidationInstrumentationProvider()); 
            }
        }

        
        private static ValidationAttributeValidatorFactory DefaultValidationAttributeValidatorFactory
        {
            get
            {
                return new ValidationAttributeValidatorFactory(new NullValidationInstrumentationProvider());
            }
        }

        /// <summary>
        /// Gets the <see cref="ValidatorFactory"/> to use by default.
        /// </summary>
        public static ValidatorFactory DefaultCompositeValidatorFactory
        {
            get
            {
                return new CompositeValidatorFactory(new NullValidationInstrumentationProvider(), 
                                                     DefaultAttributeValidatorFactory, DefaultValidationAttributeValidatorFactory);
            }
        }
    }
}
