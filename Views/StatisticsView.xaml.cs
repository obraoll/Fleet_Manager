using System.Windows.Controls;
using System;
using System.ComponentModel;
using FleetManager.ViewModels;
// LiveCharts is used in XAML; no runtime Axis wiring required here

namespace FleetManager.Views
{
    /// <summary>
    /// Code-behind pour StatisticsView
    /// </summary>
    public partial class StatisticsView : UserControl
    {
        public StatisticsView()
        {
            InitializeComponent();
        }

        public StatisticsView(StatisticsViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }

        /// <summary>
        /// Événement Loaded - initier les animations ou configurations UI
        /// </summary>
        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Les données sont chargées via ViewModel. Les labels X sont affichés
            // via l'ItemsControl lié à `MonthlyLabels` dans le XAML.
        }
        
    }
}