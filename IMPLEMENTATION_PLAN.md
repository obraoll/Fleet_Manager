# 🎯 PLAN D'ACTION - Configuration complète Tableau de Bord & Statistiques

## Phase 1: Ajouter les Commandes Manquantes (PRIORITÉ 🔴 HAUTE)

### 1.1 DashboardViewModel - 4 nouvelles commandes

**Ajouter les propriétés:**
```csharp
public ICommand ViewDetailedStatisticsCommand { get; }
public ICommand GenerateReportCommand { get; }
public ICommand ExportDataCommand { get; }
public ICommand OpenSettingsCommand { get; }
```

**Initialiser dans InitializeCommands():**
```csharp
ViewDetailedStatisticsCommand = new RelayCommand(_ => ViewDetailedStatistics());
GenerateReportCommand = new AsyncRelayCommand(GenerateReportAsync);
ExportDataCommand = new AsyncRelayCommand(ExportDataAsync);
OpenSettingsCommand = new RelayCommand(_ => OpenSettings());
```

**Implémenter les méthodes:**
```csharp
private void ViewDetailedStatistics()
{
    // Naviguer vers StatisticsView depuis MainViewModel
    var mainViewModel = Application.Current.MainWindow?.DataContext as MainViewModel;
    if (mainViewModel != null)
    {
        mainViewModel.CurrentViewCommand?.Execute("Statistics");
    }
}

private async Task GenerateReportAsync(object? parameter)
{
    var saveDialog = new SaveFileDialog { Filter = "PDF (*.pdf)|*.pdf" };
    if (saveDialog.ShowDialog() == true)
    {
        var content = GenerateReportContent();
        _exportService.GeneratePdfReport("Rapport Dashboard", content, saveDialog.FileName);
    }
}

private async Task ExportDataAsync(object? parameter)
{
    var saveDialog = new SaveFileDialog { Filter = "CSV (*.csv)|*.csv" };
    if (saveDialog.ShowDialog() == true)
    {
        var (success, msg) = await _exportService.ExportStatisticsToCsvAsync(
            TopVehiclesByConsumption.Concat(TopVehiclesByCost).ToList(), 
            saveDialog.FileName);
    }
}

private void OpenSettings()
{
    var settingsWindow = new SettingsWindow();
    settingsWindow.ShowDialog();
}

private string GenerateReportContent()
{
    return $"TABLEAU DE BORD - {DateTime.Now:dd/MM/yyyy}\n" +
           $"Véhicules: {TotalVehicles}, Actifs: {ActiveVehicles}\n" +
           $"Coût carburant: {TotalFuelCost:C}, Maintenance: {TotalMaintenanceCost:C}";
}
```

---

### 1.2 StatisticsViewModel - 5 nouvelles commandes

**Ajouter les propriétés:**
```csharp
public ICommand ViewAdvancedChartsCommand { get; }
public ICommand ComparePeriodCommand { get; }
public ICommand SendReportCommand { get; }
public ICommand SetTargetsCommand { get; }
public ICommand AnalysisSettingsCommand { get; }
```

**Initialiser dans InitializeCommands():**
```csharp
ViewAdvancedChartsCommand = new RelayCommand(_ => ViewAdvancedCharts());
ComparePeriodCommand = new RelayCommand(_ => OpenComparePeriodWindow());
SendReportCommand = new AsyncRelayCommand(SendReportAsync);
SetTargetsCommand = new RelayCommand(_ => OpenTargetsWindow());
AnalysisSettingsCommand = new RelayCommand(_ => OpenAnalysisSettings());
```

**Implémenter les méthodes:**
```csharp
private void ViewAdvancedCharts()
{
    var chartsWindow = new AdvancedChartsWindow
    {
        DataContext = new AdvancedChartsViewModel(
            _statisticsService,
            MonthlyStatistics,
            ConsumptionTrend,
            CostTrend)
    };
    chartsWindow.ShowDialog();
}

private void OpenComparePeriodWindow()
{
    var compareWindow = new ComparePeriodWindow
    {
        DataContext = new ComparePeriodViewModel(_statisticsService)
    };
    compareWindow.ShowDialog();
}

private async Task SendReportAsync(object? parameter)
{
    MessageBox.Show("Fonction Email en cours de développement", "Info", 
        MessageBoxButton.OK, MessageBoxImage.Information);
    await Task.CompletedTask;
}

private void OpenTargetsWindow()
{
    var targetsWindow = new TargetsWindow
    {
        DataContext = new TargetsViewModel(_vehicleService)
    };
    targetsWindow.ShowDialog();
}

private void OpenAnalysisSettings()
{
    var settingsWindow = new AnalysisSettingsWindow
    {
        DataContext = new AnalysisSettingsViewModel()
    };
    settingsWindow.ShowDialog();
}
```

