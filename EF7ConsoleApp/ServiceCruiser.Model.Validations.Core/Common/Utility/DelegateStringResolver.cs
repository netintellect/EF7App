using System;

namespace ServiceCruiser.Model.Validations.Core.Common.Utility
{
    /// <summary>
    /// Resolves strings by invoking a delegate and returning the resulting value.
    /// </summary>
    public sealed class DelegateStringResolver : IStringResolver
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ConstantStringResolver"/> with a delegate.
        /// </summary>
        /// <param name="resolverDelegate">The delegate to invoke when resolving a string.</param>
        public DelegateStringResolver(Func<string> resolverDelegate)
        {
            _resolverDelegate = resolverDelegate;
        }

        private readonly Func<string> _resolverDelegate;

        string IStringResolver.GetString()
        {
            return _resolverDelegate();
        }
    }
}
