using System.Collections.Generic;
using Newtonsoft.Json;

namespace ServiceCruiser.Model.Entities.Core.Infrastructure
{
    [JsonObject(MemberSerialization.OptIn)]
    public sealed class Language : CodeGroup
    {
        #region state
        public string TwoLetterIso
        {
            get
            {
                if (string.IsNullOrEmpty(Code)) return null;

                var codeParts = Code.Split('-');

                return codeParts[0];
            }
        }
        #endregion
        
        #region behavior
        public Language() {}
        public Language(string code, string value) : base(CodeGroupType.Language, code, value) {}
        public Language(string code, IEnumerable<CodeGroupTranslation> translations) : base(CodeGroupType.Language, code, translations) { }
        #endregion
    }
}
