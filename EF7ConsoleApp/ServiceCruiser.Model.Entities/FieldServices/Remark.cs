using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Infrastructure;
using ServiceCruiser.Model.Entities.Core.Utilities;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Entities.Users;
using ServiceCruiser.Model.Validations.Core;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;
using ValidationResult = ServiceCruiser.Model.Validations.Core.ValidationResult;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn)]
    [HasSelfValidation]
    public abstract class Remark : ValidatedEntity<Remark>
    {
        #region state
        private int _id;
		[JsonProperty] [KeyNew(true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}
        
        protected string _text;
        [Display(ResourceType = typeof(Translations), Name ="RemarkText")]
        [JsonProperty]
		public string Text 
		{ 
 			get {   return _text;  } 
 			set {   SetProperty(value,ref _text, () => Text);   } 
		}
    			
        private RemarkType _type;
        [DomainValidator(new object[] { RemarkType.All }, Negated = true,
                                        MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "RemarkTypeRequired")]
        [Display(ResourceType = typeof(Translations), Name ="RemarkType")]
        [JsonProperty]
		public RemarkType Type 
		{ 
 			get {   return _type;   }
            set
            {
                if (SetProperty(value, ref _type, () => Type))
                {
                    OnPropertyChanged(() => DisplayType);
                }
            } 
		}
        
        private DateTimeOffset _enteredDate;
		[JsonProperty]
		public DateTimeOffset EnteredDate 
		{ 
 			get {   return _enteredDate;    } 
 			set {   SetProperty(value,ref _enteredDate, () => EnteredDate); } 
		}

        private string _extraInfo;
		[JsonProperty]
		public string ExtraInfo 
		{ 
 			get {   return _extraInfo;  } 
 			set {   SetProperty(value,ref _extraInfo, () => ExtraInfo); } 
		}

		private int _userId;
		[JsonProperty]
		public int UserId 
		{ 
 			get {   return _userId; } 
 			set {   SetProperty(value,ref _userId, () => UserId);   } 
		}
    			
        private User _user;
        [HandleOnNesting] [Aggregation]
        [JsonProperty]
		public User User 
		{
			get {   return _user;  } 
			set 
            {   
                _user= value;
				OnPropertyChanged(() => User);
		    } 
		}

		private int _userRoleId;
		[JsonProperty]
		public int UserRoleId 
		{ 
 			get {   return _userRoleId; } 
 			set {   SetProperty(value,ref _userRoleId, () => UserRoleId);   } 
		}
        
        private UserRole _userRole;
        [HandleOnNesting] [Aggregation]
		[JsonProperty]
		public UserRole UserRole 
		{
			get {   return _userRole;   }
            set
            {
                _userRole = value;
                OnPropertyChanged(() => UserRole);
            }
		}
        
        public ObservableCollection<CodeGroup> PossibleTypes
        {
            get
            {
                return StaticFactory.Instance.GetCodeGroups(CodeGroupType.RemarkType);
            }
        }

        private bool _isInEditMode;
        public bool IsInEditMode
        {
            get { return _isInEditMode; }
            set
            {
                _isInEditMode = value;
                OnPropertyChanged(() => Text);
                OnPropertyChanged(() => IsInEditMode);
            }
        
        }

        public string DisplayType
        {
            get
            {
                return StaticFactory.Instance.GetValue(CodeGroupType.RemarkType, Type.ToString()) ?? "?";
            }    
        }

        public virtual string DisplayAudit
        {
            get
            {
                string message = Translations.RemarkDisplayAudit;
                return string.Format(message, UserRole != null ? UserRole.DisplayRole : "?",
                                              User != null ? User.DisplayName : UserRole?.User != null ? UserRole?.User.DisplayName : "?",
                                              EnteredDate != DateTimeOffset.MinValue ? EnteredDate.ToString("d") : "?",
                                              EnteredDate != DateTimeOffset.MinValue ? EnteredDate.ToString("HH:mm:ss") : "");
            }
        }

        public bool HasResultCode
        {
            get { return this is BookingRemark; }
        }
        #endregion

        #region behavior
        [SelfValidation]
        public void CheckRemarkText(ValidationResults results)
        {
            const string errorField = "Text";

            ClearValidationError(errorField);
            
            if (!(this is BookingRemark) &&
                string.IsNullOrEmpty(Text))
            {
                var validationResult = new ValidationResult(Translations.RemarkTextRequired,
                                                            this, errorField, null, EntityValidator);

                results.AddResult(validationResult);
                SetValidationError(validationResult);   
            }
        }
        #endregion
    }
}
