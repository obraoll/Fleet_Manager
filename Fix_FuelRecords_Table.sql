-- ============================================
-- CORRECTION: Recréer la table FuelRecords avec la bonne structure
-- Date: 2025-11-17
-- ============================================

USE fleet_manager;

-- Sauvegarder les données existantes si la table existe
DROP TABLE IF EXISTS FuelRecords_backup;
CREATE TABLE FuelRecords_backup AS SELECT * FROM FuelRecords;

-- Supprimer l'ancienne table
DROP TABLE IF EXISTS FuelRecords;

-- Recréer la table avec la bonne structure
CREATE TABLE FuelRecords (
    FuelRecordId INT AUTO_INCREMENT PRIMARY KEY,
    VehicleId INT NOT NULL,
    DriverId INT,
    RefuelDate DATETIME NOT NULL,
    Mileage DECIMAL(10,2) NOT NULL,
    LitersRefueled DECIMAL(10,2) NOT NULL,
    PricePerLiter DECIMAL(5,3) NOT NULL,
    TotalCost DECIMAL(10,2) NOT NULL,
    FuelType VARCHAR(20) NOT NULL,
    Station VARCHAR(100),
    CalculatedConsumption DECIMAL(5,2),
    IsFullTank BOOLEAN DEFAULT TRUE,
    PaymentMethod VARCHAR(20),
    Notes TEXT,
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(VehicleId) ON DELETE CASCADE,
    FOREIGN KEY (DriverId) REFERENCES Drivers(DriverId) ON DELETE SET NULL
) ENGINE=InnoDB;

-- Restaurer les données (adapter selon les colonnes disponibles dans la sauvegarde)
INSERT INTO FuelRecords (VehicleId, RefuelDate, Mileage, LitersRefueled, PricePerLiter, TotalCost, FuelType, Station, CalculatedConsumption, IsFullTank, CreatedAt)
SELECT 
    VehicleId, 
    RefuelDate, 
    Mileage, 
    LitersRefueled, 
    PricePerLiter, 
    TotalCost, 
    FuelType, 
    COALESCE(Station, NULL), 
    COALESCE(CalculatedConsumption, NULL), 
    COALESCE(IsFullTank, TRUE),
    COALESCE(CreatedAt, NOW())
FROM FuelRecords_backup;

-- Nettoyer
-- DROP TABLE FuelRecords_backup;

SELECT 'Table FuelRecords recréée avec succès!' AS Status;
DESCRIBE FuelRecords;
