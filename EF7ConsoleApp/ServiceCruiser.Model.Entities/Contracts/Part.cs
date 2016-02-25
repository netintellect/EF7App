using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Core.Infrastructure;
using ServiceCruiser.Model.Entities.Core.Utilities;
using ServiceCruiser.Model.Entities.FieldServices;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [HasSelfValidation]
    public abstract class Part : ValidatedEntity<Part>
    {
        #region state
        private int _id;
        [JsonProperty, KeyNew(true)]
        public int Id
        {
            get { return _id; }
            set { SetProperty(value, ref _id, () => Id); }
        }
        private string _partNo;
        [JsonProperty]
        public string PartNo
        {
            get { return _partNo; }
            set { SetProperty(value, ref _partNo, () => PartNo); }
        }
        private int _supplierId;
        [JsonProperty]
        public int SupplierId
        {
            get { return _supplierId; }
            set { SetProperty(value, ref _supplierId, () => SupplierId); }
        }
        private PartSupplier _supplier;
        [HandleOnNesting]
        [Aggregation(isComposite: false)]
        [JsonProperty]
        public PartSupplier Supplier
        {
            get { return _supplier; }
            set
            {
                _supplier = value;
                OnPropertyChanged(() => Supplier);
            }
        }
        private int _companyId;
        [JsonProperty]
        public int CompanyId
        {
            get { return _companyId; }
            set { SetProperty(value, ref _companyId, () => CompanyId); }
        }
        private Company _company;
        [HandleOnNesting]
        [Aggregation(isComposite: false)]
        [JsonProperty]
        public Company Company
        {
            get { return _company; }
            set
            {
                _company = value;
                OnPropertyChanged(() => Company);
            }
        }
        private UnitType _unitType;
        [JsonProperty]
        public UnitType UnitType
        {
            get { return _unitType; }
            set { SetProperty(value, ref _unitType, () => UnitType); }
        }
        private string _shortDescription;
        [JsonProperty]
        public string ShortDescription
        {
            get { return _shortDescription; }
            set { SetProperty(value, ref _shortDescription, () => ShortDescription); }
        }
        private string _longDescription;
        [JsonProperty]
        public string LongDescription
        {
            get { return _longDescription; }
            set { SetProperty(value, ref _longDescription, () => LongDescription); }
        }
        private ObservableCollection<PartCategory> _categories = new ObservableCollection<PartCategory>();
        [JsonProperty]
        public ICollection<PartCategory> Categories
        {
            get { return _categories; }
            set { _categories = value != null ? value.ToObservableCollection() : null; }
        }
        private int? _thumbnailId;
        [JsonProperty]
        public int? ThumbnailId
        {
            get { return _thumbnailId; }
            set { SetProperty(value, ref _thumbnailId, () => ThumbnailId); }
        }
        private Photo _thumbnail;
        [HandleOnNesting]
        [Aggregation(isComposite: false)]
        [JsonProperty]
        public Photo Thumbnail
        {
            get { return _thumbnail; }
            set
            {
                _thumbnail = value;
                OnPropertyChanged(() => Thumbnail);
            }
        }
        private ObservableCollection<Photo> _photos = new ObservableCollection<Photo>();
        [JsonProperty]
        public ICollection<Photo> Photos
        {
            get { return _photos; }
            set { _photos = value != null ? value.ToObservableCollection() : null; }
        }
        private ObservableCollection<LogOff> _logOffs = new ObservableCollection<LogOff>();
        [JsonProperty, Aggregation(isComposite: false)]
        public ICollection<LogOff> LogOffs
        {
            get { return _logOffs; }
            set { _logOffs = value != null ? value.ToObservableCollection() : null; }
        }
        private ObservableCollection<InventoryAction> _inventoryActions = new ObservableCollection<InventoryAction>();
        [JsonProperty, Aggregation(isComposite: false)]
        public ICollection<InventoryAction> InventoryActions
        {
            get { return _inventoryActions; }
            set { _inventoryActions = value != null ? value.ToObservableCollection() : null; }
        }
        private ObservableCollection<WorkLogoffRule> _workLogoffRules = new ObservableCollection<WorkLogoffRule>();
        [JsonProperty, HandleOnNesting, Aggregation(isComposite: true), ObjectCollectionValidator(typeof(WorkLogoffRule))]
        public ICollection<WorkLogoffRule> WorkLogoffRules
        {
            get { return _workLogoffRules; }
            set { _workLogoffRules = value != null ? value.ToObservableCollection() : null; }
        }

        private List<string> _whiteList;
        [JsonProperty, IgnoreOnMap]
        public List<string> WhiteList
        {
            get { return _whiteList; }
            set
            {
                _whiteList = value;
                OnPropertyChanged(() => WhiteList);
            }
        }

        public string DisplayName
        {
            get { return string.Format("({0}) {1}", PartNo, ShortDescription); }
        }

        public static ObservableCollection<CodeGroup> PossibleUnits
        {
            get { return StaticFactory.Instance.GetCodeGroups(CodeGroupType.UnitType); }
        }

        public string DisplayUnit
        {
            get { return StaticFactory.Instance.GetValue(CodeGroupType.UnitType, UnitType.ToString()) ?? "?"; }
        }

        public string ThumbnailFilePath => Photos.Any() ? Photos.First().FilePath : null;

        #endregion

        #region behavior


        #endregion
    }
}
