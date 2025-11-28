using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace FleetManager.Services
{
    /// <summary>
    /// Service pour gérer les images des utilisateurs
    /// </summary>
    public class UserImageService
    {
        private readonly string _imagesDirectory;

        public UserImageService()
        {
            // Créer le dossier d'images dans le dossier AppData de l'utilisateur
            var appDataFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "FleetManager",
                "UserImages");

            if (!Directory.Exists(appDataFolder))
            {
                Directory.CreateDirectory(appDataFolder);
            }

            _imagesDirectory = appDataFolder;
        }

        /// <summary>
        /// Copie une image source vers le dossier de stockage et retourne le chemin relatif
        /// </summary>
        public string? SaveUserImage(string sourceImagePath, int userId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sourceImagePath) || !File.Exists(sourceImagePath))
                    return null;

                // Obtenir l'extension du fichier source
                var extension = Path.GetExtension(sourceImagePath);
                
                // Créer un nom de fichier unique basé sur l'ID de l'utilisateur
                var fileName = $"user_{userId}_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                var destinationPath = Path.Combine(_imagesDirectory, fileName);

                // Copier l'image
                File.Copy(sourceImagePath, destinationPath, true);

                // Retourner le chemin complet pour stockage dans la base de données
                return destinationPath;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la sauvegarde de l'image utilisateur: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Supprime l'image d'un utilisateur
        /// </summary>
        public bool DeleteUserImage(string? imagePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imagePath) || !File.Exists(imagePath))
                    return true; // Pas d'image à supprimer

                File.Delete(imagePath);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la suppression de l'image utilisateur: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Charge une image Bitmap à partir d'un chemin
        /// </summary>
        public BitmapImage? LoadUserImage(string? imagePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(imagePath) || !File.Exists(imagePath))
                    return null;

                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
                bitmap.EndInit();
                bitmap.Freeze(); // Important pour éviter les problèmes de threading

                return bitmap;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement de l'image utilisateur: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Vérifie si une image existe
        /// </summary>
        public bool ImageExists(string? imagePath)
        {
            return !string.IsNullOrWhiteSpace(imagePath) && File.Exists(imagePath);
        }
    }
}

