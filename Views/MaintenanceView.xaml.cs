using System.Windows.Controls;
using FleetManager.ViewModels;

namespace FleetManager.Views
{
    public partial class MaintenanceView : UserControl
    {
        public MaintenanceView()
        {
            InitializeComponent();
        }

        public MaintenanceView(MaintenanceViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
    }
}
