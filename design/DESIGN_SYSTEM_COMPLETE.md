# 🎨 Fleet Manager - Système de Design Complet 2025

## 📋 Table des Matières

1. [Vue d'ensemble](#vue-densemble)
2. [Palette de Couleurs](#palette-de-couleurs)
3. [Typographie](#typographie)
4. [Composants UI](#composants-ui)
5. [Espacements & Grille](#espacements--grille)
6. [Interactions & Animations](#interactions--animations)
7. [Accessibilité](#accessibilité)

---

## Vue d'ensemble

Système de design moderne et professionnel pour l'application Fleet Manager, inspiré des meilleures pratiques UI/UX 2025. Design épuré avec coins arrondis, ombres douces et espacements généreux.

**Style visuel** : Moderne, épuré, technologique  
**Inspirations** : Notion, Linear, Vercel, Monday.com  
**Design Systems** : Material Design 3, Ant Design, Chakra UI

---

## 🎨 Palette de Couleurs

### Couleurs Principales

| Nom | Hex | Usage | Brush XAML |
|-----|-----|-------|------------|
| **Primary** | `#6366F1` | Boutons principaux, liens, éléments actifs | `{StaticResource PrimaryBrush}` |
| **Secondary** | `#8B5CF6` | Accents, highlights | `{StaticResource SecondaryBrush}` |
| **Accent** | `#EC4899` | Points d'attention, CTA spéciaux | `{StaticResource AccentBrush}` |

### Couleurs d'État

| Nom | Hex | Usage | Brush XAML |
|-----|-----|-------|------------|
| **Success** | `#10B981` | Succès, validation, disponible | `{StaticResource SuccessBrush}` |
| **Warning** | `#F59E0B` | Avertissements, en maintenance | `{StaticResource WarningBrush}` |
| **Danger** | `#EF4444` | Erreurs, alertes, hors service | `{StaticResource DangerBrush}` |

### Couleurs Neutres

| Nom | Hex | Usage | Brush XAML |
|-----|-----|-------|------------|
| **Background** | `#FFFFFF` | Fond principal | `{StaticResource BackgroundBrush}` |
| **Background Light** | `#F9FAFB` | Fond secondaire, zones de recherche | `{StaticResource BackgroundLightBrush}` |
| **Surface** | `#FFFFFF` | Cartes, modales | `{StaticResource SurfaceBrush}` |
| **Border** | `#E5E7EB` | Bordures | `{StaticResource BorderBrush}` |

### Couleurs de Texte

| Nom | Hex | Usage | Brush XAML |
|-----|-----|-------|------------|
| **Text Primary** | `#1F2937` | Titres, contenu principal | `{StaticResource TextPrimaryBrush}` |
| **Text Secondary** | `#6B7280` | Sous-titres, labels, texte secondaire | `{StaticResource TextSecondaryBrush}` |
| **Text Muted** | `#9CA3AF` | Texte désactivé, hints | `{StaticResource TextMutedBrush}` |

### Variantes Légères (pour badges et fonds)

| Nom | Hex | Usage |
|-----|-----|-------|
| **Primary Light** | `#EEF2FF` | Fond badges primaires, hover |
| **Success Light** | `#D1FAE5` | Fond badges succès |
| **Warning Light** | `#FEF3C7` | Fond badges warning |
| **Danger Light** | `#FEE2E2` | Fond badges danger |
| **Accent Light** | `#FCE7F3` | Fond badges accent |

### États Hover

| Couleur | Hover | Différence |
|---------|-------|------------|
| Primary `#6366F1` | `#4F46E5` | -10% luminosité |
| Secondary `#8B5CF6` | `#7C3AED` | -10% luminosité |
| Accent `#EC4899` | `#DB2777` | -10% luminosité |

---

## 📝 Typographie

### Police Principale

**Famille** : Inter, Segoe UI, SF Pro, -apple-system, sans-serif  
**Fallback** : System fonts pour performance optimale

### Hiérarchie Typographique

| Style | Taille | Poids | Usage | XAML Style |
|-------|--------|-------|-------|-------------|
| **H1** | 32px | Bold (700) | Titres de pages principales | `{StaticResource H1}` |
| **H2** | 24px | SemiBold (600) | Titres de sections | `{StaticResource H2}` |
| **H3** | 20px | SemiBold (600) | Sous-sections | `{StaticResource H3}` |
| **H4** | 18px | Medium (500) | Titres de cartes | `{StaticResource H4}` |
| **Body** | 14-16px | Regular (400) | Corps de texte | `{StaticResource BodyText}` |
| **Small** | 12-14px | Regular (400) | Texte secondaire, hints | `{StaticResource SmallText}` |
| **Label** | 14px | Medium (500) | Labels de formulaires | `{StaticResource LabelText}` |
| **Button** | 14-16px | Medium (500) | Texte des boutons | `{StaticResource ButtonText}` |

### Exemples d'Utilisation

```xml
<!-- Titre principal -->
<TextBlock Style="{StaticResource H1}" Text="Parc de Véhicules"/>

<!-- Sous-titre -->
<TextBlock Style="{StaticResource H2}" Text="Gestion de flotte"/>

<!-- Corps de texte -->
<TextBlock Style="{StaticResource BodyText}" Text="Description..."/>

<!-- Label de formulaire -->
<TextBlock Style="{StaticResource LabelText}" Text="Immatriculation *"/>
```

---

## 🔘 Composants UI

### 1. Boutons

#### Bouton Primaire
```xml
<Button Style="{StaticResource PrimaryButton}" Content="Enregistrer"/>
```

**Caractéristiques** :
- Fond : `#6366F1` (Primary)
- Texte : Blanc
- Border Radius : 8px
- Padding : 12px 24px
- Hauteur minimale : 44px
- Ombre : `ShadowSm` au hover
- Transition : 200ms

**États** :
- **Normal** : `#6366F1`
- **Hover** : `#4F46E5` (assombrissement)
- **Active** : `#4338CA`
- **Disabled** : `#9CA3AF` avec opacité 50%

#### Bouton Secondaire
```xml
<Button Style="{StaticResource SecondaryButton}" Content="Annuler"/>
```

**Caractéristiques** :
- Fond : Transparent
- Bordure : `#6366F1` (2px)
- Texte : `#6366F1`
- Border Radius : 8px
- Hover : Fond `#EEF2FF` (Primary Light)

#### Bouton Tertiaire
```xml
<Button Style="{StaticResource TertiaryButton}" Content="Voir tout"/>
```

**Caractéristiques** :
- Fond : Transparent
- Texte : `#6366F1`
- Hover : Fond `#F9FAFB`
- Pas de bordure

#### Bouton Destructif
```xml
<Button Style="{StaticResource DangerButton}" Content="Supprimer"/>
```

**Caractéristiques** :
- Fond : `#EF4444` (Danger)
- Texte : Blanc
- Hover : `#DC2626`

---

### 2. Cartes (Cards)

#### Carte Standard
```xml
<Border Style="{StaticResource ModernCard}">
    <!-- Contenu -->
</Border>
```

**Caractéristiques** :
- Fond : Blanc (`#FFFFFF`)
- Bordure : `#E5E7EB` (1px, très subtile)
- Border Radius : 12px
- Padding : 20-24px
- Ombre : `ShadowSm`
- Hover : Élévation de l'ombre (`ShadowMd`)

#### Carte de Statistiques (KPI Card)
```xml
<Border Style="{StaticResource StatsCard}">
    <StackPanel>
        <TextBlock Text="🚗" FontSize="28"/>
        <TextBlock Text="42" FontSize="32" FontWeight="Bold"/>
        <TextBlock Text="Véhicules totaux" Style="{StaticResource SmallText}"/>
    </StackPanel>
</Border>
```

**Dimensions** :
- Hauteur : 140px
- Largeur : Flexible (1/6 de la largeur pour 6 cartes)
- Padding : 20px

**Structure interne** :
1. Icône + Label (en haut)
2. Valeur principale (grande, bold)
3. Texte secondaire / Tendance (en bas)

---

### 3. Formulaires

#### Input Standard
```xml
<TextBox Style="{StaticResource ModernTextBox}" 
         Tag="Immatriculation"/>
```

**Caractéristiques** :
- Hauteur : 44px minimum
- Border Radius : 8px
- Bordure : `#E5E7EB` (2px)
- Padding : 12px 16px
- Focus : Bordure `#6366F1` + ombre légère
- Placeholder : Via Tag ou Watermark

**États** :
- **Normal** : Bordure grise
- **Focus** : Bordure `#6366F1` + ombre `0 0 0 4px rgba(99, 102, 241, 0.1)`
- **Erreur** : Bordure `#EF4444` + message d'erreur en dessous
- **Valide** : Bordure `#10B981` + icône ✓

#### Label
```xml
<TextBlock Style="{StaticResource LabelText}" Text="Immatriculation *"/>
```

**Caractéristiques** :
- Taille : 14px
- Poids : Medium (500)
- Couleur : `#1F2937`
- Margin bottom : 8px

#### Message d'Erreur
```xml
<TextBlock Text="Format invalide" 
           Foreground="{StaticResource DangerBrush}"
           FontSize="12"
           Margin="0,4,0,0"/>
```

---

### 4. Tableaux (DataGrid)

#### Style Moderne
```xml
<DataGrid Style="{StaticResource ModernDataGrid}">
    <!-- Colonnes -->
</DataGrid>
```

**Caractéristiques** :
- En-têtes : Fond `#F9FAFB`, texte SemiBold, hauteur 48px
- Lignes : Hauteur 72px
- Zebra striping : Alternance subtile `#FFFFFF` / `#FAFAFA`
- Hover : Fond `#F0F9FF` (bleu très clair)
- Sélection : Fond `#EEF2FF` (Primary Light)
- Bordures : `#E5E7EB` (1px)

---

### 5. Badges / Tags

#### Badge Succès (Disponible)
```xml
<Border Style="{StaticResource SuccessBadge}">
    <StackPanel Orientation="Horizontal">
        <Ellipse Width="8" Height="8" Fill="{StaticResource SuccessBrush}" Margin="0,0,6,0"/>
        <TextBlock Text="Disponible" Foreground="{StaticResource SuccessBrush}"/>
    </StackPanel>
</Border>
```

**Caractéristiques** :
- Fond : `#D1FAE5` (Success Light)
- Texte : `#10B981` (Success)
- Border Radius : 16px (full) ou 8px
- Padding : 6px 12px
- Indicateur : Point coloré (optionnel)

#### Badge Warning (En Maintenance)
- Fond : `#FEF3C7`
- Texte : `#F59E0B`

#### Badge Danger (Hors Service)
- Fond : `#FEE2E2`
- Texte : `#EF4444`

#### Badge Info (En Service)
- Fond : `#DBEAFE`
- Texte : `#3B82F6`

---

### 6. Navigation (Sidebar)

#### Structure
```xml
<Border Style="{StaticResource Sidebar}" Width="260">
    <StackPanel>
        <!-- Logo -->
        <!-- Menu Items -->
    </StackPanel>
</Border>
```

**Caractéristiques** :
- Largeur : 260px (fixe)
- Fond : Blanc ou `#F9FAFB`
- Bordure droite : `#E5E7EB` (1px)

#### Item de Navigation
```xml
<Button Style="{StaticResource NavButton}">
    <StackPanel Orientation="Horizontal">
        <TextBlock Text="📊" FontSize="20" Margin="0,0,12,0"/>
        <TextBlock Text="Dashboard" FontSize="14"/>
    </StackPanel>
</Button>
```

**Caractéristiques** :
- Hauteur : 48px
- Padding : 16px 12px
- Icône : 20px, marge droite 12px
- Texte : 14px, Medium

**États** :
- **Normal** : Fond transparent
- **Hover** : Fond `#F9FAFB`
- **Active** : Fond `#EEF2FF` + bordure gauche `#6366F1` (3px)

---

### 7. Modales

#### Overlay
```xml
<Border Background="#80000000" 
        HorizontalAlignment="Stretch" 
        VerticalAlignment="Stretch">
    <!-- Modal Container -->
</Border>
```

**Caractéristiques** :
- Fond : Noir avec opacité 50% (`#80000000`)
- Animation : Fade in 300ms

#### Container Modal
```xml
<Border Background="{StaticResource SurfaceBrush}"
        Width="800"
        MaxHeight="720"
        CornerRadius="16"
        Effect="{StaticResource ShadowLg}">
    <!-- Contenu -->
</Border>
```

**Caractéristiques** :
- Largeur : 800px (ou flexible selon contenu)
- Hauteur max : 720px (scrollable)
- Border Radius : 16px
- Ombre : `ShadowLg`
- Animation : Scale + Fade (300ms)

---

### 8. Graphiques

**Style** :
- Courbes lissées (smooth)
- Couleurs de la palette principale
- Tooltips au hover
- Légende claire en bas
- Grille subtile en arrière-plan (`#F3F4F6`)

**Couleurs recommandées** :
- Ligne 1 : `#6366F1` (Primary)
- Ligne 2 : `#8B5CF6` (Secondary)
- Ligne 3 : `#EC4899` (Accent)
- Ligne 4 : `#10B981` (Success)

---

## 📐 Espacements & Grille

### Système d'Espacement (basé sur 8px)

| Nom | Valeur | Usage |
|-----|--------|-------|
| **XS** | 4px | Espacement très serré |
| **SM** | 8px | Espacement serré |
| **MD** | 16px | Espacement standard |
| **LG** | 24px | Espacement généreux |
| **XL** | 32px | Espacement très généreux |
| **2XL** | 40px | Espacement entre sections |
| **3XL** | 48px | Espacement entre grandes sections |

### Border Radius

| Nom | Valeur | Usage |
|-----|--------|-------|
| **SM** | 6px | Badges, petits éléments |
| **MD** | 8px | Inputs, boutons |
| **LG** | 12px | Cartes, icônes |
| **XL** | 16px | Modales |
| **Full** | 9999px | Badges ronds, avatars |

### Grille Responsive

**Desktop (> 1920px)** :
- KPI Cards : 6 colonnes
- Graphiques : 2 colonnes (60/40)

**Medium (1280-1920px)** :
- KPI Cards : 3 colonnes (2 lignes)
- Graphiques : 1 colonne (empilés)

**Small (< 1280px)** :
- KPI Cards : 2 colonnes (3 lignes)
- Graphiques : 1 colonne

---

## ✨ Interactions & Animations

### Transitions

**Durée standard** : 200-300ms  
**Easing** : `ease-out` (CubicEase)

### Animations Principales

#### 1. Hover Button
```xml
<Storyboard>
    <ColorAnimation Storyboard.TargetProperty="(Button.Background).(SolidColorBrush.Color)"
                    To="#4F46E5" Duration="0:0:0.2"/>
</Storyboard>
```

#### 2. Apparition Modal
```xml
<Storyboard>
    <!-- Fade In -->
    <DoubleAnimation Storyboard.TargetProperty="Opacity"
                    From="0" To="1" Duration="0:0:0.3"/>
    <!-- Scale Up -->
    <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(ScaleTransform.ScaleX)"
                    From="0.9" To="1" Duration="0:0:0.3">
        <DoubleAnimation.EasingFunction>
            <CubicEase EasingMode="EaseOut"/>
        </DoubleAnimation.EasingFunction>
    </DoubleAnimation>
</Storyboard>
```

#### 3. Hover Card
- Élévation de l'ombre : `ShadowSm` → `ShadowMd`
- Transition : 200ms

#### 4. Focus Input
- Bordure : Gris → Primary
- Ombre : Apparition `0 0 0 4px rgba(99, 102, 241, 0.1)`
- Transition : 200ms

### États Visuels

Tous les éléments interactifs doivent avoir des états clairs :
- **Default** : État initial
- **Hover** : Au survol (feedback immédiat)
- **Active** : Pendant le clic
- **Focus** : Navigation clavier (visible)
- **Disabled** : Désactivé (opacité 50%)

---

## ♿ Accessibilité

### Contraste

**WCAG AA Minimum** :
- Texte normal : Ratio 4.5:1 minimum
- Texte large (18px+) : Ratio 3:1 minimum

**Exemples validés** :
- Primary (`#6366F1`) sur blanc : ✅ 4.8:1
- Texte primaire (`#1F2937`) sur blanc : ✅ 12.6:1
- Texte secondaire (`#6B7280`) sur blanc : ✅ 4.9:1

### Focus Visible

Tous les éléments interactifs doivent avoir un indicateur de focus visible :
```xml
<Style TargetType="Button">
    <Setter Property="FocusVisualStyle">
        <Setter.Value>
            <Style>
                <Setter Property="Control.Template">
                    <Setter.Value>
                        <ControlTemplate>
                            <Border BorderBrush="{StaticResource PrimaryBrush}"
                                    BorderThickness="2"
                                    CornerRadius="4"/>
                        </ControlTemplate>
                    </Setter.Value>
                </Setter>
            </Style>
        </Setter.Value>
    </Setter>
</Style>
```

### Tailles Clicables

**Minimum** : 44×44px pour tous les boutons et éléments interactifs

### Textes Alternatifs

Toutes les icônes doivent avoir un `ToolTip` ou un texte alternatif pour les lecteurs d'écran.

---

## 📊 Exemples de Combinaisons

### Carte de Statistiques Complète
```xml
<Border Style="{StaticResource StatsCard}" Width="200" Height="140">
    <StackPanel Padding="20">
        <Grid Margin="0,0,0,12">
            <TextBlock Text="VÉHICULES" 
                       Style="{StaticResource SmallText}"
                       Foreground="{StaticResource TextSecondaryBrush}"/>
            <TextBlock Text="🚗" 
                       FontSize="28"
                       HorizontalAlignment="Right"/>
        </Grid>
        <TextBlock Text="42" 
                   FontSize="32" 
                   FontWeight="Bold"
                   Foreground="{StaticResource TextPrimaryBrush}"/>
        <StackPanel Orientation="Horizontal" Margin="0,8,0,0">
            <TextBlock Text="↗" 
                       Foreground="{StaticResource SuccessBrush}"
                       FontSize="16"
                       Margin="0,0,4,0"/>
            <TextBlock Text="+2 ce mois" 
                       Style="{StaticResource SmallText}"
                       Foreground="{StaticResource TextSecondaryBrush}"/>
        </StackPanel>
    </StackPanel>
</Border>
```

### Formulaire avec Validation
```xml
<StackPanel>
    <TextBlock Style="{StaticResource LabelText}" Text="Immatriculation *"/>
    <Border BorderBrush="{Binding HasError, Converter={StaticResource ErrorToColorConverter}}"
            BorderThickness="2"
            CornerRadius="8">
        <TextBox Text="{Binding RegistrationNumber}"
                 Padding="12,12,12,12"
                 FontSize="14"/>
    </Border>
    <TextBlock Text="{Binding ErrorMessage}"
               Foreground="{StaticResource DangerBrush}"
               FontSize="12"
               Margin="0,4,0,0"
               Visibility="{Binding HasError, Converter={StaticResource BoolToVisibilityConverter}}"/>
</StackPanel>
```

---

## 🎯 Checklist d'Implémentation

### ✅ À faire
- [ ] Utiliser les couleurs du thème (`{StaticResource PrimaryBrush}`)
- [ ] Appliquer les styles prédéfinis (`ModernButton`, `ModernCard`)
- [ ] Respecter les espacements (8px, 16px, 24px)
- [ ] Tester le contraste des couleurs (WCAG AA)
- [ ] Ajouter des transitions fluides (200-300ms)
- [ ] Implémenter tous les états (hover, focus, disabled)
- [ ] Utiliser les composants UserControls réutilisables

### ❌ À éviter
- [ ] Définir des couleurs hardcodées (`#FF0000`)
- [ ] Créer des styles custom sans base
- [ ] Mélanger différents rayons de bordure
- [ ] Ignorer les états hover/focus
- [ ] Surcharger les ombres (max ShadowLg)
- [ ] Utiliser des tailles de police arbitraires

---

**Version** : 2.0.0  
**Date** : 2025  
**Auteur** : Fleet Manager Design Team  
**Licence** : Propriétaire

