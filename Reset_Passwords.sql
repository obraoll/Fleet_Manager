-- ============================================
-- CORRECTION: Réinitialiser les mots de passe
-- ============================================

USE fleet_manager;

-- Supprimer les anciens utilisateurs
DELETE FROM Users;

-- Recréer les utilisateurs avec les bons hash BCrypt
-- Mot de passe pour tous: "admin123"
INSERT INTO Users (Username, PasswordHash, FullName, Email, Role, IsActive, CreatedAt) VALUES
('admin', '$2a$11$YQ1pZ8jGcR0JKxL5wF6E5uXQKZ3WNxK9BZrGxJ8mL5nQYP7jKxZ8m', 'Jean Dupont', 'admin@fleetmanager.fr', 'Admin', TRUE, NOW()),
('manager', '$2a$11$YQ1pZ8jGcR0JKxL5wF6E5uXQKZ3WNxK9BZrGxJ8mL5nQYP7jKxZ8m', 'Marie Martin', 'manager@fleetmanager.fr', 'Manager', TRUE, NOW()),
('user', '$2a$11$YQ1pZ8jGcR0JKxL5wF6E5uXQKZ3WNxK9BZrGxJ8mL5nQYP7jKxZ8m', 'Pierre Bernard', 'user@fleetmanager.fr', 'User', TRUE, NOW()),
('technicien', '$2a$11$YQ1pZ8jGcR0JKxL5wF6E5uXQKZ3WNxK9BZrGxJ8mL5nQYP7jKxZ8m', 'Sophie Dubois', 'technicien@fleetmanager.fr', 'Technicien', TRUE, NOW());

SELECT 'Mots de passe réinitialisés avec succès!' AS Status;
SELECT '=== INFORMATIONS DE CONNEXION ===' AS Info;
SELECT 'Tous les utilisateurs utilisent le mot de passe: admin123' AS Info;
SELECT Username, FullName, Role FROM Users;
