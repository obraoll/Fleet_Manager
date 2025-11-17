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
        public int FuelRecordId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required]
        public DateTime RefuelDate { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Mileage { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,2)")]
        public decimal LitersRefueled { get; set; }

        [Required]
        [Column(TypeName = "decimal(6,2)")]
        public decimal PricePerLiter { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalCost { get; set; }

        [Required]
        public FuelType FuelType { get; set; }

        public bool IsFullTank { get; set; } = true;

        [MaxLength(100)]
        public string? Station { get; set; }

        [Column(TypeName = "decimal(5,2)")]
        public decimal? CalculatedConsumption { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? DistanceSinceLastRefuel { get; set; }

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