using System.Collections.Generic;
using System.Collections.ObjectModel;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PartCategory : ValidatedEntity<PartCategory>
    {
        #region state
        private int _id;
        [JsonProperty, KeyNew(true)]
        public int Id
        {
            get { return _id; }
            set { SetProperty(value, ref _id, () => Id); }
        }
        private string _name;
        [JsonProperty]
        public string Name
        {
            get { return _name; }
            set { SetProperty(value, ref _name, () => Name); }
        }
        private int? _parentId;
        [JsonProperty]
        public int? ParentId
        {
            get { return _parentId; }
            set { SetProperty(value, ref _parentId, () => ParentId); }
        }
        private PartCategory _parent;
        [JsonProperty]
        public PartCategory Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                OnPropertyChanged(() => Parent);
            }
        }
        private ObservableCollection<Part> _parts = new ObservableCollection<Part>();
        [JsonProperty]
        public ICollection<Part> Parts
        {
            get { return _parts; }
            set { _parts = value != null ? value.ToObservableCollection() : null; }
        }
        private ObservableCollection<PartCategory> _subCategories = new ObservableCollection<PartCategory>();
        [JsonProperty]
        public ICollection<PartCategory> SubCategories
        {
            get { return _subCategories; }
            set { _subCategories = value != null ? value.ToObservableCollection() : null; }
        }
        #endregion
    }
}
