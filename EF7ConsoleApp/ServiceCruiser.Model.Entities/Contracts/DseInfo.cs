using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ServiceCruiser.Model.Entities.Core;
using ServiceCruiser.Model.Entities.Core.Attributes;
using ServiceCruiser.Model.Entities.Resources;

namespace ServiceCruiser.Model.Entities.Contracts
{
    public class DseInfo : ValidatedEntity<DseInfo>
    {
        #region state
        private int _id;
        [JsonProperty]
        [Key(true)]
        public int Id
        {
            get { return _id; }
            set { SetProperty(value, ref _id, () => Id); }
        }

        private string _code;
        [JsonProperty]
        public string Code
        {
            get { return _code; }
            set { SetProperty(value, ref _code, () => Code); }
        }

        private ScheduleModeType _scheduleMode ;
        [JsonProperty]
        public ScheduleModeType ScheduleMode
        {
            get { return _scheduleMode; }
            set
            {
                if (SetProperty(value, ref _scheduleMode, () => ScheduleMode))
                {
                    OnPropertyChanged(() => DisplayScheduleMode);
                }
            }
        }

        private int _baseValue;
        [JsonProperty]
        public int BaseValue
        {
            get { return _baseValue; }
            set { SetProperty(value, ref _baseValue, () => BaseValue); }
        }

        private List<DseSpecificationBase> _dseSpecifications = new List<DseSpecificationBase>();
        [JsonProperty]
        public List<DseSpecificationBase> DseSpecifications
        {
            get { return _dseSpecifications; }
            set { _dseSpecifications = value; }
        }

        private static IDictionary<int, string> _possibleScheduleModes;
        public static IDictionary<int, string> PossibleScheduleModes
        {
            get { return _possibleScheduleModes = _possibleScheduleModes ?? CreatePossibleScheduleModes(); }
        }

        public string DisplayScheduleMode
        {
            get
            {
                string translation;
                if (PossibleScheduleModes.TryGetValue((int)ScheduleMode, out translation))
                    return translation;
                return "?";
            }
        }
        #endregion

        #region behavior
        private static IDictionary<int, string> CreatePossibleScheduleModes()
        {
            IDictionary<int, string> modes = new Dictionary<int, string>();
            foreach (var enumValue in Enum.GetValues(typeof(ScheduleModeType)))
            {
                string translation = string.Empty;
                switch ((int)enumValue)
                {
                    case (int)ScheduleModeType.Collect:
                        translation = Translations.SchedulingModeCollect;
                        break;
                    case (int)ScheduleModeType.Normal:
                        translation = Translations.SchedulingModeNormal;
                        break;
                    case (int)ScheduleModeType.Pool:
                        translation = Translations.SchedulingModePool;
                        break;
                    case (int)ScheduleModeType.Urgent:
                        translation = Translations.SchedulingModeUrgent;
                        break;
                }
                modes.Add((int)enumValue, translation);
            }
            return modes;
        }
        #endregion

    }
}
