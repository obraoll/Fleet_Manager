using System;

namespace FleetManager.Models
{
    /// <summary>
    /// DTO pour MaintenanceRecord sans les propriétés fantômes d'EF Core
    /// </summary>
    public class MaintenanceRecordDto
    {
        public int MaintenanceRecordId { get; set; }
        public int VehicleId { get; set; }
        public DateTime MaintenanceDate { get; set; }
        public string MaintenanceType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Mileage { get; set; }
        public decimal Cost { get; set; }
        public string? Garage { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }
        public decimal? NextMaintenanceMileage { get; set; }
        public string? Parts { get; set; }
        public string? TechnicianName { get; set; }
        public string Status { get; set; } = "Terminée";
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Convertit le DTO en MaintenanceRecord
        /// </summary>
        public MaintenanceRecord ToMaintenanceRecord()
        {
            return new MaintenanceRecord
            {
                MaintenanceRecordId = this.MaintenanceRecordId,
                VehicleId = this.VehicleId,
                MaintenanceDate = this.MaintenanceDate,
                MaintenanceType = this.MaintenanceType,
                Description = this.Description,
                Mileage = this.Mileage,
                Cost = this.Cost,
                Garage = this.Garage,
                NextMaintenanceDate = this.NextMaintenanceDate,
                NextMaintenanceMileage = this.NextMaintenanceMileage,
                Parts = this.Parts,
                TechnicianName = this.TechnicianName,
                Status = this.Status,
                Notes = this.Notes,
                CreatedAt = this.CreatedAt
            };
        }
    }
}
