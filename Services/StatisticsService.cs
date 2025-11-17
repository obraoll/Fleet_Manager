using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FleetManager.Models;
using Microsoft.EntityFrameworkCore;

namespace FleetManager.Services
{
    /// <summary>
    /// Service avancé pour les statistiques et analyses de la flotte
    /// </summary>
    public class StatisticsService
    {
        private readonly FleetDbContext _context;

        public StatisticsService(FleetDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtient les données complètes du tableau de bord
        /// </summary>
        public async Task<DashboardData> GetDashboardDataAsync()
        {
            var dashboardData = new DashboardData
            {
                FleetStats = await GetFleetStatisticsAsync(),
                TopVehiclesByConsumption = await GetTopVehiclesByConsumptionAsync(5),
                TopVehiclesByCost = await GetTopVehiclesByCostAsync(5),
                MonthlyTrends = await GetMonthlyTrendsAsync(12),
                TypeBreakdown = await GetVehicleTypeStatisticsAsync(),
                FuelBreakdown = await GetFuelTypeStatisticsAsync(),
                Alerts = await GetDashboardAlertsAsync(),
                ConsumptionTrend = await GetConsumptionTrendAsync(30),
                CostTrend = await GetCostTrendAsync(30)
            };

            return dashboardData;
        }

        /// <summary>
        /// Obtient les statistiques globales de la flotte
        /// </summary>
        public async Task<FleetStatistics> GetFleetStatisticsAsync()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            var fuelRecords = await _context.FuelRecords.ToListAsync();
            var maintenanceRecords = await _context.MaintenanceRecords.ToListAsync();

            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            var monthlyFuelRecords = fuelRecords.Where(f =>
                f.RefuelDate.Month == currentMonth &&
                f.RefuelDate.Year == currentYear).ToList();

            var monthlyMaintenanceRecords = maintenanceRecords.Where(m =>
                m.MaintenanceDate.Month == currentMonth &&
                m.MaintenanceDate.Year == currentYear).ToList();

            return new FleetStatistics
            {
                TotalVehicles = vehicles.Count,
                ActiveVehicles = vehicles.Count(v => v.Status == VehicleStatus.Actif),
                VehiclesInMaintenance = vehicles.Count(v => v.Status == VehicleStatus.EnMaintenance),
                OutOfServiceVehicles = vehicles.Count(v => v.Status == VehicleStatus.HorsService),
                TotalFuelCost = fuelRecords.Sum(f => f.TotalCost),
                TotalLiters = fuelRecords.Sum(f => f.LitersRefueled),
                AverageFleetConsumption = CalculateAverageConsumption(fuelRecords),
                MonthlyFuelCost = monthlyFuelRecords.Sum(f => f.TotalCost),
                TotalMaintenanceCost = maintenanceRecords.Sum(m => m.Cost),
                MonthlyMaintenanceCost = monthlyMaintenanceRecords.Sum(m => m.Cost),
                VehiclesDueMaintenance = await GetVehiclesDueMaintenanceCountAsync(),
                TotalMileage = vehicles.Sum(v => v.CurrentMileage),
                AverageVehicleMileage = vehicles.Any() ? vehicles.Average(v => v.CurrentMileage) : 0
            };
        }

        /// <summary>
        /// Obtient les statistiques détaillées d'un véhicule
        /// </summary>
        public async Task<VehicleStatistics> GetVehicleStatisticsAsync(int vehicleId)
        {
            var vehicle = await _context.Vehicles.FindAsync(vehicleId);
            if (vehicle == null) throw new ArgumentException("Véhicule introuvable");

            var fuelRecords = await _context.FuelRecords
                .Where(f => f.VehicleId == vehicleId)
                .ToListAsync();

            var maintenanceRecords = await _context.MaintenanceRecords
                .Where(m => m.VehicleId == vehicleId)
                .ToListAsync();

            return new VehicleStatistics
            {
                VehicleId = vehicle.VehicleId,
                VehicleName = $"{vehicle.Brand} {vehicle.Model}",
                RegistrationNumber = vehicle.RegistrationNumber,
                CurrentMileage = vehicle.CurrentMileage,
                TotalRefuels = fuelRecords.Count,
                TotalLiters = fuelRecords.Sum(f => f.LitersRefueled),
                TotalFuelCost = fuelRecords.Sum(f => f.TotalCost),
                AverageConsumption = fuelRecords.Where(f => f.CalculatedConsumption > 0)
                    .DefaultIfEmpty()
                    .Average(f => f?.CalculatedConsumption ?? 0),
                AveragePricePerLiter = fuelRecords.Any() ? fuelRecords.Average(f => f.PricePerLiter) : 0,
                TotalMaintenances = maintenanceRecords.Count,
                TotalMaintenanceCost = maintenanceRecords.Sum(m => m.Cost),
                LastMaintenanceDate = maintenanceRecords.OrderByDescending(m => m.MaintenanceDate)
                    .FirstOrDefault()?.MaintenanceDate,
                NextMaintenanceDate = maintenanceRecords.OrderByDescending(m => m.NextMaintenanceDate)
                    .FirstOrDefault()?.NextMaintenanceDate
            };
        }

