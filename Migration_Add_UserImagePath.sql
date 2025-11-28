-- Migration pour ajouter la colonne ImagePath à la table Users
-- Exécutez ce script directement dans MySQL si la migration automatique ne fonctionne pas

-- Vérifier si la colonne existe déjà
SELECT COUNT(*) 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = DATABASE() 
AND LOWER(TABLE_NAME) = LOWER('Users') 
AND LOWER(COLUMN_NAME) = LOWER('ImagePath');

-- Si le résultat est 0, exécutez cette commande :
ALTER TABLE Users 
ADD COLUMN ImagePath VARCHAR(500) NULL 
AFTER LastLogin;

-- Ou si votre table est en minuscules (selon la configuration MySQL) :
-- ALTER TABLE users 
-- ADD COLUMN ImagePath VARCHAR(500) NULL 
-- AFTER LastLogin;

-- Vérification après l'ajout
SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_SCHEMA = DATABASE() 
AND LOWER(TABLE_NAME) = LOWER('Users') 
AND LOWER(COLUMN_NAME) = LOWER('ImagePath');

