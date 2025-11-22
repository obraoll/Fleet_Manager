-- Script pour supprimer la colonne UserId de FuelRecords

-- Étape 1 : Supprimer la clé étrangère si elle existe
SET @fk_exists = (SELECT COUNT(*)
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS 
WHERE CONSTRAINT_SCHEMA = 'fleet_manager' 
AND TABLE_NAME = 'FuelRecords' 
AND CONSTRAINT_NAME = 'FK_FuelRecords_Users');

SET @sql = IF(@fk_exists > 0, 
    'ALTER TABLE FuelRecords DROP FOREIGN KEY FK_FuelRecords_Users', 
    'SELECT "Clé étrangère n\'existe pas" AS Info');
PREPARE stmt FROM @sql;
EXECUTE stmt;
DEALLOCATE PREPARE stmt;

-- Étape 2 : Supprimer la colonne UserId
ALTER TABLE FuelRecords DROP COLUMN IF EXISTS UserId;

-- Vérifier que la colonne a bien été supprimée
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    COLUMN_DEFAULT
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = 'fleet_manager'
AND TABLE_NAME = 'FuelRecords'
ORDER BY ORDINAL_POSITION;
