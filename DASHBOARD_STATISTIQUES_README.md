# Fleet Manager - Tableau de Bord et Statistiques

## Vue d'ensemble

Le module de statistiques et tableau de bord de Fleet Manager fournit une analyse complète de votre flotte automobile, avec des indicateurs clés de performance (KPI), des graphiques de tendances, et des outils d'export avancés.

## Composants principaux

### 1. Tableau de bord (Dashboard)
**Localisation**: `Views/DashboardView.xaml`

Le tableau de bord offre une vue d'ensemble en temps réel de votre flotte :

#### Indicateurs clés (KPI)
- **Nombre de véhicules**: Total de véhicules avec taux d'activité
- **Nombre de pleins**: Total des enregistrements de carburant ce mois
- **Consommation moyenne**: Moyenne de la flotte en L/100km
- **Coûts carburant**: Coût total du carburant
- **Coûts maintenance**: Coût total de la maintenance

#### Graphiques
- **Tendances 30 jours**: Evolution de la consommation et des coûts
- **Répartition par type**: Distribution des véhicules par catégorie
- **Top consommateurs**: 3 véhicules avec la plus forte consommation
- **Top coûteux**: 3 véhicules les plus onéreux
- **Alertes**: Notifications importantes

#### Mouvements récents
Affiche les 10 dernières opérations (pleins + maintenances)

### 2. Statistiques avancées (Statistics)
**Localisation**: `Views/StatisticsView.xaml`

Page détaillée pour l'analyse en profondeur :

#### Filtres disponibles
- **Période**: Semaine, Mois, Trimestre, Année ou personnalisée
- **Type de véhicule**: Filtrer par type (Voiture, Bus, etc.)
- **Type de carburant**: Filtrer par carburant (Essence, Diesel, Électrique, etc.)
- **Recherche textuelle**: Par immatriculation, marque ou modèle

#### Métriques globales
- Coût total carburant (avec moyenne par plein)
- Coût total maintenance (avec moyenne par intervention)
- Consommation moyenne flotte
- Kilométrage total
- Ratio carburant/maintenance

#### Tableaux d'analyse
- **Analyse détaillée par véhicule**: 
  - Kilométrage, consommation, coûts
  - Nombre de pleins et maintenances
  - Efficacité énergétique

- **Statistiques par type de véhicule**
- **Statistiques par type de carburant**

#### Performances
- **Top véhicules efficaces**: Les plus économes
- **À surveiller**: Les moins efficaces
- **Prédictions**: Tendances futures des coûts

#### Exports disponibles
- **PDF**: Rapport complet avec graphiques
- **CSV**: Données tabulaires pour analyse externe
- **Comparaison de périodes**

## Architecture technique

### ViewModels

#### DashboardViewModel
```csharp
// Propriétés principales
TotalVehicles          // Nombre total de véhicules
ActiveVehicles         // Véhicules actifs
TotalFuelCost         // Coût carburant total
TotalMaintenanceCost  // Coût maintenance total
Alerts                // Alertes actives
RecentMovements       // Mouvements récents
```

#### StatisticsViewModel
```csharp
// Collections principales
VehicleStatistics         // Stats détaillées par véhicule
MonthlyStatistics        // Tendances mensuelles
TypeStatistics           // Analyse par type
FuelStatistics           // Analyse par carburant
PerformanceComparisons   // Comparaisons de performance
Predictions              // Prédictions futures

// Filtres
StartDate / EndDate      // Période
SelectedPeriod          // Période prédéfinie
SelectedVehicleType     // Type de véhicule
SelectedFuelType        // Type de carburant
SearchText              // Recherche textuelle
```

### Services

#### StatisticsService
Méthodes principales :
```csharp
GetDashboardDataAsync()              // Ensemble des données du dashboard
GetFleetStatisticsAsync()            // Stats globales de la flotte
GetVehicleStatisticsAsync(id)        // Stats détaillées d'un véhicule
GetMonthlyTrendsAsync(months)        // Tendances mensuelles
GetVehicleTypeStatisticsAsync()      // Stats par type de véhicule
GetFuelTypeStatisticsAsync()         // Stats par type de carburant
GetDashboardAlertsAsync()            // Alertes critiques
GetConsumptionTrendAsync(days)       // Tendance consommation
GetCostTrendAsync(days)              // Tendance coûts
GetRecentMovementsAsync(count)       // Mouvements récents
GetPredictionsAsync()                // Prédictions futures
```

