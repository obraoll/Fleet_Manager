using System.Windows.Controls;
using FleetManager.ViewModels;

namespace FleetManager.Views
{
    /// <summary>
    /// Code-behind pour DashboardView
    /// </summary>
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Événement Loaded - initier les graphiques et animations
        /// </summary>
        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // Les données sont chargées via ViewModel
            // Vous pouvez ajouter des animations ou configurations supplémentaires ici
        }
    }
}