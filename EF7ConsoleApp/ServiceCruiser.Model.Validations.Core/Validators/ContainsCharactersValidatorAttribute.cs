using System;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    /// <summary>
    /// Represents a <see cref="ContainsCharactersValidator"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter,
                    AllowMultiple = true, Inherited = false)]
    public sealed class ContainsCharactersValidatorAttribute : ValueValidatorAttribute
    {
        private readonly string _characterSet;
        private readonly ContainsCharacters _containsCharacters;
        
        public ContainsCharactersValidatorAttribute(string characterSet) : this(characterSet, ContainsCharacters.Any)
        { }
        
        public ContainsCharactersValidatorAttribute(string characterSet, ContainsCharacters containsCharacters)
        {
            ValidatorArgumentsValidatorHelper.ValidateContainsCharacterValidator(characterSet);

            _characterSet = characterSet;
            _containsCharacters = containsCharacters;
        }
        
        public string CharacterSet
        {
            get { return _characterSet; }
        }

        public ContainsCharacters ContainsCharacters
        {
            get { return _containsCharacters; }
        }

        protected override Validator DoCreateValidator(Type targetType)
        {
            return new ContainsCharactersValidator(CharacterSet, ContainsCharacters, Negated);
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
