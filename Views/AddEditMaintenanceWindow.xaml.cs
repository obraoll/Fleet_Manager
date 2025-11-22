using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using FleetManager.Helpers;
using FleetManager.Models;
using FleetManager.Services;
using FleetManager.ViewModels;

namespace FleetManager.Views
{
    public partial class AddEditMaintenanceWindow : Window
    {
        private readonly AddEditMaintenanceViewModel _viewModel;

        public AddEditMaintenanceWindow(
            MaintenanceService maintenanceService,
            VehicleService vehicleService,
            MaintenanceRecord? maintenanceToEdit)
        {
            InitializeComponent();
            _viewModel = new AddEditMaintenanceViewModel(
                maintenanceService, 
                vehicleService, 
                maintenanceToEdit,
                this);
            DataContext = _viewModel;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }

    public class AddEditMaintenanceViewModel : BaseViewModel
    {
        private readonly MaintenanceService _maintenanceService;
        private readonly VehicleService _vehicleService;
        private readonly MaintenanceRecord? _maintenanceToEdit;
        private readonly Window _window;

        // Collections
        private ObservableCollection<Vehicle> _vehicles = new();
        private ObservableCollection<string> _maintenanceTypes = new();

        // Propriétés
        private Vehicle? _selectedVehicle;
        private string _selectedMaintenanceType = "Vidange";
        private DateTime _maintenanceDate = DateTime.Now;
        private decimal _mileage;
        private decimal _cost;
        private string _description = string.Empty;
        private string? _garage;
        private string? _technicianName;
        private string? _parts;
        private string _status = "Terminée";
        private DateTime? _nextMaintenanceDate;
        private decimal? _nextMaintenanceMileage;
        private string? _notes;

        private string _windowTitle = string.Empty;
        private bool _isEditMode;

        public ObservableCollection<Vehicle> Vehicles
        {
            get => _vehicles;
            set => SetProperty(ref _vehicles, value);
        }

        public ObservableCollection<string> MaintenanceTypes
        {
            get => _maintenanceTypes;
            set => SetProperty(ref _maintenanceTypes, value);
        }

        public Vehicle? SelectedVehicle
        {
            get => _selectedVehicle;
            set => SetProperty(ref _selectedVehicle, value);
        }

        public string SelectedMaintenanceType
        {
            get => _selectedMaintenanceType;
            set => SetProperty(ref _selectedMaintenanceType, value);
        }

        public DateTime MaintenanceDate
        {
            get => _maintenanceDate;
            set => SetProperty(ref _maintenanceDate, value);
        }

        public decimal Mileage
        {
            get => _mileage;
            set => SetProperty(ref _mileage, value);
        }

