using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FleetManager.Components
{
    /// <summary>
    /// Badge de statut réutilisable (Disponible, En Maintenance, etc.)
    /// </summary>
    public partial class StatusBadge : UserControl
    {
        public StatusBadge()
        {
            InitializeComponent();
        }

        // Text
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(StatusBadge), 
                new PropertyMetadata("Status"));
        
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        // Background
        public new static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register("Background", typeof(Brush), typeof(StatusBadge), 
                new PropertyMetadata(Themes.ThemeManager.SuccessLight));
        
        public new Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        // Foreground
        public new static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register("Foreground", typeof(Brush), typeof(StatusBadge), 
                new PropertyMetadata(Themes.ThemeManager.Success));
        
        public new Brush Foreground
        {
            get => (Brush)GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        // Helper method: Set status by name
        public void SetStatus(string status)
        {
            Text = status;
            Background = Themes.ThemeManager.GetStatusBackgroundColor(status);
            Foreground = Themes.ThemeManager.GetStatusColor(status);
        }
    }
}
