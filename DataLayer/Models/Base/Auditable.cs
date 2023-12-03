namespace DataLayer.Models.Base
{
    public abstract class Auditable : Creatable
    {
        public DateTime? ModifiedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? RemovedAt { get; set; }
        public Guid? RemovedBy { get; set; }

        public Auditable()
        {
            ModifiedAt = DateTime.Now;
            ModifiedBy = Guid.Parse("0e593ea8-bd94-4986-bc72-08dbec385855");
        }
    }
}
