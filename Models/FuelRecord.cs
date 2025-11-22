using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetManager.Models
{
    /// <summary>
    /// Modèle représentant un enregistrement de plein de carburant
    /// </summary>
    [Table("FuelRecords")]
    public class FuelRecord
    {
        [Key]
        [Column("FuelRecordId")]
        public int FuelRecordId { get; set; }

        [Required]
        [Column("VehicleId")]
        public int VehicleId { get; set; }

        [Column("DriverId")]
        public int? DriverId { get; set; }

        [Required]
        [Column("RefuelDate")]
        public DateTime RefuelDate { get; set; } = DateTime.Now;

        [Required]
        [Column("Mileage", TypeName = "decimal(10,2)")]
        public decimal Mileage { get; set; }

        [Required]
        [Column("LitersRefueled", TypeName = "decimal(10,2)")]
        public decimal LitersRefueled { get; set; }

        [Required]
        [Column("PricePerLiter", TypeName = "decimal(5,3)")]
        public decimal PricePerLiter { get; set; }

        [Required]
        [Column("TotalCost", TypeName = "decimal(10,2)")]
        public decimal TotalCost { get; set; }

        [Required]
        [Column("FuelType")]
        public string FuelType { get; set; } = string.Empty;

        [Column("IsFullTank")]
        public bool IsFullTank { get; set; } = true;

        [MaxLength(100)]
        [Column("Station")]
        public string? Station { get; set; }

        [Column("CalculatedConsumption", TypeName = "decimal(5,2)")]
        public decimal? CalculatedConsumption { get; set; }

        [Column("PaymentMethod")]
        public string? PaymentMethod { get; set; }

        [Column("Notes")]
        public string? Notes { get; set; }

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; } = null!;
        [ForeignKey("DriverId")]
        public virtual Driver? Driver { get; set; }
    }
}