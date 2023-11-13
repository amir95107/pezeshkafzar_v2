namespace DataLayer
{
    using Pezeshkafzar_v2.Models;
    using System;

    public partial class UserRole : BaseEntity
    {

        public Guid RoleID { get; set; }
        public Guid UserId { get; set; }

        public virtual Roles Roles { get; set; }
        public virtual Users Users { get; set; }
    }
}
