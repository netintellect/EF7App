using System.Collections.Generic;
using System.Globalization;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    /// <summary>
    /// Performs validation on strings by verifying if it contains a character set using the <see cref="ContainsCharacters"/> mode.
    /// </summary>
    public class ContainsCharactersValidator : ValueValidator<string>
    {
        private readonly string _characterSet;
        private readonly ContainsCharacters _containsCharacters = ContainsCharacters.Any;
        
        public ContainsCharactersValidator(string characterSet) : this(characterSet, ContainsCharacters.Any)
        {}
        
        public ContainsCharactersValidator(string characterSet, ContainsCharacters containsCharacters) : this(characterSet, containsCharacters, false)
        {
        }

        public ContainsCharactersValidator(string characterSet, ContainsCharacters containsCharacters, string messageTemplate)
            : this(characterSet, containsCharacters, messageTemplate, false)
        { }
        
        public ContainsCharactersValidator(string characterSet, ContainsCharacters containsCharacters, bool negated)
            : this(characterSet, containsCharacters, null, negated)
        { }

        public ContainsCharactersValidator(string characterSet, ContainsCharacters containsCharacters, string messageTemplate, bool negated)
            : base(messageTemplate, null, negated)
        {
            ValidatorArgumentsValidatorHelper.ValidateContainsCharacterValidator(characterSet);

            _characterSet = characterSet;
            _containsCharacters = containsCharacters;
        }
        
        protected override void DoValidate(string objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            bool logError = false;
            bool isObjectToValidateNull = objectToValidate == null;

            if (!isObjectToValidateNull)
            {
                if (ContainsCharacters.Any == ContainsCharacters)
                {
                    var characterSetArray = new List<char>(_characterSet.ToCharArray());
                    bool containsCharacterFromSet = false;
                    foreach (char ch in objectToValidate)
                    {
                        if (characterSetArray.Contains(ch))
                        {
                            containsCharacterFromSet = true;
                            break;
                        }

                    }
                    logError = !containsCharacterFromSet;
                }
                else if (ContainsCharacters.All == ContainsCharacters)
                {
                    var objectToValidateArray = new List<char>(objectToValidate.ToCharArray());
                    bool containsAllCharacters = true;
                    foreach (char ch in CharacterSet)
                    {
                        if (!objectToValidateArray.Contains(ch))
                        {
                            containsAllCharacters = false;
                            break;
                        }
                    }
                    logError = !containsAllCharacters;
                }
            }

            if (isObjectToValidateNull || (logError != Negated))
            {
                LogValidationResult(validationResults,
                    GetMessage(objectToValidate, key),
                    currentTarget,
                    key);
            }
        }
        
        protected internal override string GetMessage(object objectToValidate, string key)
        {
            return string.Format(CultureInfo.CurrentCulture, MessageTemplate, objectToValidate, key,
                                 Tag, CharacterSet, ContainsCharacters);
        }
        
        protected override string DefaultNonNegatedMessageTemplate
        {
            get { return Translations.ContainsCharactersNonNegatedDefaultMessageTemplate; }
        }
        
        protected override string DefaultNegatedMessageTemplate
        {
            get { return Translations.ContainsCharactersNegatedDefaultMessageTemplate; }
        }

        /// <summary>
        /// The character set to validate against.
        /// </summary>
        public string CharacterSet
        {
            get { return _characterSet; }
        }

        /// <summary>
        /// How does this validator check?
        /// </summary>
        public ContainsCharacters ContainsCharacters
        {
            get { return _containsCharacters; }
        }
    }
}
