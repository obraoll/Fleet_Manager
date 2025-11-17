using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetManager.Models
{
    /// <summary>
    /// Modèle représentant une affectation de véhicule à un conducteur
    /// </summary>
    [Table("VehicleAssignments")]
    public class VehicleAssignment
    {
        [Key]
        public int AssignmentId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required]
        public int DriverId { get; set; }

        [Required]
        public DateTime AssignmentDate { get; set; } = DateTime.Now;

        public DateTime? ReturnDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? StartMileage { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? EndMileage { get; set; }

        [MaxLength(200)]
        public string? Purpose { get; set; }

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        [ForeignKey("VehicleId")]
        public virtual Vehicle Vehicle { get; set; } = null!;

        [ForeignKey("DriverId")]
        public virtual Driver Driver { get; set; } = null!;

        // Propriétés calculées
        [NotMapped]
        public bool IsActive => !ReturnDate.HasValue || ReturnDate.Value > DateTime.Now;

        [NotMapped]
        public decimal? DistanceCovered => (EndMileage.HasValue && StartMileage.HasValue) 
            ? EndMileage.Value - StartMileage.Value 
            : null;
    }
}