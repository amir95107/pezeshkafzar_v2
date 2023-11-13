namespace Pezeshkafzar_v2.Models
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime ModifiedAt { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
