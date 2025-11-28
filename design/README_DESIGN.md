# 🎨 Fleet Manager - Guide Complet du Design UI/UX

## 📋 Vue d'ensemble

Ce dossier contient tous les fichiers de design pour l'application Fleet Manager. Le design suit les meilleures pratiques UI/UX 2025 avec un style moderne, épuré et professionnel.

---

## 📁 Structure des Fichiers

```
design/
├── README_DESIGN.md                    # Ce fichier - Guide principal
├── DESIGN_SYSTEM_COMPLETE.md           # Système de design complet (couleurs, typographie, composants)
├── INTERACTIONS_ANIMATIONS.md          # Spécifications des interactions et animations
├── css/
│   └── style.css                      # Feuille de style CSS globale (mise à jour avec nouvelle palette)
├── login.html                          # Maquette écran de connexion
├── dashboard-complete.html             # Maquette tableau de bord principal
├── vehicules-complete.html             # Maquette module gestion des véhicules
├── carburant-trajet.html              # Maquette module carburant (existant)
├── utilisateurs.html                   # Maquette module utilisateurs (existant)
└── vehicules.html                      # Maquette véhicules (existant)
```

---

## 🎨 Palette de Couleurs

### Couleurs Principales

| Couleur | Hex | Usage |
|---------|-----|-------|
| **Primary** | `#6366F1` | Boutons principaux, liens, éléments actifs |
| **Secondary** | `#8B5CF6` | Accents, highlights |
| **Accent** | `#EC4899` | Points d'attention, CTA spéciaux |

### Couleurs d'État

| Couleur | Hex | Usage |
|---------|-----|-------|
| **Success** | `#10B981` | Succès, validation, disponible |
| **Warning** | `#F59E0B` | Avertissements, en maintenance |
| **Danger** | `#EF4444` | Erreurs, alertes, hors service |

### Couleurs Neutres

| Couleur | Hex | Usage |
|---------|-----|-------|
| **Background** | `#FFFFFF` | Fond principal |
| **Background Light** | `#F9FAFB` | Fond secondaire |
| **Text Primary** | `#1F2937` | Titres, contenu principal |
| **Text Secondary** | `#6B7280` | Sous-titres, labels |
| **Border** | `#E5E7EB` | Bordures |

---

## 📐 Typographie

### Hiérarchie

- **H1** : 32px, Bold - Titres de pages principales
- **H2** : 24px, SemiBold - Titres de sections
- **H3** : 20px, SemiBold - Sous-sections
- **H4** : 18px, Medium - Titres de cartes
- **Body** : 14-16px, Regular - Corps de texte
- **Small** : 12-14px, Regular - Texte secondaire

### Police

**Famille** : Inter, Segoe UI, SF Pro, -apple-system, sans-serif

---

## 🧩 Composants Principaux

### 1. Boutons

- **Primaire** : Fond `#6366F1`, texte blanc, border-radius 8px
- **Secondaire** : Bordure `#6366F1`, fond transparent
- **Tertiaire** : Texte uniquement avec hover
- **Destructif** : Fond `#EF4444` pour actions de suppression

### 2. Cartes

- Fond blanc, bordure `#E5E7EB` (1px)
- Border-radius 12px
- Ombre légère (`ShadowSm`)
- Hover : Élévation de l'ombre

### 3. Formulaires

- Inputs : Hauteur 44px minimum, border-radius 8px
- Focus : Bordure `#6366F1` + ombre légère
- Validation : Vert pour valide, rouge pour erreur

### 4. Badges

- Border-radius 16px (full) ou 8px
- Couleurs selon le type (Success, Warning, Danger, Info)
- Indicateur point coloré optionnel

### 5. Navigation

- Sidebar : 260px de large, fond blanc
- Item actif : Fond `#EEF2FF` + bordure gauche `#6366F1` (3px)
- Hover : Fond `#F9FAFB`

---

## 📱 Écrans Disponibles

### 1. Écran de Connexion (`login.html`)

**Caractéristiques** :
- Fond gradient (Indigo → Purple → Pink)
- Carte centrée avec formulaire
- Validation en temps réel
- Animation d'apparition

**Éléments** :
- Logo + titre "FLEET MANAGER"
- Champs email et mot de passe avec icônes
- Option "Se souvenir de moi"
- Lien "Mot de passe oublié"
- Bouton de connexion proéminent

### 2. Tableau de Bord (`dashboard-complete.html`)

**Caractéristiques** :
- 6 cartes de statistiques (KPI)
- 2 graphiques (ligne et barres)
- Section activités récentes
- Section alertes et maintenance

