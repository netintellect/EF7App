using System;
using System.Collections.Generic;
using ServiceCruiser.Model.Validations.Core.Instrumentation;
using ServiceCruiser.Model.Validations.Core.Validators;

namespace ServiceCruiser.Model.Validations.Core
{
    public abstract class ValidatorFactory
    {
        private readonly IValidationInstrumentationProvider _instrumentationProvider;
        private readonly object _validatorCacheLock = new object();
        private readonly IDictionary<ValidatorCacheKey, Validator> _validatorCache = new Dictionary<ValidatorCacheKey, Validator>();
        
        protected ValidatorFactory(IValidationInstrumentationProvider instrumentationProvider)
        {
            _instrumentationProvider = instrumentationProvider;
        }

        protected IValidationInstrumentationProvider InstrumentationProvider
        {
            get { return _instrumentationProvider; }
        }

        protected virtual Validator WrapAndInstrumentValidator(Validator validator, Type type)
        {
            var validatorWrapperType = typeof(GenericValidatorWrapper<>).MakeGenericType(type);
            var validatorWrapper = (Validator)Activator.CreateInstance(validatorWrapperType,
                                                                       new object[] { validator, InstrumentationProvider });

            return validatorWrapper;
        }

        protected virtual Validator<T> WrapAndInstrumentValidator<T>(Validator validator, Type unusedType)
        {
            var validatorWrapper = new GenericValidatorWrapper<T>(validator, InstrumentationProvider);
            return validatorWrapper;
        }

        public Validator<T> CreateValidator<T>()
        {
            return CreateValidator<T>(string.Empty);
        }

        private Validator FindOrCreateValidator(ValidatorCacheKey cacheKey, Func<Validator, Type, Validator> wrapAndInstrument)
        {
            Validator wrapperValidator;

            lock (_validatorCacheLock)
            {
                Validator cachedValidator;
                if (_validatorCache.TryGetValue(cacheKey, out cachedValidator))
                {
                    return cachedValidator;
                }

                Validator validator = InnerCreateValidator(cacheKey.SourceType, cacheKey.Ruleset, this);
                wrapperValidator = wrapAndInstrument(validator, cacheKey.SourceType);

                _validatorCache[cacheKey] = wrapperValidator;
            }

            return wrapperValidator;
        }

        public virtual Validator<T> CreateValidator<T>(string ruleset)
        {
            if (ruleset == null) throw new ArgumentNullException("ruleset");

            return (Validator<T>)FindOrCreateValidator(new ValidatorCacheKey(typeof(T), ruleset, true),
                                                       WrapAndInstrumentValidator<T>);
        }

        public Validator CreateValidator(Type targetType)
        {
            return CreateValidator(targetType, string.Empty);
        }

        public virtual Validator CreateValidator(Type targetType, string ruleset)
        {
            if (targetType == null) throw new ArgumentNullException("targetType");
            if (ruleset == null) throw new ArgumentNullException("ruleset");

            return FindOrCreateValidator(new ValidatorCacheKey(targetType, ruleset, false),
                                         WrapAndInstrumentValidator);
        }

        protected internal abstract Validator InnerCreateValidator(Type targetType, string ruleset, ValidatorFactory mainValidatorFactory);

        ///<summary>
        /// Clears the internal validator cache.
        ///</summary>
        public virtual void ResetCache()
        {
            lock (_validatorCacheLock)
            {
                _validatorCache.Clear();
            }
        }

    }
}
