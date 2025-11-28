# 🎨 Fleet Manager - Guide du Système de Design

## 📋 Table des Matières
1. [Palette de Couleurs](#palette-de-couleurs)
2. [Typographie](#typographie)
3. [Composants UI](#composants-ui)
4. [Espacements](#espacements)
5. [Exemples d'Utilisation](#exemples-dutilisation)

---

## 🎨 Palette de Couleurs

### Couleurs Principales
```xaml
<!-- Indigo (Couleur primaire) -->
PrimaryBrush: #6366F1
PrimaryHoverBrush: #4F46E5
PrimaryLightBrush: #818CF8
PrimaryDarkBrush: #4338CA

<!-- Purple (Couleur secondaire) -->
SecondaryBrush: #8B5CF6
SecondaryHoverBrush: #7C3AED
SecondaryLightBrush: #A78BFA

<!-- Pink (Couleur d'accent) -->
AccentBrush: #EC4899
AccentHoverBrush: #DB2777
AccentLightBrush: #F472B6
```

### États et Notifications
```xaml
<!-- Succès -->
SuccessBrush: #10B981 (Vert)
SuccessLightBrush: #D1FAE5 (Fond clair)

<!-- Warning -->
WarningBrush: #F59E0B (Orange)
WarningLightBrush: #FEF3C7 (Fond clair)

<!-- Danger -->
DangerBrush: #EF4444 (Rouge)
DangerLightBrush: #FEE2E2 (Fond clair)
```

### Arrière-plans et Textes
```xaml
<!-- Arrière-plans -->
BackgroundBrush: #F9FAFB (Gris très clair)
SurfaceBrush: #FFFFFF (Blanc pur)
HoverBackgroundBrush: #F3F4F6
BorderBrush: #E5E7EB
DividerBrush: #D1D5DB

<!-- Textes -->
TextPrimaryBrush: #1F2937 (Gris foncé)
TextSecondaryBrush: #6B7280 (Gris moyen)
TextTertiaryBrush: #9CA3AF (Gris clair)
TextDisabledBrush: #D1D5DB
```

---

## 📝 Typographie

### Hiérarchie des Titres
```xaml
<!-- H1 - Titres de pages principales -->
Style="{StaticResource H1}"
FontSize: 32px | FontWeight: Bold | Color: TextPrimaryBrush

<!-- H2 - Titres de sections -->
Style="{StaticResource H2}"
FontSize: 24px | FontWeight: SemiBold

<!-- H3 - Sous-sections -->
Style="{StaticResource H3}"
FontSize: 20px | FontWeight: SemiBold

<!-- H4 - Titres de cartes -->
Style="{StaticResource H4}"
FontSize: 18px | FontWeight: Medium
```

### Corps de Texte
```xaml
<!-- Texte principal -->
Style="{StaticResource BodyText}"
FontSize: 14px | LineHeight: 22px

<!-- Texte secondaire -->
Style="{StaticResource SecondaryText}"
FontSize: 13px | Color: TextSecondaryBrush

<!-- Caption / Petit texte -->
Style="{StaticResource CaptionText}"
FontSize: 12px | Color: TextTertiaryBrush
```

---

## 🧩 Composants UI

### 🔘 Boutons

#### Bouton Principal (Primary)
```xaml
<Button Content="Enregistrer" 
        Style="{StaticResource PrimaryButton}"/>
```
- Couleur: Indigo (#6366F1)
- Padding: 20,12
- Border Radius: 8px
- Shadow: Ombre douce
- Hover: Assombrissement + ombre plus prononcée

#### Bouton Secondaire (Outline)
```xaml
<Button Content="Annuler" 
        Style="{StaticResource SecondaryButton}"/>
```
- Bordure: 2px Indigo
- Fond: Transparent
- Hover: Fond bleu très clair (#EEF2FF)

#### Bouton Tertiaire (Ghost)
```xaml
<Button Content="Modifier" 
        Style="{StaticResource TertiaryButton}"/>
```
- Sans bordure
- Fond transparent
- Hover: Fond gris clair

#### Bouton Destructif (Danger)
```xaml
<Button Content="Supprimer" 
        Style="{StaticResource DangerButton}"/>
```
- Couleur: Rouge (#EF4444)
- Même style que Primary mais en rouge

---

### 🃏 Cartes (Cards)

#### Carte Standard
```xaml
<Border Style="{StaticResource CardStyle}">
    <!-- Contenu -->
</Border>
```
- Fond: Blanc (#FFFFFF)
- Bordure: 1px grise (#E5E7EB)
- Border Radius: 12px
- Padding: 24px
- Shadow: Ombre douce (BlurRadius: 10, Depth: 2)

#### Carte avec Hover
```xaml
<Border Style="{StaticResource CardHoverStyle}">
    <!-- Contenu cliquable -->
</Border>
```
- Effet d'élévation au survol
- Cursor: Hand

#### Carte de Statistique (KPI)
```xaml
<Border Style="{StaticResource StatCardStyle}">
    <StackPanel>
        <TextBlock Text="VÉHICULES" Style="{StaticResource CaptionText}"/>
        <TextBlock Text="42" Style="{StaticResource H2}"/>
    </StackPanel>
</Border>
```
- Plus compacte (padding: 20px)
- Border Radius: 10px

---

### 📋 Badges et Tags

#### Badge Succès (Disponible, Actif)
```xaml
<Border Style="{StaticResource SuccessBadge}">
    <TextBlock Text="Disponible" Style="{StaticResource SuccessBadgeText}"/>
</Border>
```
- Fond: Vert clair (#D1FAE5)
- Texte: Vert foncé (#065F46)
- Border Radius: 16px (pill shape)

#### Badge Warning (En service)
```xaml
<Border Style="{StaticResource WarningBadge}">
    <TextBlock Text="En service" Style="{StaticResource WarningBadgeText}"/>
</Border>
```
- Fond: Orange clair (#FEF3C7)
- Texte: Orange foncé (#92400E)

#### Badge Danger (Hors service)
```xaml
<Border Style="{StaticResource DangerBadge}">
    <TextBlock Text="Hors service" Style="{StaticResource DangerBadgeText}"/>
</Border>
```
- Fond: Rouge clair (#FEE2E2)
- Texte: Rouge foncé (#991B1B)

#### Badge Primaire (Info)
```xaml
<Border Style="{StaticResource PrimaryBadge}">
    <TextBlock Text="Nouveau" Style="{StaticResource PrimaryBadgeText}"/>
</Border>
```
- Fond: Indigo clair (#EEF2FF)
- Texte: Indigo foncé (#3730A3)

---

### 📝 Formulaires

#### TextBox Moderne
```xaml
<TextBox Text="{Binding Immatriculation}"/>
```
- Hauteur min: 44px
- Border Radius: 8px
- Focus: Bordure indigo (2px)
- Hover: Bordure indigo clair

#### PasswordBox
```xaml
<PasswordBox Password="{Binding MotDePasse}"/>
```
- Même style que TextBox

#### ComboBox
```xaml
<ComboBox ItemsSource="{Binding VehicleTypes}"
          SelectedItem="{Binding SelectedType}"/>
```
- Dropdown avec ombre
- Border Radius: 8px
- Icône flèche personnalisée

#### CheckBox
```xaml
<CheckBox Content="Se souvenir de moi" IsChecked="{Binding RememberMe}"/>
```
- Carré arrondi (4px)
- Checkmark en indigo
- Fond bleu clair quand coché

#### Label
```xaml
<Label Content="Immatriculation"/>
```
- FontWeight: Medium
- Margin bottom: 6px

---

### 📊 DataGrid Moderne

```xaml
<DataGrid ItemsSource="{Binding Vehicles}">
    <DataGrid.Columns>
        <DataGridTextColumn Header="Immatriculation" Binding="{Binding RegistrationNumber}"/>
        <!-- ... -->
    </DataGrid.Columns>
</DataGrid>
```

**Caractéristiques:**
- En-têtes: Fond gris clair (#F9FAFB), texte gris moyen
- Lignes: Hauteur 56px
- Alternance: Fond #FAFBFC pour lignes paires
- Hover: Fond bleu très clair (#F0F9FF)
- Sélection: Fond bleu clair (#DBEAFE)
- Pas de bordures entre cellules
- Bordure fine entre lignes (#F3F4F6)

---

### 🧭 Navigation Sidebar

#### Bouton de Navigation Normal
```xaml
<Button Style="{StaticResource NavButtonStyle}">
    <StackPanel Orientation="Horizontal">
        <TextBlock Text="🚗" Margin="0,0,8,0"/>
        <TextBlock Text="Véhicules"/>
    </StackPanel>
</Button>
```
- Fond transparent
- Hover: Fond gris clair
- Padding: 16,12px
- Border Radius: 8px

#### Bouton de Navigation Actif
```xaml
<Button Style="{StaticResource NavButtonActiveStyle}">
    <StackPanel Orientation="Horizontal">
        <TextBlock Text="📊" Margin="0,0,8,0"/>
        <TextBlock Text="Dashboard"/>
    </StackPanel>
</Button>
```
- Fond: Indigo clair (#EEF2FF)
- Texte: Indigo (#6366F1)
- Bordure gauche: 3px indigo

---

## 📏 Espacements

### Système Basé sur 8px
```
8px  → Espacement minimal
16px → Espacement entre éléments proches
24px → Espacement entre sections
32px → Espacement entre groupes importants
40px → Espacement entre sections majeures
48px → Espacement maximum
```

### Marges Standards
- **Padding des conteneurs**: 24-32px
- **Margin entre cartes**: 16px
- **Margin entre sections**: 32-40px
- **Margin interne des cartes**: 24px

---

## 💡 Exemples d'Utilisation

### Exemple 1: Carte de Statistique KPI
```xaml
<Border Style="{StaticResource StatCardStyle}">
    <StackPanel>
        <!-- Label -->
        <TextBlock Text="VÉHICULES" 
                   Style="{StaticResource CaptionText}"
                   Margin="0,0,0,4"/>
        
        <!-- Valeur -->
        <StackPanel Orientation="Horizontal" Margin="0,0,0,8">
            <TextBlock Text="42" 
                       Style="{StaticResource H2}"
                       Margin="0"/>
            <TextBlock Text="🚗" 
                       FontSize="24"
                       Margin="8,0,0,0"/>
        </StackPanel>
        
        <!-- Tendance -->
        <StackPanel Orientation="Horizontal">
            <TextBlock Text="↗" 
                       Foreground="{StaticResource SuccessBrush}"
                       FontSize="14"
                       Margin="0,0,4,0"/>
            <TextBlock Text="+5% ce mois" 
                       Style="{StaticResource SecondaryText}"/>
        </StackPanel>
    </StackPanel>
</Border>
```

### Exemple 2: Liste de Véhicules avec Badges
```xaml
<Border Style="{StaticResource CardStyle}">
    <!-- En-tête -->
    <Grid Margin="0,0,0,24">
        <TextBlock Text="Parc de Véhicules" Style="{StaticResource H2}"/>
        <Button Content="+ Ajouter" 
                Style="{StaticResource PrimaryButton}"
                HorizontalAlignment="Right"/>
    </Grid>
    
    <!-- DataGrid -->
    <DataGrid ItemsSource="{Binding Vehicles}">
        <DataGrid.Columns>
            <DataGridTextColumn Header="Immatriculation" 
                              Binding="{Binding RegistrationNumber}"
                              Width="150"/>
            
            <DataGridTemplateColumn Header="Statut" Width="120">
                <DataGridTemplateColumn.CellTemplate>
                    <DataTemplate>
                        <Border Style="{StaticResource SuccessBadge}">
                            <TextBlock Text="{Binding Status}" 
                                     Style="{StaticResource SuccessBadgeText}"/>
                        </Border>
                    </DataTemplate>
                </DataGridTemplateColumn.CellTemplate>
            </DataGridTemplateColumn>
            
            <!-- Autres colonnes... -->
        </DataGrid.Columns>
    </DataGrid>
</Border>
```

### Exemple 3: Formulaire Moderne
```xaml
<Border Style="{StaticResource CardStyle}">
    <StackPanel>
        <TextBlock Text="Ajouter un Véhicule" Style="{StaticResource H2}"/>
        
        <!-- Champ Immatriculation -->
        <Label Content="Immatriculation"/>
        <TextBox Text="{Binding RegistrationNumber}"
                 MinHeight="44"
                 Margin="0,0,0,16"/>
        
        <!-- Champ Type -->
        <Label Content="Type de véhicule"/>
        <ComboBox ItemsSource="{Binding VehicleTypes}"
                  SelectedItem="{Binding SelectedType}"
                  MinHeight="44"
                  Margin="0,0,0,24"/>
        
        <!-- Boutons -->
        <StackPanel Orientation="Horizontal" HorizontalAlignment="Right">
            <Button Content="Annuler" 
                    Style="{StaticResource SecondaryButton}"
                    Margin="0,0,8,0"/>
            <Button Content="Enregistrer" 
                    Style="{StaticResource PrimaryButton}"/>
        </StackPanel>
    </StackPanel>
</Border>
```

---

## 🎯 Bonnes Pratiques

### ✅ À FAIRE
- Utiliser les styles prédéfinis pour la cohérence
- Respecter les espacements de 8px
- Utiliser les badges colorés pour les statuts
- Ajouter des ombres aux cartes
- Coins arrondis sur tous les éléments (8-12px)
- Hauteur minimale 44px pour les éléments cliquables
- Transitions fluides (200-300ms)

### ❌ À ÉVITER
- Mélanger les palettes de couleurs
- Utiliser des couleurs pures (trop vives)
- Ombres trop prononcées
- Bordures trop épaisses
- Textes trop petits (<12px)
- Espacements non alignés sur 8px
- Trop de couleurs différentes sur un même écran

---

## 🚀 Application Rapide

Pour appliquer rapidement le nouveau design à vos vues existantes:

1. **Remplacer les références de couleurs**:
   - `#2196F3` → `{StaticResource PrimaryBrush}`
   - `#4CAF50` → `{StaticResource SuccessBrush}`

2. **Mettre à jour les styles de boutons**:
   - Ajouter `Style="{StaticResource PrimaryButton}"`

3. **Moderniser les cartes**:
   - Utiliser `Style="{StaticResource CardStyle}"`
   - Augmenter `CornerRadius` à 12

4. **Ajouter des badges colorés**:
   - Remplacer TextBlock simples par Badge + BadgeText

5. **Améliorer la typographie**:
   - Ajouter les styles H1, H2, H3, H4 sur les titres

---

## 📱 Accessibilité

- **Contraste**: Tous les textes respectent WCAG AA
- **Focus visible**: Bordure indigo 2px
- **Tailles cliquables**: Minimum 44x44px
- **Textes alternatifs**: Prévoir pour images/icônes

---

**Version**: 1.0  
**Date**: Novembre 2025  
**Application**: Fleet Manager  
**Framework**: WPF .NET 8.0
