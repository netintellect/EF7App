using System;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class SelfValidationAttribute : Attribute
    {
        private string _ruleset = string.Empty;

        /// <summary>
        /// Gets or set the ruleset for which the self validation method must be included.
        /// </summary>
        public string Ruleset
        {
            get { return _ruleset; }
            set { _ruleset = value; }
        }

        private readonly Guid _typeId = Guid.NewGuid();

        /// <summary>
        /// Gets a unique identifier for this attribute.
        /// </summary>
        public object TypeId
        {
            get
            {
                return _typeId;
            }
        }
    }
}
