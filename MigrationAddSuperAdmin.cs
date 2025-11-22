using System;
using System.Linq;
using System.Threading.Tasks;
using FleetManager.Models;
using FleetManager.Services;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace FleetManager
{
    /// <summary>
    /// Migration pour ajouter ou mettre à jour le compte SuperAdmin
    /// </summary>
    public static class MigrationAddSuperAdmin
    {
        public static async Task<(bool Success, string Message)> ExecuteAsync(FleetDbContext context)
        {
            try
            {
                Console.WriteLine("=== Migration SuperAdmin ===");
                
                // Vérifier si le SuperAdmin existe déjà
                var superAdmin = await context.Users
                    .FirstOrDefaultAsync(u => u.Username == "superadmin");

                if (superAdmin == null)
                {
                    // Créer le SuperAdmin
                    Console.WriteLine("Création du compte SuperAdmin...");
                    
                    var newSuperAdmin = new User
                    {
                        Username = "superadmin",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("SuperAdmin123!"),
                        FullName = "Super Administrateur",
                        Email = "superadmin@fleetmanager.com",
                        Role = "SuperAdmin",
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    };

                    context.Users.Add(newSuperAdmin);
                    await context.SaveChangesAsync();
                    
                    Console.WriteLine("✅ SuperAdmin créé avec succès!");
                    return (true, "SuperAdmin créé avec succès");
                }
                else
                {
                    // Mettre à jour le rôle si nécessaire
                    if (superAdmin.Role != "SuperAdmin")
                    {
                        Console.WriteLine($"Mise à jour du rôle de '{superAdmin.Username}' vers SuperAdmin...");
                        superAdmin.Role = "SuperAdmin";
                        await context.SaveChangesAsync();
                        Console.WriteLine("✅ Rôle mis à jour!");
                    }
                    else
                    {
                        Console.WriteLine("ℹ️ SuperAdmin existe déjà");
                    }
                    
                    return (true, "SuperAdmin existe déjà ou a été mis à jour");
                }
            }
            catch (Exception ex)
            {
                var errorMsg = $"Erreur lors de la migration SuperAdmin: {ex.Message}";
                Console.WriteLine($"❌ {errorMsg}");
                return (false, errorMsg);
            }
        }
    }
}