        /// <summary>
        /// Obtient les véhicules avec la plus forte consommation
        /// </summary>
        public async Task<List<VehicleStatistics>> GetTopVehiclesByConsumptionAsync(int count)
        {
            var vehicles = await _context.Vehicles
                .Include(v => v.FuelRecords)
                .Where(v => v.FuelRecords.Any())
                .ToListAsync();

            var statistics = new List<VehicleStatistics>();

            foreach (var vehicle in vehicles)
            {
                var avgConsumption = vehicle.FuelRecords
                    .Where(f => f.CalculatedConsumption > 0)
                    .DefaultIfEmpty()
                    .Average(f => f?.CalculatedConsumption ?? 0);

                if (avgConsumption > 0)
                {
                    statistics.Add(new VehicleStatistics
                    {
                        VehicleId = vehicle.VehicleId,
                        VehicleName = $"{vehicle.Brand} {vehicle.Model}",
                        RegistrationNumber = vehicle.RegistrationNumber,
                        AverageConsumption = avgConsumption,
                        TotalFuelCost = vehicle.FuelRecords.Sum(f => f.TotalCost)
                    });
                }
            }

            return statistics.OrderByDescending(s => s.AverageConsumption).Take(count).ToList();
        }

        /// <summary>
        /// Obtient les véhicules les plus coûteux
        /// </summary>
        public async Task<List<VehicleStatistics>> GetTopVehiclesByCostAsync(int count)
        {
            var vehicles = await _context.Vehicles
                .Include(v => v.FuelRecords)
                .Include(v => v.MaintenanceRecords)
                .ToListAsync();

            var statistics = vehicles.Select(vehicle => new VehicleStatistics
            {
                VehicleId = vehicle.VehicleId,
                VehicleName = $"{vehicle.Brand} {vehicle.Model}",
                RegistrationNumber = vehicle.RegistrationNumber,
                TotalFuelCost = vehicle.FuelRecords.Sum(f => f.TotalCost),
                TotalMaintenanceCost = vehicle.MaintenanceRecords.Sum(m => m.Cost)
            }).ToList();

            return statistics.OrderByDescending(s => s.TotalCost).Take(count).ToList();
        }

        /// <summary>
        /// Obtient les tendances mensuelles
        /// </summary>
        public async Task<List<MonthlyStatistics>> GetMonthlyTrendsAsync(int months)
        {
            var endDate = DateTime.Now;
            var startDate = endDate.AddMonths(-months);

            var fuelRecords = await _context.FuelRecords
                .Where(f => f.RefuelDate >= startDate)
                .ToListAsync();

            var maintenanceRecords = await _context.MaintenanceRecords
                .Where(m => m.MaintenanceDate >= startDate)
                .ToListAsync();

            var monthlyStats = new List<MonthlyStatistics>();

            for (int i = 0; i < months; i++)
            {
                var date = endDate.AddMonths(-i);
                var monthFuelRecords = fuelRecords.Where(f =>
                    f.RefuelDate.Year == date.Year && f.RefuelDate.Month == date.Month).ToList();
                var monthMaintenanceRecords = maintenanceRecords.Where(m =>
                    m.MaintenanceDate.Year == date.Year && m.MaintenanceDate.Month == date.Month).ToList();

                monthlyStats.Add(new MonthlyStatistics
                {
                    Year = date.Year,
                    Month = date.Month,
                    FuelCost = monthFuelRecords.Sum(f => f.TotalCost),
                    MaintenanceCost = monthMaintenanceRecords.Sum(m => m.Cost),
                    TotalLiters = monthFuelRecords.Sum(f => f.LitersRefueled),
                    AverageConsumption = CalculateAverageConsumption(monthFuelRecords),
                    RefuelCount = monthFuelRecords.Count,
                    MaintenanceCount = monthMaintenanceRecords.Count
                });
            }

            return monthlyStats.OrderBy(m => m.Year).ThenBy(m => m.Month).ToList();
        }

