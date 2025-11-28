# 🎨 Système de Design Fleet Manager

## Vue d'ensemble

Système de design moderne inspiré de Tailwind CSS pour l'application Fleet Manager WPF. Fournit une palette de couleurs cohérente, des composants réutilisables et des styles prêts à l'emploi.

---

## 📁 Structure des fichiers

```
FleetManager/
├── Resources/
│   └── ModernTheme.xaml          # ResourceDictionary principal (500+ lignes)
├── Themes/
│   └── ThemeManager.cs           # Utilitaires C# pour accès programmatique
├── Components/
│   ├── StatsCard.xaml/.cs        # Carte de statistiques réutilisable
│   └── StatusBadge.xaml/.cs      # Badge de statut coloré
└── Views/
    └── ModernDashboard.xaml/.cs  # Exemple d'implémentation
```

---

## 🎨 Palette de couleurs

### Couleurs principales

| Nom | Hex | Usage | Brush XAML |
|-----|-----|-------|------------|
| **Primary** | `#4F46E5` | Boutons principaux, liens | `{StaticResource PrimaryBrush}` |
| **Success** | `#10B981` | Succès, validation | `{StaticResource SuccessBrush}` |
| **Warning** | `#F59E0B` | Avertissements | `{StaticResource WarningBrush}` |
| **Danger** | `#EF4444` | Erreurs, suppressions | `{StaticResource DangerBrush}` |
| **Info** | `#3B82F6` | Informations | `{StaticResource InfoBrush}` |

### Couleurs secondaires (Light)

| Nom | Hex | Usage | Brush XAML |
|-----|-----|-------|------------|
| **Primary Light** | `#EEF2FF` | Fond badges, hover | `{StaticResource PrimaryLightBrush}` |
| **Success Light** | `#D1FAE5` | Fond badges succès | `{StaticResource SuccessLightBrush}` |
| **Warning Light** | `#FEF3C7` | Fond badges warning | `{StaticResource WarningLightBrush}` |
| **Danger Light** | `#FEE2E2` | Fond badges danger | `{StaticResource DangerLightBrush}` |
| **Info Light** | `#DBEAFE` | Fond badges info | `{StaticResource InfoLightBrush}` |

### Couleurs neutres

| Nom | Hex | Usage | Brush XAML |
|-----|-----|-------|------------|
| **Background** | `#F8FAFC` | Fond principal | `{StaticResource BackgroundBrush}` |
| **Surface** | `#FFFFFF` | Cartes, modales | `{StaticResource SurfaceBrush}` |
| **Sidebar** | `#1E293B` | Sidebar sombre | `{StaticResource SidebarBrush}` |
| **Border** | `#E2E8F0` | Bordures | `{StaticResource BorderBrush}` |

### Couleurs de texte

| Nom | Hex | Usage | Brush XAML |
|-----|-----|-------|------------|
| **Text Primary** | `#0F172A` | Titres, contenu principal | `{StaticResource TextPrimaryBrush}` |
| **Text Secondary** | `#64748B` | Sous-titres, labels | `{StaticResource TextSecondaryBrush}` |
| **Text Muted** | `#94A3B8` | Texte désactivé | `{StaticResource TextMutedBrush}` |

---

## 📝 Typographie

### Tailles de police

```xml
<StaticResource x:Key="FontSizeXs" Value="11"/>   <!-- Très petit -->
<StaticResource x:Key="FontSizeSm" Value="13"/>   <!-- Petit -->
<StaticResource x:Key="FontSizeBase" Value="15"/> <!-- Base -->
<StaticResource x:Key="FontSizeLg" Value="18"/>   <!-- Grand -->
<StaticResource x:Key="FontSizeXl" Value="20"/>   <!-- Très grand -->
<StaticResource x:Key="FontSize2Xl" Value="24"/>  <!-- Titre H2 -->
<StaticResource x:Key="FontSize3Xl" Value="30"/>  <!-- Stats -->
<StaticResource x:Key="FontSize4Xl" Value="36"/>  <!-- Titre H1 -->
```

