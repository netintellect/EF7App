namespace ServiceCruiser.Model.Entities.Core.Infrastructure
{
    public enum SeedingConfigType
    {
        None,
        Alain,
        BasicScenario,  // minimal seed
        DseScenario,    // seeding with integration with dse
        EnecoToon,      // seeding for Eneco Toon project 
        DeltaModemSwap  // seeding for Delta Modem Swap
    }
}
