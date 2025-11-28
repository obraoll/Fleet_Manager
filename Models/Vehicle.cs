using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetManager.Models
{
    /// <summary>
    /// Modèle représentant un véhicule
    /// </summary>
    [Table("Vehicles")]
    public class Vehicle
    {
        [Key]
        [Column("VehicleId")]
        public int VehicleId { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("RegistrationNumber")]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("Brand")]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("Model")]
        public string Model { get; set; } = string.Empty;

        [Required]
        [Column("Year")]
        public int Year { get; set; }

        [Required]
        [Column("VehicleType")]
        public string VehicleType { get; set; } = string.Empty;

        [Required]
        [Column("FuelType")]
        public string FuelType { get; set; } = string.Empty;

        [Column("CurrentMileage", TypeName = "decimal(10,2)")]
        public decimal CurrentMileage { get; set; } = 0;

        [Column("TankCapacity", TypeName = "decimal(5,2)")]
        public decimal TankCapacity { get; set; } = 0;

        [Column("AverageFuelConsumption", TypeName = "decimal(5,2)")]
        public decimal AverageFuelConsumption { get; set; } = 0;

        [Column("Status")]
        public string Status { get; set; } = "Actif";

        [Column("PurchaseDate")]
        public DateTime? PurchaseDate { get; set; }

        [Column("PurchasePrice", TypeName = "decimal(10,2)")]
        public decimal? PurchasePrice { get; set; }

        [Column("InsuranceExpiryDate")]
        public DateTime? InsuranceExpiryDate { get; set; }

        [Column("TechnicalInspectionDate")]
        public DateTime? TechnicalInspectionDate { get; set; }

        [Column("Notes")]
        public string? Notes { get; set; }

        [MaxLength(500)]
        [Column("ImagePath")]
        public string? ImagePath { get; set; }

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<FuelRecord> FuelRecords { get; set; } = new List<FuelRecord>();
        // MaintenanceRecords navigation property retirée - utilisation d'ADO.NET uniquement
        public virtual ICollection<VehicleAssignment> VehicleAssignments { get; set; } = new List<VehicleAssignment>();

        // Propriétés calculées
        [NotMapped]
        public string DisplayName => $"{Brand} {Model} ({RegistrationNumber})";
        [NotMapped]
        public bool NeedsInspection => TechnicalInspectionDate.HasValue && TechnicalInspectionDate.Value < DateTime.Now.AddMonths(1);
        [NotMapped]
        public bool InsuranceExpiringSoon => InsuranceExpiryDate.HasValue && InsuranceExpiryDate.Value < DateTime.Now.AddMonths(1);
    }
}