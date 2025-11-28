using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using FleetManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
        public bool IsAdmin => _currentUser?.Role == "Admin" || _currentUser?.Role == "SuperAdmin";

        /// <summary>
        /// Vérifie si l'utilisateur actuel est super administrateur
        /// </summary>
        public bool IsSuperAdmin => _currentUser?.Role == "SuperAdmin";

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

                // Essayer de charger l'utilisateur avec une requête qui gère l'absence de la colonne ImagePath
                User? user = null;
                try
                {
                    // Essayer d'abord avec une requête normale
                    user = await context.Users
                        .FirstOrDefaultAsync(u => u.Username == username && u.IsActive);
                }
                catch (Exception ex) when (ex.Message.Contains("ImagePath") || ex.Message.Contains("unknown column"))
                {
                    // Si la colonne ImagePath n'existe pas encore, utiliser une requête SQL brute
                    System.Diagnostics.Debug.WriteLine($"Colonne ImagePath non trouvée, utilisation d'une requête SQL alternative: {ex.Message}");
                    
                    // Utiliser une requête SQL brute pour éviter le problème de colonne manquante
                    var connection = context.Database.GetDbConnection();
                    await connection.OpenAsync();
                    
                    using var command = connection.CreateCommand();
                    command.CommandText = @"
                        SELECT UserId, Username, PasswordHash, FullName, Email, Role, IsActive, CreatedAt, LastLogin
                        FROM Users
                        WHERE Username = @username AND IsActive = 1
                        LIMIT 1";
                    
                    var usernameParam = command.CreateParameter();
                    usernameParam.ParameterName = "@username";
                    usernameParam.Value = username;
                    command.Parameters.Add(usernameParam);
                    
                    using var reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        user = new User
                        {
                            UserId = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            PasswordHash = reader.GetString(2),
                            FullName = reader.GetString(3),
                            Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Role = reader.GetString(5),
                            IsActive = reader.GetBoolean(6),
                            CreatedAt = reader.GetDateTime(7),
                            LastLogin = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                            ImagePath = null // La colonne n'existe pas encore
                        };
                    }
                    
                    await connection.CloseAsync();
                }

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

                // Mettre à jour la dernière connexion (sans ImagePath)
                try
                {
                    using var updateScope = _scopeFactory.CreateScope();
                    var updateContext = updateScope.ServiceProvider.GetRequiredService<FleetDbContext>();
                    var userToUpdate = await updateContext.Users.FindAsync(user.UserId);
                    if (userToUpdate != null)
                    {
                        userToUpdate.LastLogin = DateTime.Now;
                        await updateContext.SaveChangesAsync();
                    }
                }
                catch
                {
                    // Ignorer l'erreur de mise à jour si la colonne n'existe pas encore
                }

                _currentUser = user;
                return (true, "Connexion réussie.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur LoginAsync: {ex.Message}\n{ex.StackTrace}");
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
                User? existingUser = null;
                try
                {
                    existingUser = await context.Users
                        .FirstOrDefaultAsync(u => u.Username == username);
                }
                catch (Exception ex) when (ex.Message.Contains("ImagePath") || ex.Message.Contains("unknown column"))
                {
                    // Si la colonne ImagePath n'existe pas encore, utiliser une requête SQL brute
                    var connection = context.Database.GetDbConnection();
                    await connection.OpenAsync();
                    
                    try
                    {
                        using var command = connection.CreateCommand();
                        command.CommandText = @"
                            SELECT UserId, Username
                            FROM Users
                            WHERE Username = @username
                            LIMIT 1";
                        
                        var usernameParam = command.CreateParameter();
                        usernameParam.ParameterName = "@username";
                        usernameParam.Value = username;
                        command.Parameters.Add(usernameParam);
                        
                        using var reader = await command.ExecuteReaderAsync();
                        if (await reader.ReadAsync())
                        {
                            existingUser = new User { UserId = reader.GetInt32(0), Username = reader.GetString(1) };
                        }
                    }
                    finally
                    {
                        await connection.CloseAsync();
                    }
                }

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
                    Role = role.ToString(),
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
                    // Créer le super administrateur par défaut
                    await CreateUserAsync("superadmin", "SuperAdmin123!", "Super Administrateur", "superadmin@fleetmanager.com", UserRole.SuperAdmin);

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

        public async Task<List<User>> GetAllUsersAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FleetDbContext>();
            
            try
            {
                // Essayer d'abord avec une requête normale
                return await context.Users.OrderBy(u => u.Username).ToListAsync();
            }
            catch (Exception ex) when (ex.Message.Contains("ImagePath") || ex.Message.Contains("unknown column"))
            {
                // Si la colonne ImagePath n'existe pas encore, utiliser une requête SQL brute
                System.Diagnostics.Debug.WriteLine($"Colonne ImagePath non trouvée dans GetAllUsersAsync, utilisation d'une requête SQL alternative: {ex.Message}");
                
                var connection = context.Database.GetDbConnection();
                await connection.OpenAsync();
                
                var users = new List<User>();
                
                try
                {
                    using var command = connection.CreateCommand();
                    command.CommandText = @"
                        SELECT UserId, Username, PasswordHash, FullName, Email, Role, IsActive, CreatedAt, LastLogin
                        FROM Users
                        ORDER BY Username";
                    
                    using var reader = await command.ExecuteReaderAsync();
                    while (await reader.ReadAsync())
                    {
                        users.Add(new User
                        {
                            UserId = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            PasswordHash = reader.GetString(2),
                            FullName = reader.GetString(3),
                            Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Role = reader.GetString(5),
                            IsActive = reader.GetBoolean(6),
                            CreatedAt = reader.GetDateTime(7),
                            LastLogin = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                            ImagePath = null // La colonne n'existe pas encore
                        });
                    }
                }
                finally
                {
                    await connection.CloseAsync();
                }
                
                return users;
            }
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FleetDbContext>();
            
            try
            {
                return await context.Users.FindAsync(userId);
            }
            catch (Exception ex) when (ex.Message.Contains("ImagePath") || ex.Message.Contains("unknown column"))
            {
                // Si la colonne ImagePath n'existe pas encore, utiliser une requête SQL brute
                System.Diagnostics.Debug.WriteLine($"Colonne ImagePath non trouvée dans GetUserByIdAsync, utilisation d'une requête SQL alternative: {ex.Message}");
                
                var connection = context.Database.GetDbConnection();
                await connection.OpenAsync();
                
                User? user = null;
                
                try
                {
                    using var command = connection.CreateCommand();
                    command.CommandText = @"
                        SELECT UserId, Username, PasswordHash, FullName, Email, Role, IsActive, CreatedAt, LastLogin
                        FROM Users
                        WHERE UserId = @userId
                        LIMIT 1";
                    
                    var userIdParam = command.CreateParameter();
                    userIdParam.ParameterName = "@userId";
                    userIdParam.Value = userId;
                    command.Parameters.Add(userIdParam);
                    
                    using var reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        user = new User
                        {
                            UserId = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            PasswordHash = reader.GetString(2),
                            FullName = reader.GetString(3),
                            Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Role = reader.GetString(5),
                            IsActive = reader.GetBoolean(6),
                            CreatedAt = reader.GetDateTime(7),
                            LastLogin = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                            ImagePath = null // La colonne n'existe pas encore
                        };
                    }
                }
                finally
                {
                    await connection.CloseAsync();
                }
                
                return user;
            }
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<FleetDbContext>();
            
            try
            {
                return await context.Users.FirstOrDefaultAsync(u => u.Username == username);
            }
            catch (Exception ex) when (ex.Message.Contains("ImagePath") || ex.Message.Contains("unknown column"))
            {
                // Si la colonne ImagePath n'existe pas encore, utiliser une requête SQL brute
                System.Diagnostics.Debug.WriteLine($"Colonne ImagePath non trouvée dans GetUserByUsernameAsync, utilisation d'une requête SQL alternative: {ex.Message}");
                
                var connection = context.Database.GetDbConnection();
                await connection.OpenAsync();
                
                User? user = null;
                
                try
                {
                    using var command = connection.CreateCommand();
                    command.CommandText = @"
                        SELECT UserId, Username, PasswordHash, FullName, Email, Role, IsActive, CreatedAt, LastLogin
                        FROM Users
                        WHERE Username = @username
                        LIMIT 1";
                    
                    var usernameParam = command.CreateParameter();
                    usernameParam.ParameterName = "@username";
                    usernameParam.Value = username;
                    command.Parameters.Add(usernameParam);
                    
                    using var reader = await command.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        user = new User
                        {
                            UserId = reader.GetInt32(0),
                            Username = reader.GetString(1),
                            PasswordHash = reader.GetString(2),
                            FullName = reader.GetString(3),
                            Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                            Role = reader.GetString(5),
                            IsActive = reader.GetBoolean(6),
                            CreatedAt = reader.GetDateTime(7),
                            LastLogin = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                            ImagePath = null // La colonne n'existe pas encore
                        };
                    }
                }
                finally
                {
                    await connection.CloseAsync();
                }
                
                return user;
            }
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<FleetDbContext>();
                
                User? existingUser = null;
                try
                {
                    existingUser = await context.Users.FindAsync(user.UserId);
                }
                catch (Exception ex) when (ex.Message.Contains("ImagePath") || ex.Message.Contains("unknown column"))
                {
                    // Si la colonne ImagePath n'existe pas encore, utiliser une requête SQL brute
                    var connection = context.Database.GetDbConnection();
                    await connection.OpenAsync();
                    
                    try
                    {
                        using var command = connection.CreateCommand();
                        command.CommandText = @"
                            SELECT UserId, Username, PasswordHash, FullName, Email, Role, IsActive, CreatedAt, LastLogin
                            FROM Users
                            WHERE UserId = @userId
                            LIMIT 1";
                        
                        var userIdParam = command.CreateParameter();
                        userIdParam.ParameterName = "@userId";
                        userIdParam.Value = user.UserId;
                        command.Parameters.Add(userIdParam);
                        
                        using var reader = await command.ExecuteReaderAsync();
                        if (await reader.ReadAsync())
                        {
                            existingUser = new User
                            {
                                UserId = reader.GetInt32(0),
                                Username = reader.GetString(1),
                                PasswordHash = reader.GetString(2),
                                FullName = reader.GetString(3),
                                Email = reader.IsDBNull(4) ? null : reader.GetString(4),
                                Role = reader.GetString(5),
                                IsActive = reader.GetBoolean(6),
                                CreatedAt = reader.GetDateTime(7),
                                LastLogin = reader.IsDBNull(8) ? null : reader.GetDateTime(8),
                                ImagePath = null
                            };
                        }
                    }
                    finally
                    {
                        await connection.CloseAsync();
                    }
                }
                
                if (existingUser == null) return false;

                // Protection: Seul un SuperAdmin peut modifier un SuperAdmin
                if (existingUser.Role == "SuperAdmin" && _currentUser?.Role != "SuperAdmin")
                {
                    return false;
                }

                // Protection: Un Admin ne peut pas promouvoir quelqu'un en SuperAdmin
                if (user.Role == "SuperAdmin" && _currentUser?.Role != "SuperAdmin")
                {
                    return false;
                }

                existingUser.FullName = user.FullName;
                existingUser.Email = user.Email;
                existingUser.Role = user.Role;
                existingUser.IsActive = user.IsActive;
                
                // Mettre à jour ImagePath seulement si la colonne existe
                try
                {
                    existingUser.ImagePath = user.ImagePath;
                    await context.SaveChangesAsync();
                }
                catch (Exception ex) when (ex.Message.Contains("ImagePath") || ex.Message.Contains("unknown column"))
                {
                    // Si la colonne n'existe pas encore, mettre à jour sans ImagePath
                    System.Diagnostics.Debug.WriteLine($"Colonne ImagePath non trouvée lors de la mise à jour, utilisation d'une requête SQL alternative: {ex.Message}");
                    
                    var connection = context.Database.GetDbConnection();
                    await connection.OpenAsync();
                    
                    try
                    {
                        using var command = connection.CreateCommand();
                        command.CommandText = @"
                            UPDATE Users 
                            SET FullName = @fullName, 
                                Email = @email, 
                                Role = @role, 
                                IsActive = @isActive
                            WHERE UserId = @userId";
                        
                        var userIdParam = command.CreateParameter();
                        userIdParam.ParameterName = "@userId";
                        userIdParam.Value = user.UserId;
                        command.Parameters.Add(userIdParam);
                        
                        var fullNameParam = command.CreateParameter();
                        fullNameParam.ParameterName = "@fullName";
                        fullNameParam.Value = user.FullName;
                        command.Parameters.Add(fullNameParam);
                        
                        var emailParam = command.CreateParameter();
                        emailParam.ParameterName = "@email";
                        emailParam.Value = user.Email ?? (object)DBNull.Value;
                        command.Parameters.Add(emailParam);
                        
                        var roleParam = command.CreateParameter();
                        roleParam.ParameterName = "@role";
                        roleParam.Value = user.Role;
                        command.Parameters.Add(roleParam);
                        
                        var isActiveParam = command.CreateParameter();
                        isActiveParam.ParameterName = "@isActive";
                        isActiveParam.Value = user.IsActive;
                        command.Parameters.Add(isActiveParam);
                        
                        await command.ExecuteNonQueryAsync();
                    }
                    finally
                    {
                        await connection.CloseAsync();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erreur UpdateUserAsync: {ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<FleetDbContext>();
                
                var user = await context.Users.FindAsync(userId);
                if (user == null) return false;

                // Protection: Un SuperAdmin ne peut jamais être supprimé
                if (user.Role == "SuperAdmin")
                {
                    return false;
                }

                // Protection: Seul un SuperAdmin peut supprimer un Admin
                if (user.Role == "Admin" && _currentUser?.Role != "SuperAdmin")
                {
                    return false;
                }

                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(int userId, string newPassword)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<FleetDbContext>();
                
                var user = await context.Users.FindAsync(userId);
                if (user == null) return false;

                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                await context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
