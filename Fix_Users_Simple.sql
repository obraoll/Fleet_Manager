-- ============================================
-- SOLUTION SIMPLE: Supprimer les utilisateurs
-- L'application les recréera automatiquement au démarrage
-- ============================================

USE fleet_manager;

-- Supprimer tous les utilisateurs
DELETE FROM Users;

-- Vérifier
SELECT COUNT(*) as NombreUtilisateurs FROM Users;

-- ============================================
-- IMPORTANT: Après avoir exécuté ce script
-- ============================================
-- 1. Relancez l'application FleetManager
-- 2. Elle créera automatiquement les utilisateurs par défaut:
--    - Utilisateur: admin | Mot de passe: admin123
--    - Utilisateur: user  | Mot de passe: user123
