using ServiceCruiser.Model.Validations.Core.Validators;

namespace ServiceCruiser.Model.Validations.Core
{
    public class ValueAccessValidatorBuilder : CompositeValidatorBuilder
    {
        private readonly ValueAccess _valueAccess;
        
        public ValueAccessValidatorBuilder(ValueAccess valueAccess, IValidatedElement validatedElement)
            : base(validatedElement)
        {
            _valueAccess = valueAccess;
        }

        protected override Validator DoGetValidator()
        {
            return new ValueAccessValidator(_valueAccess, base.DoGetValidator());
        }

        #region test only properties
        public ValueAccess ValueAccess
        {
            get { return _valueAccess; }
        }
        #endregion
    }
}
