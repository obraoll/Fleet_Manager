using System.Windows;
using FleetManager.ViewModels;

namespace FleetManager.Views
{
    /// <summary>
    /// Logique d'interaction pour AddVehicleWindow.xaml
    /// </summary>
    public partial class AddVehicleWindow : Window
    {
        public AddVehicleWindow(AddVehicleViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
