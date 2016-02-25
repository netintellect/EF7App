using System;
using System.Reflection;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    /// <summary>
    /// Performs validation by invoking a method on the validated object.
    /// </summary>
    public class SelfValidationValidator : Validator
    {
        private readonly MethodInfo _methodInfo;

        /// <summary>
        /// <para>Initializes a new instance of the <see cref="SelfValidationValidator"/> class with the 
        /// method that is to be invoked when performing validation.</para>
        /// </summary>
        /// <param name="methodInfo">The self validation method to invoke.</param>
        /// <exception cref="ArgumentNullException">when <paramref name="methodInfo"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentException">when <paramref name="methodInfo"/> does not have the required signature.</exception>
        public SelfValidationValidator(MethodInfo methodInfo) : base(null, null)
        {
            if (null == methodInfo)
                throw new ArgumentNullException("methodInfo");
            if (typeof(void) != methodInfo.ReturnType)
                throw new ArgumentException(Translations.ExceptionSelfValidationMethodWithInvalidSignature, "methodInfo");
            ParameterInfo[] parameters = methodInfo.GetParameters();
            if (1 != parameters.Length || typeof(ValidationResults) != parameters[0].ParameterType)
                throw new ArgumentException(Translations.ExceptionSelfValidationMethodWithInvalidSignature, "methodInfo");

            _methodInfo = methodInfo;
        }

        public override void DoValidate(object objectToValidate, object currentTarget, string key,
                                        ValidationResults validationResults)
        {
            if (null == objectToValidate)
            {
                LogValidationResult(validationResults, Translations.SelfValidationValidatorMessage, currentTarget, key);
            }
            else if (!_methodInfo.DeclaringType.IsAssignableFrom(objectToValidate.GetType()))
            {
                LogValidationResult(validationResults, Translations.SelfValidationValidatorMessage, currentTarget, key);
            }
            else
            {
                try
                {
                    _methodInfo.Invoke(objectToValidate, new object[] { validationResults });
                }
                catch (Exception)
                {
                    LogValidationResult(validationResults, Translations.SelfValidationMethodThrownMessage, currentTarget, key);
                }
            }
        }

        /// <summary>
        /// Gets the message template to use when logging results no message is supplied.
        /// </summary>
        protected override string DefaultMessageTemplate
        {
            get { return null; }
        }
    }
}
