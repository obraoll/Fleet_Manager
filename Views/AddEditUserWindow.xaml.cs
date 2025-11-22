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

        public ICommand SaveCommand { get; }

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

            InitializeRoles();

            if (_userToEdit != null)
            {
                LoadUserData(_userToEdit);
            }
        }

        private void InitializeRoles()
        {
            // Seul un SuperAdmin peut assigner le rôle SuperAdmin
            if (_authService.CurrentUser?.Role == "SuperAdmin")
            {
                Roles = new ObservableCollection<string>
                {
                    "User",
                    "Admin",
                    "SuperAdmin"
                };
            }
            else
            {
                Roles = new ObservableCollection<string>
                {
                    "User",
                    "Admin"
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
                    var userRole = SelectedRole == "Admin" ? UserRole.Admin : UserRole.User;
                    var (success, message) = await _authService.CreateUserAsync(
                        Username, Password, FullName, Email, userRole);

                    if (success)
                    {
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

                    _userToEdit.FullName = FullName;
                    _userToEdit.Email = Email;
                    _userToEdit.Role = SelectedRole;
                    _userToEdit.IsActive = IsActive;

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
