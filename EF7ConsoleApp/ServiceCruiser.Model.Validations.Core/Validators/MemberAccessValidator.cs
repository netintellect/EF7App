namespace ServiceCruiser.Model.Validations.Core.Validators
{
	public abstract class MemberAccessValidator<T> : Validator<T>
	{
		private readonly ValueAccessValidator _valueAccessValidator;

		protected MemberAccessValidator(ValueAccess valueAccess, Validator valueValidator)
			: base(null, null)
		{
			_valueAccessValidator = new ValueAccessValidator(valueAccess, valueValidator);
		    Validator = valueValidator;
		}
        
		protected override void DoValidate(T objectToValidate, object currentTarget, string key, 
			ValidationResults validationResults)
		{
			_valueAccessValidator.DoValidate(objectToValidate, currentTarget, key, validationResults);
		}

		protected override string DefaultMessageTemplate
		{
			get { return null; }
		}
        
        public Validator Validator
        {
            get; private set;
        }
        
        public string Key
        {
            get { return _valueAccessValidator.Key; }
        }
	}
}
