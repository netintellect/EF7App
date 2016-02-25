using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Core.Extensibility;
using ServiceCruiser.Model.Entities.Resources;
using ServiceCruiser.Model.Entities.Users;
using ServiceCruiser.Model.Validations.Core.Validators;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn), DeletePriority(90)]
    public class Contact : ValidatedEntity<Contact>
    {
        #region state
        private int _id;
		[JsonProperty] [KeyNew(true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}
        
        private string _firstName;
        /*
        [NotNullValidator(MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "ContactFirstNameRequired")]
        [StringLengthValidator(0, RangeBoundaryType.Inclusive, 150, RangeBoundaryType.Inclusive,
                                MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "ContactFirstNameRange")]
        [Display(ResourceType = typeof(Translations), Name ="ContactFirstName")]
        */
		[JsonProperty]
		public string FirstName 
		{ 
 			get {   return _firstName;}
            set
            {
                if (SetProperty(value, ref _firstName, () => FirstName))
                {
                    OnPropertyChanged(() => Name);
                }
            } 
		}

        private string _lastName;
        [NotNullValidator(MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "ContactLastNameRange")]
        [StringLengthValidator(1, RangeBoundaryType.Inclusive, 150, RangeBoundaryType.Inclusive,
                                MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "ContactLastNameRange")]
        [Display(ResourceType = typeof(Translations), Name = "ContactLastName")]
        [JsonProperty]
        public string LastName
        {
            get {   return _lastName; }
            set
            {
                if (SetProperty(value, ref _lastName, () => LastName))
                {
                    OnPropertyChanged(() => Name);   
                }
            }
        }

        private string _prefix;
        [ValidatorComposition(CompositionType.Or)]
        [NotNullValidator(Negated = true)]
        [StringLengthValidator(1, RangeBoundaryType.Inclusive, 150, RangeBoundaryType.Inclusive, MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "ContactPrefixRange")]
        [JsonProperty]
        public string Prefix
        {
            get { return _prefix; }
            set { SetProperty(value, ref _prefix, () => Prefix); }
        }

        public string Name
        {
            get
            {
                return (FirstName ?? "" + " " + LastName ?? "").Trim();
            }
        }

        private string _language;
        [NotNullValidator(MessageTemplateResourceType = typeof(Translations), MessageTemplateResourceName = "ContactLanguageRequired")]
        [Display(ResourceType = typeof(Translations), Name = "ContactLanguage")]
        [JsonProperty]
        public string Language
        {
            get {   return _language; }
            set {   SetProperty(value, ref _language, () => Language);  }
        }
        
        private string _comment;
        [JsonProperty]
        public string Comment
        {
            get {   return _comment; }
            set {   SetProperty(value, ref _comment, () => Comment);    }
        }
        
        private bool _isMain;
        [JsonProperty]
        public bool IsMain
        {
            get {   return _isMain; }
            set {   SetProperty(value, ref _isMain, () => IsMain);  }
        }

        private int? _locationId;
        [JsonProperty]
        public int? LocationId
        {
            get {   return _locationId; }
            set {   SetProperty(value, ref _locationId, () => LocationId);  }
        }

        private Location _location;
        [JsonProperty]
        public Location Location
        {
            get { return _location; }
            set 
            {
                _location = value;
                OnPropertyChanged(() => Location);
            }
        }
        
        private ObservableCollection<ContactTool> _contactTools = new ObservableCollection<ContactTool>();
        [JsonProperty, HandleOnNesting, Aggregation(isComposite: true)]
        public ICollection<ContactTool> ContactTools
        {
            get { return _contactTools; }
            set { _contactTools = value != null ? value.ToObservableCollection() : null; }
        }

        public string Phone
        {
            get
            {
                var contactTool = ContactTools.FirstOrDefault(ct => ct.ToolType == ContactToolType.FixedPhone);
                return contactTool == null ? null : contactTool.ToolValue;
            }
            set
            {
                var contactTool = HandleContactTool(ContactToolType.FixedPhone);
                if (contactTool != null) contactTool.ToolValue = value;
            }
        }

        public string Mobile
        {
            get
            {
                var contactTool = ContactTools.FirstOrDefault(ct => ct.ToolType == ContactToolType.MobilePhone);
                return contactTool == null ? null : contactTool.ToolValue;
            }
            set
            {
                var contactTool = HandleContactTool(ContactToolType.MobilePhone);
                if (contactTool != null) contactTool.ToolValue = value;
            }
        }

        public string Email
        {
            get
            {
                var contactTool = ContactTools.FirstOrDefault(ct => ct.ToolType == ContactToolType.Email);
                return contactTool == null ? null : contactTool.ToolValue;
            }
            set
            {
                var contactTool = HandleContactTool(ContactToolType.Email);
                if (contactTool != null) contactTool.ToolValue = value;
            }
        }
        #endregion

        #region behavior

        public static Contact Create(Location location, User user)
        {
            if (location == null) return null;

            var contact = new Contact
            {
                Location = location,
                LocationId = location.Id,
                Language = "nl"
            };
            user?.SetAuditInfo(user.Login);
            return contact;
        }

        private ContactTool HandleContactTool(ContactToolType type)
        {
            var contactTool = ContactTools.FirstOrDefault(ct => ct.ToolType == type);
            if (contactTool == null)
            {
                contactTool = ContactTool.Create(type);
                ContactTools.Add(contactTool);
            }
            return contactTool;
        }

        #endregion
    }
}
