using System.Windows.Controls;
using System;
using System.ComponentModel;
using FleetManager.ViewModels;
using System.Threading.Tasks;

namespace FleetManager.Views
{
    public partial class StatisticsView : UserControl
    {
        private bool _isLoaded = false;

        public StatisticsView()
        {
            InitializeComponent();
            Loaded += StatisticsView_Loaded;
        }

        public StatisticsView(StatisticsViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }

        private void StatisticsView_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!_isLoaded && DataContext is StatisticsViewModel viewModel)
            {
                _isLoaded = true;
                // Chargement différé pour éviter les problèmes d'initialisation
                Dispatcher.InvokeAsync(async () =>
                {
                    await Task.Delay(100); // Petit délai pour s'assurer que tout est initialisé
                    viewModel.RefreshCommand?.Execute(null);
                }, System.Windows.Threading.DispatcherPriority.Background);
            }
        }
    }
}