        /// <summary>
        /// Obtient les statistiques par type de véhicule
        /// </summary>
        public async Task<List<VehicleTypeStatistics>> GetVehicleTypeStatisticsAsync()
        {
            var vehicles = await _context.Vehicles
                .Include(v => v.FuelRecords)
                .Include(v => v.MaintenanceRecords)
                .ToListAsync();

            return vehicles.GroupBy(v => v.VehicleType)
                .Select(g => new VehicleTypeStatistics
                {
                    VehicleType = g.Key,
                    Count = g.Count(),
                    AverageConsumption = g.SelectMany(v => v.FuelRecords)
                        .Where(f => f.CalculatedConsumption > 0)
                        .DefaultIfEmpty()
                        .Average(f => f?.CalculatedConsumption ?? 0),
                    TotalFuelCost = g.SelectMany(v => v.FuelRecords).Sum(f => f.TotalCost),
                    TotalMaintenanceCost = g.SelectMany(v => v.MaintenanceRecords).Sum(m => m.Cost),
                    AverageMileage = g.Average(v => v.CurrentMileage)
                }).OrderBy(t => t.Count).ToList();
        }

        /// <summary>
        /// Obtient les statistiques par type de carburant
        /// </summary>
        public async Task<List<FuelTypeStatistics>> GetFuelTypeStatisticsAsync()
        {
            var vehicles = await _context.Vehicles.ToListAsync();
            var fuelRecords = await _context.FuelRecords.ToListAsync();

            var totalCost = fuelRecords.Sum(f => f.TotalCost);

            var fuelStats = vehicles.GroupBy(v => v.FuelType)
                .Select(g => new FuelTypeStatistics
                {
                    FuelType = g.Key,
                    VehicleCount = g.Count(),
                    TotalLiters = fuelRecords.Where(f =>
                        g.Any(v => v.VehicleId == f.VehicleId)).Sum(f => f.LitersRefueled),
                    TotalCost = fuelRecords.Where(f =>
                        g.Any(v => v.VehicleId == f.VehicleId)).Sum(f => f.TotalCost),
                    AverageConsumption = g.Average(v => v.AverageFuelConsumption),
                    AveragePricePerLiter = fuelRecords.Where(f =>
                        g.Any(v => v.VehicleId == f.VehicleId))
                        .DefaultIfEmpty()
                        .Average(f => f?.PricePerLiter ?? 0)
                }).ToList();

            // Calculer les pourcentages
            foreach (var stat in fuelStats)
            {
                stat.Percentage = totalCost > 0 ? (stat.TotalCost / totalCost) * 100 : 0;
            }

            return fuelStats;
        }

        /// <summary>
        /// Obtient les alertes du tableau de bord
        /// </summary>
        public async Task<List<DashboardAlert>> GetDashboardAlertsAsync()
        {
            var alerts = new List<DashboardAlert>();
            var vehicles = await _context.Vehicles
                .Include(v => v.MaintenanceRecords)
                .ToListAsync();

            foreach (var vehicle in vehicles)
            {
                // Vérifier la maintenance due
                var lastMaintenance = vehicle.MaintenanceRecords
                    .OrderByDescending(m => m.MaintenanceDate)
                    .FirstOrDefault();

                if (lastMaintenance != null && lastMaintenance.NextMaintenanceDate.HasValue &&
                    lastMaintenance.NextMaintenanceDate.Value <= DateTime.Now.AddDays(30))
                {
                    alerts.Add(new DashboardAlert
                    {
                        Type = AlertType.MaintenanceDue,
                        Title = "Maintenance due",
                        Message = $"Maintenance prévue le {lastMaintenance.NextMaintenanceDate.Value:dd/MM/yyyy}",
                        VehicleRegistration = vehicle.RegistrationNumber,
                        Date = DateTime.Now,
                        Priority = lastMaintenance.NextMaintenanceDate.Value <= DateTime.Now ?
                            AlertPriority.High : AlertPriority.Medium
                    });
                }

                // Vérifier le contrôle technique
                if (vehicle.TechnicalInspectionDate.HasValue &&
                    vehicle.TechnicalInspectionDate.Value <= DateTime.Now.AddDays(60))
                {
                    alerts.Add(new DashboardAlert
                    {
                        Type = AlertType.InspectionExpired,
                        Title = "Contrôle technique",
                        Message = $"Contrôle technique expire le {vehicle.TechnicalInspectionDate.Value:dd/MM/yyyy}",
                        VehicleRegistration = vehicle.RegistrationNumber,
                        Date = DateTime.Now,
                        Priority = vehicle.TechnicalInspectionDate.Value <= DateTime.Now ?
                            AlertPriority.Critical : AlertPriority.High
                    });
                }

                // Vérifier l'assurance
                if (vehicle.InsuranceExpiryDate.HasValue &&
                    vehicle.InsuranceExpiryDate.Value <= DateTime.Now.AddDays(30))
                {
                    alerts.Add(new DashboardAlert
                    {
                        Type = AlertType.InsuranceExpired,
                        Title = "Assurance expire",
                        Message = $"Assurance expire le {vehicle.InsuranceExpiryDate.Value:dd/MM/yyyy}",
                        VehicleRegistration = vehicle.RegistrationNumber,
                        Date = DateTime.Now,
                        Priority = vehicle.InsuranceExpiryDate.Value <= DateTime.Now ?
                            AlertPriority.Critical : AlertPriority.High
                    });
                }

                // Vérifier la consommation élevée
                if (vehicle.AverageFuelConsumption > 0)
                {
                    var fleetAverage = vehicles.Where(v => v.AverageFuelConsumption > 0)
                        .Average(v => v.AverageFuelConsumption);

                    if (vehicle.AverageFuelConsumption > fleetAverage * 1.3m) // 30% au-dessus de la moyenne
                    {
                        alerts.Add(new DashboardAlert
                        {
                            Type = AlertType.HighConsumption,
                            Title = "Consommation élevée",
                            Message = $"Consommation: {vehicle.AverageFuelConsumption:F1} L/100km (moyenne: {fleetAverage:F1})",
                            VehicleRegistration = vehicle.RegistrationNumber,
                            Date = DateTime.Now,
                            Priority = AlertPriority.Medium
                        });
                    }
                }
            }

            return alerts.OrderByDescending(a => a.Priority).ThenByDescending(a => a.Date).ToList();
        }

