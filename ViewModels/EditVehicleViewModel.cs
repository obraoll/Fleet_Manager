using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FleetManager.Helpers;
using FleetManager.Models;
using FleetManager.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace FleetManager.ViewModels
{
    public class EditVehicleViewModel : BaseViewModel
    {
                // Propriétés d'affichage système
                public string CreatedAtDisplay
                {
                    get => _createdAtDisplay;
                    set => SetProperty(ref _createdAtDisplay, value);
                }

                public string UpdatedAtDisplay
                {
                    get => _updatedAtDisplay;
                    set => SetProperty(ref _updatedAtDisplay, value);
                }

                public string ValidationMessage
                {
                    get => _validationMessage;
                    set => SetProperty(ref _validationMessage, value);
                }

                public bool HasValidationError
                {
                    get => _hasValidationError;
                    set => SetProperty(ref _hasValidationError, value);
                }

                public bool CanSave
                {
                    get => _canSave;
                    set => SetProperty(ref _canSave, value);
                }
        private readonly VehicleService _vehicleService;
        private readonly Vehicle _originalVehicle;

        // Propriétés du véhicule
        private int _vehicleId;
        private string _registrationNumber = string.Empty;
        private string _brand = string.Empty;
        private string _model = string.Empty;
        private int _year = DateTime.Now.Year;
        private string _vehicleType = string.Empty;
        private string _fuelType = string.Empty;
        private decimal _currentMileage;
        private decimal _averageFuelConsumption = 7;
        private DateTime? _purchaseDate;
        private decimal? _purchasePrice;
        private string _status = string.Empty;
        private DateTime? _insuranceExpiryDate;
        private DateTime? _technicalInspectionDate;
        private string _notes = string.Empty;
        private string? _selectedImagePath;
        private System.Windows.Media.Imaging.BitmapImage? _selectedImagePreview;
        private string? _currentImagePath;
        private bool _imageRemoved;

        // Propriétés d'affichage système
        private string _createdAtDisplay = string.Empty;
        private string _updatedAtDisplay = string.Empty;

        // Propriétés de validation
        private string _validationMessage = string.Empty;
        private bool _hasValidationError;
        private bool _canSave = true;

        public EditVehicleViewModel(VehicleService vehicleService, Vehicle vehicle)
        {
            _vehicleService = vehicleService;
            _originalVehicle = vehicle;

            // Initialiser les commandes
            SaveCommand = new AsyncRelayCommand(SaveVehicleAsync, CanExecuteSave);
            CancelCommand = new RelayCommand(param => CancelEdit(param as Window));
            SelectImageCommand = new RelayCommand(_ => SelectImage());
            RemoveImageCommand = new RelayCommand(_ => RemoveImage());

            // Charger les données du véhicule
            LoadVehicleData();

            // Validation en temps réel
            PropertyChanged += OnPropertyChanged;
        }

        #region Propriétés du véhicule

        public int VehicleId
        {
            get => _vehicleId;
            set => SetProperty(ref _vehicleId, value);
        }

        public string RegistrationNumber
        {
            get => _registrationNumber;
            set
            {
                SetProperty(ref _registrationNumber, value);
                ValidateForm();
            }
        }

        public string Brand
        {
            get => _brand;
            set
            {
                SetProperty(ref _brand, value);
                ValidateForm();
            }
        }

        public string Model
        {
            get => _model;
            set
            {
                SetProperty(ref _model, value);
                ValidateForm();
            }
        }

        public int Year
        {
            get => _year;
            set
            {
                SetProperty(ref _year, value);
                ValidateForm();
            }
        }

        public string VehicleType
        {
            get => _vehicleType;
            set => SetProperty(ref _vehicleType, value);
        }

        public string FuelType
        {
            get => _fuelType;
            set => SetProperty(ref _fuelType, value);
        }

        public decimal CurrentMileage
        {
            get => _currentMileage;
            set => SetProperty(ref _currentMileage, value);
        }

        public decimal AverageFuelConsumption
        {
            get => _averageFuelConsumption;
            set => SetProperty(ref _averageFuelConsumption, value);
        }

        public DateTime? PurchaseDate
        {
            get => _purchaseDate;
            set => SetProperty(ref _purchaseDate, value);
        }

        public decimal? PurchasePrice
        {
            get => _purchasePrice;
            set => SetProperty(ref _purchasePrice, value);
        }

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public DateTime? InsuranceExpiryDate
        {
            get => _insuranceExpiryDate;
            set => SetProperty(ref _insuranceExpiryDate, value);
        }

        public DateTime? TechnicalInspectionDate
        {
            get => _technicalInspectionDate;
            set => SetProperty(ref _technicalInspectionDate, value);
        }

        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        public string? SelectedImagePath
        {
            get => _selectedImagePath;
            set
            {
                if (SetProperty(ref _selectedImagePath, value))
                {
                    UpdateImagePreview();
                }
            }
        }

        public System.Windows.Media.Imaging.BitmapImage? SelectedImagePreview
        {
            get => _selectedImagePreview;
            set => SetProperty(ref _selectedImagePreview, value);
        }

        #endregion

        #region Commandes
        public AsyncRelayCommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand SelectImageCommand { get; }
        public ICommand RemoveImageCommand { get; }

        #endregion

        #region Méthodes

        private void LoadVehicleData()
        {
            VehicleId = _originalVehicle.VehicleId;
            RegistrationNumber = _originalVehicle.RegistrationNumber;
            Brand = _originalVehicle.Brand;
            Model = _originalVehicle.Model;
            Year = _originalVehicle.Year;
            VehicleType = _originalVehicle.VehicleType ?? string.Empty;
            FuelType = _originalVehicle.FuelType ?? string.Empty;
            CurrentMileage = _originalVehicle.CurrentMileage;
            AverageFuelConsumption = _originalVehicle.AverageFuelConsumption;
            PurchaseDate = _originalVehicle.PurchaseDate;
            PurchasePrice = _originalVehicle.PurchasePrice;
            Status = _originalVehicle.Status ?? string.Empty;
            InsuranceExpiryDate = _originalVehicle.InsuranceExpiryDate;
            TechnicalInspectionDate = _originalVehicle.TechnicalInspectionDate;
            Notes = _originalVehicle.Notes ?? string.Empty;
            _currentImagePath = _originalVehicle.ImagePath;
            _imageRemoved = false; // Initialiser le flag

            // Charger l'image existante si elle existe
            if (!string.IsNullOrWhiteSpace(_currentImagePath))
            {
                UpdateImagePreviewFromPath(_currentImagePath);
            }

            // Informations système
            CreatedAtDisplay = $"Créé le : {_originalVehicle.CreatedAt:dd/MM/yyyy à HH:mm}";
            UpdatedAtDisplay = string.Empty;

            System.Diagnostics.Debug.WriteLine($"Données véhicule chargées: {RegistrationNumber} - {Brand} {Model}");
        }

        private void SelectImage()
        {
            var openDialog = new OpenFileDialog
            {
                Title = "Sélectionner une image du véhicule",
                Filter = "Images|*.jpg;*.jpeg;*.png;*.bmp;*.gif|Tous les fichiers|*.*",
                FilterIndex = 1
            };

            if (openDialog.ShowDialog() == true)
            {
                SelectedImagePath = openDialog.FileName;
            }
        }

        private void RemoveImage()
        {
            // Marquer que l'image doit être supprimée
            _imageRemoved = true;
            SelectedImagePath = null;
            SelectedImagePreview = null;
        }

        private void UpdateImagePreview()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SelectedImagePath) || !System.IO.File.Exists(SelectedImagePath))
                {
                    // Si pas de nouvelle image, afficher l'image existante
                    if (!string.IsNullOrWhiteSpace(_currentImagePath))
                    {
                        UpdateImagePreviewFromPath(_currentImagePath);
                    }
                    else
                    {
                        SelectedImagePreview = null;
                    }
                    return;
                }

                var bitmap = new System.Windows.Media.Imaging.BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(SelectedImagePath, UriKind.Absolute);
                bitmap.EndInit();
                bitmap.Freeze();
                SelectedImagePreview = bitmap;
            }
            catch
            {
                SelectedImagePreview = null;
            }
        }

        private void UpdateImagePreviewFromPath(string imagePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imagePath) || !System.IO.File.Exists(imagePath))
                {
                    SelectedImagePreview = null;
                    return;
                }

                var bitmap = new System.Windows.Media.Imaging.BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                bitmap.EndInit();
                bitmap.Freeze();
                SelectedImagePreview = bitmap;
            }
            catch
            {
                SelectedImagePreview = null;
            }
        }

        private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(ValidationMessage) &&
                e.PropertyName != nameof(HasValidationError) &&
                e.PropertyName != nameof(CanSave) &&
                e.PropertyName != nameof(CreatedAtDisplay) &&
                e.PropertyName != nameof(UpdatedAtDisplay) &&
                e.PropertyName != nameof(SelectedImagePath) &&
                e.PropertyName != nameof(SelectedImagePreview))
            {
                ValidateForm();
            }
        }

        private void ValidateForm()
        {
            var errors = new List<string>();

            // Validation des champs obligatoires
            if (string.IsNullOrWhiteSpace(RegistrationNumber))
                errors.Add("L'immatriculation est obligatoire");

            if (string.IsNullOrWhiteSpace(Brand))
                errors.Add("La marque est obligatoire");

            if (string.IsNullOrWhiteSpace(Model))
                errors.Add("Le modèle est obligatoire");

            if (Year < 1900 || Year > DateTime.Now.Year + 1)
                errors.Add("L'année doit être comprise entre 1900 et " + (DateTime.Now.Year + 1));

            // Validation du format de l'immatriculation (français)
            if (!string.IsNullOrWhiteSpace(RegistrationNumber))
            {
                var regexPattern = @"^[A-Z]{2}-\d{3}-[A-Z]{2}$|^\d{3,4}\s?[A-Z]{1,3}\s?\d{2}$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(RegistrationNumber.ToUpper(), regexPattern))
                {
                    errors.Add("Format d'immatriculation invalide (ex: AB-123-CD ou 123 AB 45)");
                }
            }

            // VIN validation removed

            // Validation des valeurs numériques
            if (CurrentMileage < 0)
                errors.Add("Le kilométrage ne peut pas être négatif");

            if (AverageFuelConsumption <= 0)
                errors.Add("La consommation moyenne doit être positive");

            if (PurchasePrice < 0)
                errors.Add("Le prix d'achat ne peut pas être négatif");

            // Vérifier que le kilométrage n'a pas diminué de manière importante
            if (CurrentMileage < _originalVehicle.CurrentMileage - 1000)
            {
                errors.Add("Le kilométrage ne peut pas diminuer de plus de 1000 km par rapport à la valeur précédente");
            }

            // Mettre à jour l'état de validation
            HasValidationError = errors.Count > 0;
            ValidationMessage = string.Join("\n", errors);
            CanSave = !HasValidationError;

            // Notifier le changement pour la commande
            SaveCommand.RaiseCanExecuteChanged();
        }

        private bool CanExecuteSave(object? parameter)
        {
            return CanSave && !string.IsNullOrWhiteSpace(RegistrationNumber) &&
                   !string.IsNullOrWhiteSpace(Brand) && !string.IsNullOrWhiteSpace(Model);
        }

        private async Task SaveVehicleAsync(object? parameter)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("=== MODIFICATION VÉHICULE DÉMARRÉE ===");

                // Validation finale
                ValidateForm();
                if (HasValidationError)
                {
                    MessageBox.Show("Veuillez corriger les erreurs avant de continuer.",
                        "Validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Gérer l'image : nouvelle, suppression, ou conservation
                string? newImagePath = null;
                var imageService = App.ServiceProvider.GetRequiredService<VehicleImageService>();
                
                if (!string.IsNullOrWhiteSpace(SelectedImagePath) && SelectedImagePath != _currentImagePath)
                {
                    // Une nouvelle image a été sélectionnée
                    // Supprimer l'ancienne image si elle existe
                    if (!string.IsNullOrWhiteSpace(_currentImagePath))
                    {
                        imageService.DeleteVehicleImage(_currentImagePath);
                    }
                    
                    // Sauvegarder la nouvelle image
                    newImagePath = imageService.SaveVehicleImage(SelectedImagePath, VehicleId);
                    _imageRemoved = false; // Réinitialiser le flag
                }
                else if (_imageRemoved && !string.IsNullOrWhiteSpace(_currentImagePath))
                {
                    // L'image a été explicitement supprimée par l'utilisateur
                    // Supprimer l'ancienne image du disque
                    imageService.DeleteVehicleImage(_currentImagePath);
                    newImagePath = null; // Pas d'image
                }
                else
                {
                    // Conserver l'image existante (pas de changement)
                    newImagePath = _currentImagePath;
                }

                // Créer l'objet véhicule avec les modifications
                var updatedVehicle = new Vehicle
                {
                    VehicleId = VehicleId,
                    RegistrationNumber = RegistrationNumber.ToUpper().Trim(),
                    Brand = Brand.Trim(),
                    Model = Model.Trim(),
                    Year = Year,
                    VehicleType = VehicleType,
                    FuelType = FuelType,
                    CurrentMileage = CurrentMileage,
                    AverageFuelConsumption = AverageFuelConsumption,
                    PurchaseDate = PurchaseDate,
                    PurchasePrice = PurchasePrice,
                    Status = Status,
                    InsuranceExpiryDate = InsuranceExpiryDate,
                    TechnicalInspectionDate = TechnicalInspectionDate,
                    Notes = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim(),
                    ImagePath = newImagePath ?? _currentImagePath, // Garder l'ancienne si pas de nouvelle
                    CreatedAt = _originalVehicle.CreatedAt
                };

                System.Diagnostics.Debug.WriteLine($"Modification véhicule: {updatedVehicle.RegistrationNumber} - {updatedVehicle.Brand} {updatedVehicle.Model}");

                // Sauvegarder via le service
                var (success, message) = await _vehicleService.UpdateVehicleAsync(updatedVehicle);

                if (success)
                {
                    System.Diagnostics.Debug.WriteLine("Véhicule modifié avec succès");

                    MessageBox.Show("Véhicule modifié avec succès !", "Succès",
                        MessageBoxButton.OK, MessageBoxImage.Information);

                    // Fermer la fenêtre
                    if (parameter is Window window)
                    {
                        window.DialogResult = true;
                        window.Close();
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Erreur modification véhicule: {message}");
                    MessageBox.Show($"Erreur lors de la modification du véhicule:\n\n{message}",
                        "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception modification véhicule: {ex.Message}");
                MessageBox.Show($"Erreur inattendue:\n\n{ex.Message}", "Erreur",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelEdit(Window? window)
        {
            // Vérifier s'il y a des modifications
            bool hasChanges = HasDataChanged();

            if (hasChanges)
            {
                var result = MessageBox.Show(
                    "Êtes-vous sûr de vouloir annuler ?\nTous les modifications seront perdues.",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                    return;
            }

            if (window != null)
            {
                window.DialogResult = false;
                window.Close();
            }
        }

        private bool HasDataChanged()
        {
                 return RegistrationNumber != _originalVehicle.RegistrationNumber ||
                     Brand != _originalVehicle.Brand ||
                     Model != _originalVehicle.Model ||
                     Year != _originalVehicle.Year ||
                     VehicleType != (_originalVehicle.VehicleType ?? string.Empty) ||
                     FuelType != (_originalVehicle.FuelType ?? string.Empty) ||
                     CurrentMileage != _originalVehicle.CurrentMileage ||
                     AverageFuelConsumption != _originalVehicle.AverageFuelConsumption ||
                     PurchaseDate != _originalVehicle.PurchaseDate ||
                     PurchasePrice != _originalVehicle.PurchasePrice ||
                     Status != (_originalVehicle.Status ?? string.Empty) ||
                     InsuranceExpiryDate != _originalVehicle.InsuranceExpiryDate ||
                     TechnicalInspectionDate != _originalVehicle.TechnicalInspectionDate ||
                     Notes != (_originalVehicle.Notes ?? string.Empty);
        }

        #endregion
    }
}
