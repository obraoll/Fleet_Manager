using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FleetManager
{
    /// <summary>
    /// Migration pour ajouter la colonne ImagePath à la table Vehicles
    /// </summary>
    public static class MigrationAddImagePath
    {
        public static async Task<(bool Success, string Message)> ExecuteAsync(string connectionString)
        {
            try
            {
                using var connection = new MySqlConnection(connectionString);
                await connection.OpenAsync();

                // Vérifier si la colonne existe déjà (insensible à la casse)
                var checkColumnSql = @"
                    SELECT COUNT(*) 
                    FROM INFORMATION_SCHEMA.COLUMNS 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND LOWER(TABLE_NAME) = LOWER('Vehicles') 
                    AND LOWER(COLUMN_NAME) = LOWER('ImagePath')";

                using var checkCommand = new MySqlCommand(checkColumnSql, connection);
                var columnExists = Convert.ToInt32(await checkCommand.ExecuteScalarAsync()) > 0;

                if (columnExists)
                {
                    return (true, "La colonne ImagePath existe déjà.");
                }

                // Obtenir le nom réel de la table (peut être en minuscules selon la configuration MySQL)
                var getTableNameSql = @"
                    SELECT TABLE_NAME 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_SCHEMA = DATABASE() 
                    AND LOWER(TABLE_NAME) = LOWER('Vehicles')
                    LIMIT 1";

                string? actualTableName = null;
                using (var getTableCommand = new MySqlCommand(getTableNameSql, connection))
                {
                    var result = await getTableCommand.ExecuteScalarAsync();
                    if (result != null)
                    {
                        actualTableName = result.ToString();
                    }
                }

                if (string.IsNullOrEmpty(actualTableName))
                {
                    return (false, "La table Vehicles n'a pas été trouvée dans la base de données.");
                }

                // Ajouter la colonne ImagePath
                var alterTableSql = $@"
                    ALTER TABLE `{actualTableName}` 
                    ADD COLUMN ImagePath VARCHAR(500) NULL 
                    AFTER Notes";

                using var alterCommand = new MySqlCommand(alterTableSql, connection);
                await alterCommand.ExecuteNonQueryAsync();

                return (true, "Colonne ImagePath ajoutée avec succès.");
            }
            catch (Exception ex)
            {
                return (false, $"Erreur lors de l'ajout de la colonne ImagePath: {ex.Message}");
            }
        }
    }
}

