using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using FleetManager.Helpers;
using FleetManager.Services;
using FleetManager.Views;
using FleetManager.Models;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FleetManager.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly AuthenticationService _authService;
        private readonly IServiceProvider _serviceProvider;
        private object? _currentView;
        private string _currentUserName = "Utilisateur";
        private string _currentUserRole = "User";
        private bool _isAdmin;
        private bool _isSuperAdmin;
        private string _searchText = string.Empty;
        private bool _isDarkMode = false;

        public event PropertyChangedEventHandler? PropertyChanged;

        public object? CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public string CurrentUserName
        {
            get => _currentUserName;
            set
            {
                _currentUserName = value;
                OnPropertyChanged();
            }
        }

        public string CurrentUserRole
        {
            get => _currentUserRole;
            set
            {
                _currentUserRole = value;
                OnPropertyChanged();
            }
        }

        public bool IsAdmin
        {
            get => _isAdmin;
            set
            {
                _isAdmin = value;
                OnPropertyChanged();
            }
        }

        public bool IsSuperAdmin
        {
            get => _isSuperAdmin;
            set
            {
                _isSuperAdmin = value;
                OnPropertyChanged();
            }
        }

        public bool IsAdminOrSuperAdmin => _isAdmin || _isSuperAdmin;

        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                PerformSearch();
            }
        }

        public bool IsDarkMode
        {
            get => _isDarkMode;
            set
            {
                _isDarkMode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ThemeIcon));
                OnPropertyChanged(nameof(ThemeText));
            }
        }

        public string ThemeIcon => _isDarkMode ? "🌙" : "☀️";
        public string ThemeText => _isDarkMode ? "Dark" : "Light";

        // Commandes de navigation
        public ICommand NavigateCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand NavigateToDashboardCommand { get; }
        public ICommand NavigateToVehiclesCommand { get; }
        public ICommand NavigateToFuelCommand { get; }
        public ICommand NavigateToStatisticsCommand { get; }
        public ICommand NavigateToMaintenanceCommand { get; }
        public ICommand NavigateToUsersCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ToggleThemeCommand { get; }
        public ICommand ShowNotificationsCommand { get; }
        public ICommand ExportCommand { get; }

        public MainViewModel(AuthenticationService authService, IServiceProvider serviceProvider)
        {
            _authService = authService;
            _serviceProvider = serviceProvider;

            // Initialisation des commandes
            NavigateCommand = new RelayCommand(param => Navigate(param as string));
            LogoutCommand = new RelayCommand(param => Logout(param as Window));
            NavigateToDashboardCommand = new RelayCommand(param => Navigate("DashboardView"));
            NavigateToVehiclesCommand = new RelayCommand(param => Navigate("VehiclesView"));
            NavigateToFuelCommand = new RelayCommand(param => Navigate("FuelView"));
            NavigateToStatisticsCommand = new RelayCommand(param => Navigate("StatisticsView"));
            NavigateToMaintenanceCommand = new RelayCommand(param => Navigate("MaintenanceView"));
            NavigateToUsersCommand = new RelayCommand(param => Navigate("UsersView"));
            SearchCommand = new RelayCommand(param => PerformSearch());
            ToggleThemeCommand = new RelayCommand(param => ToggleTheme());
            ShowNotificationsCommand = new RelayCommand(param => ShowNotifications());
            ExportCommand = new RelayCommand(param => ShowExportMenu(param));

            // Initialisation des informations utilisateur
            InitializeUserInfo();

            // Vue par défaut
            Navigate("DashboardView");
        }

        private void InitializeUserInfo()
        {
            try
            {
                var currentUser = _authService.CurrentUser;
                if (currentUser != null)
                {
                    CurrentUserName = currentUser.FullName ?? currentUser.Username;
                    CurrentUserRole = GetRoleDisplayName(currentUser.Role);
                    IsAdmin = currentUser.Role == "Admin" || currentUser.Role == "SuperAdmin";
                    IsSuperAdmin = currentUser.Role == "SuperAdmin";
                    OnPropertyChanged(nameof(IsAdminOrSuperAdmin));

                    System.Diagnostics.Debug.WriteLine($"Utilisateur connecté: {CurrentUserName} ({CurrentUserRole}) - Admin: {IsAdmin} - SuperAdmin: {IsSuperAdmin}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("ATTENTION: Aucun utilisateur connecté trouvé dans MainViewModel");
                    CurrentUserName = "Utilisateur inconnu";
                    CurrentUserRole = "Non connecté";
                    IsAdmin = false;
                    IsSuperAdmin = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de l'initialisation des infos utilisateur: {ex.Message}");
            }
        }

        private string GetRoleDisplayName(string role)
        {
            return role switch
            {
                "SuperAdmin" => "Super Administrateur",
                "Admin" => "Administrateur",
                "User" => "Utilisateur",
                _ => role
            };
        }

        private void Navigate(string? viewName)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                System.Diagnostics.Debug.WriteLine("Nom de vue null ou vide");
                return;
            }

            try
            {
                System.Diagnostics.Debug.WriteLine($"=== NAVIGATION VERS: {viewName} ===");
                System.Diagnostics.Debug.WriteLine($"ServiceProvider null? {_serviceProvider == null}");

                switch (viewName)
                {
                    case "DashboardView":
                        System.Diagnostics.Debug.WriteLine("Récupération DashboardView...");
                        if (_serviceProvider != null)
                        {
                            var dashboardViewModel = _serviceProvider.GetRequiredService<DashboardViewModel>();
                            var dashboardView = new DashboardView(dashboardViewModel);
                            System.Diagnostics.Debug.WriteLine($"DashboardView DataContext: {dashboardView.DataContext?.GetType().Name}");
                            CurrentView = dashboardView;
                            System.Diagnostics.Debug.WriteLine("DashboardView chargée et assignée");
                        }
                        break;

                    case "VehiclesView":
                        System.Diagnostics.Debug.WriteLine("Récupération VehiclesView...");
                        if (_serviceProvider != null)
                        {
                            var vehiclesView = _serviceProvider.GetRequiredService<VehiclesView>();
                            System.Diagnostics.Debug.WriteLine($"VehiclesView récupérée: {vehiclesView != null}");
                            System.Diagnostics.Debug.WriteLine($"VehiclesView DataContext: {vehiclesView?.DataContext?.GetType().Name}");
                            CurrentView = vehiclesView;
                            System.Diagnostics.Debug.WriteLine("VehiclesView chargée et assignée");
                        }
                        break;

                    case "FuelView":
                        System.Diagnostics.Debug.WriteLine("Récupération FuelView...");
                        if (_serviceProvider != null)
                        {
                            var fuelViewModel = _serviceProvider.GetRequiredService<FuelViewModel>();
                            var fuelView = new FuelView(fuelViewModel);
                            System.Diagnostics.Debug.WriteLine($"FuelView DataContext: {fuelView.DataContext?.GetType().Name}");
                            CurrentView = fuelView;
                            System.Diagnostics.Debug.WriteLine("FuelView chargée et assignée");
                        }
                        break;

                    case "StatisticsView":
                        if (_serviceProvider != null)
                        {
                            var statisticsViewModel = _serviceProvider.GetRequiredService<StatisticsViewModel>();
                            var statisticsView = new StatisticsView(statisticsViewModel);
                            System.Diagnostics.Debug.WriteLine($"StatisticsView DataContext: {statisticsView.DataContext?.GetType().Name}");
                            CurrentView = statisticsView;
                            System.Diagnostics.Debug.WriteLine("StatisticsView chargée et assignée");
                        }
                        break;

                    case "MaintenanceView":
                        System.Diagnostics.Debug.WriteLine("Récupération MaintenanceView...");
                        if (_serviceProvider != null)
                        {
                            var maintenanceViewModel = _serviceProvider.GetRequiredService<MaintenanceViewModel>();
                            var maintenanceView = new MaintenanceView(maintenanceViewModel);
                            System.Diagnostics.Debug.WriteLine($"MaintenanceView DataContext: {maintenanceView.DataContext?.GetType().Name}");
                            CurrentView = maintenanceView;
                            System.Diagnostics.Debug.WriteLine("MaintenanceView chargée et assignée");
                        }
                        break;

                    case "UsersView":
                        if (IsAdmin && _serviceProvider != null)
                        {
                            System.Diagnostics.Debug.WriteLine("Récupération UsersView...");
                            var usersViewModel = _serviceProvider.GetRequiredService<UsersViewModel>();
                            var usersView = new UsersView(usersViewModel);
                            System.Diagnostics.Debug.WriteLine($"UsersView DataContext: {usersView.DataContext?.GetType().Name}");
                            CurrentView = usersView;
                            System.Diagnostics.Debug.WriteLine("UsersView chargée et assignée");
                        }
                        else
                        {
                            MessageBox.Show("Accès refusé. Seuls les administrateurs peuvent accéder à cette section.",
                                "Accès refusé", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        break;

                    default:
                        System.Diagnostics.Debug.WriteLine($"Vue inconnue: {viewName}");
                        MessageBox.Show($"Vue non implémentée: {viewName}", "Information",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la navigation vers {viewName}: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                MessageBox.Show($"Erreur lors du chargement de la vue {viewName}:\n\n{ex.Message}",
                    "Erreur de navigation", MessageBoxButton.OK, MessageBoxImage.Error);

                // Fallback vers le dashboard en cas d'erreur
                if (viewName != "DashboardView")
                {
                    Navigate("DashboardView");
                }
            }
        }

        private object CreateTemporaryStatisticsView()
        {
            var textBlock = new System.Windows.Controls.TextBlock
            {
                Text = "📊 Module Statistiques\n\nCette section sera bientôt disponible.\nElle contiendra les graphiques et rapports de performance du parc automobile.",
                FontSize = 16,
                TextAlignment = System.Windows.TextAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                Margin = new Thickness(20)
            };
            return textBlock;
        }

        private object CreateTemporaryMaintenanceView()
        {
            var textBlock = new System.Windows.Controls.TextBlock
            {
                Text = "🔧 Module Entretien\n\nCette section sera bientôt disponible.\nElle permettra de gérer les maintenances et réparations des véhicules.",
                FontSize = 16,
                TextAlignment = System.Windows.TextAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                Margin = new Thickness(20)
            };
            return textBlock;
        }

        private object CreateTemporaryUsersView()
        {
            var textBlock = new System.Windows.Controls.TextBlock
            {
                Text = "👥 Gestion des Utilisateurs\n\nCette section sera bientôt disponible.\nElle permettra de gérer les comptes utilisateurs et leurs permissions.",
                FontSize = 16,
                TextAlignment = System.Windows.TextAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                Margin = new Thickness(20)
            };
            return textBlock;
        }

        private void Logout(Window? window)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Déconnexion demandée");

                var result = MessageBox.Show("Êtes-vous sûr de vouloir vous déconnecter ?",
                    "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _authService.Logout();
                    System.Diagnostics.Debug.WriteLine("Utilisateur déconnecté");

                    if (_serviceProvider != null)
                    {
                        var loginWindow = _serviceProvider.GetRequiredService<LoginWindow>();
                        loginWindow.Show();
                        System.Diagnostics.Debug.WriteLine("LoginWindow affichée");

                        if (window != null)
                        {
                            window.Close();
                            System.Diagnostics.Debug.WriteLine("MainWindow fermée");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la déconnexion: {ex.Message}");
                MessageBox.Show($"Erreur lors de la déconnexion: {ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PerformSearch()
        {
            // Déléguer la recherche au ViewModel de la vue active
            if (CurrentView == null) return;

            try
            {
                var viewType = CurrentView.GetType().Name;
                System.Diagnostics.Debug.WriteLine($"Recherche dans la vue: {viewType}");

                // Utiliser la réflexion pour accéder à la propriété SearchText du ViewModel de la vue
                var dataContext = CurrentView.GetType().GetProperty("DataContext")?.GetValue(CurrentView);
                
                if (dataContext != null)
                {
                    var searchTextProperty = dataContext.GetType().GetProperty("SearchText");
                    
                    if (searchTextProperty != null && searchTextProperty.CanWrite)
                    {
                        // Mettre à jour la SearchText du ViewModel actif
                        searchTextProperty.SetValue(dataContext, SearchText);
                        System.Diagnostics.Debug.WriteLine($"Recherche transférée au ViewModel: {dataContext.GetType().Name}");
                    }
                    else
                    {
                        // Essayer d'appeler une commande SearchCommand si elle existe
                        var searchCommandProperty = dataContext.GetType().GetProperty("SearchCommand");
                        if (searchCommandProperty != null)
                        {
                            var searchCommand = searchCommandProperty.GetValue(dataContext) as ICommand;
                            if (searchCommand?.CanExecute(SearchText) == true)
                            {
                                searchCommand.Execute(SearchText);
                                System.Diagnostics.Debug.WriteLine("Commande de recherche exécutée");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la recherche: {ex.Message}");
            }
        }

        private void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
            System.Diagnostics.Debug.WriteLine($"Thème changé: {(IsDarkMode ? "Dark" : "Light")}");
            
            try
            {
                // Changer le thème dynamiquement
                var resources = Application.Current.Resources;
                
                if (IsDarkMode)
                {
                    // Ajouter le thème dark
                    var darkTheme = new ResourceDictionary
                    {
                        Source = new Uri("pack://application:,,,/Resources/DarkTheme.xaml", UriKind.Absolute)
                    };
                    
                    // Retirer le thème dark existant s'il y en a un
                    for (int i = resources.MergedDictionaries.Count - 1; i >= 0; i--)
                    {
                        var dict = resources.MergedDictionaries[i];
                        if (dict.Source != null && dict.Source.ToString().Contains("DarkTheme.xaml"))
                        {
                            resources.MergedDictionaries.RemoveAt(i);
                        }
                    }
                    
                    resources.MergedDictionaries.Add(darkTheme);
                }
                else
                {
                    // Retirer le thème dark pour revenir au light
                    for (int i = resources.MergedDictionaries.Count - 1; i >= 0; i--)
                    {
                        var dict = resources.MergedDictionaries[i];
                        if (dict.Source != null && dict.Source.ToString().Contains("DarkTheme.xaml"))
                        {
                            resources.MergedDictionaries.RemoveAt(i);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du changement de thème: {ex.Message}");
                MessageBox.Show($"Erreur lors du changement de thème:\n{ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ShowNotifications()
        {
            try
            {
                var window = new NotificationsWindow();
                
                // Vérifier que MainWindow est visible avant de définir Owner
                var mainWindow = Application.Current.MainWindow;
                if (mainWindow != null && mainWindow.IsLoaded && mainWindow.IsVisible)
                {
                    window.Owner = mainWindow;
                    window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                }
                else
                {
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                }
                
                window.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'ouverture des notifications:\n{ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetAlertIcon(Models.AlertType type)
        {
            return type switch
            {
                Models.AlertType.MaintenanceDue => "🔧",
                Models.AlertType.InspectionExpired => "📋",
                Models.AlertType.InsuranceExpired => "📄",
                Models.AlertType.HighConsumption => "⛽",
                Models.AlertType.CostThreshold => "💰",
                Models.AlertType.VehicleInactive => "🚗",
                _ => "ℹ️"
            };
        }

        private System.Windows.Media.Brush GetPriorityColor(Models.AlertPriority priority)
        {
            return priority switch
            {
                Models.AlertPriority.Critical => new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(239, 68, 68)),
                Models.AlertPriority.High => new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(245, 158, 11)),
                Models.AlertPriority.Medium => new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(59, 130, 246)),
                Models.AlertPriority.Low => new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(100, 116, 139)),
                _ => System.Windows.Media.Brushes.Gray
            };
        }

        private string GetRelativeTime(DateTime date)
        {
            var timeSpan = DateTime.Now - date;
            if (timeSpan.TotalDays >= 1)
                return $"Il y a {(int)timeSpan.TotalDays} jour{(timeSpan.TotalDays >= 2 ? "s" : "")}";
            if (timeSpan.TotalHours >= 1)
                return $"Il y a {(int)timeSpan.TotalHours} heure{(timeSpan.TotalHours >= 2 ? "s" : "")}";
            if (timeSpan.TotalMinutes >= 1)
                return $"Il y a {(int)timeSpan.TotalMinutes} minute{(timeSpan.TotalMinutes >= 2 ? "s" : "")}";
            return "À l'instant";
        }

        private void ShowExportMenu(object? parameter)
        {
            try
            {
                var contextMenu = new System.Windows.Controls.ContextMenu();
                
                var exportCsvItem = new System.Windows.Controls.MenuItem
                {
                    Header = "📊 Exporter en CSV",
                    Command = new RelayCommand(_ => ExportToCsv())
                };
                
                var exportPdfItem = new System.Windows.Controls.MenuItem
                {
                    Header = "📄 Exporter en PDF",
                    Command = new RelayCommand(_ => ExportToPdf())
                };
                
                var exportExcelItem = new System.Windows.Controls.MenuItem
                {
                    Header = "📈 Exporter en Excel",
                    Command = new RelayCommand(_ => ExportToExcel())
                };

                contextMenu.Items.Add(exportCsvItem);
                contextMenu.Items.Add(exportPdfItem);
                contextMenu.Items.Add(exportExcelItem);

                if (parameter is System.Windows.FrameworkElement element)
                {
                    contextMenu.PlacementTarget = element;
                    contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
                    contextMenu.IsOpen = true;
                }
                else
                {
                    // Fallback : afficher directement les options d'export
                    var result = MessageBox.Show(
                        "Choisissez le format d'export:\n\nOui = CSV\nNon = PDF\nAnnuler = Excel",
                        "Options d'Export",
                        MessageBoxButton.YesNoCancel,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        ExportToCsv();
                    }
                    else if (result == MessageBoxResult.No)
                    {
                        ExportToPdf();
                    }
                    else
                    {
                        ExportToExcel();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'affichage du menu d'export:\n{ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void ExportToCsv()
        {
            try
            {
                var saveDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Fichiers CSV (*.csv)|*.csv",
                    FileName = $"FleetManager_Export_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (saveDialog.ShowDialog() == true && _serviceProvider != null)
                {
                    var exportService = _serviceProvider.GetRequiredService<ExportService>();
                    var vehicleService = _serviceProvider.GetRequiredService<VehicleService>();
                    
                    // Récupérer tous les véhicules pour l'export
                    var vehicles = await vehicleService.GetAllVehiclesAsync();
                    
                    if (vehicles != null && vehicles.Any())
                    {
                        var (success, message) = await exportService.ExportVehiclesToCsvAsync(vehicles.ToList(), saveDialog.FileName);
                        
                        if (success)
                        {
                            MessageBox.Show($"✅ {message}\n\nFichier: {saveDialog.FileName}",
                                "Export CSV réussi", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show($"❌ {message}",
                                "Erreur d'export", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Aucun véhicule à exporter.",
                            "Export CSV", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'export CSV:\n{ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportToPdf()
        {
            try
            {
                var saveDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Fichiers PDF (*.pdf)|*.pdf",
                    FileName = $"FleetManager_Report_{DateTime.Now:yyyyMMdd_HHmmss}.pdf"
                };

                if (saveDialog.ShowDialog() == true && _serviceProvider != null)
                {
                    var exportService = _serviceProvider.GetRequiredService<ExportService>();
                    
                    // Créer un rapport PDF basique
                    var content = $"Rapport Fleet Manager\n\nDate: {DateTime.Now:dd/MM/yyyy HH:mm}\n\n";
                    
                    var (success, message) = exportService.GeneratePdfReport(
                        "Rapport Fleet Manager",
                        content,
                        saveDialog.FileName);
                    
                    if (success)
                    {
                        MessageBox.Show($"✅ Rapport PDF généré avec succès!\n\nFichier: {saveDialog.FileName}",
                            "Export PDF réussi", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show($"❌ {message}",
                            "Erreur d'export", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'export PDF:\n{ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportToExcel()
        {
            MessageBox.Show("L'export Excel sera bientôt disponible.",
                "Export Excel", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
