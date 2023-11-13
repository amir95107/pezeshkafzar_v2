namespace DataLayer
{
    using Pezeshkafzar_v2.Models;

    public partial class UserInfo : BaseEntity
    {
        public Guid UserID { get; set; }
        public string FullName { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }

        public virtual Users Users { get; set; }
    }
}
