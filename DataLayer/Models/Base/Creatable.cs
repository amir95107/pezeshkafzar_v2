namespace DataLayer.Models.Base
{
    public abstract class Creatable : EntityBase<Guid>
    {
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }

        public Creatable()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            CreatedBy = Guid.Parse("0e593ea8-bd94-4986-bc72-08dbec385855");
        }
    }
}
