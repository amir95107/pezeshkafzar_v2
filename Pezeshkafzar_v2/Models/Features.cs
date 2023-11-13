namespace DataLayer
{
    using Pezeshkafzar_v2.Models;
    using System.Collections.Generic;

    public partial class Features : BaseEntity
    {
        public int FeatureID { get; set; }
        public string FeatureTitle { get; set; }

        public virtual ICollection<Product_Features> Product_Features { get; set; } = new HashSet<Product_Features>();
    }
}
