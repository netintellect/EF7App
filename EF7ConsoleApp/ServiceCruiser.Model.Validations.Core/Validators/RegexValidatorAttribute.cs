using System;
using System.Text.RegularExpressions;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    /// <summary>
    /// Represents a <see cref="RegexValidator"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter,
                    AllowMultiple = true, Inherited = false)]
    public sealed class RegexValidatorAttribute : ValueValidatorAttribute
    {
        private readonly string _pattern;
        private readonly RegexOptions _options;
        private readonly string _patternResourceName;
        private readonly Type _patternResourceType;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RegexValidatorAttribute"/> class with a regex pattern.</para>
        /// </summary>
        /// <param name="pattern">The pattern to match.</param>
        public RegexValidatorAttribute(string pattern)
            : this(pattern, RegexOptions.None)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RegexValidatorAttribute"/> class with a regex pattern.</para>
        /// </summary>
        /// <param name="patternResourceName">The resource name containing the pattern for the regular expression.</param>
        /// <param name="patternResourceType">The type containing the resource for the regular expression.</param>
        public RegexValidatorAttribute(string patternResourceName, Type patternResourceType)
            : this(patternResourceName, patternResourceType, RegexOptions.None)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RegexValidatorAttribute"/> class with a regex pattern and 
        /// matching options.</para>
        /// </summary>
        /// <param name="pattern">The pattern to match.</param>
        /// <param name="options">The <see cref="RegexOptions"/> to use when matching.</param>
        public RegexValidatorAttribute(string pattern, RegexOptions options)
            : this(pattern, null, null, options)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RegexValidatorAttribute"/> class with a regex pattern.</para>
        /// </summary>
        /// <param name="patternResourceName">The resource name containing the pattern for the regular expression.</param>
        /// <param name="patternResourceType">The type containing the resource for the regular expression.</param>
        /// <param name="options">The <see cref="RegexOptions"/> to use when matching.</param>
        public RegexValidatorAttribute(string patternResourceName, Type patternResourceType, RegexOptions options)
            : this(null, patternResourceName, patternResourceType, options)
        { }

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="RegexValidatorAttribute"/> class with a regex pattern, 
        /// matching options and a failure message template.</para>
        /// </summary>
        /// <param name="pattern">The pattern to match.</param>
        /// <param name="patternResourceName">The resource name containing the pattern for the regular expression.</param>
        /// <param name="patternResourceType">The type containing the resource for the regular expression.</param>
        /// <param name="options">The <see cref="RegexOptions"/> to use when matching.</param>
        internal RegexValidatorAttribute(string pattern, string patternResourceName, Type patternResourceType, RegexOptions options)
        {
            ValidatorArgumentsValidatorHelper.ValidateRegexValidator(pattern, patternResourceName, patternResourceType);

            _pattern = pattern;
            _options = options;
            _patternResourceName = patternResourceName;
            _patternResourceType = patternResourceType;
        }

        /// <summary>
        /// >The pattern to match.
        /// </summary>
        public string Pattern
        {
            get { return _pattern; }
        }

        /// <summary>
        /// The <see cref="RegexOptions"/> to use when matching.
        /// </summary>
        public RegexOptions Options
        {
            get { return _options; }
        }

        /// <summary>
        /// The resource name containing the pattern for the regular expression.
        /// </summary>
        public string PatternResourceName
        {
            get { return _patternResourceName; }
        }

        /// <summary>
        /// The type containing the resource for the regular expression.
        /// </summary>
        public Type PatternResourceType
        {
            get { return _patternResourceType; }
        }

        /// <summary>
        /// Creates the <see cref="RegexValidator"/> described by the attribute object.
        /// </summary>
        /// <param name="targetType">The type of object that will be validated by the validator.</param>
        /// <remarks>This operation must be overridden by subclasses.</remarks>
        /// <returns>The created <see cref="RegexValidator"/>.</returns>
        protected override Validator DoCreateValidator(Type targetType)
        {
            return new RegexValidator(Pattern,
                PatternResourceName,
                PatternResourceType,
                Options,
                MessageTemplate,
                Negated);
        }

        private readonly Guid typeId = Guid.NewGuid();

        /// <summary>
        /// Gets a unique identifier for this attribute.
        /// </summary>
        public object TypeId
        {
            get
            {
                return typeId;
            }
        }
    }
}
