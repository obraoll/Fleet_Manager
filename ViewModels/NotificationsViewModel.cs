using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using FleetManager.Models;
using FleetManager.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace FleetManager.ViewModels
{
    public class NotificationsViewModel : ObservableObject
    {
        private readonly StatisticsService _statisticsService;
        private readonly NotificationPersistenceService _persistenceService;
        private ObservableCollection<DashboardAlert> _allAlerts = new();
        private ObservableCollection<DashboardAlert> _visibleAlerts = new();

        public NotificationsViewModel(StatisticsService statisticsService, NotificationPersistenceService persistenceService)
        {
            _statisticsService = statisticsService ?? throw new ArgumentNullException(nameof(statisticsService));
            _persistenceService = persistenceService ?? throw new ArgumentNullException(nameof(persistenceService));
            
            DeleteAlertCommand = new AsyncRelayCommand<DashboardAlert>(DeleteAlertAsync);
            DeleteAllAlertsCommand = new AsyncRelayCommand(DeleteAllAlertsAsync);
            LoadAlertsCommand = new AsyncRelayCommand(LoadAlertsAsync);
        }

        public ObservableCollection<DashboardAlert> VisibleAlerts
        {
            get => _visibleAlerts;
            set => SetProperty(ref _visibleAlerts, value);
        }

        public ICommand DeleteAlertCommand { get; }
        public ICommand DeleteAllAlertsCommand { get; }
        public ICommand LoadAlertsCommand { get; }

        private async System.Threading.Tasks.Task LoadAlertsAsync()
        {
            try
            {
                var alerts = await _statisticsService.GetDashboardAlertsAsync();
                _allAlerts = new ObservableCollection<DashboardAlert>(alerts);
                ApplyFilters();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement des alertes: {ex.Message}");
            }
        }

        private async System.Threading.Tasks.Task DeleteAlertAsync(DashboardAlert? alert)
        {
            if (alert == null) return;

            // Sauvegarder la suppression de manière permanente
            await _persistenceService.MarkAsDeletedAsync(alert.Id);
            ApplyFilters();
        }

        private async System.Threading.Tasks.Task DeleteAllAlertsAsync()
        {
            if (_visibleAlerts.Count == 0) return;

            // Récupérer tous les IDs des alertes visibles
            var idsToDelete = _visibleAlerts.Select(a => a.Id).ToList();
            
            // Sauvegarder la suppression de manière permanente
            await _persistenceService.MarkMultipleAsDeletedAsync(idsToDelete);
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            // Récupérer les IDs supprimés depuis le service de persistance
            var deletedIds = _persistenceService.GetDeletedNotificationIds();
            
            var filtered = _allAlerts
                .Where(alert => !deletedIds.Contains(alert.Id))
                .OrderByDescending(alert => alert.Date)
                .ToList();

            VisibleAlerts = new ObservableCollection<DashboardAlert>(filtered);
        }

        public void Initialize()
        {
            LoadAlertsCommand.Execute(null);
        }
    }
}

