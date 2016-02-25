using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ServiceCruiser.Model.Validations.Core
{
    public class ValidationResults : IEnumerable<ValidationResult>
    {
        private readonly List<ValidationResult> _validationResults = new List<ValidationResult>();
        public void AddResult(ValidationResult validationResult)
        {
            _validationResults.Add(validationResult);
        }

        public void AddAllResults(IEnumerable<ValidationResult> sourceValidationResults)
        {
            _validationResults.AddRange(sourceValidationResults);
        }
        
        public ValidationResults FindAll(TagFilter tagFilter, params string[] tags)
        {
            // workaround for params behavior - a single null parameter will be interpreted 
            // as null array, not as an array with null as element
            if (tags == null)
            {
                tags = new string[] { null };
            }

            var filteredValidationResults = new ValidationResults();

            foreach (ValidationResult validationResult in this)
            {
                bool matches = tags.Any(tag => (tag == null && validationResult.Tag == null) || (tag != null && tag.Equals(validationResult.Tag)));

                // if ignore, look for !match
                // if include, look for match
                if (matches ^ (tagFilter == TagFilter.Ignore))
                {
                    filteredValidationResults.AddResult(validationResult);
                }
            }

            return filteredValidationResults;
        }

        public bool IsValid
        {
            get { return _validationResults.Count == 0; }
        }

        public int Count
        {
            get { return _validationResults.Count; }
        }

        public void Clear()
        {
            _validationResults.Clear();
        }

        public IEnumerator<ValidationResult> GetEnumerator()
        {
            return _validationResults.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
