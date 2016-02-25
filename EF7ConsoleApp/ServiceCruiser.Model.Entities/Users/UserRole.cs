using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Resources;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Data;
using ServiceCruiser.Model.Entities.FieldServices;
using ServiceCruiser.Model.Entities.Repositories;
using ContractModel = ServiceCruiser.Model.Entities.Contracts.ContractModel;

namespace ServiceCruiser.Model.Entities.Users
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserRole : ValidatedEntity<UserRole>
    {
        #region state

        private int _id;
        [JsonProperty] [KeyNew(true)]
        public int Id
        {
            get {   return _id; }
            set {   SetProperty(value, ref _id, () => Id);  }
        }
        
        private int _userId;
        [JsonProperty]
        public int UserId
        {
            get {   return _userId; }
            set {   SetProperty(value, ref _userId, () => UserId);  }
        }

        private User _user;
        [JsonProperty]
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged(() => User);
            }
        }
        

        private RoleType _roleType;
        [JsonProperty]
        public RoleType RoleType
        {
            get {   return _roleType; }
            set
            {
                if (SetProperty(value, ref _roleType, () => RoleType))
                {
                    OnPropertyChanged(() => DisplayRole);
                }
            }
        }
        
        private int? _contractModelId;
        [JsonProperty]
        public int? ContractModelId
        {
            get {   return _contractModelId; }
            set {   SetProperty(value, ref _contractModelId, () => ContractModelId);    }
        }

        private ContractModel _contractModel;
        [JsonProperty, Aggregation]
        public ContractModel ContractModel
        {
            get
            {
                if (_contractModel != null || !UseRepositoryFinder) return _contractModel;
                var repository = RepositoryFinder.GetRepository<IContractModelRepository>() as IContractModelRepository;
                return repository?.Filter(cm => cm.Id == ContractModelId).FirstOrDefault();
            }
            set
            {
                _contractModel = value;
                OnPropertyChanged(() => ContractModel);
            }
        }

        private ObservableCollection<Permission> _permissions = new ObservableCollection<Permission>();
        [JsonProperty, Aggregation(true)]
        public ICollection<Permission> Permissions
        {
            get { return _permissions; }
            set { _permissions = value != null ? value.ToObservableCollection() : null; }
        }

        private ObservableCollection<VisitHistory> _visitHistories = new ObservableCollection<VisitHistory>();
        [JsonProperty, Aggregation]
        public ICollection<VisitHistory> VisitHistories
        {
            get { return _visitHistories; }
            set { _visitHistories = value != null ? value.ToObservableCollection() : null; }
        }

        private ObservableCollection<ServiceOrderHistory> _serviceOrderHistories = new ObservableCollection<ServiceOrderHistory>();
        [JsonProperty, Aggregation]
        public ICollection<ServiceOrderHistory> ServiceOrderHistories
        {
            get { return _serviceOrderHistories; }
            set { _serviceOrderHistories = value != null ? value.ToObservableCollection() : null; }
        }

        private ObservableCollection<WorkOrderHistory> _workOrderHistories = new ObservableCollection<WorkOrderHistory>();
        [JsonProperty, Aggregation]
        public ICollection<WorkOrderHistory> WorkOrderHistories
        {
            get { return _workOrderHistories; }
            set { _workOrderHistories = value != null ? value.ToObservableCollection() : null; }
        }

        private ObservableCollection<Remark> _remarks = new ObservableCollection<Remark>();
        [JsonProperty, Aggregation]
        public ICollection<Remark> Remarks
        {
            get { return _remarks; }
            set { _remarks = value != null ? value.ToObservableCollection() : null; }
        }

        private ObservableCollection<Attachment> _attachments = new ObservableCollection<Attachment>();
        [JsonProperty, Aggregation]
        public ICollection<Attachment> Attachments
        {
            get { return _attachments; }
            set { _attachments = value != null ? value.ToObservableCollection() : null; }
        }

        private ObservableCollection<InventoryAction> _inventoryActions = new ObservableCollection<InventoryAction>();
        [JsonProperty, Aggregation]
        public ICollection<InventoryAction> InventoryActions
        {
            get { return _inventoryActions; }
            set { _inventoryActions = value != null ? value.ToObservableCollection() : null; }
        }

        
        private static IDictionary<int, string> _possibleRoleTypes;
        public static IDictionary<int, string> PossibleRoleTypes
        {
            get { return _possibleRoleTypes = _possibleRoleTypes ?? CreatePossibleRoleTypes(); }
        }

        public string DisplayRole
        {
            get
            {
                string translation;
                if (PossibleRoleTypes.TryGetValue((int)RoleType, out translation))
                    return translation;
                return "";
            }
        }
        #endregion

        #region behavior
        private static IDictionary<int, string> CreatePossibleRoleTypes()
        {
            IDictionary<int, string> roleTypes = new Dictionary<int, string>();
            foreach (var enumValue in Enum.GetValues(typeof(RoleType)))
            {
                switch ((int)enumValue)
                {
                    case (int)RoleType.ContractorDispatcher:
                    case (int)RoleType.Dispatcher:
                        roleTypes.Add((int)enumValue, Translations.RoleTypeDispatcher);
                        break;
                    case (int)RoleType.Technician:
                        roleTypes.Add((int)enumValue, Translations.RoleTypeTechnician);
                        break;
                    case (int)RoleType.CapacityRequestor:
                        roleTypes.Add((int)enumValue, Translations.RoleTypeCapacityRequestor);
                        break;
                    case (int)RoleType.ContractorAdmin:
                        roleTypes.Add((int)enumValue, Translations.RoleTypeContractorAdmin);
                        break;
                    case (int)RoleType.CallAgent:
                        roleTypes.Add((int)enumValue, Translations.RoleTypeCallAgent);
                        break;
                    case (int)RoleType.ContractingAdmin:
                        roleTypes.Add((int)enumValue, Translations.RoleTypeContractingAdmin);
                        break;
                    default:
                        roleTypes.Add((int)enumValue, "");
                        break;
                }
            }
            return roleTypes;
        }
        #endregion
    }
}
