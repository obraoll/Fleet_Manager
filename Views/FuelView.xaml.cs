using System.Windows.Controls;
using FleetManager.ViewModels;

namespace FleetManager.Views
{
    /// <summary>
    /// Logique d'interaction pour FuelView.xaml
    /// </summary>
    public partial class FuelView : UserControl
    {
        public FuelView(FuelViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            System.Diagnostics.Debug.WriteLine("=== FUELVIEW CRÉÉE ===");
            System.Diagnostics.Debug.WriteLine($"DataContext défini: {DataContext != null}");
            System.Diagnostics.Debug.WriteLine($"ViewModel type: {viewModel?.GetType().Name}");
        }
    }
}
