using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetManager.Models
{
    /// <summary>
    /// Modèle représentant un conducteur
    /// </summary>
    [Table("Drivers")]
    public class Driver
    {
        [Key]
        public int DriverId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(30)]
        public string LicenseNumber { get; set; } = string.Empty;

        public DateTime? LicenseExpiryDate { get; set; }

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(100)]
        public string? Email { get; set; }

        public DateTime? HireDate { get; set; }

        public DriverStatus Status { get; set; } = DriverStatus.Actif;

        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<VehicleAssignment> VehicleAssignments { get; set; } = new List<VehicleAssignment>();

        // Propriétés calculées
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public bool LicenseExpiringSoon => LicenseExpiryDate.HasValue && 
                                            LicenseExpiryDate.Value < DateTime.Now.AddMonths(2);
    }
}