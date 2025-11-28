using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using FleetManager.Services;

namespace FleetManager.Helpers
{
    /// <summary>
    /// Convertisseur qui convertit un chemin d'image utilisateur en BitmapImage
    /// </summary>
    public class UserImagePathConverter : IValueConverter
    {
        private static readonly UserImageService _imageService = new UserImageService();

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string imagePath && !string.IsNullOrWhiteSpace(imagePath))
            {
                if (File.Exists(imagePath))
                {
                    return _imageService.LoadUserImage(imagePath);
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

