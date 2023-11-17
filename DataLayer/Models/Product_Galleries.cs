using DataLayer.Models;

namespace DataLayer.Models
{

    public partial class Product_Galleries : BaseEntity
    {
        public Guid ProductID { get; set; }
        public string ImageName { get; set; }
        public string Title { get; set; }

        public virtual Products Products { get; set; }
    }
}
