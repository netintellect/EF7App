using System.Globalization;
using System.Reflection;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    /// <summary>
    /// Represents the logic to access values from a field.
    /// </summary>
    /// <seealso cref="ValueAccess"/>
    public sealed class FieldValueAccess : ValueAccess
    {
        private readonly FieldInfo _fieldInfo;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fieldInfo"></param>
        public FieldValueAccess(FieldInfo fieldInfo)
        {
            _fieldInfo = fieldInfo;
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
            if (!_fieldInfo.DeclaringType.IsAssignableFrom(source.GetType()))
            {
                valueAccessFailureMessage
                    = string.Format(
                        CultureInfo.CurrentCulture,
                       Translations.ErrorValueAccessInvalidType,
                        Key,
                        source.GetType().FullName);
                return false;
            }

            value = _fieldInfo.GetValue(source);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Key
        {
            get { return _fieldInfo.Name; }
        }

        #region test only properties

        /// <summary>
        /// 
        /// </summary>
        public FieldInfo FieldInfo
        {
            get { return _fieldInfo; }
        }

        #endregion
    }
}
