namespace DataLayer.Models
{
    using System.Collections.Generic;

    public partial class Blogs: BaseEntity
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Text { get; set; }
        public string ImageName { get; set; }
        public string Author { get; set; }
        public string Tags { get; set; }
        public DateTime CreateDate { get; set; }
        public int Visit { get; set; }
        public string SefUrl { get; set; }
    
        public virtual ICollection<Comments> Comments { get; set; } = new HashSet<Comments>();
    }
}
