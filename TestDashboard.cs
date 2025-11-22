using System;
using System.Linq;
using System.Threading.Tasks;
using FleetManager.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FleetManager
{
    /// <summary>
    /// Classe de test pour vérifier les données du dashboard
    /// </summary>
    public class TestDashboard
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("=== TEST DASHBOARD DATA ===");
            
            // Configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            Console.WriteLine($"Connection string: {connectionString}");

            // Créer le contexte
            var optionsBuilder = new DbContextOptionsBuilder<FleetDbContext>();
            optionsBuilder.UseMySql(connectionString, 
                ServerVersion.AutoDetect(connectionString));

            using var context = new FleetDbContext(optionsBuilder.Options);

            try
            {
                Console.WriteLine("\n--- Vérification de la connexion ---");
                var canConnect = await context.Database.CanConnectAsync();
                Console.WriteLine($"Connexion à la base de données: {(canConnect ? "OK" : "ÉCHEC")}");

                if (!canConnect)
                {
                    Console.WriteLine("Impossible de se connecter à la base de données");
                    return;
                }

                Console.WriteLine("\n--- Comptage des données ---");
                var vehicleCount = await context.Vehicles.CountAsync();
                Console.WriteLine($"Nombre de véhicules: {vehicleCount}");

                var fuelRecordCount = await context.FuelRecords.CountAsync();
                Console.WriteLine($"Nombre d'enregistrements carburant: {fuelRecordCount}");

                var maintenanceRepo = new MaintenanceRepository(context);
                var maintenanceCount = await maintenanceRepo.CountAsync();
                Console.WriteLine($"Nombre d'enregistrements maintenance: {maintenanceCount}");

                var driverCount = await context.Drivers.CountAsync();
                Console.WriteLine($"Nombre de conducteurs: {driverCount}");

                if (vehicleCount == 0)
                {
                    Console.WriteLine("\n⚠️ ATTENTION: Aucun véhicule dans la base de données!");
                    return;
                }

                Console.WriteLine("\n--- Détails des véhicules ---");
                var vehicles = await context.Vehicles.ToListAsync();
                foreach (var vehicle in vehicles)
                {
                    Console.WriteLine($"  - {vehicle.Brand} {vehicle.Model} ({vehicle.RegistrationNumber})");
                    Console.WriteLine($"    Status: {vehicle.Status}, Km: {vehicle.CurrentMileage}");
                }

                if (fuelRecordCount > 0)
                {
                    Console.WriteLine("\n--- Enregistrements carburant récents ---");
                    var recentFuel = await context.FuelRecords
                        .OrderByDescending(f => f.RefuelDate)
                        .Take(5)
                        .ToListAsync();
                    
                    foreach (var fuel in recentFuel)
                    {
                        Console.WriteLine($"  - Véhicule {fuel.VehicleId}: {fuel.LitersRefueled}L à {fuel.PricePerLiter}€/L");
                        Console.WriteLine($"    Date: {fuel.RefuelDate:dd/MM/yyyy}, Total: {fuel.TotalCost}€");
                    }
                }

                Console.WriteLine("\n--- Test du service StatisticsService ---");
                var statisticsService = new StatisticsService(context);

                Console.WriteLine("Appel de GetFleetStatisticsAsync...");
                var fleetStats = await statisticsService.GetFleetStatisticsAsync();
                Console.WriteLine($"  Total véhicules: {fleetStats.TotalVehicles}");
                Console.WriteLine($"  Véhicules actifs: {fleetStats.ActiveVehicles}");
                Console.WriteLine($"  Coût total carburant: {fleetStats.TotalFuelCost}€");
                Console.WriteLine($"  Consommation moyenne: {fleetStats.AverageFleetConsumption} L/100km");

                Console.WriteLine("\nAppel de GetDashboardDataAsync...");
                var dashboardData = await statisticsService.GetDashboardDataAsync();
                Console.WriteLine($"  TopVehiclesByConsumption: {dashboardData.TopVehiclesByConsumption.Count} véhicules");
                Console.WriteLine($"  TopVehiclesByCost: {dashboardData.TopVehiclesByCost.Count} véhicules");
                Console.WriteLine($"  Alertes: {dashboardData.Alerts.Count}");
                Console.WriteLine($"  MonthlyTrends: {dashboardData.MonthlyTrends.Count} mois");

                Console.WriteLine("\n✅ Test terminé avec succès!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ ERREUR: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }
}
