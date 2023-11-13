using Pezeshkafzar_v2.Models;

namespace DataLayer
{
    public partial class Addresses : BaseEntity
    {
        public Guid UserID { get; set; }
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
