using System.Collections.Generic;
using Newtonsoft.Json;

namespace ServiceCruiser.Model.Entities.Core.Infrastructure
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Country : CodeGroup
    {
        #region state
        private string _longCode;
        [JsonProperty]
        public string LongCode
        {
            get { return _longCode; }
            set
            {
                _longCode = value;
                OnPropertyChanged(() => LongCode);
            }
        }
        
        #endregion

        #region behavior
        public Country() { }
        public Country(string code, string value) : base(CodeGroupType.Country, code, value) {}
        public Country(string code, IEnumerable<CodeGroupTranslation> translations) : base(CodeGroupType.Country, code, translations) { }
        #endregion
    }
}
