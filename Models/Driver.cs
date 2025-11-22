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
        [Column("DriverId")]
        public int DriverId { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("FirstName")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        [Column("LastName")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [Column("LicenseNumber")]
        public string LicenseNumber { get; set; } = string.Empty;

        [Column("LicenseExpiryDate")]
        public DateTime? LicenseExpiryDate { get; set; }

        [MaxLength(20)]
        [Column("PhoneNumber")]
        public string? PhoneNumber { get; set; }

        [MaxLength(100)]
        [Column("Email")]
        public string? Email { get; set; }

        [Column("HireDate")]
        public DateTime? HireDate { get; set; }

        [Column("Status")]
        public string Status { get; set; } = "Actif";

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation properties
        public virtual ICollection<VehicleAssignment> VehicleAssignments { get; set; } = new List<VehicleAssignment>();

        // Propriétés calculées
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";
        [NotMapped]
        public bool LicenseExpiringSoon => LicenseExpiryDate.HasValue && LicenseExpiryDate.Value < DateTime.Now.AddMonths(2);
    }
}