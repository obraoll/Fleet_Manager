-- Script pour corriger le problème de UserId dans MaintenanceRecords

USE fleet_manager;

-- 1. Vérifier si la colonne UserId existe dans MaintenanceRecords
SELECT COLUMN_NAME, DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = 'fleet_manager'
  AND TABLE_NAME = 'MaintenanceRecords';

-- 2. Supprimer la clé étrangère si elle existe
SET @constraint_name = (
    SELECT CONSTRAINT_NAME
    FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE
    WHERE TABLE_SCHEMA = 'fleet_manager'
      AND TABLE_NAME = 'MaintenanceRecords'
      AND COLUMN_NAME = 'UserId'
      AND REFERENCED_TABLE_NAME IS NOT NULL
);

SET @drop_fk_query = IF(@constraint_name IS NOT NULL,
    CONCAT('ALTER TABLE MaintenanceRecords DROP FOREIGN KEY ', @constraint_name),
    'SELECT "Pas de clé étrangère à supprimer"');

PREPARE stmt FROM @drop_fk_query;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- 3. Supprimer la colonne UserId si elle existe
SET @col_exists = (
    SELECT COUNT(*)
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA = 'fleet_manager'
      AND TABLE_NAME = 'MaintenanceRecords'
      AND COLUMN_NAME = 'UserId'
);

SET @drop_col_query = IF(@col_exists > 0,
    'ALTER TABLE MaintenanceRecords DROP COLUMN UserId',
    'SELECT "Colonne UserId n\'existe pas"');

PREPARE stmt FROM @drop_col_query;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- 4. Vérifier le résultat final
SELECT 'Colonnes restantes dans MaintenanceRecords:' AS Info;
SELECT COLUMN_NAME, DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = 'fleet_manager'
  AND TABLE_NAME = 'MaintenanceRecords'
ORDER BY ORDINAL_POSITION;
