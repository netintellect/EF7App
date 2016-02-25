using System;

namespace ServiceCruiser.Model.Entities
{
    public interface IAuditInfo
    {
        DateTimeOffset? CreatedOn { get; set; }
        string CreatedBy { get; set; }
        DateTimeOffset? ModifiedOn { get; set; }
        string ModifiedBy { get; set; }
        byte[] RowVersion { get; set; }
    }
}
