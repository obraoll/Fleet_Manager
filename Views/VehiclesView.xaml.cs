using System.Windows;
using System.Windows.Controls;
using FleetManager.ViewModels;

namespace FleetManager.Views
{
    /// <summary>
    /// Logique d'interaction pour VehiclesView.xaml
    /// </summary>
    public partial class VehiclesView : UserControl
    {
        public VehiclesView(VehiclesViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            System.Diagnostics.Debug.WriteLine("=== VEHICLESVIEW CRÉÉE ===");
            System.Diagnostics.Debug.WriteLine($"DataContext défini: {DataContext != null}");
            System.Diagnostics.Debug.WriteLine($"ViewModel type: {viewModel?.GetType().Name}");
        }
    }
}
