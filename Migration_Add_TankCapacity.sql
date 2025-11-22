-- ============================================
-- MIGRATION: Ajout des colonnes manquantes
-- Date: 2025-11-17
-- ============================================

USE fleet_manager;

-- ============================================
-- 1. Ajout de TankCapacity dans Vehicles
-- ============================================
SET @dbname = DATABASE();
SET @tablename = "Vehicles";
SET @columnname = "TankCapacity";
SET @preparedStatement = (SELECT IF(
  (
    SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
    WHERE
      (table_name = @tablename)
      AND (table_schema = @dbname)
      AND (column_name = @columnname)
  ) > 0,
  "SELECT 'La colonne TankCapacity existe déjà' AS Message;",
  "ALTER TABLE Vehicles ADD COLUMN TankCapacity DECIMAL(5,2) NOT NULL DEFAULT 50.00 AFTER CurrentMileage;"
));

PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SELECT 'Migration TankCapacity terminée' AS Status;

-- ============================================
-- 2. Ajout de DriverId dans FuelRecords
-- ============================================
SET @tablename = "FuelRecords";
SET @columnname = "DriverId";
SET @preparedStatement = (SELECT IF(
  (
    SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
    WHERE
      (table_name = @tablename)
      AND (table_schema = @dbname)
      AND (column_name = @columnname)
  ) > 0,
  "SELECT 'La colonne DriverId existe déjà' AS Message;",
  "ALTER TABLE FuelRecords ADD COLUMN DriverId INT NULL AFTER VehicleId, ADD FOREIGN KEY (DriverId) REFERENCES Drivers(DriverId) ON DELETE SET NULL;"
));

PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SELECT 'Migration DriverId terminée' AS Status;

-- ============================================
-- 3. Ajout de PaymentMethod dans FuelRecords
-- ============================================
SET @tablename = "FuelRecords";
SET @columnname = "PaymentMethod";
SET @preparedStatement = (SELECT IF(
  (
    SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
    WHERE
      (table_name = @tablename)
      AND (table_schema = @dbname)
      AND (column_name = @columnname)
  ) > 0,
  "SELECT 'La colonne PaymentMethod existe déjà' AS Message;",
  "ALTER TABLE FuelRecords ADD COLUMN PaymentMethod VARCHAR(20) NULL AFTER IsFullTank;"
));

PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SELECT 'Migration PaymentMethod terminée' AS Status;

-- ============================================
-- 4. Supprimer UserId de FuelRecords (colonne obsolète)
-- ============================================
SET @tablename = "FuelRecords";
SET @columnname = "UserId";
SET @preparedStatement = (SELECT IF(
  (
    SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS
    WHERE
      (table_name = @tablename)
      AND (table_schema = @dbname)
      AND (column_name = @columnname)
  ) > 0,
  "ALTER TABLE FuelRecords DROP COLUMN UserId;",
  "SELECT 'La colonne UserId n''existe pas (OK)' AS Message;"
));

PREPARE alterIfNotExists FROM @preparedStatement;
EXECUTE alterIfNotExists;
DEALLOCATE PREPARE alterIfNotExists;

SELECT 'Migration UserId (suppression) terminée' AS Status;

-- ============================================
-- 5. Afficher la structure des tables
-- ============================================
SELECT 'Structure de la table Vehicles:' AS Info;
DESCRIBE Vehicles;

SELECT 'Structure de la table FuelRecords:' AS Info;
DESCRIBE FuelRecords;

SELECT 'Migration complète terminée avec succès!' AS Status;
