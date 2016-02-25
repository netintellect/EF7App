using System.Collections.Generic;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    public class AndCompositeValidator : Validator
    {
        private readonly IEnumerable<Validator> _validators;
        
        public AndCompositeValidator(params Validator[] validators) : base(null, null)
        {
            _validators = validators;
        }
        
        public override void DoValidate(object objectToValidate, object currentTarget, string key,
                                        ValidationResults validationResults)
        {
            foreach (Validator validator in _validators)
            {
                validator.DoValidate(objectToValidate, currentTarget, key, validationResults);
            }
        }
        
        protected override string DefaultMessageTemplate
        {
            get { return null; }
        }
        
        public IEnumerable<Validator> Validators
        {
            get
            {
                return _validators;
            }
        }
    }
}
