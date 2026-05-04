using Car_Dealership_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Car_Dealership_System.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<UserProfile> UserProfiles => Set<UserProfile>();
        public DbSet<Car> Cars => Set<Car>();
        public DbSet<ClientInterest> ClientInterests => Set<ClientInterest>();
        public DbSet<Reservation> Reservations => Set<Reservation>();
        public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.CarsSold)
                .WithOne(c => c.Seller)
                .HasForeignKey(c => c.SellerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientInterest>()
                .HasOne(ci => ci.Client)
                .WithMany(u => u.Interests)
                .HasForeignKey(ci => ci.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientInterest>()
                .HasOne(ci => ci.Car)
                .WithMany(c => c.Interests)
                .HasForeignKey(ci => ci.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Client)
                .WithMany(u => u.Reservations)
                .HasForeignKey(r => r.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Reservation>()
                .HasOne(r => r.Car)
                .WithMany(c => c.Reservations)
                .HasForeignKey(r => r.CarId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>()
                .HasMany(u => u.RefreshTokens)
                .WithOne(rt => rt.User)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ClientInterest>()
                .HasIndex(ci => new { ci.ClientId, ci.CarId })
                .IsUnique();
        }
    }
}