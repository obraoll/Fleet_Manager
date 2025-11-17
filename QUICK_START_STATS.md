# Guide d'utilisation rapide - Tableau de bord et Statistiques

## Accès rapide

### Depuis le menu principal
1. Cliquez sur **"📊 Tableau de bord"** pour l'aperçu général
2. Cliquez sur **"📈 Statistiques"** pour l'analyse détaillée

## Tableau de bord - Étape par étape

### 1. Vue d'ensemble immédiate
- **Nombre de véhicules**: Vue globale de la flotte
- **Indicateurs colorés**: 
  - 🔵 Bleu = Carburant
  - 🟢 Vert = Véhicules
  - 🟠 Orange = Consommation
  - 🔴 Rouge = Alertes

### 2. Consulter les alertes
- En haut à droite: nombre d'alertes actives
- Section "Alertes" en bas: liste détaillée
- 🚨 Critique = Action immédiate requise
- ⚠️ Élevée = À vérifier
- ℹ️ Moyenne = Information
- ✓ Basse = Suivi

### 3. Analyser les tendances
- Onglet "Consommation": évolution des L/100km
- Onglet "Coûts": dépenses mensuelles
- Cliquez sur "🔄 Actualiser" pour mettre à jour

### 4. Identifier les problèmes
- **"⚡ Top consommateurs"**: Véhicules à contrôler
- **"💸 Top coûteux"**: Véhicules chers à exploiter
- **"🚙 Répartition par type"**: Distribution de la flotte

## Statistiques - Analyse détaillée

### 1. Configurer les filtres
```
Étape 1: Période
├─ Semaine (7 jours)
├─ Mois (30 jours)
├─ Trimestre (90 jours)
├─ Année (365 jours)
└─ Personnalisé (dates libres)

Étape 2: Type de véhicule (optionnel)
└─ Sélectionner dans la dropdown

Étape 3: Type de carburant (optionnel)
└─ Sélectionner dans la dropdown

Étape 4: Recherche (optionnel)
└─ Taper l'immatriculation ou modèle
```

### 2. Lire le résumé global
```
📊 VUE D'ENSEMBLE
├─ ⛽ Coût carburant total
├─ 🔧 Coût maintenance
├─ 📊 Consommation moyenne
├─ 🛣️ Kilométrage total
└─ ⚖️ Ratio Carburant/Maintenance
```

### 3. Analyser par véhicule
Le tableau montre pour chaque véhicule:
- **Km**: Kilométrage actuel
- **Conso.**: Consommation en L/100km
- **Pleins**: Nombre de ravitaillements
- **Coût Carb.**: Dépenses carburant
- **Maintenance**: Dépenses maintenance
- **Total**: Coût global
- **€/km**: Coût par kilomètre (important!)
- **Efficacité**: Km par litre

### 4. Interpréter les couleurs
```
Performance:
✓ Vert        = Bon (< moyenne flotte)
🟡 Orange     = À surveiller (proche de la moyenne)
🔴 Rouge      = Mauvais (> moyenne flotte + 30%)
```

### 5. Consulter les recommandations
- **Prédictions** (🔮): 
  - Tendances futures
  - Changements prévus en %
  - Indicateurs up/down/stable

- **Comparaisons** (🏆):
  - Grade A à E
  - Score d'efficacité /5
  - Recommandations personnalisées

## Exporter des données

### Export CSV
1. Cliquez sur **"📊 Export CSV"**
2. Choisissez le dossier
3. Nom généré automatiquement: `Statistiques_Vehicules_YYYYMMDD.csv`
4. Ouvrez dans Excel pour analyser

### Export PDF
1. Cliquez sur **"📄 Rapport PDF"**
2. Choisissez le dossier
3. Nom généré automatiquement: `Statistiques_FleetManager_YYYYMMDD.pdf`
4. Partageables et imprimables

### Comparaison de périodes
1. Cliquez sur **"📊 Comparer véhicules"**
2. Sélectionnez deux périodes
3. Consultez les écarts

## Cas d'usage courants

### 1. Identifier un véhicule consommant trop
```
Étapes:
1. Allez à "Statistiques"
2. Regardez "⚡ Top consommateurs"
3. Vérifiez sa consommation vs moyenne flotte
4. Consultez son historique d'entretien
5. Réalisez une révision technique si > +30%
```

### 2. Analyser les coûts mensuels
```
Étapes:
1. Allez à "Tableau de bord"
2. Consultez "📈 Évolution sur 12 mois"
3. Identifiez les pics de dépenses
4. Allez à "Statistiques" > "Évolution mensuelle"
5. Analysez les raisons (ex: maintenance groupée)
```

### 3. Générer un rapport pour la direction
```
Étapes:
1. Allez à "Statistiques"
2. Configurez la période souhaitée
3. Cliquez sur "📄 Rapport PDF"
4. Le fichier se crée automatiquement
5. Envoyez par email ou imprimez
```

### 4. Vérifier la maintenance due
```
Étapes:
1. Consultez le "Tableau de bord"
2. Section "⚠️ Alertes" > "Maintenance due"
3. Cliquez sur l'alerte
4. Planifiez l'intervention
5. Mettez à jour après maintenance
```

### 5. Benchmarker deux véhicules
```
Étapes:
1. Allez à "Statistiques"
2. Sélectionnez le véhicule 1
3. Prenez note de ses stats
4. Sélectionnez le véhicule 2
5. Comparez les métriques
```

## Indicateurs clés à surveiller

### Pour chaque véhicule:
- **Consommation**: Doit être stable (±5%)
- **Coût/km**: Doit être < moyenne flotte
- **Dérive**: Si +20% → vérification mécanique
- **Maintenance**: Tous les 15 000-20 000 km

### Pour la flotte:
- **Consommation moyenne**: Établir la baseline
- **Ratio C/M**: Carburant doit être > Maintenance
- **Total mensuel**: Doit être < budget

### Alertes prioritaires:
1. 🚨 Inspections/Assurance expirées → Action immédiate
2. 🔧 Maintenance > 30 jours retard → Programmer
3. ⛽ Consommation +50% → Diagnostic
4. 💰 Coût/km > +40% → Étude economique

## Raccourcis clavier

| Action | Raccourci |
|--------|-----------|
| Actualiser | Ctrl+R |
| Exporter CSV | Ctrl+E |
| Exporter PDF | Ctrl+P |
| Réinitialiser filtres | Ctrl+R |
| Recherche | Ctrl+F |

## FAQ

**Q: Comment actualiser les données?**
A: Cliquez sur "🔄 Actualiser" ou attendez la mise à jour automatique (5 min)

**Q: Peut-on modifier les seuils d'alerte?**
A: Oui, dans les paramètres > Alertes (en développement)

**Q: Les données historiques sont conservées combien longtemps?**
A: Indéfiniment dans la base de données

**Q: Comment retrouver un export ancien?**
A: Consultez le dossier ./Exports/ ou la base de données

**Q: Peut-on personnaliser le tableau de bord?**
A: Fonctionnalité prévue en Q2 2024

## Support

Pour toute question :
1. Consultez l'aide intégrée (?)
2. Vérifiez le README détaillé
3. Consultez les logs (Fichier > Logs)
4. Contactez l'équipe support
