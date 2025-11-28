-- Migration pour ajouter la colonne ImagePath à la table Vehicles
-- Exécutez ce script directement dans MySQL si la migration automatique ne fonctionne pas

-- Étape 1: Vérifier si la colonne existe déjà
SELECT COUNT(*) as ColumnExists
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = DATABASE() 
AND LOWER(TABLE_NAME) = LOWER('Vehicles') 
AND LOWER(COLUMN_NAME) = LOWER('ImagePath');

-- Étape 2: Si le résultat est 0, exécutez cette commande pour ajouter la colonne :
-- (Remplacez 'Vehicles' par le nom réel de votre table si différent)
ALTER TABLE Vehicles 
ADD COLUMN ImagePath VARCHAR(500) NULL 
AFTER Notes;

-- Alternative si la table est en minuscules :
-- ALTER TABLE vehicles 
-- ADD COLUMN ImagePath VARCHAR(500) NULL 
-- AFTER Notes;