        public decimal Cost
        {
            get => _cost;
            set => SetProperty(ref _cost, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string? Garage
        {
            get => _garage;
            set => SetProperty(ref _garage, value);
        }

        public string? TechnicianName
        {
            get => _technicianName;
            set => SetProperty(ref _technicianName, value);
        }

        public string? Parts
        {
            get => _parts;
            set => SetProperty(ref _parts, value);
        }

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public DateTime? NextMaintenanceDate
        {
            get => _nextMaintenanceDate;
            set => SetProperty(ref _nextMaintenanceDate, value);
        }

        public decimal? NextMaintenanceMileage
        {
            get => _nextMaintenanceMileage;
            set => SetProperty(ref _nextMaintenanceMileage, value);
        }

        public string? Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        public string WindowTitle
        {
            get => _windowTitle;
            set => SetProperty(ref _windowTitle, value);
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set => SetProperty(ref _isEditMode, value);
        }

        public ICommand SaveCommand { get; }

        public AddEditMaintenanceViewModel(
            MaintenanceService maintenanceService,
            VehicleService vehicleService,
            MaintenanceRecord? maintenanceToEdit,
            Window window)
        {
            _maintenanceService = maintenanceService;
            _vehicleService = vehicleService;
            _maintenanceToEdit = maintenanceToEdit;
            _window = window;

            IsEditMode = maintenanceToEdit != null;
            WindowTitle = IsEditMode ? "✏️ Modifier un entretien" : "➕ Nouvel entretien";

            SaveCommand = new AsyncRelayCommand(SaveAsync);

            InitializeMaintenanceTypes();
            _ = LoadDataAsync();
        }

        private void InitializeMaintenanceTypes()
        {
            MaintenanceTypes = new ObservableCollection<string>
            {
                "Vidange",
                "Révision",
                "Pneus",
                "Freins",
                "Courroie",
                "Climatisation",
                "Batterie",
                "Contrôle technique",
                "Réparation",
                "Carrosserie",
                "Autre"
            };
        }

        private async System.Threading.Tasks.Task LoadDataAsync()
        {
            try
            {
                // Charger les véhicules
                var vehicles = await _vehicleService.GetAllVehiclesAsync();
                Vehicles = new ObservableCollection<Vehicle>(vehicles);

                // Si mode modification, charger les données
                if (_maintenanceToEdit != null)
                {
                    LoadMaintenanceData(_maintenanceToEdit);
                }
                else
                {
                    // Mode ajout : présélectionner le premier véhicule
                    SelectedVehicle = Vehicles.FirstOrDefault();
                    if (SelectedVehicle != null)
                    {
                        Mileage = SelectedVehicle.CurrentMileage;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur de chargement:\n\n{ex.Message}",
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadMaintenanceData(MaintenanceRecord maintenance)
        {
            SelectedVehicle = Vehicles.FirstOrDefault(v => v.VehicleId == maintenance.VehicleId);
            MaintenanceDate = maintenance.MaintenanceDate;
            SelectedMaintenanceType = maintenance.MaintenanceType;
            Mileage = maintenance.Mileage;
            Cost = maintenance.Cost;
            Description = maintenance.Description;
            Garage = maintenance.Garage;
            TechnicianName = maintenance.TechnicianName;
            Parts = maintenance.Parts;
            Status = maintenance.Status;
            NextMaintenanceDate = maintenance.NextMaintenanceDate;
            NextMaintenanceMileage = maintenance.NextMaintenanceMileage;
            Notes = maintenance.Notes;
        }

        private async System.Threading.Tasks.Task SaveAsync(object? parameter)
        {
            try
            {
                // Validation
                if (SelectedVehicle == null)
                {
                    MessageBox.Show("Veuillez sélectionner un véhicule", "Validation",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(SelectedMaintenanceType))
                {
                    MessageBox.Show("Veuillez sélectionner un type d'entretien", "Validation",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Description))
                {
                    MessageBox.Show("Veuillez entrer une description", "Validation",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Mileage < 0)
                {
                    MessageBox.Show("Le kilométrage ne peut pas être négatif", "Validation",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Cost < 0)
                {
                    MessageBox.Show("Le coût ne peut pas être négatif", "Validation",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Créer ou modifier l'entretien
                var maintenance = IsEditMode && _maintenanceToEdit != null
                    ? _maintenanceToEdit
                    : new MaintenanceRecord();

                maintenance.VehicleId = SelectedVehicle.VehicleId;
                maintenance.MaintenanceDate = MaintenanceDate;
                maintenance.MaintenanceType = SelectedMaintenanceType;
                maintenance.Mileage = Mileage;
                maintenance.Cost = Cost;
                maintenance.Description = Description;
                maintenance.Garage = Garage;
                maintenance.TechnicianName = TechnicianName;
                maintenance.Parts = Parts;
                maintenance.Status = Status;
                maintenance.NextMaintenanceDate = NextMaintenanceDate;
                maintenance.NextMaintenanceMileage = NextMaintenanceMileage;
                maintenance.Notes = Notes;

                (bool success, string message) result;

                if (IsEditMode)
                {
                    result = await _maintenanceService.UpdateMaintenanceAsync(maintenance);
                }
                else
                {
                    maintenance.CreatedAt = DateTime.Now;
                    result = await _maintenanceService.AddMaintenanceAsync(maintenance);
                }

                if (result.success)
                {
                    MessageBox.Show(result.message, "Succès",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                    _window.DialogResult = true;
                    _window.Close();
                }
                else
                {
                    MessageBox.Show(result.message, "Erreur",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur:\n\n{ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
