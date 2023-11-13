namespace DataLayer
{
    using Pezeshkafzar_v2.Models;
    using System;

    public partial class Product_Features : BaseEntity
    {
        public Guid ProductID { get; set; }
        public Guid FeatureID { get; set; }
        public string Value { get; set; }
        public string ImageName { get; set; }

        public virtual Features Features { get; set; }
        public virtual Products Products { get; set; }
    }
}
