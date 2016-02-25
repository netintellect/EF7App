using System.ComponentModel;

namespace ServiceCruiser.Model.Validations.Core
{
    public enum ValidationSpecificationSource
    {
        /// <summary>
        /// Validation information is to be retrieved from attributes.
        /// </summary>
        Attributes = 1,

        /// <summary>
        /// Validation information is to be retrieved from configuration.
        /// </summary>
        Configuration = 2,

        /// <summary>
        /// Validation information is to be retrieved from both attributes and configuration.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Advanced)]
        Both = 3,

        /// <summary>
        /// Validation information is to be retrieved from Data Annotations Validation Attributes.
        /// </summary>
        DataAnnotations = 4,

        /// <summary>
        /// Validation information is to be retrieved from all possible sources.
        /// </summary>
        All = 7
    }

    internal static class ValidationSpecificationSourceExtension
    {
        public static bool IsSet(this ValidationSpecificationSource source, ValidationSpecificationSource value)
        {
            return value == (source & value);
        }
    }
}
