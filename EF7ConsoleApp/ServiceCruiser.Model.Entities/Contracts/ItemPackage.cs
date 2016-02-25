using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.FieldServices;
using ServiceCruiser.Model.Validations.Core.Validators;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ItemPackage : Part, ITrackedPart
    {
        #region state
        private string _reversePartNo;
        [JsonProperty]
        public string ReversePartNo
        {
            get { return _reversePartNo; }
            set { SetProperty(value, ref _reversePartNo, () => ReversePartNo); }
        }
        private ObservableCollection<PartNoAlias> _aliases = new ObservableCollection<PartNoAlias>();
        [HandleOnNesting, Aggregation(isComposite: true)]
        [ObjectCollectionValidator(typeof(PartNoAlias))]
        [JsonProperty]
        public ICollection<PartNoAlias> Aliases
        {
            get { return _aliases; }
            set { _aliases = value != null ? value.ToObservableCollection() : null; }
        }
        #endregion
    }
}
