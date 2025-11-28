using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void UsernameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FindName("EmailBorder") is Border border)
            {
                border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6366F1"));
                border.BorderThickness = new Thickness(2);
            }
        }

        private void UsernameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindName("EmailBorder") is Border border)
            {
                border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E5E7EB"));
                border.BorderThickness = new Thickness(1);
            }
        }


        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (FindName("PasswordBorder") is Border border)
            {
                border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#6366F1"));
                border.BorderThickness = new Thickness(2);
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (FindName("PasswordBorder") is Border border)
            {
                border.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E5E7EB"));
                border.BorderThickness = new Thickness(1);
            }
        }
    }
}
