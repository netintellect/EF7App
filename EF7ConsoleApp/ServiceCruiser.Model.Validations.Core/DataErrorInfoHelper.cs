using System;
using System.Collections;
using System.Reflection;
using System.Text;

namespace ServiceCruiser.Model.Validations.Core
{
    public class DataErrorInfoHelper
    {
        private readonly object _target;
        private readonly Type _targetType;
        private readonly ValidationSpecificationSource _source;
        private readonly string _ruleset;

        public DataErrorInfoHelper(object target) : this(target, ValidationSpecificationSource.All, "")
        { }
        
        public DataErrorInfoHelper(object target, ValidationSpecificationSource source, string ruleset)
        {
            if (target == null) throw new ArgumentNullException("target");

            _target = target;
            _targetType = target.GetType();
            _source = source;
            _ruleset = ruleset;
        }

        public string Error
        {
            get { return ""; }
        }

        public string this[string columnName]
        {
            get
            {
                return GetValidationMessage(GetPropertyValidator(columnName));
            }
        }

        private Validator GetPropertyValidator(string propertyName)
        {
            Validator validator = null;

            PropertyInfo property = null;
            try
            {
                property = _targetType.GetProperty(propertyName);
            }
            catch (ArgumentException) { }
            catch (AmbiguousMatchException) { }
            if (property != null)
            {
                validator =
                    PropertyValidationFactory.GetPropertyValidator(_targetType, property, _ruleset, _source,
                                                                   new ReflectionMemberValueAccessBuilder());
            }

            return validator;
        }

        public ValidationResults GetValidationResults(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)) return null;

            var validator = GetPropertyValidator(propertyName);
            if (validator == null) return null;

            return validator.Validate(_target);
        }

        private string GetValidationMessage(Validator validator)
        {
            if (validator == null)
                return "";

            var results = validator.Validate(_target);
            if (results.IsValid)
                return "";

            var errorTextBuilder = new StringBuilder();
            foreach (var result in results)
            {
                errorTextBuilder.AppendLine(result.Message);
            }

            return errorTextBuilder.ToString();
        }
    }
}
