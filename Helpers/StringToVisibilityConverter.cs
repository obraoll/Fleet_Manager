using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FleetManager.Helpers
{
    /// <summary>
    /// Convertisseur qui affiche un élément quand une chaîne est vide (pour les placeholders)
    /// </summary>
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringValue = value?.ToString() ?? string.Empty;
            bool isEmpty = string.IsNullOrWhiteSpace(stringValue);
            
            // Si le paramètre est "Inverse", on inverse la logique
            if (parameter?.ToString() == "Inverse")
            {
                return isEmpty ? Visibility.Visible : Visibility.Collapsed;
            }
            
            return isEmpty ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

