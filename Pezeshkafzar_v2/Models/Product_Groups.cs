namespace DataLayer
{
    using Pezeshkafzar_v2.Models;
    using System.Collections.Generic;

    public partial class Product_Groups : BaseEntity
    {
        public string GroupTitle { get; set; }
        public Guid? ParentID { get; set; }
        public bool IsDeleted { get; set; }
        public int ShowOrder { get; set; }

        public virtual Product_Groups Parent { get; set; }
        public virtual ICollection<Product_Selected_Groups> Product_Selected_Groups { get; set; } = new HashSet<Product_Selected_Groups>();
    }
}
