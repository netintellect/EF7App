using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    public class InventoryLocation : ValidatedEntity<InventoryLocation>
    {
        #region state
        private int _id;
        [JsonProperty, Key(true)]
        public int Id
        {
            get { return _id; }
            set { SetProperty(value, ref _id, () => Id); }
        }
        private string _externalRef;
        [JsonProperty]
        public string ExternalRef
        {
            get { return _externalRef; }
            set { SetProperty(value, ref _externalRef, () => ExternalRef); }
        }
        #endregion
    }
}
