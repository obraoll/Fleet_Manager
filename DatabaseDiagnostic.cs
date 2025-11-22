using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MySqlConnector;

namespace FleetManager
{
    public class DatabaseDiagnostic
    {
        public static async Task TestConnectionAsync()
        {
            Console.WriteLine("=== TEST DE CONNEXION MYSQL ===\n");

            try
            {
                // Charger la configuration
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");
                Console.WriteLine($"Chaîne de connexion : {connectionString}\n");

                // Test de connexion direct
                Console.WriteLine("1. Test de connexion directe...");
                using var connection = new MySqlConnection(connectionString);

                await connection.OpenAsync();
                Console.WriteLine("✅ Connexion réussie !");

                // Test de version MySQL
                Console.WriteLine("\n2. Version de MySQL...");
                using var command = new MySqlCommand("SELECT VERSION()", connection);
                var version = await command.ExecuteScalarAsync();
                Console.WriteLine($"✅ Version MySQL : {version}");

                // Test de création de base de données
                Console.WriteLine("\n3. Création de la base 'fleet_manager' si nécessaire...");
                var createDbCommand = new MySqlCommand("CREATE DATABASE IF NOT EXISTS fleet_manager", connection);
                await createDbCommand.ExecuteNonQueryAsync();
                Console.WriteLine("✅ Base de données prête");

                // Test de sélection de la base
                Console.WriteLine("\n4. Sélection de la base de données...");
                var useDbCommand = new MySqlCommand("USE fleet_manager", connection);
                await useDbCommand.ExecuteNonQueryAsync();
                Console.WriteLine("✅ Base sélectionnée");

                // Test de création d'une table simple
                Console.WriteLine("\n5. Test de création de table...");
                var createTableCommand = new MySqlCommand(@"
                    CREATE TABLE IF NOT EXISTS test_connection (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    )", connection);
                await createTableCommand.ExecuteNonQueryAsync();
                Console.WriteLine("✅ Table de test créée");

                // Test d'insertion
                Console.WriteLine("\n6. Test d'insertion...");
                var insertCommand = new MySqlCommand("INSERT INTO test_connection () VALUES ()", connection);
                await insertCommand.ExecuteNonQueryAsync();
                Console.WriteLine("✅ Insertion réussie");

                // Test de lecture
                Console.WriteLine("\n7. Test de lecture...");
                var selectCommand = new MySqlCommand("SELECT COUNT(*) FROM test_connection", connection);
                var count = await selectCommand.ExecuteScalarAsync();
                Console.WriteLine($"✅ Nombre d'enregistrements : {count}");

                // Nettoyage
                Console.WriteLine("\n8. Nettoyage...");
                var dropCommand = new MySqlCommand("DROP TABLE test_connection", connection);
                await dropCommand.ExecuteNonQueryAsync();
                Console.WriteLine("✅ Table supprimée");

                Console.WriteLine("\n🎉 TOUS LES TESTS RÉUSSIS !");
                Console.WriteLine("\nVotre base de données MySQL est prête pour Fleet Manager.");
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\n❌ ERREUR MYSQL : {ex.Message}");
                Console.WriteLine($"Code d'erreur : {ex.Number}");

                switch (ex.Number)
                {
                    case 1045:
                        Console.WriteLine("\n🔧 SOLUTION :");
                        Console.WriteLine("- Vérifiez que l'utilisateur 'root' peut se connecter sans mot de passe");
                        Console.WriteLine("- Ou modifiez la chaîne de connexion dans appsettings.json avec le bon mot de passe");
                        break;
                    case 1049:
                        Console.WriteLine("\n🔧 SOLUTION :");
                        Console.WriteLine("- La base 'fleet_manager' sera créée automatiquement");
                        break;
                    case 2003:
                        Console.WriteLine("\n🔧 SOLUTION :");
                        Console.WriteLine("- Vérifiez que MySQL est démarré");
                        Console.WriteLine("- Vérifiez le port (3306)");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ ERREUR : {ex.Message}");
                Console.WriteLine($"Type : {ex.GetType().Name}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Erreur interne : {ex.InnerException.Message}");
                }
            }
        }

        public static async Task<bool> TestDatabaseConnectionAsync()
        {
            Console.WriteLine("=== TEST DE CONNEXION MYSQL ===\n");

            try
            {
                // Charger la configuration
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                var connectionString = configuration.GetConnectionString("DefaultConnection");
                Console.WriteLine($"Chaîne de connexion : {connectionString}\n");

                // Test de connexion direct
                Console.WriteLine("1. Test de connexion directe...");
                using var connection = new MySqlConnection(connectionString);

                await connection.OpenAsync();
                Console.WriteLine("✅ Connexion réussie !");

                // Test de version MySQL
                Console.WriteLine("\n2. Version de MySQL...");
                using var command = new MySqlCommand("SELECT VERSION()", connection);
                var version = await command.ExecuteScalarAsync();
                Console.WriteLine($"✅ Version MySQL : {version}");

                // Test de création de base de données
                Console.WriteLine("\n3. Création de la base 'fleet_manager' si nécessaire...");
                var createDbCommand = new MySqlCommand("CREATE DATABASE IF NOT EXISTS fleet_manager", connection);
                await createDbCommand.ExecuteNonQueryAsync();
                Console.WriteLine("✅ Base de données prête");

                // Test de sélection de la base
                Console.WriteLine("\n4. Sélection de la base de données...");
                var useDbCommand = new MySqlCommand("USE fleet_manager", connection);
                await useDbCommand.ExecuteNonQueryAsync();
                Console.WriteLine("✅ Base sélectionnée");

                // Test de création d'une table simple
                Console.WriteLine("\n5. Test de création de table...");
                var createTableCommand = new MySqlCommand(@"
                    CREATE TABLE IF NOT EXISTS test_connection (
                        id INT AUTO_INCREMENT PRIMARY KEY,
                        created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
                    )", connection);
                await createTableCommand.ExecuteNonQueryAsync();
                Console.WriteLine("✅ Table de test créée");

                // Test d'insertion
                Console.WriteLine("\n6. Test d'insertion...");
                var insertCommand = new MySqlCommand("INSERT INTO test_connection () VALUES ()", connection);
                await insertCommand.ExecuteNonQueryAsync();
                Console.WriteLine("✅ Insertion réussie");

                // Test de lecture
                Console.WriteLine("\n7. Test de lecture...");
                var selectCommand = new MySqlCommand("SELECT COUNT(*) FROM test_connection", connection);
                var count = await selectCommand.ExecuteScalarAsync();
                Console.WriteLine($"✅ Nombre d'enregistrements : {count}");

                // Nettoyage
                Console.WriteLine("\n8. Nettoyage...");
                var dropCommand = new MySqlCommand("DROP TABLE test_connection", connection);
                await dropCommand.ExecuteNonQueryAsync();
                Console.WriteLine("✅ Table supprimée");

                Console.WriteLine("\n🎉 TOUS LES TESTS RÉUSSIS !");
                Console.WriteLine("\nVotre base de données MySQL est prête pour Fleet Manager.");
                return true;
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"\n❌ ERREUR MYSQL : {ex.Message}");
                Console.WriteLine($"Code d'erreur : {ex.Number}");

                switch (ex.Number)
                {
                    case 1045:
                        Console.WriteLine("\n🔧 SOLUTION :");
                        Console.WriteLine("- Vérifiez que l'utilisateur 'root' peut se connecter sans mot de passe");
                        Console.WriteLine("- Ou modifiez la chaîne de connexion dans appsettings.json avec le bon mot de passe");
                        break;
                    case 1049:
                        Console.WriteLine("\n🔧 SOLUTION :");
                        Console.WriteLine("- La base 'fleet_manager' sera créée automatiquement");
                        break;
                    case 2003:
                        Console.WriteLine("\n🔧 SOLUTION :");
                        Console.WriteLine("- Vérifiez que MySQL est démarré");
                        Console.WriteLine("- Vérifiez le port (3306)");
                        break;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n❌ ERREUR : {ex.Message}");
                Console.WriteLine($"Type : {ex.GetType().Name}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Erreur interne : {ex.InnerException.Message}");
                }
                return false;
            }
        }
    }
}
