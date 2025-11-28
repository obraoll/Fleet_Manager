using System;
using System.Globalization;
using System.Windows.Data;

namespace FleetManager.Helpers
{
    public class RelativeTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                var timeSpan = DateTime.Now - date;
                if (timeSpan.TotalDays >= 1)
                    return $"Il y a {(int)timeSpan.TotalDays} jour{(timeSpan.TotalDays >= 2 ? "s" : "")}";
                if (timeSpan.TotalHours >= 1)
                    return $"Il y a {(int)timeSpan.TotalHours} heure{(timeSpan.TotalHours >= 2 ? "s" : "")}";
                if (timeSpan.TotalMinutes >= 1)
                    return $"Il y a {(int)timeSpan.TotalMinutes} minute{(timeSpan.TotalMinutes >= 2 ? "s" : "")}";
                return "À l'instant";
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

