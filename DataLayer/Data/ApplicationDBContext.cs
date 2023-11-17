using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Data;
public class ApplicationDBContext : DbContext
{
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    {

    }

    public virtual DbSet<Addresses> Addresses { get; set; }
    public virtual DbSet<Blogs> Blogs { get; set; }
    public virtual DbSet<Brands> Brands { get; set; }
    public virtual DbSet<ContactForm> ContactForm { get; set; }
    public virtual DbSet<DeliveryWays> DeliveryWays { get; set; }
    public virtual DbSet<Discounts> Discounts { get; set; }
    public virtual DbSet<Features> Features { get; set; }
    public virtual DbSet<Lead_Clients> Lead_Clients { get; set; }
    public virtual DbSet<LikeProduct> LikeProduct { get; set; }
    public virtual DbSet<OrderDetails> OrderDetails { get; set; }
    public virtual DbSet<Orders> Orders { get; set; }
    public virtual DbSet<Product_Features> Product_Features { get; set; }
    public virtual DbSet<Product_Galleries> Product_Galleries { get; set; }
    public virtual DbSet<Product_Groups> Product_Groups { get; set; }
    public virtual DbSet<Product_Selected_Groups> Product_Selected_Groups { get; set; }
    public virtual DbSet<Product_Tags> Product_Tags { get; set; }
    public virtual DbSet<ProductBrand> ProductBrand { get; set; }
    public virtual DbSet<Products> Products { get; set; }
    public virtual DbSet<Roles> Roles { get; set; }
    public virtual DbSet<Slider> Slider { get; set; }
    public virtual DbSet<Page> Page { get; set; }
    public virtual DbSet<UserInfo> UserInfo { get; set; }
    public virtual DbSet<Users> Users { get; set; }
    public virtual DbSet<Sellers> Sellers { get; set; }
    public virtual DbSet<Comments> Comments { get; set; }
    public virtual DbSet<CallReport> CallReport { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Addresses>()
                    .HasOne(x => x.Users)
                    .WithMany()
                    .HasForeignKey(m => m.UserID)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Comments>()
                    .HasOne(x => x.Blogs)
                    .WithMany()
                    .HasForeignKey(m => m.BlogID)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Comments>()
                    .HasOne(x => x.Products)
                    .WithMany()
                    .HasForeignKey(m => m.ProductID)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LikeProduct>()
                    .HasOne(x => x.Users)
                    .WithMany()
                    .HasForeignKey(m => m.UserID)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LikeProduct>()
                    .HasOne(x => x.Products)
                    .WithMany()
                    .HasForeignKey(m => m.ProductID)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderDetails>()
                    .HasOne(x => x.Orders)
                    .WithMany()
                    .HasForeignKey(m => m.OrderID)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product_Features>()
                    .HasOne(x => x.Products)
                    .WithMany()
                    .HasForeignKey(m => m.ProductID)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product_Features>()
                    .HasOne(x => x.Features)
                    .WithMany()
                    .HasForeignKey(m => m.FeatureID)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product_Galleries>()
                    .HasOne(x => x.Products)
                    .WithMany()
                    .HasForeignKey(m => m.ProductID)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product_Groups>()
                    .HasOne(x => x.Parent)
                    .WithMany()
                    .HasForeignKey(m => m.ParentID)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product_Selected_Groups>()
                    .HasOne(x => x.Product_Groups)
                    .WithMany()
                    .HasForeignKey(m => m.GroupID)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product_Selected_Groups>()
                    .HasOne(x => x.Products)
                    .WithMany()
                    .HasForeignKey(m => m.ProductID)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product_Tags>()
                    .HasOne(x => x.Products)
                    .WithMany()
                    .HasForeignKey(m => m.ProductID)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductBrand>()
                    .HasOne(x => x.Products)
                    .WithMany()
                    .HasForeignKey(m => m.ProductID)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserRole>()
                    .HasOne(x => x.Users)
                    .WithMany()
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserRole>()
                    .HasOne(x => x.Roles)
                    .WithMany()
                    .HasForeignKey(m => m.RoleID)
                    .OnDelete(DeleteBehavior.Restrict);
    }
}

