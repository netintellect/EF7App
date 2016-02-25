using Newtonsoft.Json;

namespace ServiceCruiser.Model.Entities.Contracts
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DseSpecificationPower : DseSpecificationBase
    {
        #region state

        private double _startProportion;
        [JsonProperty]
        public double StartProportion
        {
            get { return _startProportion; }
            set { SetProperty(value, ref _startProportion, () => StartProportion); }
        }

        private double _endProportion;
        [JsonProperty]
        public double EndProportion
        {
            get { return _endProportion; }
            set { SetProperty(value, ref _endProportion, () => EndProportion); }
        }

        private double _curveShape;
        [JsonProperty]
        public double CurveShape
        {
            get { return _curveShape; }
            set { SetProperty(value, ref _curveShape, () => CurveShape); }
        }

        private double _ageingFactor;
        [JsonProperty]
        public double AgeingFactor
        {
            get { return _ageingFactor; }
            set { SetProperty(value, ref _ageingFactor, () => AgeingFactor); }
        }

        #endregion
    }
}
