namespace DataLayer
{
    using Pezeshkafzar_v2.Models;
    using System;

    public partial class Comments : BaseEntity
    {
        public Guid? ProductID { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid? ParentID { get; set; }
        public bool IsShow { get; set; }
        public Guid? BlogID { get; set; }

        public virtual Blogs Blogs { get; set; }
        public virtual Products Products { get; set; }
    }
}
