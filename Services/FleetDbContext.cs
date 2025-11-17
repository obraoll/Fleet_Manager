using Microsoft.EntityFrameworkCore;
using FleetManager.Models;

namespace FleetManager.Services
{
    /// <summary>
    /// Contexte de base de données Entity Framework pour Fleet Manager
    /// </summary>
    public class FleetDbContext : DbContext
    {
        public FleetDbContext(DbContextOptions<FleetDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Vehicle> Vehicles { get; set; } = null!;
        public DbSet<FuelRecord> FuelRecords { get; set; } = null!;
        public DbSet<MaintenanceRecord> MaintenanceRecords { get; set; } = null!;
        public DbSet<Driver> Drivers { get; set; } = null!;
        public DbSet<VehicleAssignment> VehicleAssignments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration des enums pour MySQL
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.Property(v => v.VehicleType).HasConversion<string>();
                entity.Property(v => v.FuelType).HasConversion<string>();
                entity.Property(v => v.Status).HasConversion<string>();

                // Index
                entity.HasIndex(v => v.RegistrationNumber).IsUnique();
                entity.HasIndex(v => v.VIN).IsUnique();
            });

            modelBuilder.Entity<FuelRecord>(entity =>
            {
                entity.Property(f => f.FuelType).HasConversion<string>();

                // Relations
                entity.HasOne(f => f.Vehicle)
                    .WithMany(v => v.FuelRecords)
                    .HasForeignKey(f => f.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(f => f.Creator)
                    .WithMany(u => u.FuelRecords)
                    .HasForeignKey(f => f.CreatedBy)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<MaintenanceRecord>(entity =>
            {
                entity.Property(m => m.MaintenanceType).HasConversion<string>();

                // Relations
                entity.HasOne(m => m.Vehicle)
                    .WithMany(v => v.MaintenanceRecords)
                    .HasForeignKey(m => m.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(m => m.Creator)
                    .WithMany(u => u.MaintenanceRecords)
                    .HasForeignKey(m => m.CreatedBy)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<Driver>(entity =>
            {
                entity.Property(d => d.Status).HasConversion<string>();
                entity.HasIndex(d => d.LicenseNumber).IsUnique();
            });

            modelBuilder.Entity<VehicleAssignment>(entity =>
            {
                entity.HasOne(va => va.Vehicle)
                    .WithMany(v => v.VehicleAssignments)
                    .HasForeignKey(va => va.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(va => va.Driver)
                    .WithMany(d => d.VehicleAssignments)
                    .HasForeignKey(va => va.DriverId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
