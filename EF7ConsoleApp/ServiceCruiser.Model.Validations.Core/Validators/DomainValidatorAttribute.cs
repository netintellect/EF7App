using System;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    /// <summary>
    /// Represents a <see cref="DomainValidatorAttribute"/>.
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter,
        AllowMultiple = true, Inherited = false)]
    public sealed class DomainValidatorAttribute : ValueValidatorAttribute
    {
        private readonly object[] _domain;

        public DomainValidatorAttribute() : this(new object[0])
        {
        }

        public DomainValidatorAttribute(params object[] domain) : base()
        {
            ValidatorArgumentsValidatorHelper.ValidateDomainValidator(domain);

            _domain = domain;
        }

        public DomainValidatorAttribute(object domain1) : this(new[] {domain1})
        {
        }

        public DomainValidatorAttribute(object domain1, object domain2) : this(new[] {domain1, domain2})
        {
        }

        public DomainValidatorAttribute(object domain1, object domain2, object domain3)
            : this(new[] {domain1, domain2, domain3})
        {
        }

        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4)
            : this(new[] {domain1, domain2, domain3, domain4})
        {
        }

        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5)
            : this(new[] {domain1, domain2, domain3, domain4, domain5})
        {
        }

        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5,
            object domain6)
            : this(new[] {domain1, domain2, domain3, domain4, domain5, domain6})
        {
        }

        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5,
            object domain6, object domain7)
            : this(new[] {domain1, domain2, domain3, domain4, domain5, domain6, domain7})
        {
        }

        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5,
            object domain6, object domain7, object domain8)
            : this(new[] {domain1, domain2, domain3, domain4, domain5, domain6, domain7, domain8})
        {
        }

        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5,
            object domain6, object domain7, object domain8, object domain9)
            : this(new[] {domain1, domain2, domain3, domain4, domain5, domain6, domain7, domain8, domain9})
        {
        }

        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5,
            object domain6, object domain7, object domain8, object domain9, object domain10, object domain11)
            : this(
                new[]
                {domain1, domain2, domain3, domain4, domain5, domain6, domain7, domain8, domain9, domain10, domain11})
        {
        }

        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5,
            object domain6, object domain7, object domain8, object domain9, object domain10, object domain11,
            object domain12)
            : this(
                new[]
                {
                    domain1, domain2, domain3, domain4, domain5, domain6, domain7, domain8, domain9, domain10, domain11,
                    domain12
                })
        {
        }

        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5,
            object domain6, object domain7, object domain8, object domain9, object domain10, object domain11,
            object domain12, object domain13)
            : this(
                new[]
                {
                    domain1, domain2, domain3, domain4, domain5, domain6, domain7, domain8, domain9, domain10, domain11,
                    domain12, domain13
                })
        {
        }

        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5,
            object domain6, object domain7, object domain8, object domain9, object domain10, object domain11,
            object domain12, object domain13, object domain14)
            : this(
                new[]
                {
                    domain1, domain2, domain3, domain4, domain5, domain6, domain7, domain8, domain9, domain10, domain11,
                    domain12, domain13, domain14
                })
        {
        }

        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5,
            object domain6, object domain7, object domain8, object domain9, object domain10, object domain11,
            object domain12, object domain13, object domain14, object domain15)
            : this(
                new[]
                {
                    domain1, domain2, domain3, domain4, domain5, domain6, domain7, domain8, domain9, domain10, domain11,
                    domain12, domain13, domain14, domain15
                })
        {
        }


        public DomainValidatorAttribute(object domain1, object domain2, object domain3, object domain4, object domain5,
            object domain6, object domain7, object domain8, object domain9, object domain10, object domain11,
            object domain12, object domain13, object domain14, object domain15, object domain16)
            : this(
                new[]
                {
                    domain1, domain2, domain3, domain4, domain5, domain6, domain7, domain8, domain9, domain10, domain11,
                    domain12, domain13, domain14, domain15, domain16
                })
        {
        }

        /// <summary>
        /// 1st value to be used in the validation
        /// </summary>
        public object Domain1
        {
            get { return GetDomain(1); }
        }

        /// <summary>
        /// 2nd value to be used in the validation
        /// </summary>
        public object Domain2
        {
            get { return GetDomain(2); }
        }

        /// <summary>
        /// 3rd value to be used in the validation
        /// </summary>
        public object Domain3
        {
            get { return GetDomain(3); }
        }

        /// <summary>
        /// 4th value to be used in the validation
        /// </summary>
        public object Domain4
        {
            get { return GetDomain(4); }
        }

        /// <summary>
        /// 5th value to be used in the validation
        /// </summary>
        public object Domain5
        {
            get { return GetDomain(5); }
        }

        /// <summary>
        /// 6th value to be used in the validation
        /// </summary>
        public object Domain6
        {
            get { return GetDomain(6); }
        }

        /// <summary>
        /// 7th value to be used in the validation
        /// </summary>
        public object Domain7
        {
            get { return GetDomain(7); }
        }

        /// <summary>
        /// 8th value to be used in the validation
        /// </summary>
        public object Domain8
        {
            get { return GetDomain(8); }
        }

        /// <summary>
        /// 9th value to be used in the validation
        /// </summary>
        public object Domain9
        {
            get { return GetDomain(9); }
        }

        /// <summary>
        /// 10th value to be used in the validation
        /// </summary>
        public object Domain10
        {
            get { return GetDomain(10); }
        }

        /// <summary>
        /// 11th value to be used in the validation
        /// </summary>
        public object Domain11
        {
            get { return GetDomain(11); }
        }

        /// <summary>
        /// 12th value to be used in the validation
        /// </summary>
        public object Domain12
        {
            get { return GetDomain(12); }
        }

        /// <summary>
        /// 13th value to be used in the validation
        /// </summary>
        public object Domain13
        {
            get { return GetDomain(13); }
        }

        /// <summary>
        /// 14th value to be used in the validation
        /// </summary>
        public object Domain14
        {
            get { return GetDomain(14); }
        }

        /// <summary>
        /// 15th value to be used in the validation
        /// </summary>
        public object Domain15
        {
            get { return GetDomain(15); }
        }

        /// <summary>
        /// 16th value to be used in the validation
        /// </summary>
        public object Domain16
        {
            get { return GetDomain(16); }
        }

        /// <summary>
        /// Return the domain object corresponding to the specified index.
        /// </summary>
        /// <param name="index">The index of the domain object (1 based).</param>
        private object GetDomain(int index)
        {
            if (index < 1)
            {
                throw new InvalidOperationException();
            }

            return _domain.Length >= index ? _domain[index - 1] : null;
        }

        /// <summary>
        /// List of values to be used in the validation.
        /// </summary>
        private object[] Domain
        {
            get { return _domain; }
        }

        protected override Validator DoCreateValidator(Type targetType)
        {
            return new DomainValidator<object>(Negated, Domain);
        }

        private readonly Guid typeId = Guid.NewGuid();

        /// <summary>
        /// Gets a unique identifier for this attribute.
        /// </summary>
        public object TypeId
        {
            get { return this.typeId; }
        }
    }
}
