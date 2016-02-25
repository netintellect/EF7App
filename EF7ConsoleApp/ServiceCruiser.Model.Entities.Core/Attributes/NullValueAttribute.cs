using System;

namespace ServiceCruiser.Model.Entities.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NullValueAttribute : Attribute
    {
        #region state
        public bool AllowEmptyStrings { get; set; }
        public object NullableSymbol { get; set; }
        #endregion

        #region methods
        public NullValueAttribute() { }
        #endregion
    }

}