        /// <summary>
        /// Obtient la tendance de consommation
        /// </summary>
        public async Task<List<TimeSeriesData>> GetConsumptionTrendAsync(int days)
        {
            var endDate = DateTime.Now;
            var startDate = endDate.AddDays(-days);

            var fuelRecords = await _context.FuelRecords
                .Where(f => f.RefuelDate >= startDate && f.CalculatedConsumption > 0)
                .OrderBy(f => f.RefuelDate)
                .ToListAsync();

            return fuelRecords.GroupBy(f => f.RefuelDate.Date)
                .Select(g => new TimeSeriesData
                {
                    Date = g.Key,
                    Value = g.Average(f => f.CalculatedConsumption) ?? 0m,
                    Label = g.Key.ToString("dd/MM"),
                    Category = "Consommation"
                }).ToList();
        }

        /// <summary>
        /// Obtient la tendance des coûts
        /// </summary>
        public async Task<List<TimeSeriesData>> GetCostTrendAsync(int days)
        {
            var endDate = DateTime.Now;
            var startDate = endDate.AddDays(-days);

            var fuelRecords = await _context.FuelRecords
                .Where(f => f.RefuelDate >= startDate)
                .OrderBy(f => f.RefuelDate)
                .ToListAsync();

            return fuelRecords.GroupBy(f => f.RefuelDate.Date)
                .Select(g => new TimeSeriesData
                {
                    Date = g.Key,
                    Value = g.Sum(f => f.TotalCost),
                    Label = g.Key.ToString("dd/MM"),
                    Category = "Coût"
                }).ToList();
        }

        /// <summary>
        /// Obtient les mouvements récents
        /// </summary>
        public async Task<List<RecentMovement>> GetRecentMovementsAsync(int count)
        {
            var movements = new List<RecentMovement>();

            // Pleins récents
            var recentFuelRecords = await _context.FuelRecords
                .Include(f => f.Vehicle)
                .OrderByDescending(f => f.RefuelDate)
                .Take(count / 2)
                .ToListAsync();

            movements.AddRange(recentFuelRecords.Select(f => new RecentMovement
            {
                VehicleName = $"{f.Vehicle.Brand} {f.Vehicle.Model} ({f.Vehicle.RegistrationNumber})",
                MovementType = "Plein",
                Date = f.RefuelDate,
                Description = $"{f.LitersRefueled:F1}L - {f.Station}",
                Cost = f.TotalCost,
                Mileage = f.Mileage
            }));

            // Maintenances récentes
            var recentMaintenances = await _context.MaintenanceRecords
                .Include(m => m.Vehicle)
                .OrderByDescending(m => m.MaintenanceDate)
                .Take(count / 2)
                .ToListAsync();

            movements.AddRange(recentMaintenances.Select(m => new RecentMovement
            {
                VehicleName = $"{m.Vehicle.Brand} {m.Vehicle.Model} ({m.Vehicle.RegistrationNumber})",
                MovementType = "Maintenance",
                Date = m.MaintenanceDate,
                Description = $"{m.MaintenanceType} - {m.Description}",
                Cost = m.Cost,
                Mileage = m.Mileage
            }));

            return movements.OrderByDescending(m => m.Date).Take(count).ToList();
        }