### Styles de texte

```xml
<!-- Titres -->
<TextBlock Style="{StaticResource H1}" Text="Titre principal"/>
<TextBlock Style="{StaticResource H2}" Text="Sous-titre"/>
<TextBlock Style="{StaticResource H3}" Text="Section"/>

<!-- Corps de texte -->
<TextBlock Style="{StaticResource BodyText}" Text="Texte normal"/>
<TextBlock Style="{StaticResource SmallText}" Text="Petit texte"/>
<TextBlock Style="{StaticResource MutedText}" Text="Texte secondaire"/>
```

---

## 🔘 Boutons

### Variantes disponibles

```xml
<!-- Bouton principal (Primary blue) -->
<Button Style="{StaticResource ModernButton}" Content="Action principale"/>

<!-- Bouton secondaire (Gris) -->
<Button Style="{StaticResource SecondaryButton}" Content="Annuler"/>

<!-- Bouton succès (Vert) -->
<Button Style="{StaticResource SuccessButton}" Content="Valider"/>

<!-- Bouton warning (Orange) -->
<Button Style="{StaticResource WarningButton}" Content="Attention"/>

<!-- Bouton danger (Rouge) -->
<Button Style="{StaticResource DangerButton}" Content="Supprimer"/>

<!-- Bouton icône (Transparent avec hover) -->
<Button Style="{StaticResource IconButton}" Content="⋮"/>
```

### Effets hover

