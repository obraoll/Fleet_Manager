using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using FleetManager.Models;
using FleetManager.Services;
using FleetManager.ViewModels;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace FleetManager.Views
{
    /// <summary>
    /// Logique d'interaction pour AdvancedChartsWindow.xaml
    /// </summary>
    public partial class AdvancedChartsWindow : Window, INotifyPropertyChanged
    {
        private readonly StatisticsService _statisticsService;
        private readonly VehicleService _vehicleService;
        private readonly ExportService _exportService;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Collections pour les données des graphiques
        public ObservableCollection<Vehicle> Vehicles { get; set; } = new();
        public ObservableCollection<MonthlyStatistics> MonthlyStats { get; set; } = new();
        public ObservableCollection<VehicleStatistics> VehicleStats { get; set; } = new();
        public ObservableCollection<TimeSeriesData> ConsumptionTrend { get; set; } = new();
        public ObservableCollection<TimeSeriesData> CostTrend { get; set; } = new();

        // Séries LiveCharts
        private IEnumerable<ISeries> _vehicleConsumptionSeries = Array.Empty<ISeries>();
        private IEnumerable<ISeries> _vehicleCostSeries = Array.Empty<ISeries>();
        private IEnumerable<ISeries> _consumptionTrendSeries = Array.Empty<ISeries>();
        private IEnumerable<ISeries> _costTrendSeries = Array.Empty<ISeries>();
        private IEnumerable<ISeries> _mileageTrendSeries = Array.Empty<ISeries>();
        private IEnumerable<ISeries> _vehicleTypePieSeries = Array.Empty<ISeries>();
        private IEnumerable<ISeries> _fuelTypePieSeries = Array.Empty<ISeries>();
        private IEnumerable<ISeries> _costBreakdownSeries = Array.Empty<ISeries>();
        private IEnumerable<ISeries> _efficiencyScoreSeries = Array.Empty<ISeries>();
        private IEnumerable<ISeries> _costPerKmSeries = Array.Empty<ISeries>();

        public IEnumerable<ISeries> VehicleConsumptionSeries
        {
            get => _vehicleConsumptionSeries;
            set { _vehicleConsumptionSeries = value; OnPropertyChanged(); }
        }
        public IEnumerable<ISeries> VehicleCostSeries
        {
            get => _vehicleCostSeries;
            set { _vehicleCostSeries = value; OnPropertyChanged(); }
        }
        public IEnumerable<ISeries> ConsumptionTrendSeries
        {
            get => _consumptionTrendSeries;
            set { _consumptionTrendSeries = value; OnPropertyChanged(); }
        }
        public IEnumerable<ISeries> CostTrendSeries
        {
            get => _costTrendSeries;
            set { _costTrendSeries = value; OnPropertyChanged(); }
        }
        public IEnumerable<ISeries> MileageTrendSeries
        {
            get => _mileageTrendSeries;
            set { _mileageTrendSeries = value; OnPropertyChanged(); }
        }
        public IEnumerable<ISeries> VehicleTypePieSeries
        {
            get => _vehicleTypePieSeries;
            set { _vehicleTypePieSeries = value; OnPropertyChanged(); }
        }
        public IEnumerable<ISeries> FuelTypePieSeries
        {
            get => _fuelTypePieSeries;
            set { _fuelTypePieSeries = value; OnPropertyChanged(); }
        }
        public IEnumerable<ISeries> CostBreakdownSeries
        {
            get => _costBreakdownSeries;
            set { _costBreakdownSeries = value; OnPropertyChanged(); }
        }
        public IEnumerable<ISeries> EfficiencyScoreSeries
        {
            get => _efficiencyScoreSeries;
            set { _efficiencyScoreSeries = value; OnPropertyChanged(); }
        }
        public IEnumerable<ISeries> CostPerKmSeries
        {
            get => _costPerKmSeries;
            set { _costPerKmSeries = value; OnPropertyChanged(); }
        }

        // Propriétés pour les contrôles
        private bool _isLoading;
        private string _loadingMessage = "";
        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }
        public string LoadingMessage
        {
            get => _loadingMessage;
            set { _loadingMessage = value; OnPropertyChanged(); }
        }

        // Constructeur sans paramètres pour DataContext externe
        public AdvancedChartsWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        // Constructeur avec paramètres (conservé pour compatibilité)
        public AdvancedChartsWindow(
            StatisticsService statisticsService,
            VehicleService vehicleService,
            ExportService exportService)
        {
            InitializeComponent();

            _statisticsService = statisticsService;
            _vehicleService = vehicleService;
            _exportService = exportService;

            DataContext = this;

            Loaded += AdvancedChartsWindow_Loaded;
        }

        private async void AdvancedChartsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadDataAsync();
            InitializeCharts();
        }

        /// <summary>
        /// Charge les données nécessaires pour les graphiques
        /// </summary>
        private async Task LoadDataAsync()
        {
            try
            {
                if (_statisticsService == null || _vehicleService == null)
                {
                    LoadingMessage = "Services non initialisés. Utilisez le DataContext pour passer les services.";
                    return;
                }

                IsLoading = true;
                LoadingMessage = "Chargement des données...";

                // Charger les véhicules
                var vehicles = await _vehicleService.GetAllVehiclesAsync();
                Vehicles.Clear();
                foreach (var vehicle in vehicles)
                {
                    Vehicles.Add(vehicle);
                }

                // Charger les statistiques mensuelles
                var monthlyStats = await _statisticsService.GetMonthlyTrendsAsync(12);
                MonthlyStats.Clear();
                foreach (var stat in monthlyStats)
                {
                    MonthlyStats.Add(stat);
                }

                // Charger les statistiques des véhicules
                var vehicleStats = new List<VehicleStatistics>();
                foreach (var vehicle in vehicles.Take(10)) // Limiter à 10 pour les graphiques
                {
                    try
                    {
                        var stats = await _statisticsService.GetVehicleStatisticsAsync(vehicle.VehicleId);
                        vehicleStats.Add(stats);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Erreur chargement stats véhicule {vehicle.VehicleId}: {ex.Message}");
                    }
                }

                VehicleStats.Clear();
                foreach (var stat in vehicleStats)
                {
                    VehicleStats.Add(stat);
                }

                // Charger les tendances
                var consumptionTrend = await _statisticsService.GetConsumptionTrendAsync(90);
                ConsumptionTrend.Clear();
                foreach (var trend in consumptionTrend)
                {
                    ConsumptionTrend.Add(trend);
                }

                var costTrend = await _statisticsService.GetCostTrendAsync(90);
                CostTrend.Clear();
                foreach (var trend in costTrend)
                {
                    CostTrend.Add(trend);
                }

                LoadingMessage = "Données chargées avec succès";
            }
            catch (Exception ex)
            {
                LoadingMessage = $"Erreur: {ex.Message}";
                MessageBox.Show($"Erreur lors du chargement des données:\n\n{ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Initialise les graphiques avec les données
        /// </summary>
        private void InitializeCharts()
        {
            try
            {
                InitializeVehicleComparisonCharts();
                InitializeTrendCharts();
                InitializePieCharts();
                InitializePerformanceCharts();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur initialisation graphiques: {ex.Message}");
            }
        }

        /// <summary>
        /// Initialise les graphiques de comparaison des véhicules
        /// </summary>
        private void InitializeVehicleComparisonCharts()
        {
            if (VehicleStats.Count == 0) return;

            // Graphique consommation par véhicule
            var consumptionValues = VehicleStats.Select(v => (double)v.AverageConsumption).ToArray();
            VehicleConsumptionSeries = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = consumptionValues,
                    Name = "Consommation",
                    Fill = new SolidColorPaint(SKColors.CornflowerBlue),
                    YToolTipLabelFormatter = point => $"{point.Coordinate.PrimaryValue:F2} L/100km"
                }
            };

            // Graphique coûts par véhicule
            var fuelCosts = VehicleStats.Select(v => (double)v.TotalFuelCost).ToArray();
            var maintenanceCosts = VehicleStats.Select(v => (double)v.TotalMaintenanceCost).ToArray();
            VehicleCostSeries = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = fuelCosts,
                    Name = "Carburant",
                    Fill = new SolidColorPaint(SKColors.MediumSeaGreen),
                    YToolTipLabelFormatter = point => $"{point.Coordinate.PrimaryValue:C0}"
                },
                new ColumnSeries<double>
                {
                    Values = maintenanceCosts,
                    Name = "Maintenance",
                    Fill = new SolidColorPaint(SKColors.SteelBlue),
                    YToolTipLabelFormatter = point => $"{point.Coordinate.PrimaryValue:C0}"
                }
            };
        }

        /// <summary>
        /// Initialise les graphiques de tendances
        /// </summary>
        private void InitializeTrendCharts()
        {
            // Tendance consommation
            if (ConsumptionTrend.Count > 0)
            {
                var values = ConsumptionTrend.Select(t => (double)t.Value).ToArray();
                ConsumptionTrendSeries = new ISeries[]
                {
                    new LineSeries<double>
                    {
                        Values = values,
                        Name = "Consommation",
                        Stroke = new SolidColorPaint(SKColors.DodgerBlue, 2),
                        Fill = null,
                        GeometrySize = 4,
                        YToolTipLabelFormatter = point => $"{point.Coordinate.PrimaryValue:F2} L/100km"
                    }
                };
            }

            // Tendance coûts
            if (CostTrend.Count > 0)
            {
                var values = CostTrend.Select(t => (double)t.Value).ToArray();
                CostTrendSeries = new ISeries[]
                {
                    new LineSeries<double>
                    {
                        Values = values,
                        Name = "Coûts",
                        Stroke = new SolidColorPaint(SKColors.OrangeRed, 2),
                        Fill = new SolidColorPaint(SKColors.OrangeRed.WithAlpha(30)),
                        GeometrySize = 4,
                        YToolTipLabelFormatter = point => $"{point.Coordinate.PrimaryValue:C0}"
                    }
                };
            }

            // Tendance kilométrage
            if (VehicleStats.Count > 0)
            {
                var mileageValues = VehicleStats.Select(v => (double)v.CurrentMileage).ToArray();
                MileageTrendSeries = new ISeries[]
                {
                    new LineSeries<double>
                    {
                        Values = mileageValues,
                        Name = "Kilométrage",
                        Stroke = new SolidColorPaint(SKColors.Purple, 2),
                        Fill = null,
                        GeometrySize = 4,
                        YToolTipLabelFormatter = point => $"{point.Coordinate.PrimaryValue:N0} km"
                    }
                };
            }
        }

        /// <summary>
        /// Initialise les graphiques circulaires (pie charts)
        /// </summary>
        private async void InitializePieCharts()
        {
            try
            {
                // Répartition par type de véhicule
                var vehicleTypeStats = await _statisticsService.GetVehicleTypeStatisticsAsync();
                VehicleTypePieSeries = vehicleTypeStats.Select(t => new PieSeries<double>
                {
                    Values = new double[] { (double)t.TotalCost },
                    Name = t.TypeName,
                    DataLabelsPaint = new SolidColorPaint(SKColors.White),
                    DataLabelsSize = 14,
                    DataLabelsFormatter = point => $"{t.TypeName}: {point.Coordinate.PrimaryValue:C0}"
                }).ToArray();

                // Répartition par type de carburant
                var fuelTypeStats = await _statisticsService.GetFuelTypeStatisticsAsync();
                FuelTypePieSeries = fuelTypeStats.Select(f => new PieSeries<double>
                {
                    Values = new double[] { (double)f.TotalCost },
                    Name = f.TypeName,
                    DataLabelsPaint = new SolidColorPaint(SKColors.White),
                    DataLabelsSize = 14,
                    DataLabelsFormatter = point => $"{f.TypeName}: {point.Coordinate.PrimaryValue:C0}"
                }).ToArray();

                // Répartition Carburant vs Maintenance
                var totalFuelCost = VehicleStats.Sum(v => v.TotalFuelCost);
                var totalMaintenanceCost = VehicleStats.Sum(v => v.TotalMaintenanceCost);
                CostBreakdownSeries = new ISeries[]
                {
                    new PieSeries<double>
                    {
                        Values = new double[] { (double)totalFuelCost },
                        Name = "Carburant",
                        Fill = new SolidColorPaint(SKColors.MediumSeaGreen),
                        DataLabelsPaint = new SolidColorPaint(SKColors.White),
                        DataLabelsSize = 16,
                        DataLabelsFormatter = point => $"Carburant\n{point.Coordinate.PrimaryValue:C0}"
                    },
                    new PieSeries<double>
                    {
                        Values = new double[] { (double)totalMaintenanceCost },
                        Name = "Maintenance",
                        Fill = new SolidColorPaint(SKColors.SteelBlue),
                        DataLabelsPaint = new SolidColorPaint(SKColors.White),
                        DataLabelsSize = 16,
                        DataLabelsFormatter = point => $"Maintenance\n{point.Coordinate.PrimaryValue:C0}"
                    }
                };
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur initialisation pie charts: {ex.Message}");
            }
        }

        /// <summary>
        /// Initialise les graphiques de performance
        /// </summary>
        private void InitializePerformanceCharts()
        {
            if (VehicleStats.Count == 0) return;

            // Score d'efficacité (basé sur consommation et coûts)
            var fleetAvgConsumption = VehicleStats.Where(v => v.AverageConsumption > 0).Average(v => v.AverageConsumption);
            var efficiencyScores = VehicleStats.Select(v =>
            {
                if (v.AverageConsumption == 0) return 0.0;
                var consumptionRatio = (double)(fleetAvgConsumption / v.AverageConsumption);
                return Math.Min(5.0, Math.Max(0.0, consumptionRatio * 3.0)); // Score sur 5
            }).ToArray();

            EfficiencyScoreSeries = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = efficiencyScores,
                    Name = "Score d'efficacité",
                    Fill = new SolidColorPaint(SKColors.Goldenrod),
                    YToolTipLabelFormatter = point => $"Score: {point.Coordinate.PrimaryValue:F1}/5"
                }
            };

            // Coût au kilomètre
            var costPerKmValues = VehicleStats.Select(v => (double)v.CostPerKilometer).ToArray();
            CostPerKmSeries = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = costPerKmValues,
                    Name = "Coût/km",
                    Fill = new SolidColorPaint(SKColors.Crimson),
                    YToolTipLabelFormatter = point => $"{point.Coordinate.PrimaryValue:C3}/km"
                }
            };
        }

        #region Gestionnaires d'événements

        private async void Refresh_Click(object sender, RoutedEventArgs e)
        {
            await LoadDataAsync();
            InitializeCharts();
        }

        private async void ExportPdf_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var saveDialog = new Microsoft.Win32.SaveFileDialog
                {
                    Filter = "Fichiers PDF (*.pdf)|*.pdf",
                    FileName = $"Graphiques_FleetManager_{DateTime.Now:yyyyMMdd_HHmm}.pdf"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    var fleetStats = await _statisticsService.GetFleetStatisticsAsync();
                    var (success, message) = _exportService.GenerateAdvancedPdfReport(
                        "Rapport Graphiques Fleet Manager",
                        fleetStats,
                        VehicleStats.ToList(),
                        saveDialog.FileName);

                    if (success)
                    {
                        MessageBox.Show("Export PDF réussi!", "Succès",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'export PDF:\n\n{ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExportImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBox.Show("Export PNG en développement.\n\nFonctionnalité prévue:\n" +
                    "- Capture des graphiques en image haute résolution\n" +
                    "- Export individuel ou groupé\n" +
                    "- Options de format (PNG, JPEG, SVG)",
                    "Export Image", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

        #region Propriétés pour le binding

        /// <summary>
        /// Indique si des données sont disponibles
        /// </summary>
        public bool HasData => Vehicles.Count > 0;

        /// <summary>
        /// Nombre total de véhicules
        /// </summary>
        public int TotalVehicles => Vehicles.Count;

        /// <summary>
        /// Coût total de la flotte
        /// </summary>
        public decimal TotalFleetCost => VehicleStats.Sum(v => v.TotalCost);

        /// <summary>
        /// Consommation moyenne de la flotte
        /// </summary>
        public decimal AverageFleetConsumption
        {
            get
            {
                var vehiclesWithConsumption = VehicleStats.Where(v => v.AverageConsumption > 0).ToList();
                return vehiclesWithConsumption.Any() ? vehiclesWithConsumption.Average(v => v.AverageConsumption) : 0;
            }
        }

        #endregion
    }
}
