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
        public int MaintenanceId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required]
        public DateTime MaintenanceDate { get; set; } = DateTime.Now;

        [Required]
        public MaintenanceType MaintenanceType { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Mileage { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Cost { get; set; }

        [MaxLength(100)]
        public string? Provider { get; set; }

        public DateTime? NextMaintenanceDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? NextMaintenanceMileage { get; set; }

        [MaxLength(50)]
        public string? InvoiceNumber { get; set; }

        public string? Notes { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; } = null!;

        [ForeignKey("CreatedBy")]
        public virtual User? Creator { get; set; }
    }
}