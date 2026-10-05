using Microsoft.EntityFrameworkCore;
using SMS.API.Models;

namespace SMS.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<SalesHeader> SalesHeaders => Set<SalesHeader>();
    public DbSet<SalesDetail> SalesDetails => Set<SalesDetail>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("UserMaster");
            e.HasKey(x => x.UserId);
            e.HasIndex(x => x.Username).IsUnique();
        });

        modelBuilder.Entity<Customer>(e =>
        {
            e.ToTable("CustomerMaster");
            e.HasKey(x => x.CustomerId);
            e.HasIndex(x => x.Code).IsUnique();
        });

        modelBuilder.Entity<Item>(e =>
        {
            e.ToTable("ItemMaster");
            e.HasKey(x => x.ItemId);
            e.HasIndex(x => x.Code).IsUnique();
            e.Property(x => x.Cost).HasColumnType("decimal(18,2)");
            e.Property(x => x.SalesPrice).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<SalesHeader>(e =>
        {
            e.ToTable("SalesHeader");
            // Composite primary key as per spec: Invoice Type + Invoice Number
            e.HasKey(x => new { x.InvoiceType, x.InvoiceNumber });
            e.Property(x => x.TotalAmount).HasColumnType("decimal(18,2)");
            e.HasOne(x => x.Customer)
                .WithMany(c => c.SalesHeaders)
                .HasForeignKey(x => x.CustomerId);
        });

        modelBuilder.Entity<SalesDetail>(e =>
        {
            e.ToTable("SalesDetails");
            e.HasKey(x => x.SalesDetailId);
            e.Property(x => x.Qty).HasColumnType("decimal(18,2)");
            e.Property(x => x.Price).HasColumnType("decimal(18,2)");
            e.Ignore(x => x.Amount); // computed in DB as persisted column, EF just reads it via raw SQL/views if needed

            e.HasOne(x => x.SalesHeader)
                .WithMany(h => h.SalesDetails)
                .HasForeignKey(x => new { x.InvoiceType, x.InvoiceNumber });

            e.HasOne(x => x.Item)
                .WithMany()
                .HasForeignKey(x => x.ItemId);
        });

        base.OnModelCreating(modelBuilder);
    }
}
