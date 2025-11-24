using System.Windows;
using FleetManager.ViewModels;

namespace FleetManager.Views
{
    public partial class DetailedStatisticsWindow : Window
    {
        public DetailedStatisticsWindow(DetailedStatisticsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