        /// <summary>
        /// Calcule la consommation moyenne d'une liste d'enregistrements
        /// </summary>
        private static decimal CalculateAverageConsumption(List<FuelRecord> fuelRecords)
        {
            var recordsWithConsumption = fuelRecords.Where(f => f.CalculatedConsumption > 0).ToList();
            return recordsWithConsumption.Any() ? (recordsWithConsumption.Average(f => f.CalculatedConsumption) ?? 0m) : 0m;
        }

        /// <summary>
        /// Compte les véhicules dus pour maintenance
        /// </summary>
        private async Task<int> GetVehiclesDueMaintenanceCountAsync()
        {
            var vehicles = await _context.Vehicles
                .Include(v => v.MaintenanceRecords)
                .ToListAsync();

            return vehicles.Count(v =>
                v.MaintenanceRecords.Any(m =>
                    m.NextMaintenanceDate.HasValue &&
                    m.NextMaintenanceDate.Value <= DateTime.Now.AddDays(30)));
        }

        /// <summary>
        /// Obtient les prédictions simples basées sur les tendances
        /// </summary>
        public async Task<List<PredictionData>> GetPredictionsAsync()
        {
            var predictions = new List<PredictionData>();

            try
            {
                // Prédiction coût mensuel carburant
                var lastThreeMonths = await GetMonthlyTrendsAsync(3);
                if (lastThreeMonths.Count >= 3)
                {
                    var avgIncrease = (lastThreeMonths[2].FuelCost - lastThreeMonths[0].FuelCost) / 3;
                    var currentValue = lastThreeMonths[2].FuelCost;
                    var predictedValue = currentValue + avgIncrease;

                    predictions.Add(new PredictionData
                    {
                        Category = "Coût carburant mensuel",
                        CurrentValue = currentValue,
                        PredictedValue = predictedValue,
                        ChangePercentage = currentValue > 0 ? (avgIncrease / currentValue) * 100 : 0,
                        Trend = avgIncrease > 0 ? "up" : avgIncrease < 0 ? "down" : "stable",
                        PredictionDate = DateTime.Now.AddMonths(1)
                    });
                }

                // Prédiction coût maintenance
                if (lastThreeMonths.Count >= 3)
                {
                    var avgIncrease = (lastThreeMonths[2].MaintenanceCost - lastThreeMonths[0].MaintenanceCost) / 3;
                    var currentValue = lastThreeMonths[2].MaintenanceCost;
                    var predictedValue = currentValue + avgIncrease;

                    predictions.Add(new PredictionData
                    {
                        Category = "Coût maintenance mensuel",
                        CurrentValue = currentValue,
                        PredictedValue = predictedValue,
                        ChangePercentage = currentValue > 0 ? (avgIncrease / currentValue) * 100 : 0,
                        Trend = avgIncrease > 0 ? "up" : avgIncrease < 0 ? "down" : "stable",
                        PredictionDate = DateTime.Now.AddMonths(1)
                    });
                }

                // Prédiction consommation moyenne
                var lastThreeMonthsConsumption = lastThreeMonths.Where(m => m.AverageConsumption > 0).ToList();
                if (lastThreeMonthsConsumption.Count >= 3)
                {
                    var avgConsumption = lastThreeMonthsConsumption.Average(m => m.AverageConsumption);
                    var avgIncrease = (lastThreeMonthsConsumption[2].AverageConsumption - lastThreeMonthsConsumption[0].AverageConsumption) / 3;

                    predictions.Add(new PredictionData
                    {
                        Category = "Consommation moyenne",
                        CurrentValue = lastThreeMonthsConsumption[2].AverageConsumption,
                        PredictedValue = lastThreeMonthsConsumption[2].AverageConsumption + avgIncrease,
                        ChangePercentage = avgIncrease,
                        Trend = avgIncrease > 0 ? "up" : avgIncrease < 0 ? "down" : "stable",
                        PredictionDate = DateTime.Now.AddMonths(1)
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du calcul des prédictions: {ex.Message}");
            }

            return predictions;
        }
    }
}
