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
        public int VehicleId { get; set; }

        [Required]
        [MaxLength(20)]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Brand { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Model { get; set; } = string.Empty;

        [Required]
        public int Year { get; set; }

        [Required]
        public VehicleType VehicleType { get; set; }

        [Required]
        public FuelType FuelType { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal CurrentMileage { get; set; } = 0;

        [Column(TypeName = "decimal(5,2)")]
        public decimal AverageFuelConsumption { get; set; } = 0;

        public VehicleStatus Status { get; set; } = VehicleStatus.Actif;

        public DateTime? PurchaseDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? PurchasePrice { get; set; }

        [MaxLength(30)]
        public string? Color { get; set; }

        [MaxLength(17)]
        public string? VIN { get; set; }

        public DateTime? InsuranceExpiryDate { get; set; }

        public DateTime? TechnicalInspectionDate { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<FuelRecord> FuelRecords { get; set; } = new List<FuelRecord>();
        public virtual ICollection<MaintenanceRecord> MaintenanceRecords { get; set; } = new List<MaintenanceRecord>();
        public virtual ICollection<VehicleAssignment> VehicleAssignments { get; set; } = new List<VehicleAssignment>();

        // Propriétés calculées
        [NotMapped]
        public string DisplayName => $"{Brand} {Model} ({RegistrationNumber})";

        [NotMapped]
        public bool NeedsInspection => TechnicalInspectionDate.HasValue && 
                                        TechnicalInspectionDate.Value < DateTime.Now.AddMonths(1);

        [NotMapped]
        public bool InsuranceExpiringSoon => InsuranceExpiryDate.HasValue && 
                                              InsuranceExpiryDate.Value < DateTime.Now.AddMonths(1);
    }
}