---

## Phase 2: Créer les Services Manquants (PRIORITÉ 🟡 MOYENNE)

### 2.1 EmailService (Services/EmailService.cs)

```csharp
using System;
using System.Threading.Tasks;

namespace FleetManager.Services
{
    public interface IEmailService
    {
        Task<(bool success, string message)> SendEmailAsync(string to, string subject, string body);
        Task<(bool success, string message)> SendReportAsync(string to, string reportContent, string filename);
    }

    public class EmailService : IEmailService
    {
        // Configuration SMTP (à mettre dans appsettings.json)
        private readonly string _smtpServer = "smtp.gmail.com";
        private readonly int _smtpPort = 587;
        private readonly string _fromEmail = "noreply@fleetmanager.com";
        private readonly string _fromPassword = ""; // À configurer

        public async Task<(bool, string)> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                // Implementation avec System.Net.Mail (simple version)
                MessageBox.Show("Email envoyé simulé (non configuré)", "Info");
                return (true, "Email envoyé");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur: {ex.Message}");
            }
        }

        public async Task<(bool, string)> SendReportAsync(string to, string reportContent, string filename)
        {
            return await SendEmailAsync(to, "Rapport Fleet Manager", reportContent);
        }
    }
}
```

### 2.2 ConfigurationService (Services/ConfigurationService.cs)

```csharp
using System;
using System.Collections.Generic;

namespace FleetManager.Services
{
    public interface IConfigurationService
    {
        Dictionary<string, object> GetDashboardSettings();
        void SetDashboardSettings(Dictionary<string, object> settings);
        int GetAlertThreshold(string type);
    }

    public class ConfigurationService : IConfigurationService
    {
        private Dictionary<string, object> _settings = new()
        {
            { "HighConsumptionThreshold", 12.0m },
            { "MaintenanceIntervalDays", 365 },
            { "CostAlertThreshold", 1000m },
            { "RefreshIntervalMinutes", 5 }
        };

        public Dictionary<string, object> GetDashboardSettings() => _settings;
        
        public void SetDashboardSettings(Dictionary<string, object> settings) => _settings = settings;
        
        public int GetAlertThreshold(string type) => type switch
        {
            "consumption" => 12,
            "maintenance" => 365,
            "cost" => 1000,
            _ => 100
        };
    }
}
```

### 2.3 TargetService (Services/TargetService.cs)

```csharp
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FleetManager.Models;

namespace FleetManager.Services
{
    public class VehicleTarget
    {
        public int VehicleId { get; set; }
        public decimal TargetConsumption { get; set; }
        public decimal TargetMonthlyBudget { get; set; }
        public DateTime SetDate { get; set; }
    }

    public interface ITargetService
    {
        Task<VehicleTarget> GetVehicleTargetAsync(int vehicleId);
        Task<bool> SetVehicleTargetAsync(VehicleTarget target);
    }

    public class TargetService : ITargetService
    {
        private readonly FleetManagerContext _context;

        public TargetService(FleetManagerContext context) => _context = context;

        public async Task<VehicleTarget> GetVehicleTargetAsync(int vehicleId)
        {
            return new VehicleTarget { VehicleId = vehicleId };
        }

        public async Task<bool> SetVehicleTargetAsync(VehicleTarget target)
        {
            return true;
        }
    }
}
```

---

## Phase 3: Créer les Converters Manquants (PRIORITÉ 🟡 MOYENNE)

### 3.1 PriorityToColorConverter (Helpers/PriorityToColorConverter.cs)

```csharp
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using FleetManager.Models;

namespace FleetManager.Helpers
{
    public class PriorityToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AlertPriority priority)
            {
                return priority switch
                {
                    AlertPriority.Critical => new SolidColorBrush(Color.FromRgb(244, 67, 54)),  // Red
                    AlertPriority.High => new SolidColorBrush(Color.FromRgb(255, 152, 0)),     // Orange
                    AlertPriority.Medium => new SolidColorBrush(Color.FromRgb(255, 193, 7)),   // Amber
                    AlertPriority.Low => new SolidColorBrush(Color.FromRgb(76, 175, 80)),      // Green
                    _ => new SolidColorBrush(Colors.Gray)
                };
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
```

### 3.2 NumericToHeightConverter (Helpers/NumericToHeightConverter.cs)

