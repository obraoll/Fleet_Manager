using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using FleetManager.Helpers;
using FleetManager.ViewModels;

namespace FleetManager.Views
{
    public partial class SetTargetsWindow : Window
    {
        private readonly SetTargetsViewModel _viewModel;

        public SetTargetsWindow()
        {
            InitializeComponent();
            _viewModel = new SetTargetsViewModel();
            DataContext = _viewModel;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public class SetTargetsViewModel : BaseViewModel
    {
        private decimal _targetConsumption = 8.0m;
        private decimal _targetFuelCost = 5000m;
        private decimal _targetMaintenanceCost = 3000m;
        private int _targetAnnualMileage = 50000;
        private decimal _targetUtilizationRate = 80m;
        private string _targetDescription = "";
        private DateTime _targetStartDate = DateTime.Now;

        public decimal TargetConsumption
        {
            get => _targetConsumption;
            set => SetProperty(ref _targetConsumption, value);
        }

        public decimal TargetFuelCost
        {
            get => _targetFuelCost;
            set => SetProperty(ref _targetFuelCost, value);
        }

        public decimal TargetMaintenanceCost
        {
            get => _targetMaintenanceCost;
            set => SetProperty(ref _targetMaintenanceCost, value);
        }

        public int TargetAnnualMileage
        {
            get => _targetAnnualMileage;
            set => SetProperty(ref _targetAnnualMileage, value);
        }

        public decimal TargetUtilizationRate
        {
            get => _targetUtilizationRate;
            set => SetProperty(ref _targetUtilizationRate, value);
        }

        public string TargetDescription
        {
            get => _targetDescription;
            set => SetProperty(ref _targetDescription, value);
        }

        public DateTime TargetStartDate
        {
            get => _targetStartDate;
            set => SetProperty(ref _targetStartDate, value);
        }

        public ICommand SaveCommand { get; }

        public SetTargetsViewModel()
        {
            SaveCommand = new RelayCommand(SaveTargets);
        }

        private void SaveTargets(object? parameter)
        {
            try
            {
                // TODO: Sauvegarder dans la base de données ou fichier de configuration
                // Pour l'instant, affichage d'un message de confirmation

                var message = $"Objectifs enregistrés:\n\n" +
                             $"Consommation cible: {TargetConsumption:F2} L/100km\n" +
                             $"Budget carburant: {TargetFuelCost:C}/mois\n" +
                             $"Budget maintenance: {TargetMaintenanceCost:C}/mois\n" +
                             $"Kilométrage annuel: {TargetAnnualMileage:N0} km\n" +
                             $"Taux d'utilisation: {TargetUtilizationRate}%\n\n" +
                             $"Appliqués à partir du: {TargetStartDate:dd/MM/yyyy}";

                MessageBox.Show(message, "✅ Objectifs enregistrés", 
                    MessageBoxButton.OK, MessageBoxImage.Information);

                // Fermer la fenêtre
                Application.Current.Dispatcher.Invoke(() =>
                {
                    var window = Application.Current.Windows.OfType<SetTargetsWindow>().FirstOrDefault();
                    window?.Close();
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'enregistrement:\n\n{ex.Message}", 
                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
