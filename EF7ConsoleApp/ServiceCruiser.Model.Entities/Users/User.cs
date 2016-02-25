using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Capacities;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;
using ServiceCruiser.Model.Entities.FieldServices;

namespace ServiceCruiser.Model.Entities.Users
{
    [JsonObject(MemberSerialization.OptIn)]
    public class User : ValidatedEntity<User>
    {
        #region state
        private int _id;
        [JsonProperty] [KeyNew(isIdentity:true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}

        private string _firstName;
		[JsonProperty]
		public string FirstName 
		{ 
 			get {   return _firstName;  }
		    set
		    {
		        if (SetProperty(value, ref _firstName, () => FirstName))
		        {
                    OnPropertyChanged(() => DisplayName);   
		        }
		    } 
		}

        private string _lastName;
		[JsonProperty]
		public string LastName 
		{ 
 			get {   return _lastName;   }
		    set
		    {
		        if (SetProperty(value, ref _lastName, () => LastName))
		        {
                    OnPropertyChanged(() => DisplayName);
		        }
		    } 
		}

        private string _email;
		[JsonProperty]
		public string Email 
		{ 
 			get {   return _email;  } 
 			set {   SetProperty(value,ref _email, () => Email); } 
		}
    			
		private string _mobilePhone;
		[JsonProperty]
		public string MobilePhone 
		{ 
 			get {   return _mobilePhone;    } 
 			set {   SetProperty(value,ref _mobilePhone, () => MobilePhone); } 
		}
    			
		private int _companyId;
		[JsonProperty]
		public int CompanyId 
		{ 
 			get {   return _companyId;  } 
 			set {   SetProperty(value,ref _companyId, () => CompanyId); } 
		}

        private Company _company;
		[JsonProperty, Aggregation]
		public Company Company 
		{
			get {   return _company;    }
		    set
		    {
		        _company = value;
		        OnPropertyChanged(() => Company);
		    }
		}

        private ObservableCollection<UserRole> _userRoles = new ObservableCollection<UserRole>();
        [HandleOnNesting] [Aggregation(isComposite:true)]
        [JsonProperty]
        public ICollection<UserRole> UserRoles
        {
            get { return _userRoles; }
            set { _userRoles = value != null ? value.ToObservableCollection() : null; }
        }

        private ObservableCollection<CapacityRequest> _capacityRequests = new ObservableCollection<CapacityRequest>();
        [JsonProperty, Aggregation]
        public ICollection<CapacityRequest> CapacityRequests
        {
            get { return _capacityRequests; }
            set { _capacityRequests = value != null ? value.ToObservableCollection() : null; }
        }

        private ObservableCollection<CapacityCycle> _capacityCycles = new ObservableCollection<CapacityCycle>();
        [JsonProperty, Aggregation]
        public ICollection<CapacityCycle> CapacityCycles
        {
            get { return _capacityCycles; }
            set { _capacityCycles = value != null ? value.ToObservableCollection() : null; }
        }

        private ObservableCollection<CapacityCycleConfiguration> _capacityCycleConfigurations = new ObservableCollection<CapacityCycleConfiguration>();
        [JsonProperty, Aggregation]
        public ICollection<CapacityCycleConfiguration> CapacityCycleConfigurations
        {
            get { return _capacityCycleConfigurations; }
            set { _capacityCycleConfigurations = value != null ? value.ToObservableCollection() : null; }
        }

        private ObservableCollection<Credentials> _credentials = new ObservableCollection<Credentials>();
        [JsonProperty]
        [HandleOnNesting, Aggregation(isComposite:true)]
        public ICollection<Credentials> Credentials
        {
            get { return _credentials; }
            set { _credentials = value != null ? value.ToObservableCollection() : null; }
        }

        private ObservableCollection<Remark> _remarks = new ObservableCollection<Remark>();
        [JsonProperty]
        public ICollection<Remark> Remarks
        {
            get { return _remarks; }
            set { _remarks = value != null ? value.ToObservableCollection() : null; }
        }

        private ObservableCollection<Attachment> _attachments = new ObservableCollection<Attachment>();
        [JsonProperty]
        public ICollection<Attachment> Attachments
        {
            get { return _attachments; }
            set { _attachments = value != null ? value.ToObservableCollection() : null; }
        }

        private UserPreferences _userPreferences;
        [JsonProperty, HandleOnNesting, Aggregation(isComposite: true)]
        public UserPreferences UserPreferences
        {
            get { return _userPreferences; }
            set
            {
                _userPreferences = value;
                OnPropertyChanged(() => UserPreferences);
            }
        }
        
        private string _login;
        [JsonProperty] [IgnoreOnMap]
        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged(() => Login);
            }
        }

        [IgnoreOnMap]
        public bool IsCurrentUser { get; set; }

        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(LastName) &&
                    !string.IsNullOrEmpty(FirstName))
                {
                    return string.Format("{0} {1}", FirstName, LastName);
                }
                if (!string.IsNullOrEmpty(LastName))
                    return LastName;
                if (!string.IsNullOrEmpty(FirstName))
                    return FirstName;
                return string.Empty;
            }
        }

        public string[] CurrentRoleNames
        {
            get
            {
                if (UserRoles == null ||
                    !UserRoles.Any()) return null;

                return UserRoles.Select(ur => ur.DisplayRole)
                                 .ToArray();
            }
        }
        #endregion

        #region behavior
        
        public bool HasRole(RoleType roleType)
        {
            if (UserRoles == null) return false;
            if (!Enumerable.Any(UserRoles)) return false;

            return Enumerable.Any(UserRoles, ur => ur.RoleType == roleType);
        }

        public ObservableCollection<ContractModel> GetContractModels(params RoleType[] roleTypes)
        {
            var result = new ObservableCollection<ContractModel>();
            if (roleTypes == null || !roleTypes.Any()) return result;
            if (UserRoles == null || !UserRoles.Any()) return result;

            var contractModels = UserRoles.Where(ur => roleTypes.Contains(ur.RoleType)).ToList()
                                          .Where(ur => ur.ContractModel != null)  
                                          .Select(ur => ur.ContractModel);
            
            return new ObservableCollection<ContractModel>(contractModels);
        }

        public ObservableCollection<ContractModel> GetContractModels()
        {
            var result = new ObservableCollection<ContractModel>();
            if (UserRoles == null || !UserRoles.Any()) return result;

            var contractModels = UserRoles.Where(ur => ur.ContractModel != null)
                                          .Select(ur => ur.ContractModel);

            return new ObservableCollection<ContractModel>(contractModels);
        }

        public override string ToString()
        {
            return string.Format("{0} for company {1} with the following roles:\n{2}", DisplayName,
                                                                                    Company != null ? Company.Name : "?",
                                                                                    CurrentRoleNames != null  ? CurrentRoleNames.Aggregate((a, b) => a + "\n" + b) : "?");
        }

        #endregion
    }
}
