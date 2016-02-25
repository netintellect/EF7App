namespace ServiceCruiser.Model.Entities.Core
{
    public interface IAuditEntity
    {
        void SetAuditInfo(string userId);
        IAuditInfo AuditInfo { get; set; }
    }
}
