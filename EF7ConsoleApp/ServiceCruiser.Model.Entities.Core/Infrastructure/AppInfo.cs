using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;

namespace ServiceCruiser.Model.Entities.Core.Infrastructure
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AppInfo : NotificationObject
    {
        #region state
        private string _bingApplicationKey;
        [JsonProperty]
        public string BingApplicationKey
        {
            get { return _bingApplicationKey; }
            set
            {
                _bingApplicationKey = value;
                OnPropertyChanged(() => BingApplicationKey);
            }
        }

        private string _scheduling360Url;
        [JsonProperty]
        public string Scheduling360Url
        {
            get { return _scheduling360Url; }
            set
            {
                _scheduling360Url = value;
                OnPropertyChanged(() => Scheduling360Url);
            }
        }

        private readonly ObservableCollection<Country> _possibleCountries = new ObservableCollection<Country>();
        [JsonProperty]
        public ICollection<Country> PossibleCountries => _possibleCountries;

        private readonly ObservableCollection<Language> _possibleLanguages = new ObservableCollection<Language>();
        [JsonProperty]
        public ICollection<Language> PossibleLanguages => _possibleLanguages;

        private readonly  ObservableCollection<CodeGroup> _possibleCodeGroups = new ObservableCollection<CodeGroup>();
        [JsonProperty]
        public ICollection<CodeGroup> PossibleCodeGroups => _possibleCodeGroups;

        private string _storageConnectionString;
        [JsonProperty]
        public string StorageConnectionString
        {
            get { return _storageConnectionString; }
            set
            {
                _storageConnectionString = value;
                OnPropertyChanged(() => StorageConnectionString);
            }
        }

        private string _databaseConnectionString;
        [JsonProperty]
        public string DatabaseConnectionString
        {
            get { return _databaseConnectionString; }
            set
            {
                _databaseConnectionString = value;
                OnPropertyChanged(() => DatabaseConnectionString);
            }
        }

        private string _backEndUrl;
        [JsonProperty]
        public string BackEndUrl
        {
            get { return _backEndUrl; }
            set
            {
                _backEndUrl = value;
                OnPropertyChanged(() => BackEndUrl);
            }
        }

        private RunEnvironmentType? _runEnvironmentType;
        public RunEnvironmentType? RunEnvironmentType
        {
            get { return _runEnvironmentType; }
            set
            {
                _runEnvironmentType = value;
                OnPropertyChanged(() => RunEnvironmentType);
            }
        }

        private string _buildEnvironment;
        [JsonProperty]
        public string BuildEnvironment
        {
            get { return _buildEnvironment;}
            set
            {
                _buildEnvironment = value;
                OnPropertyChanged(() => BuildEnvironment);
            }
        }

        private ProductVersion _productVersion;
        public ProductVersion ProductVersion
        {
            get { return _productVersion;}
            set
            {
                _productVersion = value;
                OnPropertyChanged(() => ProductVersion);
            }
        }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(BingApplicationKey) &&
                       !PossibleCountries.Any() &&
                       !PossibleLanguages.Any() &&
                       !PossibleCodeGroups.Any();
            }
        }

        #endregion
    }
}
