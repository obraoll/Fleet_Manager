# 📊 Module Statistiques - Guide d'Utilisation

## ✅ État du Module

Le **module de statistiques avancées** est maintenant **complètement fonctionnel** et intégré à FleetManager.

## 🎯 Fonctionnalités Disponibles

### 1. **Tableau de Bord Statistique Global**
   - 💰 Coût total carburant
   - 🔧 Coût total maintenance
   - ⛽ Consommation moyenne
   - 🚗 Kilométrage total
   - 📊 Ratio coût/km

### 2. **Analyse Détaillée par Véhicule**
   DataGrid avec colonnes :
   - Immatriculation
   - Modèle
   - Kilométrage total
   - Consommation moyenne (L/100km)
   - Nombre de pleins
   - Coût carburant
   - Coût maintenance
   - Coût total
   - Coût par kilomètre (€/km)
   - Indicateur d'efficacité

### 3. **Analyse de Performance**
   - 🏆 **Top Performers** : Véhicules les plus efficaces
   - ⚠️ **Bottom Performers** : Véhicules à surveiller
   - 📈 **Prédictions** : Estimations de coûts futurs

### 4. **Statistiques par Type**
   - **Par type de véhicule** : Voiture, Camion, Utilitaire
   - **Par type de carburant** : Essence, Diesel, Électrique

### 5. **Tendances Mensuelles avec LiveCharts** 📉
   Trois onglets de graphiques :
   
   #### Onglet Coûts
   - Graphique en colonnes : Carburant vs Maintenance par mois
   
   #### Onglet Consommation
   - Graphique de consommation mensuelle
   
   #### Onglet Activité
   - Tableau avec nombre de pleins et maintenances par mois

### 6. **Comparaison de Performance**
   Tableau de comparaison avec :
   - Note globale
   - Recommandations personnalisées

### 7. **Filtres Avancés**
   - 📅 **Période** : Semaine, Mois, Trimestre, Année, Personnalisée
   - 🚙 **Type de véhicule** : Filtrage par catégorie
   - ⛽ **Type de carburant** : Filtrage par énergie
   - 🔍 **Recherche textuelle** : Par immatriculation ou modèle

### 8. **Export et Rapports**
   - 📄 **Export PDF** : Génération de rapport complet
   - 📊 **Export CSV** : Export des données au format CSV
   - 📧 **Envoi par email** : Envoi automatique de rapports
   - 🎯 **Définition d'objectifs** : Configuration de KPI

## 🔧 Architecture Technique

### Structure des Fichiers

```
Views/
  └── StatisticsView.xaml         # Interface utilisateur complète (1108 lignes)
  └── StatisticsView.xaml.cs      # Code-behind avec injection du ViewModel

ViewModels/
  └── StatisticsViewModel.cs      # ViewModel complet (1047 lignes)
                                   # - 15+ ICommand pour toutes les actions
                                   # - LiveCharts Series configuration
                                   # - Filtres et tri
                                   # - Export et rapports

Models/
  └── Statistics.cs               # Modèles de données :
                                   # - VehicleStatistics
                                   # - MonthlyStatistics
                                   # - VehicleTypeStatistics
                                   # - FuelTypeStatistics
                                   # - PerformanceComparison
                                   # - PredictionData
                                   # - TimeSeriesData

Services/
  └── StatisticsService.cs        # Service de calcul des statistiques
  └── ExportService.cs            # Service d'export PDF/CSV
  └── EmailService.cs             # Service d'envoi d'emails
  └── MaintenanceRepository.cs    # Repository ADO.NET pour maintenances
```

### Technologies Utilisées

- **WPF .NET 8.0** : Interface utilisateur
- **MVVM Pattern** : Architecture Model-View-ViewModel
- **LiveChartsCore** : Graphiques interactifs avancés
- **Entity Framework Core 9** : ORM pour base de données
- **ADO.NET (MySqlConnector)** : Accès direct pour MaintenanceRecords
- **iText 9** : Génération de PDF
- **Dependency Injection** : Gestion des services

## 🚀 Comment Utiliser le Module

### 1. Accéder aux Statistiques

Dans l'application FleetManager :
1. Cliquez sur **"Statistiques"** dans le menu latéral
2. Le module charge automatiquement toutes les données

### 2. Naviguer dans les Données

- **Tableau principal** : Vue d'ensemble de tous les véhicules
- **Onglets de graphiques** : Visualisation des tendances
- **Filtres** : Affiner les données affichées

### 3. Utiliser les Filtres

```
┌─────────────────────────────────────┐
│  Période: [Année ▼]                 │
│  Type véhicule: [Tous ▼]            │
│  Type carburant: [Tous ▼]           │
│  Recherche: [_____________]         │
└─────────────────────────────────────┘
```

### 4. Générer un Rapport

1. Cliquez sur **"Générer Rapport PDF"**
2. Sélectionnez l'emplacement de sauvegarde
3. Le rapport contient toutes les statistiques filtrées

### 5. Exporter les Données

- **CSV** : Pour Excel/analyse externe
- **PDF** : Pour archivage/impression
- **Email** : Envoi automatique aux destinataires configurés

## 📊 LiveCharts - Graphiques Avancés

### Configuration des Séries

Le ViewModel expose :

