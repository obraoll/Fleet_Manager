using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FleetManager.Services;
using FleetManager.ViewModels;
using FleetManager.Views;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace FleetManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;
        public static IConfiguration Configuration { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Ajouter un gestionnaire d'exceptions global pour capturer les InnerExceptions
            DispatcherUnhandledException += (s, args) =>
            {
                var ex = args.Exception;
                var innerMsg = ex.InnerException?.Message ?? "Aucune exception interne";
                System.Diagnostics.Debug.WriteLine($"EXCEPTION NON GÉRÉE: {ex.Message}\nInner: {innerMsg}\nStack: {ex.StackTrace}");
                MessageBox.Show($"Erreur:\n{ex.Message}\n\nDétails:\n{innerMsg}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                args.Handled = true;
            };

            try
            {
                // Configuration
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                Configuration = builder.Build();

                // Dependency Injection
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                ServiceProvider = serviceCollection.BuildServiceProvider();

                // Ouvrir la fenêtre de connexion IMMÉDIATEMENT
                var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
                loginWindow.Show();

                // Initialiser la base de données en arrière-plan
                Task.Run(() => InitializeDatabaseAsync());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Erreur au démarrage de l'application:\n\n{ex.Message}\n\nDétails:\n{ex.InnerException?.Message}",
                    "Erreur fatale",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                Current.Shutdown();
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Configuration
            services.AddSingleton(Configuration);

            // DbContext
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<FleetDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            // Services
            // Dans la méthode ConfigureServices
            services.AddSingleton<AuthenticationService>();
            services.AddTransient<VehicleService>();
            services.AddTransient<FuelService>();
            services.AddTransient<ExportService>();
            services.AddTransient<StatisticsService>();
            // Nouveaux services (configuration, objectifs, email)
            services.AddSingleton<ConfigurationService>();
            services.AddSingleton<ITargetService, TargetService>();
            services.AddTransient<IEmailService, EmailService>();
            // Services et Repository pour Maintenance (ADO.NET)
            services.AddTransient<MaintenanceRepository>(sp => 
            {
                var context = sp.GetRequiredService<FleetDbContext>();
                return new MaintenanceRepository(context);
            });
            services.AddTransient<MaintenanceService>();

            // ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<VehiclesViewModel>();
            services.AddTransient<FuelViewModel>();
            services.AddTransient<StatisticsViewModel>();
            services.AddTransient<AddVehicleViewModel>();
            services.AddTransient<AddFuelRecordViewModel>();
            services.AddTransient<MaintenanceViewModel>();
            services.AddTransient<UsersViewModel>();

            // Views
            services.AddTransient<LoginWindow>();
            services.AddTransient<MainWindow>();
            services.AddTransient<DashboardView>();
            services.AddTransient<VehiclesView>();
            services.AddTransient<FuelView>();
            services.AddTransient<StatisticsView>();
            services.AddTransient<AddVehicleWindow>();
            services.AddTransient<MaintenanceView>();
            services.AddTransient<UsersView>();
        }

        private async Task InitializeDatabaseAsync()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== INITIALISATION BASE DE DONNÉES ===");

                var connectionString = Configuration.GetConnectionString("DefaultConnection");
                System.Diagnostics.Debug.WriteLine($"Chaîne de connexion : {connectionString}");

                using var scope = ServiceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<FleetDbContext>();
                var authService = scope.ServiceProvider.GetRequiredService<AuthenticationService>();

                System.Diagnostics.Debug.WriteLine("Test de connexion à la base de données...");

                // Vérifier la connexion (peut prendre du temps si MySQL est down)
                bool canConnect = await Task.Run(() => dbContext.Database.CanConnect());
                System.Diagnostics.Debug.WriteLine($"Connexion possible : {canConnect}");

                if (!canConnect)
                {
                    throw new Exception("Impossible de se connecter à MySQL. Vérifiez que MySQL est démarré et que la configuration est correcte.");
                }

                System.Diagnostics.Debug.WriteLine("Création des tables si nécessaire...");
                // Créer les tables si elles n'existent pas
                bool created = await Task.Run(() => dbContext.Database.EnsureCreated());
                System.Diagnostics.Debug.WriteLine($"Tables créées/vérifiées : {created}");

                // Exécuter la migration TankCapacity
                System.Diagnostics.Debug.WriteLine("Vérification/ajout de la colonne TankCapacity...");
                var migrationResult = await MigrationTankCapacity.ExecuteAsync(connectionString);
                System.Diagnostics.Debug.WriteLine($"Migration TankCapacity: {migrationResult.Message}");

                // Exécuter la migration SuperAdmin
                System.Diagnostics.Debug.WriteLine("Vérification/ajout du compte SuperAdmin...");
                var superAdminResult = await MigrationAddSuperAdmin.ExecuteAsync(dbContext);
                System.Diagnostics.Debug.WriteLine($"Migration SuperAdmin: {superAdminResult.Message}");

                System.Diagnostics.Debug.WriteLine("Initialisation des utilisateurs par défaut...");
                // Initialiser les utilisateurs par défaut
                await authService.InitializeDefaultUsersAsync();
                System.Diagnostics.Debug.WriteLine("Utilisateurs par défaut initialisés");

                System.Diagnostics.Debug.WriteLine("=== INITIALISATION TERMINÉE AVEC SUCCÈS ===");
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException dbEx)
            {
                System.Diagnostics.Debug.WriteLine($"ERREUR DB UPDATE: {dbEx.Message}");
                System.Diagnostics.Debug.WriteLine($"Inner Exception: {dbEx.InnerException?.Message}");

                await Dispatcher.InvokeAsync(() =>
                {
                    MessageBox.Show(
                        $"Erreur de base de données:\n\n{dbEx.Message}\n\nDétail: {dbEx.InnerException?.Message}\n\nVérifiez la structure de la base de données.",
                        "Erreur Base de Données",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                });
            }
            catch (MySqlConnector.MySqlException mysqlEx)
            {
                System.Diagnostics.Debug.WriteLine($"ERREUR MYSQL: Code {mysqlEx.Number} - {mysqlEx.Message}");

                string errorMessage = mysqlEx.Number switch
                {
                    1045 => "Accès refusé - Vérifiez le nom d'utilisateur et mot de passe MySQL",
                    2003 => "Serveur MySQL inaccessible - Vérifiez que MySQL est démarré sur le port 3306",
                    1049 => "Base de données 'fleet_manager' introuvable - Elle sera créée automatiquement",
                    1044 => "Accès refusé à la base - Vérifiez les permissions de l'utilisateur MySQL",
                    _ => $"Erreur MySQL #{mysqlEx.Number}: {mysqlEx.Message}"
                };

                await Dispatcher.InvokeAsync(() =>
                {
                    MessageBox.Show(
                        $"Erreur MySQL:\n\n{errorMessage}\n\nChaîne de connexion:\n{Configuration.GetConnectionString("DefaultConnection")}",
                        "Erreur MySQL",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERREUR GÉNÉRALE: {ex.GetType().Name} - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");

                // Utiliser le Dispatcher pour afficher le MessageBox depuis le thread UI
                await Dispatcher.InvokeAsync(() =>
                {
                    MessageBox.Show(
                        $"Erreur lors de l'initialisation:\n\n{ex.Message}\n\nType: {ex.GetType().Name}\n\nDétails: {ex.InnerException?.Message}",
                        "Erreur d'initialisation",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error);
                });
            }
        }
    }
}
