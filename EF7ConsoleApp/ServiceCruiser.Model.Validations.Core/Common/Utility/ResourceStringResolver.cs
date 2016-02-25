using System;

namespace ServiceCruiser.Model.Validations.Core.Common.Utility
{
    /// <summary>
    /// Resolves strings by retrieving them from assembly resources, falling back to a specified
    /// value.
    /// </summary>
    /// <remarks>
    /// If both the resource type and the resource name are available, a resource lookup will be 
    /// performed; otherwise, the default value will be returned.
    /// </remarks>
    public sealed class ResourceStringResolver : IStringResolver
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ResourceStringResolver"/>
        /// for a resource type, a resource name and a fallback value.
        /// </summary>
        /// <param name="resourceType">The type that identifies the resources file.</param>
        /// <param name="resourceName">The name of the resource.</param>
        /// <param name="fallbackValue">The fallback value, to use when any of the resource
        /// identifiers is not available.</param>
        public ResourceStringResolver(Type resourceType, string resourceName, string fallbackValue)
        {
            _resourceType = resourceType;
            _resourceName = resourceName;
            _fallbackValue = fallbackValue;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ResourceStringResolver"/>
        /// for a resource type name, a resource name and a fallback value.
        /// </summary>
        /// <param name="resourceTypeName">The name of the type that identifies the resources file.</param>
        /// <param name="resourceName">The name of the resource.</param>
        /// <param name="fallbackValue">The fallback value, to use when any of the resource
        /// identifiers is not available.</param>
        public ResourceStringResolver(string resourceTypeName, string resourceName, string fallbackValue)
            : this(LoadType(resourceTypeName), resourceName, fallbackValue)
        { }

        private static Type LoadType(string resourceTypeName)
        {
            return Type.GetType(resourceTypeName ?? string.Empty, false);
        }

        private readonly Type _resourceType;
        private readonly string _resourceName;
        private readonly string _fallbackValue;

        string IStringResolver.GetString()
        {
            string value;

            if (!(_resourceType == null || string.IsNullOrEmpty(_resourceName)))
            {
                value
                    = ResourceStringLoader.LoadString(
                        _resourceType.FullName,
                        _resourceName,
                        _resourceType.Assembly);
            }
            else
            {
                value = _fallbackValue;
            }

            return value;
        }
    }
}
