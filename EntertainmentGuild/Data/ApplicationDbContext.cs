#nullable disable
using EntertainmentGuild.Models;
using Microsoft.EntityFrameworkCore;

namespace EntertainmentGuild.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public virtual DbSet<Book_genre> Book_genre { get; set; }
        public virtual DbSet<Book_genre_NEW> Book_genre_NEW { get; set; }
        public virtual DbSet<Game_genre> Game_genre { get; set; }
        public virtual DbSet<Genre> Genres { get; set; }
        public virtual DbSet<Movie_genre> Movie_genre { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Patron> Patrons { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Source> Sources { get; set; }
        public virtual DbSet<Stocktake> Stocktakes { get; set; }
        public virtual DbSet<TO> TOes { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ProductInOrders> ProductsInOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           

            modelBuilder.Entity<Genre>()
                .HasMany(e => e.Products)
                .WithOne(e => e.Genre1)
                .HasForeignKey(e => e.Genre);

            modelBuilder.Entity<Genre>()
                .HasMany(e => e.Sources)
                .WithOne(e => e.Genre1)
                .HasForeignKey(e => e.Genre);

            modelBuilder.Entity<Patron>()
                .Property(e => e.Salt)
                .IsUnicode(false);

            modelBuilder.Entity<Patron>()
                .Property(e => e.HashPW)
                .IsUnicode(false);

            modelBuilder.Entity<Patron>()
                .HasMany(e => e.TOes)
                .WithOne(e => e.Patron)
                .HasForeignKey(e => e.PatronId);

            modelBuilder.Entity<TO>()
                .Property(e => e.Expiry)
                .IsUnicode(false);

            modelBuilder.Entity<TO>()
                .HasMany(e => e.Orders)
                .WithOne(e => e.TO)
                .HasForeignKey(e => e.customer);

            modelBuilder.Entity<User>()
                .Property(e => e.Salt)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.HashPW)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Products)
                .WithOne(e => e.User)
                .HasForeignKey(e => e.LastUpdatedBy);
        }      

    }
}