- **Primary**: Fond s'assombrit (#4338CA)
- **Secondary**: Fond gris (#F1F5F9)
- **Success/Warning/Danger**: Saturation augmentée
- **Icon**: Fond gris léger (#F8FAFC)

---

## 🎴 Cartes

### Carte basique

```xml
<Border Style="{StaticResource ModernCard}">
    <StackPanel>
        <TextBlock Style="{StaticResource CardTitle}" Text="Titre"/>
        <TextBlock Style="{StaticResource CardSubtitle}" Text="Sous-titre"/>
        <!-- Contenu -->
    </StackPanel>
</Border>
```

### Carte de statistiques

```xml
<Border Style="{StaticResource StatsCard}">
    <StackPanel>
        <!-- Icône -->
        <Border Style="{StaticResource StatsIcon}" 
                Background="{StaticResource PrimaryBrush}">
            <TextBlock Text="🚗" FontSize="24"/>
        </Border>
        
        <!-- Valeur -->
        <TextBlock Style="{StaticResource StatsValue}" Text="127"/>
        
        <!-- Label -->
        <TextBlock Style="{StaticResource StatsLabel}" Text="Véhicules"/>
    </StackPanel>
</Border>
```

---

## 🏷️ Badges

### Styles prédéfinis

```xml
<!-- Badge succès -->
<Border Style="{StaticResource BadgeSuccess}">
    <TextBlock Text="Disponible"/>
</Border>

<!-- Badge warning -->
<Border Style="{StaticResource BadgeWarning}">
    <TextBlock Text="En maintenance"/>
</Border>

<!-- Badge danger -->
<Border Style="{StaticResource BadgeDanger}">
    <TextBlock Text="Hors service"/>
</Border>

<!-- Badge info -->
<Border Style="{StaticResource BadgeInfo}">
    <TextBlock Text="En route"/>
</Border>
```

---

## 📋 Formulaires

### TextBox moderne

```xml
<TextBox Style="{StaticResource ModernTextBox}" 
         Tag="Immatriculation"/>
```

### ComboBox moderne

```xml
<ComboBox Style="{StaticResource ModernComboBox}">
    <ComboBoxItem Content="Option 1"/>
    <ComboBoxItem Content="Option 2"/>
</ComboBox>
```

### Caractéristiques

- **Border radius**: 8px
- **Focus**: Bordure Primary (#4F46E5)
- **Padding**: 12px
- **Placeholder**: Affichage via Tag

---

## 📊 DataGrid

```xml
<DataGrid Style="{StaticResource ModernDataGrid}" 
          AutoGenerateColumns="False">
    <DataGrid.Columns>
        <DataGridTextColumn Header="Colonne 1" Binding="{Binding Property1}"/>
        <DataGridTextColumn Header="Colonne 2" Binding="{Binding Property2}"/>
    </DataGrid.Columns>
</DataGrid>
```

### Styles appliqués

- **En-tête**: Fond gris (#F8FAFC), texte semi-bold
- **Ligne hover**: Fond gris clair (#F8FAFC)
- **Ligne sélectionnée**: Fond Primary light (#EEF2FF)
- **Bordures**: Gris (#E2E8F0)

---

## 🧭 Sidebar

```xml
<Border Style="{StaticResource Sidebar}">
    <StackPanel>
        <Button Style="{StaticResource SidebarButton}">
            <StackPanel Orientation="Horizontal">
                <TextBlock Text="📊" Margin="0,0,12,0"/>
                <TextBlock Text="Dashboard"/>
            </StackPanel>
        </Button>
    </StackPanel>
</Border>
```

### Caractéristiques

- **Largeur**: 280px recommandée
- **Fond**: Slate sombre (#1E293B)
- **Hover**: Slate clair (#334155)
- **Boutons**: Texte blanc, padding 16px

---

## 💬 Modales

```xml
<!-- Overlay sombre -->
<Border Style="{StaticResource ModalOverlay}">
    <!-- Conteneur de la modale -->
    <Border Style="{StaticResource ModalContainer}" 
            Width="500" 
            MaxHeight="600">
        <StackPanel>
            <TextBlock Style="{StaticResource H2}" Text="Titre modale"/>
            <!-- Contenu -->
            <StackPanel Orientation="Horizontal" 
                       HorizontalAlignment="Right" 
                       Margin="0,24,0,0">
                <Button Style="{StaticResource SecondaryButton}" 
                        Content="Annuler" 
                        Margin="0,0,12,0"/>
                <Button Style="{StaticResource ModernButton}" 
                        Content="Confirmer"/>
            </StackPanel>
        </StackPanel>
    </Border>
</Border>
```

---

## ✨ Effets & Ombres

### Ombres disponibles

```xml
<!-- Petite ombre (cartes secondaires) -->
<Border Effect="{StaticResource ShadowSm}"/>

<!-- Ombre moyenne (cartes principales) -->
<Border Effect="{StaticResource ShadowMd}"/>

<!-- Grande ombre (modales, dropdowns) -->
<Border Effect="{StaticResource ShadowLg}"/>
```

### Paramètres

- **ShadowSm**: BlurRadius=4, ShadowDepth=2
- **ShadowMd**: BlurRadius=8, ShadowDepth=4
- **ShadowLg**: BlurRadius=15, ShadowDepth=6

---

## 🧩 Composants personnalisés

### StatsCard (UserControl)

```xml
<components:StatsCard Icon="🚗"
                     IconBackground="{StaticResource PrimaryBrush}"
                     Value="127"
                     Label="Véhicules totaux"
                     TrendText="+5 ce mois"
                     TrendIcon="↑"
                     TrendColor="{StaticResource SuccessBrush}"
                     TrendVisibility="Visible"/>
```

#### Propriétés

| Propriété | Type | Description |
|-----------|------|-------------|
| `Icon` | string | Emoji ou symbole |
| `IconBackground` | Brush | Couleur de fond de l'icône |
| `Value` | string | Valeur principale (grande) |
| `Label` | string | Description de la stat |
| `TrendText` | string | Texte de tendance (ex: "+12%") |
| `TrendIcon` | string | ↑ ou ↓ |
| `TrendColor` | Brush | Couleur de la tendance |
| `TrendVisibility` | Visibility | Afficher/masquer la tendance |

### StatusBadge (UserControl)

```xml
<components:StatusBadge Text="Disponible"
                       Background="{StaticResource SuccessLightBrush}"
                       Foreground="{StaticResource SuccessBrush}"/>
```

#### Méthode helper (C#)

```csharp
var badge = new StatusBadge();
badge.SetStatus("Disponible");  // Configure automatiquement les couleurs
```

---

## 🔧 Accès programmatique (C#)

### ThemeManager

```csharp
using FleetManager.Themes;

// Accéder aux couleurs
Brush primaryColor = ThemeManager.Primary;
Brush successColor = ThemeManager.Success;

// Obtenir une couleur par statut
Brush statusColor = ThemeManager.GetStatusColor("Disponible");
// Retourne: Success (vert) pour "Disponible", Warning (orange) pour "En Maintenance", etc.

// Obtenir un fond clair pour badge
Brush bgColor = ThemeManager.GetStatusBackgroundColor("Hors Service");
// Retourne: DangerLight (#FEE2E2)

// Appliquer le thème (si non mergé dans App.xaml)
ThemeManager.ApplyModernTheme();
```

### Extensions

```csharp
using FleetManager.Themes;

// Convertir int en CornerRadius
CornerRadius radius = 12.ToCornerRadius();

// Convertir int en Thickness
Thickness margin = 16.ToThickness();
```

---

## 📐 Espacements & Rayons

### Border Radius

| Nom | Valeur | Usage |
|-----|--------|-------|
| `BorderRadiusSm` | 6px | Badges, petits éléments |
| `BorderRadiusMd` | 8px | Inputs, boutons |
| `BorderRadiusLg` | 12px | Cartes, icônes |
| `BorderRadiusXl` | 16px | Modales |

### Espacements

```csharp
ThemeManager.SpacingXs  // 4px
ThemeManager.SpacingSm  // 8px
ThemeManager.SpacingMd  // 16px
ThemeManager.SpacingLg  // 24px
ThemeManager.SpacingXl  // 32px
```

---

## 🚀 Démarrage rapide

### 1. Intégration dans App.xaml

```xml
<Application.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceDictionary Source="Resources/ModernTheme.xaml"/>
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Application.Resources>
```

### 2. Utiliser dans une fenêtre

```xml
<Window Background="{StaticResource BackgroundBrush}">
    <Grid>
        <!-- Votre contenu avec les styles ModernButton, ModernCard, etc. -->
    </Grid>
</Window>
```

### 3. Exemple minimal

```xml
<Border Style="{StaticResource ModernCard}" Padding="24">
    <StackPanel>
        <TextBlock Style="{StaticResource H2}" Text="Bienvenue"/>
        <TextBlock Style="{StaticResource BodyText}" 
                  Text="Système de design moderne" 
                  Margin="0,8,0,16"/>
        <Button Style="{StaticResource ModernButton}" 
                Content="Commencer"/>
    </StackPanel>
</Border>
```

---

## 🎯 Bonnes pratiques

### ✅ À faire

- Utiliser les couleurs du thème (`{StaticResource PrimaryBrush}`)
- Appliquer les styles prédéfinis (`ModernButton`, `ModernCard`)
- Respecter les espacements (8px, 16px, 24px)
- Utiliser les composants UserControls quand possible
- Tester le contraste des couleurs (AAA pour texte)

### ❌ À éviter

- Définir des couleurs hardcodées (`#FF0000`)
- Créer des styles custom sans base
- Mélanger différents rayons de bordure
- Ignorer les états hover/focus
- Surcharger les ombres (max ShadowLg)

---

## 📱 Responsive

### Grid columns

```xml
<Grid>
    <Grid.ColumnDefinitions>
        <!-- Sidebar fixe -->
        <ColumnDefinition Width="280"/>
        <!-- Contenu flexible -->
        <ColumnDefinition Width="*"/>
    </Grid.ColumnDefinitions>
</Grid>
```

### Stats cards

```xml
<!-- 4 colonnes sur desktop -->
<Grid>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="*"/>
        <ColumnDefinition Width="*"/>
        <ColumnDefinition Width="*"/>
        <ColumnDefinition Width="*"/>
    </Grid.ColumnDefinitions>
</Grid>
```

---

## 🎨 Exemples visuels

### Dashboard complet

Voir `Views/ModernDashboard.xaml` pour un exemple complet incluant:

- Sidebar avec navigation
- Grid de 4 StatsCards
- DataGrid moderne avec badges
- Cartes d'actions rapides
- Layout responsive 2 colonnes

### Formulaire

```xml
<Border Style="{StaticResource ModernCard}">
    <StackPanel>
        <TextBlock Style="{StaticResource H3}" Text="Nouveau véhicule"/>
        
        <TextBox Style="{StaticResource ModernTextBox}" 
                 Tag="Immatriculation" 
                 Margin="0,16,0,0"/>
        
        <TextBox Style="{StaticResource ModernTextBox}" 
                 Tag="Marque" 
                 Margin="0,12,0,0"/>
        
        <ComboBox Style="{StaticResource ModernComboBox}" 
                  Margin="0,12,0,0">
            <ComboBoxItem Content="Essence"/>
            <ComboBoxItem Content="Diesel"/>
            <ComboBoxItem Content="Électrique"/>
        </ComboBox>
        
        <StackPanel Orientation="Horizontal" 
                   Margin="0,24,0,0" 
                   HorizontalAlignment="Right">
            <Button Style="{StaticResource SecondaryButton}" 
                    Content="Annuler" 
                    Margin="0,0,12,0"/>
            <Button Style="{StaticResource ModernButton}" 
                    Content="Créer"/>
        </StackPanel>
    </StackPanel>
</Border>
```

---

## 🐛 Dépannage

### Les styles ne s'appliquent pas

**Solution**: Vérifier que `ModernTheme.xaml` est bien mergé dans `App.xaml`:

```xml
<ResourceDictionary.MergedDictionaries>
    <ResourceDictionary Source="Resources/ModernTheme.xaml"/>
</ResourceDictionary.MergedDictionaries>
```

### Erreur "StaticResource not found"

**Cause**: Ordre de chargement des ResourceDictionaries

**Solution**: Placer `ModernTheme.xaml` en premier dans les MergedDictionaries

### Les ombres ne s'affichent pas

**Solution**: Vérifier que `AllowsTransparency="True"` sur la Window (peut impacter les performances)

```xml
<Window AllowsTransparency="True" WindowStyle="None">
```

---

## 📊 Métriques

- **500+ lignes** de styles XAML réutilisables
- **20+ couleurs** dans la palette
- **6 variantes** de boutons
- **4 types** de badges
- **3 niveaux** d'ombres
- **2 composants** UserControl personnalisés
- **100% compatible** WPF .NET 8.0

---

## 🔄 Mises à jour futures

### Prochaines fonctionnalités

- [ ] Dark mode variant
- [ ] Animation transitions (Storyboard)
- [ ] Toast notifications component
- [ ] Loading spinner component
- [ ] Pagination component
- [ ] Chart components (intégration LiveCharts)
- [ ] Icon font (Material Icons)

---

## 📞 Support

Pour toute question ou amélioration du système de design:

1. Consulter `ModernDashboard.xaml` pour des exemples
2. Lire les commentaires dans `ModernTheme.xaml`
3. Utiliser `ThemeManager` pour l'accès programmatique

---

**Version**: 1.0.0  
**Date**: 2024  
**Auteur**: Fleet Manager Team  
**Licence**: Propriétaire
