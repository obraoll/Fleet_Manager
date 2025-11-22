using System.Windows;
using FleetManager.Services;
using FleetManager.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FleetManager.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            
            // Récupérer le ViewModel depuis le service provider
            var authService = App.ServiceProvider.GetRequiredService<AuthenticationService>();
            DataContext = new LoginViewModel(authService);
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.Password = PasswordBox.Password;
            }
        }
    }
}
