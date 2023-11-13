namespace DataLayer
{
    using Pezeshkafzar_v2.Models;

    public partial class Lead_Clients : BaseEntity
    {
        public string Mobile { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
