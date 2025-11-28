using System;
using System.Globalization;
using System.Windows.Data;
using FleetManager.Models;

namespace FleetManager.Helpers
{
    public class AlertIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AlertType type)
            {
                return type switch
                {
                    AlertType.MaintenanceDue => "🔧",
                    AlertType.InspectionExpired => "📋",
                    AlertType.InsuranceExpired => "📄",
                    AlertType.HighConsumption => "⛽",
                    AlertType.CostThreshold => "💰",
                    AlertType.VehicleInactive => "🚗",
                    _ => "ℹ️"
                };
            }
            return "ℹ️";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

