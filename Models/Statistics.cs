using System;
using System.Collections.Generic;

namespace FleetManager.Models
{
    /// <summary>
    /// Modèle pour les statistiques d'un véhicule
    /// </summary>
    public class VehicleStatistics
    {
        public int VehicleId { get; set; }
        public string VehicleName { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public decimal TotalMileage { get; set; }
        public decimal CurrentMileage { get; set; }

        // Statistiques carburant
        public int TotalRefuels { get; set; }
        public decimal TotalLiters { get; set; }
        public decimal TotalFuelCost { get; set; }
        public decimal AverageConsumption { get; set; }
        public decimal AveragePricePerLiter { get; set; }

        // Statistiques maintenance
        public int TotalMaintenances { get; set; }
        public decimal TotalMaintenanceCost { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? NextMaintenanceDate { get; set; }

        // Coûts totaux
        public decimal TotalCost => TotalFuelCost + TotalMaintenanceCost;

        // Coût par kilomètre
        public decimal CostPerKilometer => CurrentMileage > 0 ? TotalCost / CurrentMileage : 0;

        // Efficacité énergétique (km/L)
        public decimal FuelEfficiency => TotalLiters > 0 ? CurrentMileage / TotalLiters : 0;
    }

    /// <summary>
    /// Modèle pour les statistiques globales de la flotte
    /// </summary>
    public class FleetStatistics
    {
        public int TotalVehicles { get; set; }
        public int ActiveVehicles { get; set; }
        public int VehiclesInMaintenance { get; set; }
        public int OutOfServiceVehicles { get; set; }

        // Statistiques carburant
        public decimal TotalFuelCost { get; set; }
        public decimal TotalLiters { get; set; }
        public decimal AverageFleetConsumption { get; set; }
        public decimal MonthlyFuelCost { get; set; }

        // Statistiques maintenance
        public decimal TotalMaintenanceCost { get; set; }
        public decimal MonthlyMaintenanceCost { get; set; }
        public int VehiclesDueMaintenance { get; set; }

        // Coûts totaux
        public decimal TotalCost => TotalFuelCost + TotalMaintenanceCost;
        public decimal MonthlyCost => MonthlyFuelCost + MonthlyMaintenanceCost;

        // Kilométrage
        public decimal TotalMileage { get; set; }
        public decimal AverageVehicleMileage { get; set; }
    }

    /// <summary>
    /// Modèle pour les statistiques mensuelles
    /// </summary>
    public class MonthlyStatistics
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string MonthName => new DateTime(Year, Month, 1).ToString("MMMM yyyy");

        public decimal FuelCost { get; set; }
        public decimal MaintenanceCost { get; set; }
        public decimal TotalCost => FuelCost + MaintenanceCost;

        public decimal TotalLiters { get; set; }
        public decimal AverageConsumption { get; set; }
        public decimal TotalMileage { get; set; }

        public int RefuelCount { get; set; }
        public int MaintenanceCount { get; set; }
    }

    /// <summary>
    /// Modèle pour les statistiques par type de véhicule
    /// </summary>
    public class VehicleTypeStatistics
    {
        public string VehicleType { get; set; } = string.Empty;
        public string TypeName => VehicleType;
        public int Count { get; set; }
        public decimal AverageConsumption { get; set; }
        public decimal TotalFuelCost { get; set; }
        public decimal TotalMaintenanceCost { get; set; }
        public decimal AverageMileage { get; set; }
        public decimal TotalCost => TotalFuelCost + TotalMaintenanceCost;
    }

    /// <summary>
    /// Modèle pour les statistiques de consommation par carburant
    /// </summary>
    public class FuelTypeStatistics
    {
        public string FuelType { get; set; } = string.Empty;
        public string TypeName => FuelType;
        public int VehicleCount { get; set; }
        public decimal TotalLiters { get; set; }
        public decimal TotalCost { get; set; }
        public decimal AverageConsumption { get; set; }
        public decimal AveragePricePerLiter { get; set; }
        public decimal Percentage { get; set; }
    }

    /// <summary>
    /// Modèle pour l'évolution temporelle des données
    /// </summary>
    public class TimeSeriesData
    {
        public DateTime Date { get; set; }
        public decimal Value { get; set; }
        public string Label { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }

    /// <summary>
    /// Modèle pour les alertes et notifications
    /// </summary>
    public class DashboardAlert
    {
        public AlertType Type { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string VehicleRegistration { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public AlertPriority Priority { get; set; }
        public bool IsRead { get; set; }
    }

    /// <summary>
    /// Type d'alerte
    /// </summary>
    public enum AlertType
    {
        MaintenanceDue,
        InspectionExpired,
        InsuranceExpired,
        HighConsumption,
        CostThreshold,
        VehicleInactive
    }

    /// <summary>
    /// Priorité des alertes
    /// </summary>
    public enum AlertPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    /// <summary>
    /// Modèle pour les données du tableau de bord
    /// </summary>
    public class DashboardData
    {
        public FleetStatistics FleetStats { get; set; } = new();
        public List<VehicleStatistics> TopVehiclesByConsumption { get; set; } = new();
        public List<VehicleStatistics> TopVehiclesByCost { get; set; } = new();
        public List<MonthlyStatistics> MonthlyTrends { get; set; } = new();
        public List<VehicleTypeStatistics> TypeBreakdown { get; set; } = new();
        public List<FuelTypeStatistics> FuelBreakdown { get; set; } = new();
        public List<DashboardAlert> Alerts { get; set; } = new();
        public List<TimeSeriesData> ConsumptionTrend { get; set; } = new();
        public List<TimeSeriesData> CostTrend { get; set; } = new();
    }

    /// <summary>
    /// Modèle pour les mouvements récents
    /// </summary>
    public class RecentMovement
    {
        public string VehicleName { get; set; } = string.Empty;
        public string MovementType { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal? Cost { get; set; }
        public decimal? Mileage { get; set; }
    }

    /// <summary>
    /// Modèle pour les prédictions et tendances
    /// </summary>
    public class PredictionData
    {
        public string Category { get; set; } = string.Empty;
        public decimal CurrentValue { get; set; }
        public decimal PredictedValue { get; set; }
        public decimal ChangePercentage { get; set; }
        public string Trend { get; set; } = string.Empty; // "up", "down", "stable"
        public DateTime PredictionDate { get; set; }
    }

    /// <summary>
    /// Modèle pour les comparaisons de performance
    /// </summary>
    public class PerformanceComparison
    {
        public string VehicleRegistration { get; set; } = string.Empty;
        public decimal ConsumptionVsFleet { get; set; }
        public decimal CostVsFleet { get; set; }
        public decimal EfficiencyRating { get; set; }
        public string PerformanceGrade { get; set; } = string.Empty; // A, B, C, D, E
        public List<string> Recommendations { get; set; } = new();
    }
}
