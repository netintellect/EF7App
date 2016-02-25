using System.Globalization;
using System.Reflection;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    /// <summary>
    /// Represents the logic to access values from a property.
    /// </summary>
    /// <seealso cref="ValueAccess"/>
    public sealed class PropertyValueAccess : ValueAccess
    {
        private readonly PropertyInfo _propertyInfo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo"></param>
        public PropertyValueAccess(PropertyInfo propertyInfo)
        {
            _propertyInfo = propertyInfo;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <param name="valueAccessFailureMessage"></param>
        /// <returns></returns>
        public override bool GetValue(object source, out object value, out string valueAccessFailureMessage)
        {
            value = null;
            valueAccessFailureMessage = null;

            if (null == source)
            {
                valueAccessFailureMessage
                    = string.Format(
                        CultureInfo.CurrentCulture,
                        Translations.ErrorValueAccessNull,
                        Key);
                return false;
            }
            if (!_propertyInfo.DeclaringType.IsAssignableFrom(source.GetType()))
            {
                valueAccessFailureMessage
                    = string.Format(
                        CultureInfo.CurrentCulture,
                        Translations.ErrorValueAccessInvalidType,
                        Key,
                        source.GetType().FullName);
                return false;
            }

            value = _propertyInfo.GetValue(source, null);

            return true;
        }

        /// <summary>
        /// Key used to retrieve item - the property name.
        /// </summary>
        public override string Key
        {
            get { return _propertyInfo.Name; }
        }

        #region test only properties

        /// <summary>
        /// 
        /// </summary>
        public PropertyInfo PropertyInfo
        {
            get { return _propertyInfo; }
        }

        #endregion
    }
}
