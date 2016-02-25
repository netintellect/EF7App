using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core.Extensibility;

namespace ServiceCruiser.Model.Entities.Core.Infrastructure
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CodeGroup : NotificationObject 
    {
        #region state
        private const string DefaultLanguage = "en";
        private const string DefaultCulture = "GB";
        
        // class variables
        private static string CurrentLanguage = DefaultLanguage;
        private static string CurrentCulture = DefaultCulture;

        // instance variables
        private CodeGroupType _type;
        [JsonProperty]
        public CodeGroupType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged(() => Type);
                OnPropertyChanged(() => Name);
            }
        }

        public string Name
        {
            get { return _type.ToString(); }
        }

        private string _code;
        [JsonProperty]
        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                OnPropertyChanged(() => Code);
            }
        }
        
        public string Value
        {
            get
            {
                if (!_translations.Any()) return null;

                var translation = _translations.FirstOrDefault(t => t.Culture.Equals(CurrentCulture, StringComparison.OrdinalIgnoreCase) &&
                                                                    t.Language.Equals(CurrentLanguage, StringComparison.OrdinalIgnoreCase));
                return translation == null ? null : translation.Value;
            }
        }
        
        private ObservableCollection<CodeGroupTranslation> _translations = new ObservableCollection<CodeGroupTranslation>();
        [JsonProperty]
        public ICollection<CodeGroupTranslation> Translations 
        {
            get { return _translations; }
            set { _translations = value != null ? value.ToObservableCollection() : null; }
        } 
        #endregion

        #region behavior
        public CodeGroup() {}
        public CodeGroup(CodeGroupType type, string code, string value)
        {
            _type = type;
            _code = code;

            _translations.Add(new CodeGroupTranslation(culture:CurrentCulture, language: CurrentLanguage, value: value));
        }

        public CodeGroup(string name, string code, IEnumerable<CodeGroupTranslation> translations)
        {
            CodeGroupType type;
            if (Enum.TryParse(name, true, out type))
            {
                _type = type;
                _code = code;

                if (translations == null || !translations.Any()) return;

                foreach (var translation in translations)
                {
                    _translations.Add(translation);
                }
                return;
            }
            throw new InvalidCastException("No enumeration defined for" + name);
        }

        public CodeGroup(CodeGroupType type, string code, IEnumerable<CodeGroupTranslation> translations)
        {
            _type = type;
            _code = code;

            if (translations == null || !translations.Any()) return;

            foreach (var translation in translations)
            {
                _translations.Add(translation);
            }
        }

        public static void InitializeCulture(string cultureName, string languageName)
        {
            if (!string.IsNullOrEmpty(cultureName))
                CurrentCulture = cultureName.ToUpper();
            if (!string.IsNullOrEmpty(languageName))
                CurrentLanguage = languageName.ToLower();
        }

        public static void InitializeCulture(CultureInfo cultureInfo)
        {
            if (cultureInfo == null) return;

            CurrentCulture = new RegionInfo(cultureInfo.Name).TwoLetterISORegionName;
            CurrentLanguage = cultureInfo.TwoLetterISOLanguageName;
        }

        public void ClearTranslations()
        {
            _translations.Clear();
        }

        public void AddTranslation(string value)
        {
            AddTranslation(DefaultCulture, DefaultLanguage, value);
        }

        public void AddTranslation(string culture, string language, string value)
        {
            var translation = _translations.FirstOrDefault(t => t.Culture.Equals(culture, StringComparison.OrdinalIgnoreCase) &&
                                                                t.Language.Equals(language, StringComparison.OrdinalIgnoreCase)) ??
                              new CodeGroupTranslation(culture, language, value);
            translation.Value = value;
        }

        public string GetTranslation(string culture, string language)
        {
            var translation = _translations.FirstOrDefault(t => t.Culture == culture &&
                                                                t.Language == language);
            if (translation == null) return null;
            
            return translation.Value;
        }
        #endregion 
    }
}
