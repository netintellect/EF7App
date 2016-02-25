using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ServiceCruiser.Model.Validations.Core.Resources;
using ServiceCruiser.Model.Validations.Core.Validators;

namespace ServiceCruiser.Model.Validations.Core
{
    /// <summary>
    /// Factory for creating <see cref="Validator"/> objects for properties.
    /// </summary>
    /// <seealso cref="Validator"/>
    public static class PropertyValidationFactory
    {
        private static readonly IDictionary<PropertyValidatorCacheKey, Validator> AttributeOnlyPropertyValidatorsCache = new Dictionary<PropertyValidatorCacheKey, Validator>();
        private static readonly IDictionary<PropertyValidatorCacheKey, Validator> DefaultConfigurationOnlyPropertyValidatorsCache = new Dictionary<PropertyValidatorCacheKey, Validator>();
        private static readonly IDictionary<PropertyValidatorCacheKey, Validator> ValidationAttributeOnlyPropertyValidatorsCache = new Dictionary<PropertyValidatorCacheKey, Validator>();

        public static void ResetCaches()
        {
            lock (AttributeOnlyPropertyValidatorsCache)
            {
                AttributeOnlyPropertyValidatorsCache.Clear();
            }
            lock (DefaultConfigurationOnlyPropertyValidatorsCache)
            {
                DefaultConfigurationOnlyPropertyValidatorsCache.Clear();
            }
            lock (ValidationAttributeOnlyPropertyValidatorsCache)
            {
                ValidationAttributeOnlyPropertyValidatorsCache.Clear();
            }
        }

        public static Validator GetPropertyValidator(Type type, PropertyInfo propertyInfo, string ruleset,
                                                     ValidationSpecificationSource validationSpecificationSource,
                                                     MemberValueAccessBuilder memberValueAccessBuilder)
        {
            return GetPropertyValidator(type, propertyInfo, ruleset, validationSpecificationSource,
                                        new MemberAccessValidatorBuilderFactory(memberValueAccessBuilder));
        }

        
        public static Validator GetPropertyValidator(Type type, PropertyInfo propertyInfo, string ruleset,
                                                     ValidationSpecificationSource validationSpecificationSource,
                                                     MemberAccessValidatorBuilderFactory memberAccessValidatorBuilderFactory)
        {
            if (null == type)
                throw new InvalidOperationException(Translations.ExceptionTypeNotFound);
            if (null == propertyInfo)
                throw new InvalidOperationException(Translations.ExceptionPropertyNotFound);
            if (!propertyInfo.CanRead)
                throw new InvalidOperationException(Translations.ExceptionPropertyNotReadable);

            var validators = new List<Validator>();
            if (validationSpecificationSource.IsSet(ValidationSpecificationSource.Attributes))
            {
                validators.Add(GetPropertyValidatorFromAttributes(type, propertyInfo, ruleset, memberAccessValidatorBuilderFactory));
            }
            if (validationSpecificationSource.IsSet(ValidationSpecificationSource.DataAnnotations))
            {
                validators.Add(GetPropertyValidatorFromValidationAttributes(type, propertyInfo, ruleset, memberAccessValidatorBuilderFactory));
            }

            var effectiveValidators = validators.Where(v => v != null).ToArray();
            if (effectiveValidators.Length == 1)
            {
                return effectiveValidators[0];
            }
            if (effectiveValidators.Length > 1)
            {
                return new AndCompositeValidator(effectiveValidators);
            }
            
            return null;
        }

        public static Validator GetPropertyValidatorFromAttributes(Type type, PropertyInfo propertyInfo, string ruleset,
                                                                   MemberAccessValidatorBuilderFactory memberAccessValidatorBuilderFactory)
        {
            Validator validator = null;

            lock (AttributeOnlyPropertyValidatorsCache)
            {
                var key = new PropertyValidatorCacheKey(type, propertyInfo.Name, ruleset);
                if (!AttributeOnlyPropertyValidatorsCache.TryGetValue(key, out validator))
                {
                    validator = new MetadataValidatorBuilder(memberAccessValidatorBuilderFactory, ValidationFactory.DefaultCompositeValidatorFactory).CreateValidatorForProperty(propertyInfo, ruleset);

                    AttributeOnlyPropertyValidatorsCache[key] = validator;
                }
            }

            return validator;
        }
        
        
        public static Validator GetPropertyValidatorFromValidationAttributes(Type type, PropertyInfo propertyInfo, string ruleset, 
                                                                             MemberAccessValidatorBuilderFactory memberAccessValidatorBuilderFactory)
        {
            Validator validator;

            lock (ValidationAttributeOnlyPropertyValidatorsCache)
            {
                var key = new PropertyValidatorCacheKey(type, propertyInfo.Name, ruleset);
                if (!ValidationAttributeOnlyPropertyValidatorsCache.TryGetValue(key, out validator))
                {
                    validator =
                        string.IsNullOrEmpty(ruleset)
                            ? new ValidationAttributeValidatorBuilder(memberAccessValidatorBuilderFactory, ValidationFactory.DefaultCompositeValidatorFactory).CreateValidatorForProperty(propertyInfo)
                            : new AndCompositeValidator();

                    ValidationAttributeOnlyPropertyValidatorsCache[key] = validator;
                }
            }

            return validator;
        }

        
        private struct PropertyValidatorCacheKey : IEquatable<PropertyValidatorCacheKey>
        {
            private readonly Type _sourceType;
            private readonly string _propertyName;
            private readonly string _ruleset;

            public PropertyValidatorCacheKey(Type sourceType, string propertyName, string ruleset)
            {
                _sourceType = sourceType;
                _propertyName = propertyName;
                _ruleset = ruleset;
            }

            public override int GetHashCode()
            {
                return _sourceType.GetHashCode()
                    ^ _propertyName.GetHashCode()
                    ^ (_ruleset != null ? this._ruleset.GetHashCode() : 0);
            }

            #region IEquatable<PropertyValidatorCacheKey> Members

            bool IEquatable<PropertyValidatorCacheKey>.Equals(PropertyValidatorCacheKey other)
            {
                return (_sourceType == other._sourceType)
                    && (_propertyName.Equals(other._propertyName))
                    && (_ruleset == null ? other._ruleset == null : _ruleset.Equals(other._ruleset));
            }

            #endregion
        }
    }
}
