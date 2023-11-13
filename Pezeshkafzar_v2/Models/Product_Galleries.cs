using Pezeshkafzar_v2.Models;

namespace DataLayer
{

    public partial class Product_Galleries : BaseEntity
    {
        public Guid ProductID { get; set; }
        public string ImageName { get; set; }
        public string Title { get; set; }

        public virtual Products Products { get; set; }
    }
}
