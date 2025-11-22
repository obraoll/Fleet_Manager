using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using FleetManager.Helpers;
using FleetManager.Models;
using FleetManager.Services;
using FleetManager.ViewModels;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using Microsoft.Win32;

namespace FleetManager.Views
{
    public partial class CompareVehiclesWindow : Window
    {
        private readonly CompareVehiclesViewModel _viewModel;

        public CompareVehiclesWindow(VehicleService vehicleService, StatisticsService statisticsService, ExportService exportService)
        {
            InitializeComponent();
            _viewModel = new CompareVehiclesViewModel(vehicleService, statisticsService, exportService);
            DataContext = _viewModel;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class CompareVehiclesViewModel : BaseViewModel
    {
        private readonly VehicleService _vehicleService;
        private readonly StatisticsService _statisticsService;
        private readonly ExportService _exportService;

        private ObservableCollection<Vehicle> _availableVehicles = new();
        private ObservableCollection<VehicleStatistics> _comparisonResults = new();
        private ObservableCollection<string> _recommendations = new();
        private string _selectionMessage = "Sélectionnez 2 à 5 véhicules pour les comparer";

        private IEnumerable<ISeries> _consumptionSeries = Array.Empty<ISeries>();
        private IEnumerable<ISeries> _costSeries = Array.Empty<ISeries>();

        public ObservableCollection<Vehicle> AvailableVehicles
        {
            get => _availableVehicles;
            set => SetProperty(ref _availableVehicles, value);
        }

        public ObservableCollection<VehicleStatistics> ComparisonResults
        {
            get => _comparisonResults;
            set => SetProperty(ref _comparisonResults, value);
        }

        public ObservableCollection<string> Recommendations
        {
            get => _recommendations;
            set => SetProperty(ref _recommendations, value);
        }

        public string SelectionMessage
        {
            get => _selectionMessage;
            set => SetProperty(ref _selectionMessage, value);
        }

        public IEnumerable<ISeries> ConsumptionSeries
        {
            get => _consumptionSeries;
            set => SetProperty(ref _consumptionSeries, value);
        }

        public IEnumerable<ISeries> CostSeries
        {
            get => _costSeries;
            set => SetProperty(ref _costSeries, value);
        }

        public ICommand CompareCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand GeneratePdfCommand { get; }

        public CompareVehiclesViewModel(VehicleService vehicleService, StatisticsService statisticsService, ExportService exportService)
        {
            _vehicleService = vehicleService;
            _statisticsService = statisticsService;
            _exportService = exportService;

            CompareCommand = new AsyncRelayCommand(CompareAsync);
            ResetCommand = new RelayCommand(_ => Reset());
            ExportCommand = new AsyncRelayCommand(ExportAsync);
            GeneratePdfCommand = new AsyncRelayCommand(GeneratePdfAsync);

            _ = LoadVehiclesAsync();
        }

        private async System.Threading.Tasks.Task LoadVehiclesAsync()
        {
            try
            {
                var vehicles = await _vehicleService.GetAllVehiclesAsync();
                AvailableVehicles = new ObservableCollection<Vehicle>(vehicles);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de chargement: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async System.Threading.Tasks.Task CompareAsync(object? parameter)
        {
            try
            {
                // Récupérer les véhicules sélectionnés depuis la ListBox
                var window = Application.Current.Windows.OfType<CompareVehiclesWindow>().FirstOrDefault();
                if (window == null) return;

                var selectedItems = window.VehicleListBox.SelectedItems;
                var selectedVehicles = selectedItems.Cast<Vehicle>().ToList();

                if (selectedVehicles.Count < 2)
                {
                    SelectionMessage = "❌ Veuillez sélectionner au moins 2 véhicules";
                    return;
                }

                if (selectedVehicles.Count > 5)
                {
                    SelectionMessage = "❌ Maximum 5 véhicules peuvent être comparés";
                    return;
                }

                SelectionMessage = $"✅ Comparaison de {selectedVehicles.Count} véhicules...";

                // Charger les statistiques pour chaque véhicule
                var results = new List<VehicleStatistics>();
                foreach (var vehicle in selectedVehicles)
                {
                    var stats = await _statisticsService.GetVehicleStatisticsAsync(vehicle.VehicleId);
                    if (stats != null)
                    {
                        results.Add(stats);
                    }
                }

                ComparisonResults = new ObservableCollection<VehicleStatistics>(results);

                // Créer les graphiques
                CreateCharts(results);

                // Générer les recommandations
                GenerateRecommendations(results);

                SelectionMessage = $"✅ {results.Count} véhicules comparés avec succès";
            }
            catch (Exception ex)
            {
                SelectionMessage = $"❌ Erreur: {ex.Message}";
            }
        }

        private void CreateCharts(List<VehicleStatistics> results)
        {
            // Graphique de consommation
            var consumptionValues = results.Select(r => (double)r.AverageConsumption).ToArray();
            var labels = results.Select(r => r.RegistrationNumber).ToArray();

            ConsumptionSeries = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = consumptionValues,
                    Name = "Consommation (L/100km)",
                    Fill = new SolidColorPaint(SKColors.MediumSeaGreen),
                    Stroke = new SolidColorPaint(SKColors.SeaGreen, 2),
                    DataLabelsSize = 14,
                    DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                    DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
                    DataLabelsFormatter = (point) => $"{point.Coordinate.PrimaryValue:F2}"
                }
            };

            // Graphique de coûts
            var costValues = results.Select(r => (double)r.TotalCost).ToArray();

            CostSeries = new ISeries[]
            {
                new ColumnSeries<double>
                {
                    Values = costValues,
                    Name = "Coût Total (€)",
                    Fill = new SolidColorPaint(SKColors.SteelBlue),
                    Stroke = new SolidColorPaint(SKColors.MidnightBlue, 2),
                    DataLabelsSize = 14,
                    DataLabelsPaint = new SolidColorPaint(SKColors.Black),
                    DataLabelsPosition = LiveChartsCore.Measure.DataLabelsPosition.Top,
                    DataLabelsFormatter = (point) => $"{point.Coordinate.PrimaryValue:F0}€"
                }
            };
        }

        private void GenerateRecommendations(List<VehicleStatistics> results)
        {
            var recommendations = new List<string>();

            if (results.Count == 0) return;

            // Meilleure consommation
            var bestConsumption = results.MinBy(r => r.AverageConsumption);
            if (bestConsumption != null)
            {
                recommendations.Add($"🏆 {bestConsumption.RegistrationNumber} a la meilleure consommation: {bestConsumption.AverageConsumption:F2} L/100km");
            }

            // Pire consommation
            var worstConsumption = results.MaxBy(r => r.AverageConsumption);
            if (worstConsumption != null && worstConsumption != bestConsumption)
            {
                recommendations.Add($"⚠️ {worstConsumption.RegistrationNumber} consomme le plus: {worstConsumption.AverageConsumption:F2} L/100km - Envisager un diagnostic");
            }

            // Coût le plus élevé
            var highestCost = results.MaxBy(r => r.TotalCost);
            if (highestCost != null)
            {
                recommendations.Add($"💰 {highestCost.RegistrationNumber} a le coût total le plus élevé: {highestCost.TotalCost:C} - Surveiller les dépenses");
            }

            // Meilleur ratio coût/km
            var bestRatio = results.MinBy(r => r.CostPerKilometer);
            if (bestRatio != null)
            {
                recommendations.Add($"✨ {bestRatio.RegistrationNumber} est le plus économique: {bestRatio.CostPerKilometer:F4} €/km");
            }

            // Moyenne de consommation
            var avgConsumption = results.Average(r => r.AverageConsumption);
            recommendations.Add($"📊 Consommation moyenne du groupe: {avgConsumption:F2} L/100km");

            Recommendations = new ObservableCollection<string>(recommendations);
        }

        private void Reset()
        {
            ComparisonResults.Clear();
            Recommendations.Clear();
            ConsumptionSeries = Array.Empty<ISeries>();
            CostSeries = Array.Empty<ISeries>();
            SelectionMessage = "Sélectionnez 2 à 5 véhicules pour les comparer";
        }

        private async System.Threading.Tasks.Task ExportAsync(object? parameter)
        {
            try
            {
                if (ComparisonResults.Count == 0)
                {
                    MessageBox.Show("Aucune comparaison à exporter", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var saveDialog = new SaveFileDialog
                {
                    Filter = "Fichiers CSV (*.csv)|*.csv",
                    FileName = $"Comparaison_Vehicules_{DateTime.Now:yyyyMMdd}.csv"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    var (success, message) = await _exportService.ExportStatisticsToCsvAsync(
                        ComparisonResults.ToList(), saveDialog.FileName);

                    if (success)
                    {
                        MessageBox.Show("Export réussi!", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async System.Threading.Tasks.Task GeneratePdfAsync(object? parameter)
        {
            try
            {
                if (ComparisonResults.Count == 0)
                {
                    MessageBox.Show("Aucune comparaison à exporter", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var saveDialog = new SaveFileDialog
                {
                    Filter = "Fichiers PDF (*.pdf)|*.pdf",
                    FileName = $"Comparaison_Vehicules_{DateTime.Now:yyyyMMdd}.pdf"
                };

                if (saveDialog.ShowDialog() == true)
                {
                    var content = GenerateReportContent();
                    var (success, message) = _exportService.GeneratePdfReport(
                        "Comparaison de Véhicules - Fleet Manager",
                        content,
                        saveDialog.FileName);

                    if (success)
                    {
                        MessageBox.Show("Rapport PDF généré!", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show(message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GenerateReportContent()
        {
            var content = $"COMPARAISON DE VÉHICULES\nGénéré le: {DateTime.Now:dd/MM/yyyy HH:mm}\n\n";

            content += "=== VÉHICULES COMPARÉS ===\n";
            foreach (var result in ComparisonResults)
            {
                content += $"\n{result.VehicleName} ({result.RegistrationNumber}):\n";
                content += $"  - Consommation: {result.AverageConsumption:F2} L/100km\n";
                content += $"  - Coût carburant: {result.TotalFuelCost:C}\n";
                content += $"  - Coût maintenance: {result.TotalMaintenanceCost:C}\n";
                content += $"  - Coût total: {result.TotalCost:C}\n";
                content += $"  - Coût/km: {result.CostPerKilometer:F4} €\n";
            }

            content += "\n=== RECOMMANDATIONS ===\n";
            foreach (var recommendation in Recommendations)
            {
                content += $"- {recommendation}\n";
            }

            return content;
        }
    }
}
