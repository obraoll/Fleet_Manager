using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FleetManager.Helpers;
using FleetManager.Models;
using FleetManager.Services;
using FleetManager.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;

namespace FleetManager.Views
{
    public partial class AddEditUserWindow : Window
    {
        private readonly AddEditUserViewModel _viewModel;

        public AddEditUserWindow(AuthenticationService authService, User? userToEdit)
        {
            InitializeComponent();
            _viewModel = new AddEditUserViewModel(authService, userToEdit, this);
            DataContext = _viewModel;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                _viewModel.Password = passwordBox.Password;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                _viewModel.ConfirmPassword = passwordBox.Password;
            }
        }
    }

    public class AddEditUserViewModel : BaseViewModel
    {
        private readonly AuthenticationService _authService;
        private readonly User? _userToEdit;
        private readonly Window _window;

        // Collections
        private ObservableCollection<string> _roles = new();

        // Propriétés
        private string _username = string.Empty;
        private string _fullName = string.Empty;
        private string? _email;
        private string _password = string.Empty;
        private string _confirmPassword = string.Empty;
        private string _selectedRole = "User";
        private bool _isActive = true;
        private string? _selectedImagePath;
        private System.Windows.Media.Imaging.BitmapImage? _selectedImagePreview;
        private string? _currentImagePath;
        private bool _imageRemoved;
        private bool _hasImage;

        private string _windowTitle = string.Empty;
        private bool _isNewUser = true;

        public ObservableCollection<string> Roles
        {
            get => _roles;
            set => SetProperty(ref _roles, value);
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string FullName
        {
            get => _fullName;
            set => SetProperty(ref _fullName, value);
        }

        public string? Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        public string SelectedRole
        {
            get => _selectedRole;
            set => SetProperty(ref _selectedRole, value);
        }

        public bool IsActive
        {
            get => _isActive;
            set => SetProperty(ref _isActive, value);
        }

        public string WindowTitle
        {
            get => _windowTitle;
            set => SetProperty(ref _windowTitle, value);
        }

        public bool IsNewUser
        {
            get => _isNewUser;
            set => SetProperty(ref _isNewUser, value);
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
            set
            {
                if (SetProperty(ref _selectedImagePreview, value))
                {
                    HasImage = value != null || (!string.IsNullOrWhiteSpace(_currentImagePath) && !_imageRemoved);
                }
            }
        }

        public bool HasImage
        {
            get => _hasImage;
            set => SetProperty(ref _hasImage, value);
        }

        public ICommand SaveCommand { get; }
        public ICommand SelectImageCommand { get; }
        public ICommand RemoveImageCommand { get; }

        public AddEditUserViewModel(
            AuthenticationService authService,
            User? userToEdit,
            Window window)
        {
            _authService = authService;
            _userToEdit = userToEdit;
            _window = window;

            IsNewUser = userToEdit == null;
            WindowTitle = IsNewUser ? "➕ Nouvel utilisateur" : "✏️ Modifier l'utilisateur";

            SaveCommand = new AsyncRelayCommand(SaveAsync);
            SelectImageCommand = new RelayCommand(_ => SelectImage());
            RemoveImageCommand = new RelayCommand(_ => RemoveImage());

            InitializeRoles();

            if (_userToEdit != null)
            {
                LoadUserData(_userToEdit);
            }
        }

        private void InitializeRoles()
        {
            // Seul un SuperAdmin peut assigner les rôles Admin et SuperAdmin
            if (_authService.CurrentUser?.Role == "SuperAdmin")
            {
                Roles = new ObservableCollection<string>
                {
                    "User",
                    "Admin",
                    "SuperAdmin"
                };
            }
            else if (_authService.CurrentUser?.Role == "Admin")
            {
                // Un Admin ne peut créer que des utilisateurs User
                Roles = new ObservableCollection<string>
                {
                    "User"
                };
            }
            else
            {
                // Par défaut, seulement User
                Roles = new ObservableCollection<string>
                {
                    "User"
                };
            }
        }

        private void LoadUserData(User user)
        {
            Username = user.Username;
            FullName = user.FullName;
            Email = user.Email;
            SelectedRole = user.Role;
            IsActive = user.IsActive;
            _currentImagePath = user.ImagePath;
            _imageRemoved = false;

            // Charger l'image existante si elle existe
            if (!string.IsNullOrWhiteSpace(_currentImagePath))
            {
                UpdateImagePreviewFromPath(_currentImagePath);
                HasImage = true;
            }
            else
            {
                HasImage = false;
            }
        }

        private void SelectImage()
        {
            var openDialog = new OpenFileDialog
            {
                Title = "Sélectionner une photo de profil",
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
            _imageRemoved = true;
            SelectedImagePath = null;
            SelectedImagePreview = null;
            HasImage = false;
        }

        private void UpdateImagePreview()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SelectedImagePath) || !System.IO.File.Exists(SelectedImagePath))
                {
                    // Si pas de nouvelle image, afficher l'image existante
                    if (!string.IsNullOrWhiteSpace(_currentImagePath) && !_imageRemoved)
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

        private async Task SaveAsync(object? parameter)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(Username))
                {
                    MessageBox.Show("Le nom d'utilisateur est obligatoire.", "Validation",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(FullName))
                {
                    MessageBox.Show("Le nom complet est obligatoire.", "Validation",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (IsNewUser)
                {
                    // Validation mot de passe pour nouveau
                    if (string.IsNullOrWhiteSpace(Password))
                    {
                        MessageBox.Show("Le mot de passe est obligatoire.", "Validation",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (Password.Length < 6)
                    {
                        MessageBox.Show("Le mot de passe doit contenir au moins 6 caractères.", "Validation",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (Password != ConfirmPassword)
                    {
                        MessageBox.Show("Les mots de passe ne correspondent pas.", "Validation",
                            MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Créer nouvel utilisateur
                    // Protection : un Admin ne peut créer que des User
                    UserRole userRole;
                    if (SelectedRole == "SuperAdmin")
                    {
                        if (_authService.CurrentUser?.Role != "SuperAdmin")
                        {
                            MessageBox.Show("Seul un Super Administrateur peut créer un autre Super Administrateur.", "Action non autorisée",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        userRole = UserRole.SuperAdmin;
                    }
                    else if (SelectedRole == "Admin")
                    {
                        if (_authService.CurrentUser?.Role != "SuperAdmin")
                        {
                            MessageBox.Show("Seul un Super Administrateur peut créer un Administrateur.", "Action non autorisée",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                        userRole = UserRole.Admin;
                    }
                    else
                    {
                        userRole = UserRole.User;
                    }

                    var (success, message) = await _authService.CreateUserAsync(
                        Username, Password, FullName, Email, userRole);

                    if (success)
                    {
                        // Si une image a été sélectionnée, la sauvegarder
                        if (!string.IsNullOrWhiteSpace(SelectedImagePath))
                        {
                            // Récupérer l'utilisateur créé pour obtenir son ID
                            var createdUser = await _authService.GetUserByUsernameAsync(Username);
                            if (createdUser != null)
                            {
                                var imageService = App.ServiceProvider.GetRequiredService<UserImageService>();
                                var savedImagePath = imageService.SaveUserImage(SelectedImagePath, createdUser.UserId);
                                
                                if (!string.IsNullOrWhiteSpace(savedImagePath))
                                {
                                    // Mettre à jour l'utilisateur avec le chemin de l'image
                                    createdUser.ImagePath = savedImagePath;
                                    await _authService.UpdateUserAsync(createdUser);
                                }
                            }
                        }

                        MessageBox.Show("Utilisateur créé avec succès.", "Succès",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        _window.DialogResult = true;
                        _window.Close();
                    }
                    else
                    {
                        MessageBox.Show($"Erreur: {message}", "Erreur",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    // Modifier utilisateur existant
                    if (_userToEdit == null) return;

                    // Protection : un Admin ne peut pas modifier un autre Admin ou SuperAdmin
                    if (_userToEdit.Role == "Admin" || _userToEdit.Role == "SuperAdmin")
                    {
                        if (_authService.CurrentUser?.Role != "SuperAdmin")
                        {
                            MessageBox.Show("Seul un Super Administrateur peut modifier un Administrateur ou un Super Administrateur.", 
                                "Action non autorisée", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }

                    // Protection : un Admin ne peut pas promouvoir quelqu'un en Admin ou SuperAdmin
                    if ((SelectedRole == "Admin" || SelectedRole == "SuperAdmin") && _authService.CurrentUser?.Role != "SuperAdmin")
                    {
                        MessageBox.Show("Seul un Super Administrateur peut promouvoir un utilisateur en Administrateur.", 
                            "Action non autorisée", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    _userToEdit.FullName = FullName;
                    _userToEdit.Email = Email;
                    _userToEdit.Role = SelectedRole;
                    _userToEdit.IsActive = IsActive;

                    // Gérer l'image : nouvelle, suppression, ou conservation
                    var imageService = App.ServiceProvider.GetRequiredService<UserImageService>();
                    string? newImagePath = null;
                    
                    if (!string.IsNullOrWhiteSpace(SelectedImagePath) && SelectedImagePath != _currentImagePath)
                    {
                        // Une nouvelle image a été sélectionnée
                        // Supprimer l'ancienne image si elle existe
                        if (!string.IsNullOrWhiteSpace(_currentImagePath))
                        {
                            imageService.DeleteUserImage(_currentImagePath);
                        }
                        
                        // Sauvegarder la nouvelle image
                        newImagePath = imageService.SaveUserImage(SelectedImagePath, _userToEdit.UserId);
                        _imageRemoved = false;
                    }
                    else if (_imageRemoved && !string.IsNullOrWhiteSpace(_currentImagePath))
                    {
                        // L'image a été explicitement supprimée par l'utilisateur
                        imageService.DeleteUserImage(_currentImagePath);
                        newImagePath = null;
                    }
                    else
                    {
                        // Conserver l'image existante (pas de changement)
                        newImagePath = _currentImagePath;
                    }

                    _userToEdit.ImagePath = newImagePath;

                    var success = await _authService.UpdateUserAsync(_userToEdit);

                    if (success)
                    {
                        MessageBox.Show("Utilisateur modifié avec succès.", "Succès",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        _window.DialogResult = true;
                        _window.Close();
                    }
                    else
                    {
                        MessageBox.Show("Erreur lors de la modification de l'utilisateur.", "Erreur",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
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
