using FleetManager.ViewModels;
using System.Windows;

namespace FleetManager.Views
{
    public partial class PeriodComparisonWindow : Window
    {
        public PeriodComparisonWindow(PeriodComparisonViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
