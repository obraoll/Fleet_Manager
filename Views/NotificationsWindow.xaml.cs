using System.Windows;
using FleetManager.ViewModels;
using FleetManager.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FleetManager.Views
{
    public partial class NotificationsWindow : Window
    {
        private readonly NotificationsViewModel _viewModel;

        public NotificationsWindow()
        {
            InitializeComponent();
            
            var statisticsService = App.ServiceProvider.GetRequiredService<StatisticsService>();
            var persistenceService = App.ServiceProvider.GetRequiredService<NotificationPersistenceService>();
            _viewModel = new NotificationsViewModel(statisticsService, persistenceService);
            DataContext = _viewModel;
            
            Loaded += (s, e) => _viewModel.Initialize();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}

