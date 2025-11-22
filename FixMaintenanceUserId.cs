using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace FleetManager
{
    /// <summary>
    /// Programme pour corriger le problème de UserId dans MaintenanceRecords
    /// </summary>
    public class FixMaintenanceUserId
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("=== CORRECTION MAINTENANCERECORDS.USERID ===\n");

            try
            {
                // Configuration
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");
                Console.WriteLine($"Connexion à: {connectionString?.Split(';')[0]}\n");

                using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();
                Console.WriteLine("✅ Connexion établie\n");

                // 1. Vérifier si la colonne UserId existe
                Console.WriteLine("--- Vérification de la colonne UserId ---");
                var checkQuery = @"
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_SCHEMA = 'fleet_manager' 
                    AND TABLE_NAME = 'MaintenanceRecords' 
                    AND COLUMN_NAME = 'UserId'";

                using var checkCommand = new MySqlCommand(checkQuery, connection);
                var userIdExists = Convert.ToInt32(await checkCommand.ExecuteScalarAsync()) > 0;

                if (!userIdExists)
                {
                    Console.WriteLine("ℹ️  La colonne UserId n'existe pas dans MaintenanceRecords");
                    Console.WriteLine("✅ Aucune correction nécessaire\n");
                    
                    // Afficher les colonnes actuelles
                    Console.WriteLine("--- Colonnes actuelles dans MaintenanceRecords ---");
                    var showColumnsQuery = @"
                        SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_KEY
                        FROM INFORMATION_SCHEMA.COLUMNS
                        WHERE TABLE_SCHEMA = 'fleet_manager'
                        AND TABLE_NAME = 'MaintenanceRecords'
                        ORDER BY ORDINAL_POSITION";
                    
                    using var showCommand = new MySqlCommand(showColumnsQuery, connection);
                    using var reader = await showCommand.ExecuteReaderAsync();
                    
                    while (await reader.ReadAsync())
                    {
                        var colName = reader.GetString(0);
                        var dataType = reader.GetString(1);
                        var nullable = reader.GetString(2);
                        var key = reader.IsDBNull(3) ? "" : reader.GetString(3);
                        Console.WriteLine($"  {colName,-25} {dataType,-15} {nullable,-5} {key}");
                    }
                    
                    return;
                }

                Console.WriteLine("⚠️  La colonne UserId existe dans MaintenanceRecords");
                Console.WriteLine("🔧 Suppression en cours...\n");

                // 2. Chercher et supprimer la clé étrangère
                Console.WriteLine("--- Suppression de la clé étrangère ---");
                var findFkQuery = @"
                    SELECT CONSTRAINT_NAME
                    FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
                    WHERE TABLE_SCHEMA = 'fleet_manager'
                    AND TABLE_NAME = 'MaintenanceRecords'
                    AND COLUMN_NAME = 'UserId'
                    AND REFERENCED_TABLE_NAME IS NOT NULL";

                using var findFkCommand = new MySqlCommand(findFkQuery, connection);
                var fkName = await findFkCommand.ExecuteScalarAsync() as string;

                if (!string.IsNullOrEmpty(fkName))
                {
                    Console.WriteLine($"  Clé étrangère trouvée: {fkName}");
                    var dropFkQuery = $"ALTER TABLE MaintenanceRecords DROP FOREIGN KEY {fkName}";
                    using var dropFkCommand = new MySqlCommand(dropFkQuery, connection);
                    await dropFkCommand.ExecuteNonQueryAsync();
                    Console.WriteLine("  ✅ Clé étrangère supprimée");
                }
                else
                {
                    Console.WriteLine("  ℹ️  Aucune clé étrangère à supprimer");
                }

                // 3. Supprimer la colonne UserId
                Console.WriteLine("\n--- Suppression de la colonne UserId ---");
                var dropColumnQuery = "ALTER TABLE MaintenanceRecords DROP COLUMN UserId";
                using var dropColumnCommand = new MySqlCommand(dropColumnQuery, connection);
                await dropColumnCommand.ExecuteNonQueryAsync();
                Console.WriteLine("✅ Colonne UserId supprimée de MaintenanceRecords\n");

                // 4. Vérifier le résultat
                Console.WriteLine("--- Vérification finale ---");
                var verifyQuery = @"
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_SCHEMA = 'fleet_manager' 
                    AND TABLE_NAME = 'MaintenanceRecords' 
                    AND COLUMN_NAME = 'UserId'";

                using var verifyCommand = new MySqlCommand(verifyQuery, connection);
                var stillExists = Convert.ToInt32(await verifyCommand.ExecuteScalarAsync()) > 0;

                if (!stillExists)
                {
                    Console.WriteLine("✅ SUCCÈS : Colonne UserId supprimée avec succès!");
                    Console.WriteLine("\n🎉 Vous pouvez maintenant relancer l'application.");
                }
                else
                {
                    Console.WriteLine("❌ ERREUR : La colonne UserId existe toujours");
                }

                // Afficher les colonnes restantes
                Console.WriteLine("\n--- Colonnes restantes dans MaintenanceRecords ---");
                var showColumnsQuery2 = @"
                    SELECT COLUMN_NAME, DATA_TYPE, IS_NULLABLE, COLUMN_KEY
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_SCHEMA = 'fleet_manager'
                    AND TABLE_NAME = 'MaintenanceRecords'
                    ORDER BY ORDINAL_POSITION";
                
                using var showCommand2 = new MySqlCommand(showColumnsQuery2, connection);
                using var reader2 = await showCommand2.ExecuteReaderAsync();
                
                while (await reader2.ReadAsync())
                {
                    var colName = reader2.GetString(0);
                    var dataType = reader2.GetString(1);
                    var nullable = reader2.GetString(2);
                    var key = reader2.IsDBNull(3) ? "" : reader2.GetString(3);
                    Console.WriteLine($"  {colName,-25} {dataType,-15} {nullable,-5} {key}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ ERREUR: {ex.Message}");
                Console.WriteLine($"\nStack trace:\n{ex.StackTrace}");
                
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"\nInner exception: {ex.InnerException.Message}");
                }
            }

            Console.WriteLine("\n\nAppuyez sur une touche pour continuer...");
            Console.ReadKey();
        }
    }
}
