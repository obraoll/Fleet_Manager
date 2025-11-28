using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FleetManager.Services
{
    /// <summary>
    /// Service pour persister les notifications supprimées
    /// </summary>
    public class NotificationPersistenceService
    {
        private readonly string _storageFilePath;
        private HashSet<string> _deletedNotificationIds = new();

        public NotificationPersistenceService()
        {
            // Utiliser le dossier AppData de l'utilisateur
            var appDataFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "FleetManager");

            // Créer le dossier s'il n'existe pas
            if (!Directory.Exists(appDataFolder))
            {
                Directory.CreateDirectory(appDataFolder);
            }

            _storageFilePath = Path.Combine(appDataFolder, "deleted_notifications.json");
            LoadDeletedNotifications();
        }

        /// <summary>
        /// Charge les IDs des notifications supprimées depuis le fichier
        /// </summary>
        private void LoadDeletedNotifications()
        {
            try
            {
                if (File.Exists(_storageFilePath))
                {
                    var json = File.ReadAllText(_storageFilePath);
                    var ids = JsonSerializer.Deserialize<List<string>>(json);
                    if (ids != null)
                    {
                        _deletedNotificationIds = new HashSet<string>(ids);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors du chargement des notifications supprimées: {ex.Message}");
                _deletedNotificationIds = new HashSet<string>();
            }
        }

        /// <summary>
        /// Sauvegarde les IDs des notifications supprimées dans le fichier
        /// </summary>
        private async Task SaveDeletedNotificationsAsync()
        {
            try
            {
                var json = JsonSerializer.Serialize(_deletedNotificationIds.ToList(), new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                await File.WriteAllTextAsync(_storageFilePath, json);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur lors de la sauvegarde des notifications supprimées: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtient tous les IDs des notifications supprimées
        /// </summary>
        public HashSet<string> GetDeletedNotificationIds()
        {
            return new HashSet<string>(_deletedNotificationIds);
        }

        /// <summary>
        /// Ajoute une notification à la liste des supprimées
        /// </summary>
        public async Task MarkAsDeletedAsync(string notificationId)
        {
            if (string.IsNullOrWhiteSpace(notificationId))
                return;

            _deletedNotificationIds.Add(notificationId);
            await SaveDeletedNotificationsAsync();
        }

        /// <summary>
        /// Marque plusieurs notifications comme supprimées
        /// </summary>
        public async Task MarkMultipleAsDeletedAsync(IEnumerable<string> notificationIds)
        {
            if (notificationIds == null)
                return;

            var added = false;
            foreach (var id in notificationIds)
            {
                if (!string.IsNullOrWhiteSpace(id) && _deletedNotificationIds.Add(id))
                {
                    added = true;
                }
            }

            if (added)
            {
                await SaveDeletedNotificationsAsync();
            }
        }

        /// <summary>
        /// Vérifie si une notification est supprimée
        /// </summary>
        public bool IsDeleted(string notificationId)
        {
            return !string.IsNullOrWhiteSpace(notificationId) && _deletedNotificationIds.Contains(notificationId);
        }

        /// <summary>
        /// Supprime une notification de la liste des supprimées (pour la restaurer)
        /// </summary>
        public async Task RestoreAsync(string notificationId)
        {
            if (string.IsNullOrWhiteSpace(notificationId))
                return;

            if (_deletedNotificationIds.Remove(notificationId))
            {
                await SaveDeletedNotificationsAsync();
            }
        }

        /// <summary>
        /// Efface toutes les notifications supprimées (reset complet)
        /// </summary>
        public async Task ClearAllAsync()
        {
            _deletedNotificationIds.Clear();
            await SaveDeletedNotificationsAsync();
        }
    }
}

