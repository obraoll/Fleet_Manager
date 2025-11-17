using System.Windows;
using FleetManager.ViewModels;

namespace FleetManager.Views
{
    /// <summary>
    /// Logique d'interaction pour AddFuelRecordWindow.xaml
    /// </summary>
    public partial class AddFuelRecordWindow : Window
    {
        public AddFuelRecordWindow(AddFuelRecordViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
