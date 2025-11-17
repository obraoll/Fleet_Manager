using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FleetManager.Services;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace FleetManager
{
    /// <summary>
    /// Outil de diagnostic pour tester la connexion à la base de données
    /// </summary>
    public class DatabaseDiagnostic
    {
        public static async Task<bool> TestDatabaseConnectionAsync()
        {
            try
            {
                Console.WriteLine("=== DIAGNOSTIC DE LA BASE DE DONNÉES ===");

                // Charger la configuration
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

                var configuration = builder.Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");

                Console.WriteLine($"Chaîne de connexion : {connectionString}");

                // Créer le contexte
                var options = new DbContextOptionsBuilder<FleetDbContext>()
                    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                    .Options;

                using var context = new FleetDbContext(options);

                // Test 1: Connexion à la base
                Console.WriteLine("\n1. Test de connexion...");
                bool canConnect = await context.Database.CanConnectAsync();
                Console.WriteLine($"   Résultat : {(canConnect ? "✓ SUCCÈS" : "✗ ÉCHEC")}");

                if (!canConnect)
                {
                    Console.WriteLine("   ERREUR : Impossible de se connecter à MySQL");
                    return false;
                }

                // Test 2: Création des tables
                Console.WriteLine("\n2. Création des tables si nécessaires...");
                bool created = await context.Database.EnsureCreatedAsync();
                Console.WriteLine($"   Résultat : {(created ? "✓ Tables créées" : "✓ Tables existantes")}");

                // Test 3: Vérification des utilisateurs
                Console.WriteLine("\n3. Vérification des utilisateurs...");
                var userCount = await context.Users.CountAsync();
                Console.WriteLine($"   Nombre d'utilisateurs : {userCount}");

                if (userCount == 0)
                {
                    Console.WriteLine("   Création des utilisateurs par défaut...");

                    // Créer les utilisateurs par défaut
                    var adminUser = new FleetManager.Models.User
                    {
                        Username = "admin",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                        FullName = "Administrateur",
                        Email = "admin@fleetmanager.com",
                        Role = FleetManager.Models.UserRole.Admin,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    };

                    var standardUser = new FleetManager.Models.User
                    {
                        Username = "user",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("user123"),
                        FullName = "Utilisateur Standard",
                        Email = "user@fleetmanager.com",
                        Role = FleetManager.Models.UserRole.User,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    };

                    context.Users.AddRange(adminUser, standardUser);
                    await context.SaveChangesAsync();

                    Console.WriteLine("   ✓ Utilisateurs créés avec succès");
                }
                else
                {
                    var users = await context.Users.ToListAsync();
                    foreach (var user in users)
                    {
                        Console.WriteLine($"   - {user.Username} ({user.Role}) - Actif: {user.IsActive}");
                    }
                }

                // Test 4: Test de connexion avec les identifiants par défaut
                Console.WriteLine("\n4. Test de connexion avec admin/admin123...");
                var adminFromDb = await context.Users
                    .FirstOrDefaultAsync(u => u.Username == "admin" && u.IsActive);

                if (adminFromDb != null)
                {
                    bool passwordValid = BCrypt.Net.BCrypt.Verify("admin123", adminFromDb.PasswordHash);
                    Console.WriteLine($"   Résultat : {(passwordValid ? "✓ SUCCÈS" : "✗ ÉCHEC")}");

                    if (!passwordValid)
                    {
                        Console.WriteLine("   ERREUR : Mot de passe admin incorrect");
                        return false;
                    }
                }
                else
                {
                    Console.WriteLine("   ✗ ÉCHEC : Utilisateur admin introuvable");
                    return false;
                }

                Console.WriteLine("\n=== DIAGNOSTIC TERMINÉ : TOUT FONCTIONNE ===");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ ERREUR FATALE : {ex.Message}");
                Console.WriteLine($"Détails : {ex.InnerException?.Message}");
                Console.WriteLine($"Stack trace : {ex.StackTrace}");
                return false;
            }
        }

        /// <summary>
        /// Point d'entrée principal pour le diagnostic (désactivé - utiliser App.Main)
        /// </summary>
        public static async Task MainDiagnostic(string[] args)
        {
            Console.WriteLine("Fleet Manager - Diagnostic de base de données");
            Console.WriteLine("==============================================\n");

            bool success = await TestDatabaseConnectionAsync();

            Console.WriteLine($"\nRésultat final : {(success ? "✓ SUCCÈS" : "✗ ÉCHEC")}");

            if (!success)
            {
                Console.WriteLine("\nVérifications suggérées :");
                Console.WriteLine("1. MySQL est-il démarré ? (port 3306)");
                Console.WriteLine("2. L'utilisateur 'root' peut-il se connecter sans mot de passe ?");
                Console.WriteLine("3. La base 'fleet_manager' existe-t-elle ?");
                Console.WriteLine("4. Les tables sont-elles créées correctement ?");
            }

            Console.WriteLine("\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }
}
