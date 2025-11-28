using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace FleetManager.Components
{
    /// <summary>
    /// Carte de statistiques moderne réutilisable
    /// </summary>
    public partial class StatsCard : UserControl
    {
        public StatsCard()
        {
            InitializeComponent();
        }

        // Icon (emoji ou symbole)
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(string), typeof(StatsCard), 
                new PropertyMetadata("📊"));
        
        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        // Icon Background Color
        public static readonly DependencyProperty IconBackgroundProperty =
            DependencyProperty.Register(nameof(IconBackground), typeof(Brush), typeof(StatsCard), 
                new PropertyMetadata(Themes.ThemeManager.Primary));
        
        public Brush IconBackground
        {
            get => (Brush)GetValue(IconBackgroundProperty);
            set => SetValue(IconBackgroundProperty, value);
        }

        // Value (statistique principale)
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(string), typeof(StatsCard), 
                new PropertyMetadata("0"));
        
        public string Value
        {
            get => (string)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        // Label (description)
        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(StatsCard), 
                new PropertyMetadata("Statistique"));
        
        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        // Trend Text (ex: "+12.5%")
        public static readonly DependencyProperty TrendTextProperty =
            DependencyProperty.Register("TrendText", typeof(string), typeof(StatsCard), 
                new PropertyMetadata(string.Empty));
        
        public string TrendText
        {
            get => (string)GetValue(TrendTextProperty);
            set => SetValue(TrendTextProperty, value);
        }

        // Trend Icon (↑ ou ↓)
        public static readonly DependencyProperty TrendIconProperty =
            DependencyProperty.Register("TrendIcon", typeof(string), typeof(StatsCard), 
                new PropertyMetadata("↑"));
        
        public string TrendIcon
        {
            get => (string)GetValue(TrendIconProperty);
            set => SetValue(TrendIconProperty, value);
        }

        // Trend Color
        public static readonly DependencyProperty TrendColorProperty =
            DependencyProperty.Register(nameof(TrendColor), typeof(Brush), typeof(StatsCard), 
                new PropertyMetadata(Themes.ThemeManager.Success));
        
        public Brush TrendColor
        {
            get => (Brush)GetValue(TrendColorProperty);
            set => SetValue(TrendColorProperty, value);
        }

        // Trend Visibility
        public static readonly DependencyProperty TrendVisibilityProperty =
            DependencyProperty.Register(nameof(TrendVisibility), typeof(Visibility), typeof(StatsCard), 
                new PropertyMetadata(Visibility.Collapsed));
        
        public Visibility TrendVisibility
        {
            get => (Visibility)GetValue(TrendVisibilityProperty);
            set => SetValue(TrendVisibilityProperty, value);
        }
    }
}
