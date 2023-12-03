using DataLayer.Models.Base;

namespace DataLayer.Models
{
    public partial class Addresses : GuidAuditableAggregateRoot
    {
        public Guid UserId { get; set; }
        public string Address { get; set; }
        public string Long { get; set; }
        public string Lat { get; set; }
        public bool IsDefault { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }

        public virtual Users Users { get; set; }

        
    }
}
