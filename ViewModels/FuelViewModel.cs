using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FleetManager.Helpers;
using FleetManager.Models;
using FleetManager.Services;
using FleetManager.Views;
using Microsoft.Extensions.DependencyInjection;

namespace FleetManager.ViewModels
{
    public class FuelViewModel : BaseViewModel
    {
        private readonly FuelService _fuelService;
        private readonly VehicleService _vehicleService;
        private readonly AuthenticationService _authService;
        private readonly IServiceProvider _serviceProvider;
        private ObservableCollection<FuelRecord> _fuelRecords = new();
        private ObservableCollection<FuelRecord> _allFuelRecords = new(); // Tous les enregistrements (non filtrés)
        private ObservableCollection<Vehicle> _vehicles = new();
        private Vehicle? _selectedVehicle;
        private FuelRecord? _selectedFuelRecord;
        private string _searchText = string.Empty;

        public FuelViewModel(FuelService fuelService, VehicleService vehicleService, AuthenticationService authService, IServiceProvider serviceProvider)
        {
            _fuelService = fuelService;
            _vehicleService = vehicleService;
            _authService = authService;
            _serviceProvider = serviceProvider;
            LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);
            AddFuelRecordCommand = new AsyncRelayCommand(AddFuelRecordAsync);
            EditFuelRecordCommand = new AsyncRelayCommand(EditFuelRecordAsync);
            DeleteFuelRecordCommand = new AsyncRelayCommand(DeleteFuelRecordAsync);
            ResetFiltersCommand = new RelayCommand(ResetFilters);

            // Charger les données au démarrage
            _ = LoadDataAsync(null);
        }

        public ObservableCollection<FuelRecord> FuelRecords
        {
            get => _fuelRecords;
            set => SetProperty(ref _fuelRecords, value);
        }

        public ObservableCollection<Vehicle> Vehicles
        {
            get => _vehicles;
            set => SetProperty(ref _vehicles, value);
        }

        public Vehicle? SelectedVehicle
        {
            get => _selectedVehicle;
            set
            {
                if (SetProperty(ref _selectedVehicle, value))
                {
                    _ = ApplyFiltersAsync();
                }
            }
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    _ = ApplyFiltersAsync();
                }
            }
        }

        public FuelRecord? SelectedFuelRecord
        {
            get => _selectedFuelRecord;
            set => SetProperty(ref _selectedFuelRecord, value);
        }

        public ICommand LoadDataCommand { get; }
        public AsyncRelayCommand AddFuelRecordCommand { get; }
        public AsyncRelayCommand EditFuelRecordCommand { get; }
        public AsyncRelayCommand DeleteFuelRecordCommand { get; }
        public ICommand ResetFiltersCommand { get; }
        private async Task AddFuelRecordAsync(object? parameter)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== OUVERTURE FENÊTRE AJOUT PLEIN ===");

                // Créer le ViewModel pour l'ajout
                var addFuelRecordViewModel = new AddFuelRecordViewModel(_fuelService, _vehicleService, _authService);

                // Créer et afficher la fenêtre
                var addFuelRecordWindow = new AddFuelRecordWindow(addFuelRecordViewModel);

                // Afficher en mode modal
                System.Diagnostics.Debug.WriteLine("Affichage de la fenêtre d'ajout de plein...");
                var result = addFuelRecordWindow.ShowDialog();
                System.Diagnostics.Debug.WriteLine($"Fenêtre fermée avec résultat: {result}");

                // Si l'ajout a réussi, recharger la liste
                if (result == true)
                {
                    System.Diagnostics.Debug.WriteLine("Plein ajouté - Rechargement de la liste");
                    await LoadDataAsync(null);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION dans AddFuelRecordAsync: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Erreur lors de l'ouverture de la fenêtre d'ajout:\n\n{ex.Message}\n\nDétails: {ex.InnerException?.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task EditFuelRecordAsync(object? parameter)
        {
            if (parameter is not FuelRecord fuelRecord)
                return;

            try
            {
                System.Diagnostics.Debug.WriteLine("=== OUVERTURE FENÊTRE ÉDITION PLEIN ===");

                // Créer le ViewModel pour l'édition
                var editFuelRecordViewModel = new AddFuelRecordViewModel(_fuelService, _vehicleService, _authService, fuelRecord);

                // Créer et afficher la fenêtre
                var editFuelRecordWindow = new AddFuelRecordWindow(editFuelRecordViewModel);

                // Afficher en mode modal
                System.Diagnostics.Debug.WriteLine("Affichage de la fenêtre d'édition de plein...");
                var result = editFuelRecordWindow.ShowDialog();
                System.Diagnostics.Debug.WriteLine($"Fenêtre fermée avec résultat: {result}");

                // Si la modification a réussi, recharger la liste
                if (result == true)
                {
                    System.Diagnostics.Debug.WriteLine("Plein modifié - Rechargement de la liste");
                    await LoadDataAsync(null);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION dans EditFuelRecordAsync: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Erreur lors de la modification du plein:\n\n{ex.Message}", 
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadDataAsync(object? parameter)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("Chargement des données carburant...");

                var fuelRecords = await _fuelService.GetAllFuelRecordsAsync();
                _allFuelRecords = new ObservableCollection<FuelRecord>(fuelRecords);
                System.Diagnostics.Debug.WriteLine($"{fuelRecords.Count} enregistrements carburant chargés");

                var vehicles = await _vehicleService.GetAllVehiclesAsync();
                Vehicles = new ObservableCollection<Vehicle>(vehicles);
                System.Diagnostics.Debug.WriteLine($"{vehicles.Count} véhicules chargés");

                await ApplyFiltersAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur chargement données: {ex.Message}");
                MessageBox.Show($"Erreur lors du chargement des données: {ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Task ApplyFiltersAsync()
        {
            try
            {
                var filtered = _allFuelRecords.AsEnumerable();

                // Filtre par véhicule
                if (SelectedVehicle != null)
                {
                    filtered = filtered.Where(f => f.VehicleId == SelectedVehicle.VehicleId);
                }

                // Filtre par recherche textuelle
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    var searchLower = SearchText.ToLower();
                    filtered = filtered.Where(f =>
                        (f.Vehicle?.RegistrationNumber?.ToLower().Contains(searchLower) ?? false) ||
                        (f.Vehicle?.Brand?.ToLower().Contains(searchLower) ?? false) ||
                        (f.Vehicle?.Model?.ToLower().Contains(searchLower) ?? false) ||
                        (f.Station?.ToLower().Contains(searchLower) ?? false) ||
                        (f.FuelType?.ToLower().Contains(searchLower) ?? false));
                }

                FuelRecords = new ObservableCollection<FuelRecord>(filtered.OrderByDescending(f => f.RefuelDate).ToList());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du filtrage: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        private void ResetFilters(object? parameter)
        {
            SearchText = string.Empty;
            SelectedVehicle = null;
        }



        private async Task DeleteFuelRecordAsync(object? parameter)
        {
            if (parameter is not FuelRecord record) return;

            var result = MessageBox.Show(
                "Voulez-vous vraiment supprimer cet enregistrement ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var (success, message) = await _fuelService.DeleteFuelRecordAsync(record.FuelRecordId);
                if (success)
                {
                    await LoadDataAsync(null);
                }
                else
                {
                    MessageBox.Show(message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