```csharp
using System;
using System.Globalization;
using System.Windows.Data;

namespace FleetManager.Helpers
{
    public class NumericToHeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal decimalValue)
                return Math.Max(0, (double)decimalValue * 0.5);  // Scale factor
            if (value is double doubleValue)
                return Math.Max(0, doubleValue * 0.5);
            return 0.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
```

---

## Phase 4: Créer les Fenêtres Manquantes (PRIORITÉ 🟡 MOYENNE)

### 4.1 SettingsWindow (Views/SettingsWindow.xaml)
### 4.2 ComparePeriodWindow (Views/ComparePeriodWindow.xaml)
### 4.3 TargetsWindow (Views/TargetsWindow.xaml)
### 4.4 AnalysisSettingsWindow (Views/AnalysisSettingsWindow.xaml)

---

## Phase 5: Configurer Dependency Injection (PRIORITÉ 🔴 HAUTE)

### 5.1 App.xaml.cs - Ajouter les enregistrements

```csharp
// Dans ConfigureServices():

// Services existants
services.AddSingleton<VehicleService>();
services.AddSingleton<FuelService>();
services.AddSingleton<StatisticsService>();
services.AddSingleton<ExportService>();

// Services nouveaux
services.AddSingleton<IEmailService, EmailService>();
services.AddSingleton<IConfigurationService, ConfigurationService>();
services.AddSingleton<ITargetService, TargetService>();

// ViewModels
services.AddTransient<DashboardViewModel>();
services.AddTransient<StatisticsViewModel>();

// Views
services.AddTransient<DashboardView>();
services.AddTransient<StatisticsView>();
```

---

## Phase 6: Intégration LiveCharts (PRIORITÉ 🔴 HAUTE)

### 6.1 Remplacer Canvas par CartesianChart dans DashboardView.xaml

```xml
<!-- À la place du Canvas de consommation -->
<lvc:CartesianChart 
    Series="{Binding ConsumptionSeriesCollection}"
    XAxes="{Binding XAxes}"
    YAxes="{Binding YAxes}"
    Height="200"
    Background="White"/>
```

### 6.2 Ajouter propriétés dans DashboardViewModel

```csharp
public SeriesCollection ConsumptionSeriesCollection { get; set; }
public Axis[] XAxes { get; set; }
public Axis[] YAxes { get; set; }

// Dans LoadDataAsync():
InitializeCharts();

private void InitializeCharts()
{
    ConsumptionSeriesCollection = new SeriesCollection
    {
        new LineSeries
        {
            Values = new ChartValues<decimal>(ConsumptionTrend.Select(x => x.Value)),
            Title = "Consommation"
        }
    };
    
    XAxes = new[] { new Axis { Separator = new Separator() } };
    YAxes = new[] { new Axis { LabelFormatter = value => value.ToString("C") } };
}
```

---

## Checklist d'implémentation

### Phase 1: Commandes (Estimée: 2h)
- [ ] Ajouter 4 commandes dans DashboardViewModel
- [ ] Ajouter 5 commandes dans StatisticsViewModel
- [ ] Ajouter les bindings dans XAML
- [ ] Tester chaque commande

### Phase 2: Services (Estimée: 1.5h)
- [ ] Créer EmailService
- [ ] Créer ConfigurationService
- [ ] Créer TargetService
- [ ] Enregistrer dans DI

### Phase 3: Converters (Estimée: 30min)
- [ ] Créer PriorityToColorConverter
- [ ] Créer NumericToHeightConverter
- [ ] Ajouter ressources en XAML

### Phase 4: Fenêtres (Estimée: 3h)
- [ ] Créer SettingsWindow.xaml + ViewModel
- [ ] Créer ComparePeriodWindow.xaml + ViewModel
- [ ] Créer TargetsWindow.xaml + ViewModel
- [ ] Créer AnalysisSettingsWindow.xaml + ViewModel

### Phase 5: DI Configuration (Estimée: 30min)
- [ ] Enregistrer tous les services
- [ ] Enregistrer les ViewModels
- [ ] Enregistrer les Views
- [ ] Tester l'injection

### Phase 6: LiveCharts (Estimée: 2h)
- [ ] Importer LiveCharts NuGet (si nécessaire)
- [ ] Intégrer dans DashboardView
- [ ] Intégrer dans StatisticsView
- [ ] Intégrer dans AdvancedChartsWindow
- [ ] Créer AdvancedChartsViewModel

---

## Total estimé: **~10h de développement**

Priorité recommandée: **Phase 1 → Phase 5 → Phase 6 → Phase 2,3,4**
