using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Resources;
using System.ComponentModel.DataAnnotations;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Users
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Credentials : ValidatedEntity<Credentials>
    {
        #region state
        private int _id;
        [JsonProperty] [KeyNew(true)]
        [Display(ResourceType = typeof(Translations), Name = "CredentialsId")]
        public int Id
        {
            get {   return _id; }
            set {   SetProperty(value, ref _id, () => Id);  }
        }
        
        private CredentialType _target = CredentialType.ForServiceCruiser;
        [JsonProperty]
        public CredentialType Target
        {
            get { return _target; }
            set { SetProperty(value, ref _target, () => Target); }
        }

        private string _login;
        [JsonProperty]
        [Display(ResourceType = typeof(Translations), Name = "CredentialsLogin")]
        public string Login
        {
            get {   return _login; }
            set
            {
                if (SetProperty(value, ref _login, () => Login))
                {
                    OnPropertyChanged(() => IsProvided);
                }
            }
        }
        
        private int _userId;
        [JsonProperty]
        public int UserId
        {
            get {   return _userId; }
            set {   SetProperty(value, ref _userId, () => UserId);  }
        }
        
        private string _password;
        [JsonProperty]
        public string Password
        {
            get { return _password; }
            set
            {
                if (SetProperty(value, ref _password, () => Password))
                {
                    OnPropertyChanged(() => IsProvided);
                }
            }
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

        private string _session;
        [JsonProperty, IgnoreOnMap]
        public string Session
        {
            get { return _session; }
            set
            {
                _session = value;
                OnPropertyChanged(() => Session);
                OnPropertyChanged(() => IsInUse);
            }
        }

        public bool  IsInUse
        {
            get { return !string.IsNullOrEmpty(Session); }
        }

        public bool IsProvided
        {
            get
            {
                return (!string.IsNullOrEmpty(Login) &&
                        !string.IsNullOrEmpty(Password));
            }
        }
        #endregion

        #region behavior

        public void Reset()
        {
            Login = string.Empty;
            Password = string.Empty;
        }
        #endregion
    }
}
