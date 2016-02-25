using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using KeyNew = ServiceCruiser.Model.Entities.Core.Attributes.KeyAttribute;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Photo : ValidatedEntity<Photo>
    {
        #region state
        private int _id;
        [JsonProperty, KeyNew(true)]
        public int Id
        {
            get { return _id; }
            set { SetProperty(value, ref _id, () => Id); }
        }
        private int _partId;
        [JsonProperty]
        public int PartId
        {
            get { return _partId; }
            set { SetProperty(value, ref _partId, () => PartId); }
        }
        private Part _part;
        [HandleOnNesting]
        [Aggregation(isComposite: false)]
        [JsonProperty]
        public Part Part
        {
            get { return _part; }
            set
            {
                _part = value;
                OnPropertyChanged(() => Part);
            }
        }
        private string _name;
        [JsonProperty]
        public string Name
        {
            get { return _name; }
            set { SetProperty(value, ref _name, () => Name); }
        }

        private PhotoType _type;
        [JsonProperty]
        public PhotoType Type
        {
            get { return _type; }
            set { SetProperty(value, ref _type, () => Type); }
        }

        private string _filePath;
        [JsonProperty, IgnoreOnMap]
        public string FilePath
        {
            get { return _filePath; }
            set
            {
                _filePath = value;
                OnPropertyChanged(() => FilePath);
            }
        }

        private byte[] _fileBinary;
        public byte[] FileBinary
        {
            get { return _fileBinary; }
            set { SetProperty(value, ref _fileBinary, () => FileBinary); }
        }
        #endregion

        #region behavior
        public void SetFileBinary()
        {
            
            
        }
        #endregion
    }
}
