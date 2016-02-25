namespace ServiceCruiser.Model.Validations.Core.Common.Utility
{
    /// <summary>
    /// Resolves strings by returning a constant value.
    /// </summary>
    public sealed class ConstantStringResolver : IStringResolver
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ConstantStringResolver"/> with a constant value.
        /// </summary>
        public ConstantStringResolver(string value)
        {
            _value = value;
        }

        private readonly string _value;

        string IStringResolver.GetString()
        {
            return _value;
        }
    }
}
