using ASP_Shop_App.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ASP_Shop_App.Data
{
    public class AppDbContext:IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions options):base(options) { }

        public virtual DbSet<AppUser> AppUsers { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categorys { get; set; }
        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                   .HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUser>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Order>()
                .HasOne(c => c.User)
                .WithOne(u => u.orders)
                .HasForeignKey<Order>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUser>()
                .HasOne(u => u.orders)
                .WithOne(c => c.User)
                .HasForeignKey<AppUser>(u => u.ordersID)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            foreach (var entry in this.ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity)
                {
                    ((BaseEntity)entry.Entity).ModifiedTime = DateTime.Now;

                    if (entry.State == EntityState.Added)
                    {
                        ((BaseEntity)entry.Entity).CreatedTime = DateTime.Now;
                    }
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
