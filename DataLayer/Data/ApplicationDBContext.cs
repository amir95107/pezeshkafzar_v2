using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Data;

public static class EfExtensions
{
    public static void MapAuditableColumns(this EntityTypeBuilder modelBuilder)
    {
        modelBuilder.MapCreatableColumns();
        modelBuilder.MapModifiableColumns();
        modelBuilder.MapRemovableColumns();
    }

    public static void MapCreatableColumns(this EntityTypeBuilder modelBuilder)
    {
        modelBuilder.Property("CreatedAt")
            .HasColumnType("TIMESTAMP WITH TIME ZONE")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        modelBuilder.Property("CreatedBy")
            .HasColumnType("UUID")
            .IsRequired();
    }

    public static void MapModifiableColumns(this EntityTypeBuilder modelBuilder)
    {
        modelBuilder.Property("ModifiedAt")
            .HasColumnType("TIMESTAMP WITH TIME ZONE")
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

        modelBuilder.Property("ModifiedBy")
            .HasColumnType("UUID")
            .IsRequired();
    }

    public static void MapRemovableColumns(this EntityTypeBuilder modelBuilder)
    {
        modelBuilder.Property("RemovedAt")
            .HasColumnType("TIMESTAMP WITH TIME ZONE");

        modelBuilder.Property("RemovedBy")
            .HasColumnType("UUID");
    }
}
public class ApplicationDBContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid>
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
    public virtual DbSet<Slider> Slider { get; set; }
    public virtual DbSet<Page> Page { get; set; }
    public virtual DbSet<UserInfo> UserInfo { get; set; }
    public virtual DbSet<Users> Users { get; set; }
    public virtual DbSet<Comments> Comments { get; set; }
    public virtual DbSet<CallReport> CallReport { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        Config(modelBuilder);
    }

    private void Config(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Addresses>()
                    .HasOne(x => x.Users)
                    .WithMany(x => x.Addresses)
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Users_Addresses");

        modelBuilder.Entity<Comments>()
                    .HasOne(x => x.Blogs)
                    .WithMany(x => x.Comments)
                    .HasForeignKey(m => m.BlogID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Blogs_Comments");

        modelBuilder.Entity<Comments>()
                    .HasOne(x => x.Products)
                    .WithMany(x => x.Comments)
                    .HasForeignKey(m => m.ProductID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Products_Comments");

        modelBuilder.Entity<Comments>()
                    .HasOne(x => x.Parent)
                    .WithOne()
                    .HasForeignKey<Comments>(x => x.ParentID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Comments_Parent");

        modelBuilder.Entity<LikeProduct>()
                    .HasOne(x => x.Users)
                    .WithMany(x => x.LikeProduct)
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Users_LikeProduct");

        modelBuilder.Entity<LikeProduct>()
                    .HasOne(x => x.Products)
                    .WithMany(x => x.LikeProduct)
                    .HasForeignKey(m => m.ProductID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Products_LikeProduct");

        modelBuilder.Entity<OrderDetails>()
                    .HasOne(x => x.Products)
                    .WithMany(x => x.OrderDetails)
                    .HasForeignKey(m => m.ProductID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Product_OrderDetails");

        modelBuilder.Entity<OrderDetails>()
                    .HasOne(x => x.Orders)
                    .WithMany(x => x.OrderDetails)
                    .HasForeignKey(m => m.OrderID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Orders_OrderDetails");

        modelBuilder.Entity<Orders>()
                    .HasOne(x => x.Users)
                    .WithMany(x => x.Orders)
                    .HasForeignKey(m => m.UserId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_User_Orders");

        modelBuilder.Entity<Product_Features>()
                    .HasOne(x => x.Products)
                    .WithMany(x => x.Product_Features)
                    .HasForeignKey(m => m.ProductID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Products_Product_Features");

        modelBuilder.Entity<Product_Features>()
                    .HasOne(x => x.Features)
                    .WithMany(x => x.Product_Features)
                    .HasForeignKey(m => m.FeatureID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Features_Product_Features");

        modelBuilder.Entity<Product_Galleries>()
                    .HasOne(x => x.Products)
                    .WithMany(x => x.Product_Galleries)
                    .HasForeignKey(m => m.ProductID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Products_Product_Galleries");

        modelBuilder.Entity<Product_Groups>()
                    .HasOne(x => x.Parent)
                    .WithMany(x => x.Children)
                    .HasForeignKey(m => m.ParentID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Product_Groups_Product_Groups");

        modelBuilder.Entity<Product_Selected_Groups>()
                    .HasOne(x => x.Product_Groups)
                    .WithMany(x => x.Product_Selected_Groups)
                    .HasForeignKey(m => m.GroupID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Product_Groups_Product_Selected_Groups");

        modelBuilder.Entity<Product_Selected_Groups>()
                    .HasOne(x => x.Products)
                    .WithMany(x => x.Product_Selected_Groups)
                    .HasForeignKey(m => m.ProductID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Products_Product_Selected_Groups");

        modelBuilder.Entity<Product_Tags>()
                    .HasOne(x => x.Products)
                    .WithMany(x => x.Product_Tags)
                    .HasForeignKey(m => m.ProductID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Products_Product_Tags");

        modelBuilder.Entity<ProductBrand>()
                    .HasOne(x => x.Products)
                    .WithMany(x => x.ProductBrand)
                    .HasForeignKey(m => m.ProductID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Products_ProductBrand");

        modelBuilder.Entity<SpecialProducts>()
                    .HasOne(x => x.Products)
                    .WithMany(x => x.SpecialProducts)
                    .HasForeignKey(m => m.ProductID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Products_SpecialProducts");

        modelBuilder.Entity<ProductBrand>()
                    .HasOne(x => x.Brands)
                    .WithMany(x => x.ProductBrand)
                    .HasForeignKey(m => m.BrandID)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Brands_ProductBrand");
    }


}

