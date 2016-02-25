using Newtonsoft.Json;

namespace ServiceCruiser.Model.Entities.Core.Infrastructure
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CodeGroupTranslation : NotificationObject
    {
        #region state

        private string _culture;
        [JsonProperty]
        public string Culture
        {   
            get { return _culture; }
            set
            {
                _culture = value;
                OnPropertyChanged(() => Culture);
            }
        }
        
        private string _language;
        [JsonProperty]
        public string Language
        {
            get { return _language; }
            set
            {
                _language = value;
                OnPropertyChanged(() => Language);
            }
        }
        
        private string _value;
        [JsonProperty]
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged(() => Value);
            }
        }
        
        private string LanguageCultureName
        {
            get { return string.Format("{0}-{1}", Language, Culture); }
        }

        #endregion

        #region behavior
        public CodeGroupTranslation(string culture, string language, string value)
        {
            _culture = culture;
            _language = language;
            _value = value;
        }
        
        #endregion
    }
}
