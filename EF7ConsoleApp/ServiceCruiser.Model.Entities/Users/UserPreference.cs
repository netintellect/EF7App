using System.Text.RegularExpressions;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Contracts;
using ServiceCruiser.Model.Entities.Core;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;
using ServiceCruiser.Model.Entities.Core.Attributes;

namespace ServiceCruiser.Model.Entities.Users
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UserPreferences : ValidatedEntity<UserPreferences>
    {
        #region state
        private int _id;
        [JsonProperty, KeyNew(isIdentity: true)]
        public int UserId
        {
            get { return _id; }
            set { SetProperty(value, ref _id, () => UserId); }
        }

        private bool _showWizardOnStartup;
        [JsonProperty]
        public bool ShowWizardOnStartup
        {
            get { return _showWizardOnStartup; }
            set { SetProperty(value, ref _showWizardOnStartup, () => ShowWizardOnStartup); }
        }

        private ThemeColorType _themeColor;
        [JsonProperty]
        public ThemeColorType ThemeColor
        {
            get { return _themeColor; }
            set { SetProperty(value, ref _themeColor, () => ThemeColor); }
        }

        private User _user;
        [JsonProperty, Aggregation(isComposite: false)]
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged(() => User);
            }
        }
        #endregion
    }
}
