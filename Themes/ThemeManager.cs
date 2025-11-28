using System;
using System.Windows;
using System.Windows.Media;

namespace FleetManager.Themes
{
    /// <summary>
    /// Gestionnaire de thème centralisé pour l'application Fleet Manager
    /// Fournit un accès facile aux couleurs et styles du design system
    /// </summary>
    public static class ThemeManager
    {
        // ========================================================================================
        // COULEURS PRIMAIRES
        // ========================================================================================
        
        public static SolidColorBrush Primary => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4F46E5"));
        public static SolidColorBrush PrimaryHover => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#4338CA"));
        public static SolidColorBrush PrimaryLight => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EEF2FF"));
        
        // ========================================================================================
        // COULEURS SECONDAIRES
        // ========================================================================================
        
        public static SolidColorBrush Success => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#10B981"));
        public static SolidColorBrush SuccessLight => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#D1FAE5"));
        
        public static SolidColorBrush Warning => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F59E0B"));
        public static SolidColorBrush WarningLight => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEF3C7"));
        
        public static SolidColorBrush Danger => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF4444"));
        public static SolidColorBrush DangerLight => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FEE2E2"));
        
        public static SolidColorBrush Info => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3B82F6"));
        public static SolidColorBrush InfoLight => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DBEAFE"));
        
        // ========================================================================================
        // COULEURS NEUTRES
        // ========================================================================================
        
        public static SolidColorBrush Background => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F8FAFC"));
        public static SolidColorBrush Surface => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFF"));
        public static SolidColorBrush Sidebar => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B"));
        public static SolidColorBrush SidebarHover => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#334155"));
        
        // ========================================================================================
        // COULEURS DE TEXTE
        // ========================================================================================
        
        public static SolidColorBrush TextPrimary => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#1E293B"));
        public static SolidColorBrush TextSecondary => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#64748B"));
        public static SolidColorBrush TextMuted => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#94A3B8"));
        public static SolidColorBrush TextOnPrimary => new SolidColorBrush(Colors.White);
        
        // ========================================================================================
        // COULEURS DE BORDURE
        // ========================================================================================
        
        public static SolidColorBrush Border => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2E8F0"));
        public static SolidColorBrush BorderLight => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F1F5F9"));
        public static SolidColorBrush Divider => new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E2E8F0"));
        
        // ========================================================================================
        // TAILLES
        // ========================================================================================
        
        public const double BorderRadiusSm = 6;
        public const double BorderRadiusMd = 8;
        public const double BorderRadiusLg = 12;
        public const double BorderRadiusXl = 16;
        
        public const double SpacingXs = 4;
        public const double SpacingSm = 8;
        public const double SpacingMd = 16;
        public const double SpacingLg = 24;
        public const double SpacingXl = 32;
        
        // ========================================================================================
        // MÉTHODES UTILITAIRES
        // ========================================================================================
        
        /// <summary>
        /// Obtient une couleur de statut basée sur une chaîne
        /// </summary>
        public static SolidColorBrush GetStatusColor(string status)
        {
            return status?.ToLower() switch
            {
                "actif" or "active" or "en service" => Success,
                "maintenance" or "en maintenance" => Warning,
                "inactif" or "inactive" or "hors service" => Danger,
                "disponible" or "available" => Info,
                _ => TextSecondary
            };
        }
        
        /// <summary>
        /// Obtient une couleur de fond claire pour un statut
        /// </summary>
        public static SolidColorBrush GetStatusBackgroundColor(string status)
        {
            return status?.ToLower() switch
            {
                "actif" or "active" or "en service" => SuccessLight,
                "maintenance" or "en maintenance" => WarningLight,
                "inactif" or "inactive" or "hors service" => DangerLight,
                "disponible" or "available" => InfoLight,
                _ => BorderLight
            };
        }
        
        /// <summary>
        /// Applique le thème moderne à l'application
        /// </summary>
        public static void ApplyModernTheme()
        {
            try
            {
                var modernTheme = new ResourceDictionary
                {
                    Source = new Uri("pack://application:,,,/Resources/ModernTheme.xaml", UriKind.Absolute)
                };
                
                Application.Current.Resources.MergedDictionaries.Add(modernTheme);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de l'application du thème: {ex.Message}");
            }
        }
    }
    
    /// <summary>
    /// Extensions pour faciliter l'application des styles
    /// </summary>
    public static class ThemeExtensions
    {
        public static CornerRadius ToCornerRadius(this double value)
        {
            return new CornerRadius(value);
        }
        
        public static Thickness ToThickness(this double value)
        {
            return new Thickness(value);
        }
    }
}
