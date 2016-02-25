using System.Globalization;
using System.Reflection;
using ServiceCruiser.Model.Validations.Core.Resources;

namespace ServiceCruiser.Model.Validations.Core.Validators
{
    public sealed class MethodValueAccess : ValueAccess
    {
        private readonly MethodInfo _methodInfo;

        public MethodValueAccess(MethodInfo methodInfo)
        {
            _methodInfo = methodInfo;
        }
        
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
            if (!_methodInfo.DeclaringType.IsAssignableFrom(source.GetType()))
            {
                valueAccessFailureMessage
                    = string.Format(
                        CultureInfo.CurrentCulture,
                        Translations.ErrorValueAccessInvalidType,
                        Key,
                        source.GetType().FullName);
                return false;
            }

            value = _methodInfo.Invoke(source, null);
            return true;
        }

        /// <summary>
        /// Key used to retrieve data. In this case, it's just the method name.
        /// </summary>
        public override string Key
        {
            get { return _methodInfo.Name; }
        }

        #region test only properties

        /// <summary>
        /// 
        /// </summary>
        public MethodInfo MethodInfo
        {
            get { return _methodInfo; }
        }

        #endregion
    }
}