**KPI Cards** :
1. Véhicules totaux
2. Carburant consommé
3. Kilométrage total
4. Coûts mensuels
5. Véhicules disponibles
6. Alertes actives

### 3. Module Véhicules (`vehicules-complete.html`)

**Caractéristiques** :
- En-tête avec bouton "Ajouter Véhicule"
- Barre de filtres (Type, Statut, Marque, Année)
- Barre de recherche
- Tableau avec colonnes :
  - Image/Icône
  - Immatriculation (badge)
  - Véhicule (marque + modèle)
  - Année
  - Kilométrage
  - Statut (badge coloré)
  - Actions (Voir, Modifier, Supprimer)
- Pagination

---

## 🎬 Animations et Interactions

Voir le fichier `INTERACTIONS_ANIMATIONS.md` pour les détails complets.

### Durées Standard

- **Rapide** : 150ms (hover, focus)
- **Standard** : 200ms (transitions générales)
- **Moyenne** : 300ms (modales, apparitions)

### Principales Animations

1. **Hover Bouton** : Assombrissement + élévation
2. **Hover Carte** : Élévation de l'ombre
3. **Focus Input** : Bordure colorée + ombre
4. **Apparition Modal** : Fade in + scale up
5. **Validation** : Animation shake pour erreur, fade in pour succès

---

## 🔧 Intégration WPF

### Fichiers XAML

Le système de design XAML est dans :
- `Resources/ModernTheme.xaml` - **MIS À JOUR** avec la nouvelle palette

### Utilisation

```xml
<Window.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="Resources/ModernTheme.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Window.Resources>

<Button Style="{StaticResource ModernButton}" Content="Enregistrer"/>
<Border Style="{StaticResource ModernCard}">
    <!-- Contenu -->
</Border>
```

### Couleurs dans le Code

```csharp
// Accès aux couleurs
var primaryColor = (SolidColorBrush)Application.Current.Resources["PrimaryBrush"];
var successColor = (SolidColorBrush)Application.Current.Resources["SuccessBrush"];
```

---

## 📊 Maquettes HTML

Les maquettes HTML sont des prototypes visuels pour :
- Validation du design avant implémentation
- Présentation aux stakeholders
- Référence pour le développement

### Visualisation

Ouvrir les fichiers HTML directement dans un navigateur pour voir les maquettes.

### Structure

Toutes les maquettes utilisent :
- `css/style.css` pour les styles globaux
- Structure responsive
- Composants réutilisables

---

## ✅ Checklist d'Implémentation

### Design System
- [x] Palette de couleurs définie
- [x] Typographie hiérarchisée
- [x] Composants documentés
- [x] Espacements standardisés
- [x] Animations spécifiées

### Maquettes
- [x] Écran de connexion
- [x] Tableau de bord
- [x] Module véhicules
- [ ] Formulaire ajout/modification véhicule
- [ ] Module carburant (existant)
- [ ] Module kilométrage
- [ ] Module rapports
- [ ] Module utilisateurs (existant)

### XAML
- [x] ModernTheme.xaml mis à jour
- [ ] Styles de tous les composants
- [ ] Animations Storyboard
- [ ] Templates de contrôles

---

## 🎯 Prochaines Étapes

1. **Compléter les maquettes** :
   - Formulaire ajout/modification véhicule
   - Module kilométrage
   - Module rapports

2. **Implémenter en WPF** :
   - Convertir les maquettes HTML en XAML
   - Appliquer les styles du design system
   - Ajouter les animations

3. **Tests** :
   - Validation visuelle
   - Tests d'accessibilité
   - Tests de performance des animations

---

## 📚 Ressources

### Documentation
- `DESIGN_SYSTEM_COMPLETE.md` - Guide complet du système de design
- `INTERACTIONS_ANIMATIONS.md` - Spécifications des animations

### Fichiers de Code
- `Resources/ModernTheme.xaml` - Système de design XAML
- `css/style.css` - Styles CSS globaux

### Inspiration
- Notion, Linear, Vercel - Design moderne et épuré
- Material Design 3 - Principes d'interaction
- Ant Design - Composants professionnels

---

## 🐛 Dépannage

### Les couleurs ne s'appliquent pas

**Solution** : Vérifier que `ModernTheme.xaml` est bien mergé dans `App.xaml`

### Les animations ne fonctionnent pas

**Solution** : Vérifier que les `RenderTransform` sont définis et que les Storyboards sont correctement déclenchés

### Les maquettes HTML ne s'affichent pas correctement

**Solution** : Vérifier que `css/style.css` est dans le bon chemin relatif

---

**Version** : 2.0.0  
**Date** : 2025  
**Auteur** : Fleet Manager Design Team  
**Licence** : Propriétaire

