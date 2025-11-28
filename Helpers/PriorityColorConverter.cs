using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using FleetManager.Models;

namespace FleetManager.Helpers
{
    public class PriorityColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AlertPriority priority)
            {
                return priority switch
                {
                    AlertPriority.Critical => Color.FromRgb(239, 68, 68),
                    AlertPriority.High => Color.FromRgb(245, 158, 11),
                    AlertPriority.Medium => Color.FromRgb(59, 130, 246),
                    AlertPriority.Low => Color.FromRgb(100, 116, 139),
                    _ => Colors.Gray
                };
            }
            return Colors.Gray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

