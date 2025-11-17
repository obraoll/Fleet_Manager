using System.Windows;
using FleetManager.ViewModels;

namespace FleetManager.Views
{
    public partial class LoginWindow : Window
    {
        private readonly LoginViewModel _viewModel;

        public LoginWindow(LoginViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = viewModel;

            // Lier le PasswordBox au ViewModel
            PasswordBox.PasswordChanged += (s, e) =>
            {
                _viewModel.Password = PasswordBox.Password;
            };

            // S'assurer que le focus est sur le nom d'utilisateur au démarrage
            Loaded += (s, e) =>
            {
                // Focus sur le TextBox du nom d'utilisateur
                var usernameTextBox = FindName("UsernameTextBox") as System.Windows.Controls.TextBox;
                usernameTextBox?.Focus();
            };
        }
    }
}