#### ExportService
```csharp
ExportVehiclesToCsvAsync()                    // Export véhicules CSV
ExportStatisticsToCsvAsync()                  // Export stats CSV
ExportMonthllyStatisticsToCsvAsync()         // Export tendances CSV
ExportPerformanceComparisonsToCsvAsync()     // Export comparaisons CSV
GeneratePdfReport()                          // Rapport PDF simple
GenerateAdvancedPdfReport()                  // Rapport PDF avancé
```

### Modèles de données

#### VehicleStatistics
Contient les stats détaillées d'un véhicule :
- Kilométrage, consommation, coûts
- Dates de maintenance
- Efficacité énergétique

#### FleetStatistics
Stats globales de la flotte :
- Nombre total/actifs/en maintenance
- Coûts totaux et mensuels
- Kilométrage moyen

#### MonthlyStatistics
Tendances par mois :
- Coûts carburant/maintenance
- Consommation et litres
- Nombre d'opérations

#### DashboardAlert
Alertes et notifications :
- Types: Maintenance, Inspection, Assurance, Consommation
- Priorités: Basse, Moyenne, Haute, Critique

## Utilisation

### Accéder au tableau de bord
1. Connectez-vous à l'application
2. Cliquez sur l'onglet "Tableau de bord"
3. Les données se chargent automatiquement

### Consulter les statistiques
1. Allez dans l'onglet "Statistiques"
2. Configurez les filtres souhaités
3. Consultez les différentes sections

### Exporter des données
1. Dans les statistiques, cliquez sur l'un des boutons d'export
2. Sélectionnez le format (CSV, PDF)
3. Choisissez le chemin de sauvegarde

### Consulter les alertes
1. Sur le tableau de bord, consultez la section "Alertes"
2. Les alertes critiques s'affichent en priorité
3. Cliquez sur une alerte pour plus de détails

## Fonctionnalités avancées

### Alertes intelligentes
- **Maintenance due**: Alertes basées sur la date prévue
- **Contrôle technique**: Notifications pour l'inspection
- **Assurance expirée**: Alertes d'expiration d'assurance
- **Consommation élevée**: Détection des anomalies
- **Coûts anormaux**: Seuils personnalisables

### Prédictions
- Estimation des coûts futurs basée sur les tendances
- Détection des évolutions anormales
- Recommandations automatiques

### Graphiques et visualisations
- Graphiques en barres pour les comparaisons
- Courbes de tendance sur 90 jours
- Distribution par type/carburant
- Ratios de performance

## Configuration

### appsettings.json
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### Dépendances
- Entity Framework Core 9.0
- CsvHelper 33.1
- iTextSharp 3.7.8
- LiveChartsCore 2.0 (optionnel)

## Optimisation des performances

### Chargement des données
- Utilisation du lazy loading via EF Core
- Chargement asynchrone des collections
- Cache des résultats les plus courants

### Requêtes de base de données
- Indexation sur les colonnes de recherche
- Utilisation de projections au lieu de séries complètes
- Groupement côté serveur

### Interface utilisateur
- Virtualisation des listes longues
- ScrollViewer avec lazy rendering
- ProgressBar pour les longues opérations

## Dépannage

### Les données ne se chargent pas
1. Vérifiez la connexion à la base de données
2. Consultez les logs dans la fenêtre Debug
3. Cliquez sur "Actualiser" pour retry

### Export en PDF impossible
1. Vérifiez que le chemin est accessible
2. Assurez-vous d'avoir les permissions d'écriture
3. Vérifiez que l'espace disque est disponible

### Alertes non détectées
1. Vérifiez les dates de maintenance/inspection
2. Configurez les seuils d'alerte dans les paramètres
3. Actualisez les données

## Futures améliorations

- [ ] Graphiques interactifs avec LiveCharts 2
- [ ] Export Excel avancé
- [ ] Comparaison de périodes
- [ ] Modèles de prédiction ML
- [ ] Notifications en temps réel
- [ ] Dashboard personnalisable
- [ ] Planification de maintenance
- [ ] Analyse ROI par véhicule
