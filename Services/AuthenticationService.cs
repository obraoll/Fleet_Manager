using System;
using System.Linq;
using System.Threading.Tasks;
using FleetManager.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Microsoft.Extensions.DependencyInjection;

namespace FleetManager.Services
{
    /// <summary>
    /// Service d'authentification et de gestion des utilisateurs
    /// </summary>
    public class AuthenticationService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private User? _currentUser;

        public AuthenticationService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        /// <summary>
        /// Utilisateur actuellement connecté
        /// </summary>
        public User? CurrentUser => _currentUser;

        /// <summary>
        /// Vérifie si un utilisateur est connecté
        /// </summary>
        public bool IsAuthenticated => _currentUser != null;

        /// <summary>
        /// Vérifie si l'utilisateur actuel est administrateur
        /// </summary>
        public bool IsAdmin => _currentUser?.Role == UserRole.Admin;

        /// <summary>
        /// Authentifie un utilisateur
        /// </summary>
        public async Task<(bool Success, string Message)> LoginAsync(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return (false, "Nom d'utilisateur et mot de passe requis.");
                }

                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<FleetDbContext>();

                var user = await context.Users
                    .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);

                if (user == null)
                {
                    return (false, "Nom d'utilisateur ou mot de passe incorrect.");
                }

                // Vérifier le mot de passe avec BCrypt
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

                if (!isPasswordValid)
                {
                    return (false, "Nom d'utilisateur ou mot de passe incorrect.");
                }

                // Mettre à jour la dernière connexion
                user.LastLogin = DateTime.Now;
                await context.SaveChangesAsync();

                _currentUser = user;
                return (true, "Connexion réussie.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de la connexion: {ex.Message}");
            }
        }
        public async Task<string> GetDatabaseStatus()
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<FleetDbContext>();

                bool canConnect = await context.Database.CanConnectAsync();
                int userCount = await context.Users.CountAsync();

                return $"🔹 Connexion BD : {(canConnect ? "✅" : "❌")}\n" +
                       $"🔹 Nombre d'utilisateurs : {userCount}";
            }
            catch (Exception ex)
            {
                return $"❌ Erreur : {ex.Message}";
            }
        }

        /// <summary>
        /// Déconnecte l'utilisateur actuel
        /// </summary>
        public void Logout()
        {
            _currentUser = null;
        }

        /// <summary>
        /// Crée un nouvel utilisateur
        /// </summary>
        public async Task<(bool Success, string Message)> CreateUserAsync(string username, string password, string fullName, string? email, UserRole role)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<FleetDbContext>();

                // Vérifier si l'utilisateur existe déjà
                var existingUser = await context.Users
                    .FirstOrDefaultAsync(u => u.Username == username);

                if (existingUser != null)
                {
                    return (false, "Ce nom d'utilisateur existe déjà.");
                }

                // Hasher le mot de passe
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

                var newUser = new User
                {
                    Username = username,
                    PasswordHash = passwordHash,
                    FullName = fullName,
                    Email = email,
                    Role = role,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                context.Users.Add(newUser);
                await context.SaveChangesAsync();

                return (true, "Utilisateur créé avec succès.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de la création de l'utilisateur: {ex.Message}");
            }
        }

        /// <summary>
        /// Change le mot de passe d'un utilisateur
        /// </summary>
        public async Task<(bool Success, string Message)> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<FleetDbContext>();

                var user = await context.Users.FindAsync(userId);
                if (user == null)
                {
                    return (false, "Utilisateur introuvable.");
                }

                // Vérifier l'ancien mot de passe
                if (!BCrypt.Net.BCrypt.Verify(oldPassword, user.PasswordHash))
                {
                    return (false, "Ancien mot de passe incorrect.");
                }

                // Hasher le nouveau mot de passe
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                await context.SaveChangesAsync();

                return (true, "Mot de passe modifié avec succès.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors du changement de mot de passe: {ex.Message}");
            }
        }

        /// <summary>
        /// Initialise les utilisateurs par défaut si la base est vide
        /// </summary>
        public async Task InitializeDefaultUsersAsync()
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<FleetDbContext>();

                if (!await context.Users.AnyAsync())
                {
                    // Créer l'administrateur par défaut
                    await CreateUserAsync("admin", "admin123", "Administrateur", "admin@fleetmanager.com", UserRole.Admin);

                    // Créer un utilisateur standard
                    await CreateUserAsync("user", "user123", "Utilisateur Standard", "user@fleetmanager.com", UserRole.User);
                }
            }
            catch (Exception)
            {
                // Ignorer les erreurs d'initialisation
            }
        }
    }
}
