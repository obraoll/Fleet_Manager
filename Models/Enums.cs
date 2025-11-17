namespace FleetManager.Models
{
    /// <summary>
    /// Rôles utilisateur
    /// </summary>
    public enum UserRole
    {
        Admin,
        User
    }

    /// <summary>
    /// Types de véhicules
    /// </summary>
    public enum VehicleType
    {
        Voiture,
        Camion,
        Moto,
        Utilitaire
    }

    /// <summary>
    /// Types de carburant
    /// </summary>
    public enum FuelType
    {
        Essence,
        Diesel,
        Electrique,
        Hybride,
        GPL
    }

    /// <summary>
    /// Statut du véhicule
    /// </summary>
    public enum VehicleStatus
    {
        Actif,
        EnMaintenance,
        HorsService,
        Vendu
    }

    /// <summary>
    /// Types de maintenance
    /// </summary>
    public enum MaintenanceType
    {
        Vidange,
        Révision,
        Réparation,
        Pneus,
        Freins,
        Autre
    }

    /// <summary>
    /// Statut du conducteur
    /// </summary>
    public enum DriverStatus
    {
        Actif,
        Inactif,
        EnCongé
    }
}