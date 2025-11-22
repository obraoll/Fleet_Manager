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
        // MaintenanceRecord retiré du DbContext - utilisation d'ADO.NET uniquement
        public DbSet<Driver> Drivers { get; set; } = null!;
        public DbSet<VehicleAssignment> VehicleAssignments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Index
            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasIndex(v => v.RegistrationNumber).IsUnique();
            });

            modelBuilder.Entity<FuelRecord>(entity =>
            {
                entity.HasOne(f => f.Vehicle)
                    .WithMany(v => v.FuelRecords)
                    .HasForeignKey(f => f.VehicleId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(f => f.Driver)
                    .WithMany()
                    .HasForeignKey(f => f.DriverId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            // MaintenanceRecord configuration retirée - table gérée uniquement par ADO.NET

            modelBuilder.Entity<Driver>(entity =>
            {
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
