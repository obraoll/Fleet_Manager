using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FleetManager.Helpers
{
    /// <summary>
    /// Convertisseur qui convertit null en Collapsed et non-null en Visible (ou l'inverse)
    /// </summary>
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isNull = value == null;
            
            // Si le paramètre est "Inverse", on inverse la logique
            if (parameter?.ToString() == "Inverse")
            {
                return isNull ? Visibility.Visible : Visibility.Collapsed;
            }
            
            return isNull ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

