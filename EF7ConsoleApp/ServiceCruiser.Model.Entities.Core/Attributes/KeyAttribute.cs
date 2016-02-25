using System;

namespace ServiceCruiser.Model.Entities.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class KeyAttribute : Attribute
    {
        #region properties

        public bool IsIdentity { get; private set; }

        #endregion

        #region methods

        public KeyAttribute(bool isIdentity)
        {
            IsIdentity = isIdentity;
        }

        #endregion
    }
}
