using Newtonsoft.Json;

namespace ServiceCruiser.Model.Entities.Core.Infrastructure
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AppVersion: NotificationObject
    {
        #region state
        private string _version;
        [JsonProperty]
        public string Version
        {
            get { return _version;}
            set
            {
                _version = value;
                OnPropertyChanged(() => Version);
            }

        }

        private string _assemblyVersion;
        [JsonProperty]
        public string AssemblyVersion
        {
            get {  return _assemblyVersion;}
            set
            {
                _assemblyVersion = value;
                OnPropertyChanged(() => AssemblyVersion);
            }
        }

        private string _assemblyFileVersion;
        [JsonProperty]
        public string AssemblyFileVersion
        {
            get { return _assemblyFileVersion; }
            set
            {
                _assemblyFileVersion = value;
                OnPropertyChanged(() => AssemblyFileVersion);
            }
        }
        #endregion
    }
}
