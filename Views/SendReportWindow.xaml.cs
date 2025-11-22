using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FleetManager.Helpers;
using FleetManager.Services;
using FleetManager.ViewModels;

namespace FleetManager.Views
{
    public partial class SendReportWindow : Window
    {
        private readonly SendReportViewModel _viewModel;

        public SendReportWindow(StatisticsService statisticsService, ExportService exportService, IEmailService emailService)
        {
            InitializeComponent();
            _viewModel = new SendReportViewModel(statisticsService, exportService, emailService);
            DataContext = _viewModel;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SuggestedEmail_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                _viewModel.RecipientEmail = e.AddedItems[0]?.ToString() ?? "";
            }
        }
    }

    public class SendReportViewModel : BaseViewModel
    {
        private readonly StatisticsService _statisticsService;
        private readonly ExportService _exportService;
        private readonly IEmailService _emailService;

        private string _recipientEmail = "";
        private string _selectedReportType = "📊 Statistiques Générales";
        private DateTime _startDate = DateTime.Now.AddMonths(-1);
        private DateTime _endDate = DateTime.Now;
        private string _customMessage = "";
        private bool _isPdfFormat = true;
        private bool _isExcelFormat = false;
        private bool _includeCharts = true;
        private bool _includeVehicleDetails = true;
        private bool _includeRecommendations = true;
        private string _statusMessage = "";
        private Visibility _statusMessageVisibility = Visibility.Collapsed;

        private ObservableCollection<string> _suggestedEmails = new()
        {
            "admin@fleetmanager.com",
            "manager@fleetmanager.com",
            "finance@fleetmanager.com",
            "operations@fleetmanager.com"
        };

        public ObservableCollection<string> SuggestedEmails
        {
            get => _suggestedEmails;
            set => SetProperty(ref _suggestedEmails, value);
        }

        public string RecipientEmail
        {
            get => _recipientEmail;
            set => SetProperty(ref _recipientEmail, value);
        }

        public string SelectedReportType
        {
            get => _selectedReportType;
            set => SetProperty(ref _selectedReportType, value);
        }

        public DateTime StartDate
        {
            get => _startDate;
            set => SetProperty(ref _startDate, value);
        }

        public DateTime EndDate
        {
            get => _endDate;
            set => SetProperty(ref _endDate, value);
        }

        public string CustomMessage
        {
            get => _customMessage;
            set => SetProperty(ref _customMessage, value);
        }

        public bool IsPdfFormat
        {
            get => _isPdfFormat;
            set
            {
                if (SetProperty(ref _isPdfFormat, value) && value)
                {
                    IsExcelFormat = false;
                }
            }
        }

        public bool IsExcelFormat
        {
            get => _isExcelFormat;
            set
            {
                if (SetProperty(ref _isExcelFormat, value) && value)
                {
                    IsPdfFormat = false;
                }
            }
        }

        public bool IncludeCharts
        {
            get => _includeCharts;
            set => SetProperty(ref _includeCharts, value);
        }

        public bool IncludeVehicleDetails
        {
            get => _includeVehicleDetails;
            set => SetProperty(ref _includeVehicleDetails, value);
        }

        public bool IncludeRecommendations
        {
            get => _includeRecommendations;
            set => SetProperty(ref _includeRecommendations, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (SetProperty(ref _statusMessage, value))
                {
                    StatusMessageVisibility = string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
                }
            }
        }

        public Visibility StatusMessageVisibility
        {
            get => _statusMessageVisibility;
            set => SetProperty(ref _statusMessageVisibility, value);
        }

        public ICommand SendCommand { get; }

        public SendReportViewModel(StatisticsService statisticsService, ExportService exportService, IEmailService emailService)
        {
            _statisticsService = statisticsService;
            _exportService = exportService;
            _emailService = emailService;

            SendCommand = new AsyncRelayCommand(SendReportAsync);
        }

        private async System.Threading.Tasks.Task SendReportAsync(object? parameter)
        {
            try
            {
                // Validation
                if (string.IsNullOrWhiteSpace(RecipientEmail))
                {
                    StatusMessage = "❌ Veuillez entrer une adresse email";
                    return;
                }

                if (!IsValidEmail(RecipientEmail))
                {
                    StatusMessage = "❌ Adresse email invalide";
                    return;
                }

                StatusMessage = "📤 Génération du rapport en cours...";

                // Générer le rapport
                var tempPath = Path.Combine(Path.GetTempPath(), 
                    $"Rapport_{DateTime.Now:yyyyMMddHHmmss}.{(IsPdfFormat ? "pdf" : "xlsx")}");

                var content = GenerateReportContent();

                bool generationSuccess;
                string generationMessage;

                if (IsPdfFormat)
                {
                    (generationSuccess, generationMessage) = _exportService.GeneratePdfReport(
                        GetReportTitle(),
                        content,
                        tempPath);
                }
                else
                {
                    // Excel export (si disponible)
                    StatusMessage = "⚠️ Export Excel en cours de développement - Utilisation du PDF";
                    (generationSuccess, generationMessage) = _exportService.GeneratePdfReport(
                        GetReportTitle(),
                        content,
                        tempPath);
                }

                if (!generationSuccess)
                {
                    StatusMessage = $"❌ Erreur de génération: {generationMessage}";
                    return;
                }

                StatusMessage = "📧 Envoi de l'email en cours...";

                // Envoyer par email
                var (success, message) = await _emailService.SendReportAsync(
                    RecipientEmail, 
                    tempPath, 
                    GetReportTitle());

                if (success)
                {
                    StatusMessage = $"✅ Rapport envoyé avec succès à {RecipientEmail}";
                    
                    // Fermer la fenêtre après 2 secondes
                    await System.Threading.Tasks.Task.Delay(2000);
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var window = Application.Current.Windows.OfType<SendReportWindow>().FirstOrDefault();
                        window?.Close();
                    });
                }
                else
                {
                    StatusMessage = $"❌ Erreur d'envoi: {message}";
                }

                // Nettoyer le fichier temporaire
                try
                {
                    if (File.Exists(tempPath))
                    {
                        File.Delete(tempPath);
                    }
                }
                catch
                {
                    // Ignorer les erreurs de suppression
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"❌ Erreur: {ex.Message}";
            }
        }

        private string GetReportTitle()
        {
            var type = SelectedReportType.Replace("📊", "").Replace("📈", "").Replace("💰", "")
                .Replace("⛽", "").Replace("🔧", "").Trim();
            return $"{type} - Fleet Manager";
        }

        private string GenerateReportContent()
        {
            var content = $"{GetReportTitle()}\n";
            content += $"Généré le: {DateTime.Now:dd/MM/yyyy HH:mm}\n";
            content += $"Période: du {StartDate:dd/MM/yyyy} au {EndDate:dd/MM/yyyy}\n\n";

            if (!string.IsNullOrWhiteSpace(CustomMessage))
            {
                content += $"MESSAGE:\n{CustomMessage}\n\n";
            }

            content += "=== STATISTIQUES DE LA PÉRIODE ===\n";
            content += "[Les statistiques détaillées seront incluses ici]\n\n";

            if (IncludeVehicleDetails)
            {
                content += "=== DÉTAILS PAR VÉHICULE ===\n";
                content += "[Détails des véhicules]\n\n";
            }

            if (IncludeRecommendations)
            {
                content += "=== RECOMMANDATIONS ===\n";
                content += "- Optimiser la consommation des véhicules identifiés\n";
                content += "- Planifier les maintenances à venir\n";
                content += "- Surveiller les coûts anormaux\n\n";
            }

            content += "---\n";
            content += "Rapport généré automatiquement par Fleet Manager\n";

            return content;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
