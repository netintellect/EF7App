using ServiceCruiser.Model.Entities.Capacities;

namespace ServiceCruiser.Model.Entities
{
    public class TimeUnit  
    {
        #region state
        public int Length { get; set; }
        public TimeUnitsType Unit { get; set; }
        public TimeUnit(TimeUnitsType timeUnits, int length)
        {
            Unit = timeUnits;
            Length = length;
        }
        #endregion

        #region behavior

        public override string ToString()
        {
            return $"{Length} {Unit}";
        }

        #endregion
    }
}
