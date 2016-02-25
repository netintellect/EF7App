using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Users;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [JsonObject(MemberSerialization.OptIn), DeletePriority(91)]
    public class ContactTool : ValidatedEntity<ContactTool>
    {
        #region state
        private int _id;
		[JsonProperty] [KeyNew(true)]
		public int Id 
		{ 
 			get {   return _id; } 
 			set {   SetProperty(value,ref _id, () => Id);   } 
		}
    			
		private int? _contactId;
		[JsonProperty]
		public int? ContactId 
		{ 
 			get {   return _contactId;  } 
 			set {   SetProperty(value,ref _contactId, () => ContactId); } 
		}
    			
        private ContactToolType _toolType;
		[JsonProperty]
		public ContactToolType ToolType 
		{ 
 			get {   return _toolType;   } 
 			set {   SetProperty(value,ref _toolType, () => ToolType);   } 
		}
    			
		private string _toolValue;
		[JsonProperty]
		public string ToolValue 
		{ 
 			get {   return _toolValue;  } 
 			set {   SetProperty(value,ref _toolValue, () => ToolValue); } 
		}
    			
        private string _info;
		[JsonProperty]
		public string Info 
		{ 
 			get {   return _info;   } 
 			set {   SetProperty(value,ref _info, () => Info);   } 
		}
    	
		private Contact _contact;
		[JsonProperty]
		public Contact Contact 
		{
			get {   return _contact;    } 
			set 
            {   
				_contact= value;
				OnPropertyChanged(() => Contact);
			}
        }

        #endregion

        #region behavior

        public static ContactTool Create(ContactToolType contactToolType)
        {
            return new ContactTool
            {
                ToolType = contactToolType
            };
        }

        public static ContactTool Create(ContactToolType contactToolType, Contact contact, User user)
        {
            if (contact == null) return null;

            var contactTool = Create(contactToolType);
            if (user != null) contactTool.SetAuditInfo(user.Login);
            contactTool.Contact = contact;
            contactTool.ContactId = contactTool.Id;
            return contactTool;
        }

        #endregion
    }
}
