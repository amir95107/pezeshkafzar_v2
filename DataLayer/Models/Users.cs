namespace DataLayer.Models
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;

    public partial class Users : IdentityUser<Guid>
    {
        public bool IsUserInfoCompleted { get; set; }
        public bool? IsLocked { get; set; }
        public bool IsActive { get; set; } = true;

        public virtual ICollection<Addresses> Addresses { get; set; } = new HashSet<Addresses>();
        public virtual ICollection<ContactForm> ContactForm { get; set; } = new HashSet<ContactForm>();
        public virtual ICollection<Discounts> Discounts { get; set; } = new HashSet<Discounts>();
        public virtual ICollection<LikeProduct> LikeProduct { get; set; } = new HashSet<LikeProduct>();
        public virtual ICollection<Orders> Orders { get; set; } = new HashSet<Orders>();
        public virtual ICollection<UserInfo> UserInfo { get; set; } = new HashSet<UserInfo>();
    }
}