```csharp
public IEnumerable<ISeries> MonthlyCostSeries
public IEnumerable<ISeries> MonthlyConsumptionSeries
public string[] MonthlyLabels
```

### Types de Graphiques

1. **ColumnSeries** : Graphiques en colonnes pour les coûts mensuels
2. **LineSeries** : Courbes pour les tendances de consommation
3. **Axes personnalisés** : Labels de mois en français

## ⚡ Performances

### Optimisations Implémentées

- ✅ **ADO.NET pour MaintenanceRecords** : Accès direct sans overhead EF Core
- ✅ **Chargement asynchrone** : Interface réactive pendant le chargement
- ✅ **Filtres en mémoire** : LINQ pour filtrages rapides
- ✅ **ObservableCollection** : Mise à jour automatique de l'UI

### Temps de Chargement Estimés

- Parc de 10 véhicules : < 1 seconde
- Parc de 100 véhicules : 1-2 secondes
- Parc de 1000 véhicules : 3-5 secondes

## 🐛 Dépannage

### Le module ne charge pas de données

1. Vérifiez que des véhicules existent dans la base de données
2. Vérifiez que des enregistrements de carburant/maintenance existent
3. Consultez les logs de debug dans la console

### Les graphiques sont vides

1. Assurez-vous que la période sélectionnée contient des données
2. Vérifiez les filtres actifs
3. Essayez de réinitialiser les filtres (bouton "Réinitialiser")

### Erreur lors de l'export PDF

1. Vérifiez que vous avez les permissions d'écriture
2. Assurez-vous que le fichier n'est pas déjà ouvert
3. Vérifiez l'espace disque disponible

## 📝 Notes Importantes

### MaintenanceRecords et Entity Framework

⚠️ **Important** : Les MaintenanceRecords sont maintenant gérés par ADO.NET, pas par Entity Framework Core.

**Raison** : Résolution d'un bug critique EF Core avec propriété fantôme "UserId".

**Impact** :
- ✅ Plus de requêtes EF Core pour MaintenanceRecords
- ✅ MaintenanceRepository utilisé à la place
- ✅ Pas de navigation properties Vehicle <-> MaintenanceRecord
- ✅ Pas d'Include() ou de ThenInclude() pour les maintenances

### Dépendances Requises

Le StatisticsViewModel nécessite :
- `VehicleService` : Gestion des véhicules
- `FuelService` : Gestion du carburant
- `StatisticsService` : Calculs statistiques
- `ExportService` : Génération PDF/CSV
- `IEmailService` : Envoi d'emails

Toutes sont injectées automatiquement via Dependency Injection.

## 🎓 Pour les Développeurs

### Ajouter une Nouvelle Statistique

1. **Ajouter la propriété dans StatisticsViewModel** :
```csharp
private decimal _newMetric;
public decimal NewMetric
{
    get => _newMetric;
    set => SetProperty(ref _newMetric, value);
}
```

2. **Calculer dans LoadDataAsync** :
```csharp
NewMetric = await _statisticsService.CalculateNewMetricAsync();
```

3. **Afficher dans StatisticsView.xaml** :
```xml
<TextBlock Text="{Binding NewMetric, StringFormat='{}{0:N2}'}" />
```

### Ajouter une Nouvelle Commande

1. **Déclarer dans StatisticsViewModel** :
```csharp
public ICommand NewActionCommand { get; private set; }
```

2. **Initialiser dans InitializeCommands** :
```csharp
NewActionCommand = new RelayCommand(async param => await NewActionAsync());
```

3. **Implémenter la méthode** :
```csharp
private async Task NewActionAsync()
{
    // Votre logique ici
}
```

4. **Lier dans StatisticsView.xaml** :
```xml
<Button Content="Nouvelle Action" Command="{Binding NewActionCommand}" />
```

## ✅ Résumé des Modifications

### Fichiers Modifiés
- ✅ `Views/StatisticsView.xaml.cs` : Ajout constructeur avec ViewModel
- ✅ `ViewModels/MainViewModel.cs` : Navigation vers StatisticsView réelle

### Fichiers Existants (Non Modifiés)
- ✅ `Views/StatisticsView.xaml` : Interface complète déjà implémentée
- ✅ `ViewModels/StatisticsViewModel.cs` : ViewModel complet déjà implémenté
- ✅ `Services/StatisticsService.cs` : Service déjà fonctionnel avec MaintenanceRepository
- ✅ `Services/ExportService.cs` : Export déjà implémenté
- ✅ `Services/EmailService.cs` : Email déjà implémenté
- ✅ `Models/Statistics.cs` : Tous les modèles déjà définis

## 🎉 Conclusion

Le **module de statistiques avancées** est **100% fonctionnel** et prêt à l'emploi !

Toutes les fonctionnalités sont implémentées :
- ✅ Tableau de bord avec métriques globales
- ✅ Analyse détaillée par véhicule
- ✅ Top/Bottom performers
- ✅ Prédictions
- ✅ Statistiques par type
- ✅ Graphiques mensuels avec LiveCharts
- ✅ Filtres avancés (période, type, recherche)
- ✅ Export PDF/CSV
- ✅ Envoi par email
- ✅ Comparaisons de performance

**Lancez l'application et cliquez sur "Statistiques" pour profiter de toutes ces fonctionnalités !** 🚀
