namespace DataLayer
{
    using Pezeshkafzar_v2.Models;

    public partial class Product_Tags : BaseEntity
    {
        public Guid ProductID { get; set; }
        public string Tag { get; set; }

        public virtual Products Products { get; set; }
    }
}
