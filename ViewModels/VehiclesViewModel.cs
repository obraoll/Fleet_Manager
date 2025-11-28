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
    public class VehiclesViewModel : BaseViewModel
    {
        private readonly VehicleService _vehicleService;
        private readonly IServiceProvider _serviceProvider;
        private ObservableCollection<Vehicle> _vehicles = new();
        private ObservableCollection<Vehicle> _allVehicles = new(); // Tous les véhicules (non filtrés)
        private Vehicle? _selectedVehicle;
        private string _searchText = string.Empty;

        public VehiclesViewModel(VehicleService vehicleService, IServiceProvider serviceProvider)
        {
            System.Diagnostics.Debug.WriteLine("=== CONSTRUCTION VEHICLESVIEWMODEL ===");
            _vehicleService = vehicleService;
            _serviceProvider = serviceProvider;
            LoadVehiclesCommand = new AsyncRelayCommand(LoadVehiclesAsync);
            AddVehicleCommand = new AsyncRelayCommand(AddVehicleAsync);
            EditVehicleCommand = new RelayCommand(EditVehicle);
            DeleteVehicleCommand = new AsyncRelayCommand(DeleteVehicleAsync);
            SearchCommand = new AsyncRelayCommand(SearchVehiclesAsync);
            ResetFiltersCommand = new RelayCommand(ResetFilters);

            System.Diagnostics.Debug.WriteLine("Commandes initialisées - AddVehicleCommand: " + (AddVehicleCommand != null));

            // Charger les véhicules au démarrage
            _ = LoadVehiclesAsync(null);
        }

        public ObservableCollection<Vehicle> Vehicles
        {
            get => _vehicles;
            set => SetProperty(ref _vehicles, value);
        }

        public Vehicle? SelectedVehicle
        {
            get => _selectedVehicle;
            set => SetProperty(ref _selectedVehicle, value);
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

        public ICommand ResetFiltersCommand { get; }

        public ICommand LoadVehiclesCommand { get; }
        public AsyncRelayCommand AddVehicleCommand { get; }
        public ICommand EditVehicleCommand { get; }
        public ICommand DeleteVehicleCommand { get; }
        public ICommand SearchCommand { get; }

        private async Task AddVehicleAsync(object? parameter)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== ADDVEHICLEASYNC DÉCLENCHÉE ===");
                System.Diagnostics.Debug.WriteLine($"ServiceProvider null? {_serviceProvider == null}");

                // Créer le ViewModel pour l'ajout
                System.Diagnostics.Debug.WriteLine("Récupération AddVehicleViewModel...");
                if (_serviceProvider == null)
                {
                    MessageBox.Show("Service provider non disponible.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var addVehicleViewModel = _serviceProvider.GetRequiredService<AddVehicleViewModel>();
                System.Diagnostics.Debug.WriteLine("AddVehicleViewModel récupéré avec succès");

                // Créer et afficher la fenêtre
                System.Diagnostics.Debug.WriteLine("Création AddVehicleWindow...");
                var addVehicleWindow = new AddVehicleWindow(addVehicleViewModel);
                System.Diagnostics.Debug.WriteLine("AddVehicleWindow créée");

                // Afficher en mode modal sans Owner pour éviter les conflits
                System.Diagnostics.Debug.WriteLine("Affichage de la fenêtre modale...");
                var result = addVehicleWindow.ShowDialog();
                System.Diagnostics.Debug.WriteLine($"Fenêtre fermée avec résultat: {result}");

                // Si l'ajout a réussi, recharger la liste
                if (result == true)
                {
                    System.Diagnostics.Debug.WriteLine("Véhicule ajouté - Rechargement de la liste");
                    await LoadVehiclesAsync(null);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION dans AddVehicleAsync: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Erreur lors de l'ouverture de la fenêtre d'ajout:\n\n{ex.Message}\n\nDétails: {ex.InnerException?.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadVehiclesAsync(object? parameter)
        {
            try
            {
                var vehicles = await _vehicleService.GetAllVehiclesAsync();
                _allVehicles = new ObservableCollection<Vehicle>(vehicles);
                await ApplyFiltersAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur: {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Task ApplyFiltersAsync()
        {
            try
            {
                var filtered = _allVehicles.AsEnumerable();

                // Filtre par recherche textuelle
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    var searchLower = SearchText.ToLower();
                    filtered = filtered.Where(v =>
                        (v.RegistrationNumber?.ToLower().Contains(searchLower) ?? false) ||
                        (v.Brand?.ToLower().Contains(searchLower) ?? false) ||
                        (v.Model?.ToLower().Contains(searchLower) ?? false) ||
                        (v.VehicleType?.ToLower().Contains(searchLower) ?? false) ||
                        (v.FuelType?.ToLower().Contains(searchLower) ?? false));
                }

                Vehicles = new ObservableCollection<Vehicle>(filtered.ToList());
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
        }

        private void EditVehicle(object? parameter)
        {
            // Récupérer le véhicule depuis le paramètre ou depuis SelectedVehicle
            var vehicleToEdit = parameter as Vehicle ?? SelectedVehicle;
            
            if (vehicleToEdit == null) return;

            try
            {
                System.Diagnostics.Debug.WriteLine($"=== MODIFICATION VÉHICULE {vehicleToEdit.RegistrationNumber} ===");

                // Créer le ViewModel pour la modification
                var editVehicleViewModel = new EditVehicleViewModel(_vehicleService, vehicleToEdit);

                // Créer et afficher la fenêtre
                var editVehicleWindow = new EditVehicleWindow(editVehicleViewModel);

                // Afficher en mode modal
                System.Diagnostics.Debug.WriteLine("Affichage de la fenêtre de modification...");
                var result = editVehicleWindow.ShowDialog();
                System.Diagnostics.Debug.WriteLine($"Fenêtre fermée avec résultat: {result}");

                // Si la modification a réussi, recharger la liste
                if (result == true)
                {
                    System.Diagnostics.Debug.WriteLine("Véhicule modifié - Rechargement de la liste");
                    _ = LoadVehiclesAsync(null);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"EXCEPTION dans EditVehicle: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                MessageBox.Show($"Erreur lors de l'ouverture de la fenêtre de modification:\n\n{ex.Message}\n\nDétails: {ex.InnerException?.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task DeleteVehicleAsync(object? parameter)
        {
            // Récupérer le véhicule depuis le paramètre ou depuis SelectedVehicle
            var vehicleToDelete = parameter as Vehicle ?? SelectedVehicle;
            
            if (vehicleToDelete == null) return;

            var result = MessageBox.Show(
                $"Voulez-vous vraiment supprimer le véhicule {vehicleToDelete.RegistrationNumber} ?\nCette action est irréversible.",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                var (success, message) = await _vehicleService.DeleteVehicleAsync(vehicleToDelete.VehicleId);
                if (success)
                {
                    await LoadVehiclesAsync(null);
                    MessageBox.Show(message, "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show(message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task SearchVehiclesAsync(object? parameter)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"Recherche véhicules: '{SearchText}'");
                var vehicles = await _vehicleService.SearchVehiclesAsync(SearchText);
                Vehicles = new ObservableCollection<Vehicle>(vehicles);
                System.Diagnostics.Debug.WriteLine($"Trouvés {vehicles.Count} véhicules");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur recherche: {ex.Message}");
                MessageBox.Show($"Erreur lors de la recherche: {ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Classe temporaire pour le dialogue d'édition (sera remplacée par une vraie fenêtre)
    public class VehicleEditDialog : Window
    {
        public VehicleEditDialog(Vehicle vehicle, bool isNew)
        {
            Title = isNew ? "Nouveau véhicule" : "Modifier véhicule";
            Width = 500;
            Height = 400;
            // TODO: Implémenter l'interface complète
        }
    }
}
