using System;
using System.Collections.ObjectModel;
using System.Linq;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Core.Infrastructure;
using ServiceCruiser.Model.Validations.Core.Common.Utility;


namespace ServiceCruiser.Model.Entities.Core.Utilities
{
    public class StaticFactory
    {
        #region state
        private static StaticFactory instance;
        private static readonly object LockObject = "ForLocking";
        
        private readonly ObservableCollection<Country> _countries = new ObservableCollection<Country>();
        public ObservableCollection<Country> Countries
        {
            get { return _countries; }
        }

        private readonly ObservableCollection<Language> _languages = new ObservableCollection<Language>();
        public ObservableCollection<Language> Languages
        {
            get { return _languages; }
        }
        
        private readonly ObservableCollection<CodeGroup> _codeGroups = new ObservableCollection<CodeGroup>();
        public ObservableCollection<CodeGroup> CodeGroups
        {
            get { return _codeGroups; }
        }
        
        public static StaticFactory Instance
        {
            get
            {
                lock (LockObject)
                {
                    if (instance == null)
                        instance = new StaticFactory();
                }
                return instance;
            }
        }
        #endregion

        #region behavior
        public void Initialize(AppInfo appInfo)
        {
            if (appInfo == null) return;

            appInfo.PossibleCountries.ToList()
                                     .ForEach(pc => Countries.Add(pc));

            appInfo.PossibleLanguages.ToList()
                                     .ForEach(pl => Languages.Add(pl));   

            appInfo.PossibleCodeGroups.ToList()
                                      .ForEach(cg => CodeGroups.Add(cg));
        }

        public string GetCountryCode(string countryValue)
        {
            if (!Countries.Any()) return null;
            if (string.IsNullOrEmpty(countryValue)) return null;

            var country = Countries.FirstOrDefault(c => c.Translations.Any(t => t.Value != null &&
                                                                                t.Value.Contains(countryValue)));
            return country != null ? country.Code : null;
        }

        public string GetCountryValue(string countryCode)
        {
            if (!Countries.Any()) return null;
            if (string.IsNullOrEmpty(countryCode)) return null;

            var country = Countries.FirstOrDefault(c => c.Code.Equals(countryCode, StringComparison.OrdinalIgnoreCase));

            return country != null ? country.Value : null;
        }

        public string GetCode(CodeGroupType groupType, string value)
        {
            if (!CodeGroups.Any()) return null;
            if (string.IsNullOrEmpty(value)) return null;

            var codeGroup = CodeGroups.FirstOrDefault(cg => cg.Type == groupType &&
                                                            cg.Translations.Any(t => t.Value != null &&
                                                                                     t.Value.Contains(value)));

            return codeGroup != null ? codeGroup.Code : null;
        }

        public ObservableCollection<CodeGroup> GetCodeGroups(CodeGroupType type)
        {
            var codeGroups = new ObservableCollection<CodeGroup>();

            if (!CodeGroups.Any()) return codeGroups;

            return CodeGroups.Where(cg => cg.Type == type)
                             .ToObservableCollection();
        }

        public string GetValue(CodeGroupType groupType, string code)
        {
            if (!CodeGroups.Any()) return null;
            if (string.IsNullOrEmpty(code)) return null;

            var codeGroup = CodeGroups.FirstOrDefault(cg => cg.Type == groupType &&
                                                            cg.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

            return codeGroup != null ? codeGroup.Value : null;
        }
        
        public string GetValue(CodeGroupType groupType, string culture, string language, string code)
        {
            if (!CodeGroups.Any()) return null;
            if (string.IsNullOrEmpty(culture)) return null;
            if (string.IsNullOrEmpty(language)) return null;
            if (string.IsNullOrEmpty(code)) return null;
            
            var codeGroup = CodeGroups.FirstOrDefault(cg => cg.Type == groupType &&
                                                            cg.Translations.Any(t => t.Culture.Equals(culture, StringComparison.OrdinalIgnoreCase) &&
                                                                                     t.Language.Equals(language, StringComparison.OrdinalIgnoreCase)));

            if (codeGroup == null) return null;

            var translation = codeGroup.Translations.FirstOrDefault(t => t.Culture.Equals(culture, StringComparison.OrdinalIgnoreCase) &&
                                                                         t.Language.Equals(language, StringComparison.OrdinalIgnoreCase));
            return translation != null ? translation.Value : null;
        }

        public string[] GetValue(CodeGroupType groupType, string language, string code)
        {
            if (!CodeGroups.Any()) return null;
            if (string.IsNullOrEmpty(language)) return null;
            if (string.IsNullOrEmpty(code)) return null;

            var codeGroup = CodeGroups.FirstOrDefault(cg => cg.Type == groupType &&
                                                            cg.Translations.Any(t => t.Language.Equals(language, StringComparison.OrdinalIgnoreCase)));

            if (codeGroup == null) return null;

            var translations = codeGroup.Translations.Where(t => t.Language.Equals(language, StringComparison.OrdinalIgnoreCase));
            
            return translations.Select(t => t.Value).ToArray();
        }

        #endregion
    }
}
