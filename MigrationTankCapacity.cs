using System;
using System.Threading.Tasks;
using FleetManager.Services;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace FleetManager
{
    /// <summary>
    /// Migration pour ajouter les colonnes manquantes si elles n'existent pas
    /// </summary>
    public class MigrationTankCapacity
    {
        public static async Task<(bool Success, string Message)> ExecuteAsync(string connectionString)
        {
            try
            {
                using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                var messages = new System.Text.StringBuilder();

                // 1. Vérifier et ajouter TankCapacity dans Vehicles
                var checkTankCapacityQuery = @"
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_SCHEMA = 'fleet_manager' 
                    AND TABLE_NAME = 'Vehicles' 
                    AND COLUMN_NAME = 'TankCapacity'";

                using var checkTankCapacityCommand = new MySqlCommand(checkTankCapacityQuery, connection);
                var tankCapacityExists = Convert.ToInt32(await checkTankCapacityCommand.ExecuteScalarAsync()) > 0;

                if (!tankCapacityExists)
                {
                    var addTankCapacityQuery = @"
                        ALTER TABLE Vehicles 
                        ADD COLUMN TankCapacity DECIMAL(5,2) NOT NULL DEFAULT 50.00 
                        AFTER CurrentMileage";

                    using var addTankCapacityCommand = new MySqlCommand(addTankCapacityQuery, connection);
                    await addTankCapacityCommand.ExecuteNonQueryAsync();
                    messages.AppendLine("✅ Colonne TankCapacity ajoutée à Vehicles");
                }
                else
                {
                    messages.AppendLine("ℹ️ Colonne TankCapacity existe déjà");
                }

                // 2. Vérifier et ajouter DriverId dans FuelRecords
                var checkDriverIdQuery = @"
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_SCHEMA = 'fleet_manager' 
                    AND TABLE_NAME = 'FuelRecords' 
                    AND COLUMN_NAME = 'DriverId'";

                using var checkDriverIdCommand = new MySqlCommand(checkDriverIdQuery, connection);
                var driverIdExists = Convert.ToInt32(await checkDriverIdCommand.ExecuteScalarAsync()) > 0;

                if (!driverIdExists)
                {
                    var addDriverIdQuery = @"
                        ALTER TABLE FuelRecords 
                        ADD COLUMN DriverId INT NULL AFTER VehicleId";

                    using var addDriverIdCommand = new MySqlCommand(addDriverIdQuery, connection);
                    await addDriverIdCommand.ExecuteNonQueryAsync();

                    // Ajouter la clé étrangère
                    try
                    {
                        var addForeignKeyQuery = @"
                            ALTER TABLE FuelRecords 
                            ADD CONSTRAINT FK_FuelRecords_Drivers 
                            FOREIGN KEY (DriverId) REFERENCES Drivers(DriverId) ON DELETE SET NULL";

                        using var addFkCommand = new MySqlCommand(addForeignKeyQuery, connection);
                        await addFkCommand.ExecuteNonQueryAsync();
                        messages.AppendLine("✅ Colonne DriverId ajoutée à FuelRecords avec clé étrangère");
                    }
                    catch
                    {
                        messages.AppendLine("✅ Colonne DriverId ajoutée à FuelRecords (clé étrangère déjà existante ou non applicable)");
                    }
                }
                else
                {
                    messages.AppendLine("ℹ️ Colonne DriverId existe déjà");
                }

                // 3. Vérifier et ajouter PaymentMethod dans FuelRecords
                var checkPaymentMethodQuery = @"
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_SCHEMA = 'fleet_manager' 
                    AND TABLE_NAME = 'FuelRecords' 
                    AND COLUMN_NAME = 'PaymentMethod'";

                using var checkPaymentMethodCommand = new MySqlCommand(checkPaymentMethodQuery, connection);
                var paymentMethodExists = Convert.ToInt32(await checkPaymentMethodCommand.ExecuteScalarAsync()) > 0;

                if (!paymentMethodExists)
                {
                    var addPaymentMethodQuery = @"
                        ALTER TABLE FuelRecords 
                        ADD COLUMN PaymentMethod VARCHAR(20) NULL AFTER IsFullTank";

                    using var addPaymentMethodCommand = new MySqlCommand(addPaymentMethodQuery, connection);
                    await addPaymentMethodCommand.ExecuteNonQueryAsync();
                    messages.AppendLine("✅ Colonne PaymentMethod ajoutée à FuelRecords");
                }
                else
                {
                    messages.AppendLine("ℹ️ Colonne PaymentMethod existe déjà");
                }

                // 4. Supprimer UserId de FuelRecords si elle existe (colonne obsolète)
                var checkUserIdQuery = @"
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_SCHEMA = 'fleet_manager' 
                    AND TABLE_NAME = 'FuelRecords' 
                    AND COLUMN_NAME = 'UserId'";

                using var checkUserIdCommand = new MySqlCommand(checkUserIdQuery, connection);
                var userIdExists = Convert.ToInt32(await checkUserIdCommand.ExecuteScalarAsync()) > 0;

                if (userIdExists)
                {
                    // Supprimer d'abord la clé étrangère si elle existe
                    try
                    {
                        var dropFkQuery = @"
                            ALTER TABLE FuelRecords 
                            DROP FOREIGN KEY FK_FuelRecords_Users";
                        using var dropFkCommand = new MySqlCommand(dropFkQuery, connection);
                        await dropFkCommand.ExecuteNonQueryAsync();
                    }
                    catch { /* Clé étrangère n'existe pas */ }

                    // Supprimer la colonne
                    var dropUserIdQuery = @"
                        ALTER TABLE FuelRecords 
                        DROP COLUMN UserId";

                    using var dropUserIdCommand = new MySqlCommand(dropUserIdQuery, connection);
                    await dropUserIdCommand.ExecuteNonQueryAsync();
                    messages.AppendLine("✅ Colonne obsolète UserId supprimée de FuelRecords");
                }
                else
                {
                    messages.AppendLine("ℹ️ Colonne UserId n'existe pas (OK)");
                }

                // 5. Supprimer UserId de MaintenanceRecords si elle existe (colonne obsolète)
                var checkMaintenanceUserIdQuery = @"
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_SCHEMA = 'fleet_manager' 
                    AND TABLE_NAME = 'MaintenanceRecords' 
                    AND COLUMN_NAME = 'UserId'";

                using var checkMaintenanceUserIdCommand = new MySqlCommand(checkMaintenanceUserIdQuery, connection);
                var maintenanceUserIdExists = Convert.ToInt32(await checkMaintenanceUserIdCommand.ExecuteScalarAsync()) > 0;

                if (maintenanceUserIdExists)
                {
                    // Supprimer d'abord la clé étrangère si elle existe
                    try
                    {
                        var dropMaintenanceFkQuery = @"
                            ALTER TABLE MaintenanceRecords 
                            DROP FOREIGN KEY FK_MaintenanceRecords_Users";
                        using var dropMaintenanceFkCommand = new MySqlCommand(dropMaintenanceFkQuery, connection);
                        await dropMaintenanceFkCommand.ExecuteNonQueryAsync();
                    }
                    catch { /* Clé étrangère n'existe pas */ }

                    // Supprimer la colonne
                    var dropMaintenanceUserIdQuery = @"
                        ALTER TABLE MaintenanceRecords 
                        DROP COLUMN UserId";

                    using var dropMaintenanceUserIdCommand = new MySqlCommand(dropMaintenanceUserIdQuery, connection);
                    await dropMaintenanceUserIdCommand.ExecuteNonQueryAsync();
                    messages.AppendLine("✅ Colonne obsolète UserId supprimée de MaintenanceRecords");
                }
                else
                {
                    messages.AppendLine("ℹ️ Colonne UserId n'existe pas dans MaintenanceRecords (OK)");
                }

                // 6. Vérifier la configuration de la casse MySQL
                try
                {
                    var checkCaseQuery = "SHOW VARIABLES LIKE 'lower_case_table_names'";
                    using var checkCaseCommand = new MySqlCommand(checkCaseQuery, connection);
                    using var reader = await checkCaseCommand.ExecuteReaderAsync();
                    if (await reader.ReadAsync())
                    {
                        var value = reader.GetString(1);
                        messages.AppendLine($"ℹ️ Configuration MySQL lower_case_table_names = {value} (0=sensible à la casse, 1=insensible)");
                    }
                }
                catch { /* Ignore */ }

                return (true, messages.ToString());
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de la migration : {ex.Message}");
            }
        }

        /// <summary>
        /// Méthode pour exécuter la migration depuis l'application
        /// </summary>
        public static async Task RunMigrationAsync()
        {
            try
            {
                // Récupérer la chaîne de connexion depuis appsettings.json
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    Console.WriteLine("❌ Erreur : Chaîne de connexion introuvable dans appsettings.json");
                    return;
                }

                Console.WriteLine("🔄 Exécution de la migration TankCapacity...");
                var result = await ExecuteAsync(connectionString);

                if (result.Success)
                {
                    Console.WriteLine($"✅ {result.Message}");
                }
                else
                {
                    Console.WriteLine($"❌ {result.Message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erreur : {ex.Message}");
            }
        }
    }
}
