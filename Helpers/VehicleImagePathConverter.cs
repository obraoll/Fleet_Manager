using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using FleetManager.Services;

namespace FleetManager.Helpers
{
    /// <summary>
    /// Convertisseur qui convertit un chemin d'image en BitmapImage
    /// </summary>
    public class VehicleImagePathConverter : IValueConverter
    {
        private static readonly VehicleImageService _imageService = new VehicleImageService();

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string imagePath && !string.IsNullOrWhiteSpace(imagePath))
            {
                if (File.Exists(imagePath))
                {
                    return _imageService.LoadVehicleImage(imagePath);
                }
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

