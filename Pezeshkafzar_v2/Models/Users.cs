namespace DataLayer
{
    using Pezeshkafzar_v2.Models;
    using System;
    using System.Collections.Generic;

    public partial class Users : BaseEntity
    {

        public Guid RoleID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ActiveCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime RegisterDate { get; set; }
        public bool IsUserInfoCompleted { get; set; }
        public string Mobile { get; set; }
        public bool IsMobileConfirmed { get; set; }
        public int? LoginAtempts { get; set; }
        public bool? IsLocked { get; set; }

        public virtual ICollection<Addresses> Addresses { get; set; } = new HashSet<Addresses>();
        public virtual ICollection<ContactForm> ContactForm { get; set; } = new HashSet<ContactForm>();
        public virtual ICollection<Discounts> Discounts { get; set; } = new HashSet<Discounts>();
        public virtual ICollection<LikeProduct> LikeProduct { get; set; } = new HashSet<LikeProduct>();
        public virtual ICollection<Orders> Orders { get; set; } = new HashSet<Orders>();
        public virtual Roles Roles { get; set; }
        public virtual ICollection<UserInfo> UserInfo { get; set; } = new HashSet<UserInfo>();
        public virtual ICollection<Sellers> Sellers { get; set; } = new HashSet<Sellers>();
    }
}
