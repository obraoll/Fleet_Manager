using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetManager.Models
{
    /// <summary>
    /// Modèle représentant un enregistrement de maintenance
    /// </summary>
    [Table("MaintenanceRecords")]
    public class MaintenanceRecord
    {
        [Key]
        [Column("MaintenanceRecordId")]
        public int MaintenanceRecordId { get; set; }

        [Required]
        [Column("VehicleId")]
        public int VehicleId { get; set; }

        [Column("MaintenanceDate")]
        public DateTime MaintenanceDate { get; set; } = DateTime.Now;

        [Column("MaintenanceType")]
        public string MaintenanceType { get; set; } = string.Empty;

        [Required]
        [Column("Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column("Mileage", TypeName = "decimal(10,2)")]
        public decimal Mileage { get; set; }

        [Required]
        [Column("Cost", TypeName = "decimal(10,2)")]
        public decimal Cost { get; set; }

        [MaxLength(100)]
        [Column("Garage")]
        public string? Garage { get; set; }

        [Column("NextMaintenanceDate")]
        public DateTime? NextMaintenanceDate { get; set; }

        [Column("NextMaintenanceMileage", TypeName = "decimal(10,2)")]
        public decimal? NextMaintenanceMileage { get; set; }

        [Column("Parts")]
        public string? Parts { get; set; }

        [Column("TechnicianName")]
        public string? TechnicianName { get; set; }

        [Column("Status")]
        public string Status { get; set; } = "Terminée";

        [Column("Notes")]
        public string? Notes { get; set; }

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties retirées - MaintenanceRecord géré uniquement par ADO.NET
    }
}