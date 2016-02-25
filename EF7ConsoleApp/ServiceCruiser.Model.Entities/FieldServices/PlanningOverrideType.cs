using System;

namespace ServiceCruiser.Model.Entities.FieldServices
{
    [Flags]
    public enum PlanningOverrideType 
    {
        None = 0,
        Window = 2,
        Start = 4,
        Technician = 8,
        WindowTech = 64,
        StartTech = 128
    }
}
