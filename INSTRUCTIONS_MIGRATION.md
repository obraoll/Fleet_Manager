# 🔧 Instructions pour corriger les erreurs de colonnes manquantes

## Problème
Plusieurs erreurs se produisent à cause de différences entre les modèles C# et la base de données MySQL :
- "Champ 'v.TankCapacity' inconnu dans field list" → Colonne manquante
- "Champ 'f.DriverId' inconnu dans field list" → Colonne manquante
- "Champ 'f.PaymentMethod' inconnu dans field list" → Colonne manquante
- "Champ 'f.UserId' inconnu dans field list" → Colonne obsolète à supprimer
- "Champ 'f.fuelRecordId' inconnu dans field list" → Problème de casse (minuscule vs majuscule)

## ⚠️ SOLUTION RECOMMANDÉE - Recréer la base de données

La solution la plus simple est de supprimer et recréer toute la base avec le bon script :

### Étape 1 : Ouvrir phpMyAdmin
Accédez à http://localhost/phpmyadmin

### Étape 2 : Supprimer l'ancienne base
```sql
DROP DATABASE IF EXISTS fleet_manager;
```

### Étape 3 : Exécuter le script complet
Ouvrez le fichier `Database_SampleData.sql` dans phpMyAdmin et exécutez-le.
Ce script va :
- Créer la base de données avec toutes les tables
- Insérer des données de test
- Créer les contraintes et index

### Étape 4 : Relancer l'application
Fermez et relancez FleetManager.

## Solution Alternative - Corrections manuelles

Si vous souhaitez conserver vos données existantes :

### Via phpMyAdmin

1. **Ouvrir phpMyAdmin** (http://localhost/phpmyadmin)
2. **Sélectionner la base** `fleet_manager`
3. **Cliquer sur l'onglet "SQL"**
4. **Exécuter ces commandes une par une** :

```sql
-- Ajouter TankCapacity dans Vehicles
ALTER TABLE Vehicles 
ADD COLUMN TankCapacity DECIMAL(5,2) NOT NULL DEFAULT 50.00 
AFTER CurrentMileage;

-- Recréer la table FuelRecords avec la bonne casse
DROP TABLE IF EXISTS FuelRecords_backup;
CREATE TABLE FuelRecords_backup AS SELECT * FROM FuelRecords;
DROP TABLE FuelRecords;

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

-- Restaurer les données
INSERT INTO FuelRecords (VehicleId, RefuelDate, Mileage, LitersRefueled, PricePerLiter, TotalCost, FuelType, CreatedAt)
SELECT VehicleId, RefuelDate, Mileage, LitersRefueled, PricePerLiter, TotalCost, FuelType, CreatedAt
FROM FuelRecords_backup;
```

## Vérification

```sql
DESCRIBE Vehicles;
DESCRIBE FuelRecords;
```

⚠️ **Important** : Fermez TOUTES les instances de FleetManager avant de relancer l'application.
