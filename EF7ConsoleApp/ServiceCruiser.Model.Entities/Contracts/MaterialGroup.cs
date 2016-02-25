using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.FieldServices;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MaterialGroup : Part, ILoggedPart
    {
        #region state
        private ObservableCollection<ValueMaterial> _valueMaterials = new ObservableCollection<ValueMaterial>();
        [JsonProperty]
        public ICollection<ValueMaterial> ValueMaterials
        {
            get { return _valueMaterials; }
            set { _valueMaterials = value != null ? value.ToObservableCollection() : null; }
        }

        #region ILoggedPart
        private decimal _priceExtra;
        [JsonProperty]
        public decimal PriceExtra
        {
            get { return _priceExtra; }
            set { SetProperty(value, ref _priceExtra, () => PriceExtra); }
        }
        private int _vatExtra;
        [JsonProperty]
        public int VatExtra
        {
            get { return _vatExtra; }
            set { SetProperty(value, ref _vatExtra, () => VatExtra); }
        }
        #endregion

        #endregion
    }
}