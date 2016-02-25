using System;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.DataAnnotations
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class MetadataTypeAttribute : Attribute
    {
        private readonly Type _metadataClassType;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataTypeAttribute"/> class.
        /// </summary>
        /// <param name="metadataClassType">The metadata class to reference.</param>
        public MetadataTypeAttribute(Type metadataClassType)
        {
            _metadataClassType = metadataClassType;
        }

        /// <summary>
        /// Gets the metadata class that is associated with a data-model partial class.
        /// </summary>
        public Type MetadataClassType
        {
            get
            {
                if (_metadataClassType == null)
                {
                    throw new InvalidOperationException(Translations.MetadataTypeAttribute_TypeCannotBeNull);
                }

                return _metadataClassType;
            }
        }
    }
}
