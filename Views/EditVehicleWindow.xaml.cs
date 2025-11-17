using System.Windows;
using FleetManager.ViewModels;

namespace FleetManager.Views
{
    /// <summary>
    /// Logique d'interaction pour EditVehicleWindow.xaml
    /// </summary>
    public partial class EditVehicleWindow : Window
    {
        public EditVehicleWindow(EditVehicleViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
