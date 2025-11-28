# 🎨 Fleet Manager - Maquettes de Design UI/UX

## 📋 Table des Matières
1. [Écran de Connexion](#1-écran-de-connexion)
2. [Tableau de Bord Principal](#2-tableau-de-bord-principal)
3. [Module Gestion des Véhicules](#3-module-gestion-des-véhicules)
4. [Formulaire Ajout/Modification Véhicule](#4-formulaire-ajoutmodification-véhicule)
5. [Module Suivi Carburant](#5-module-suivi-carburant)
6. [Module Kilométrage](#6-module-kilométrage)
7. [Module Rapports et Statistiques](#7-module-rapports-et-statistiques)
8. [Module Utilisateurs (Admin)](#8-module-utilisateurs-admin)

---

## 1. 🔐 ÉCRAN DE CONNEXION

### Layout Structure
```
┌────────────────────────────────────────────────────────────┐
│                                                            │
│                    [FOND GRADIENT]                         │
│                  Indigo → Purple                           │
│                                                            │
│              ┌─────────────────────┐                       │
│              │                     │                       │
│              │  🚗 FLEET MANAGER   │                       │
│              │                     │                       │
│              │  📧 [Email______]   │                       │
│              │                     │                       │
│              │  🔒 [Password___]   │                       │
│              │                     │                       │
│              │  ☑ Se souvenir      │                       │
│              │     Mot de passe    │                       │
│              │         oublié?     │                       │
│              │                     │                       │
│              │  [SE CONNECTER]     │                       │
│              │                     │                       │
│              └─────────────────────┘                       │
│                                                            │
└────────────────────────────────────────────────────────────┘
```

### Spécifications Détaillées

#### Fond d'écran
```xaml
<Grid>
    <Grid.Background>
        <LinearGradientBrush StartPoint="0,0" EndPoint="1,1">
            <GradientStop Color="#6366F1" Offset="0"/>
            <GradientStop Color="#8B5CF6" Offset="0.5"/>
            <GradientStop Color="#EC4899" Offset="1"/>
        </LinearGradientBrush>
    </Grid.Background>
</Grid>
```

#### Carte de Connexion
- **Dimensions** : 440px largeur × 560px hauteur
- **Position** : Centré verticalement et horizontalement
- **Fond** : Blanc (#FFFFFF) avec opacité 95%
- **Border Radius** : 24px
- **Shadow** : `BlurRadius="32" ShadowDepth="8" Opacity="0.15"`
- **Padding** : 48px

#### Logo
- **Type** : Icône 🚗 + Texte "FLEET MANAGER"
- **Police** : 28px, Bold
- **Couleur** : #6366F1 (Indigo)
- **Margin Bottom** : 40px

#### Champs de Formulaire

**Email Input** :
```xaml
<Border CornerRadius="12" Background="White" BorderBrush="#E5E7EB" BorderThickness="2">
    <Grid>
        <TextBlock Text="📧" FontSize="20" Margin="16,0,0,0"/>
        <TextBox PlaceholderText="adresse@email.com" 
                 Padding="48,14,16,14"
                 FontSize="14"
                 BorderThickness="0"/>
    </Grid>
</Border>
```
- **Hauteur** : 52px
- **Icône** : 📧 (20px, marge gauche 16px)
- **Placeholder** : "adresse@email.com"
- **Margin Bottom** : 20px

**Password Input** :
```xaml
<Border CornerRadius="12" Background="White" BorderBrush="#E5E7EB" BorderThickness="2">
    <Grid>
        <TextBlock Text="🔒" FontSize="20" Margin="16,0,0,0"/>
        <PasswordBox Padding="48,14,16,14"
                     FontSize="14"
                     BorderThickness="0"/>
        <Button Content="👁" Style="{StaticResource TertiaryButton}" 
                HorizontalAlignment="Right"/>
    </Grid>
</Border>
```
- **Hauteur** : 52px
- **Icône** : 🔒 (20px)
- **Bouton "Afficher"** : 👁 à droite
- **Margin Bottom** : 16px

#### Options
```xaml
<Grid Margin="0,0,0,24">
    <CheckBox Content="Se souvenir de moi" 
              FontSize="13"
              HorizontalAlignment="Left"/>
    <Button Content="Mot de passe oublié?" 
            Style="{StaticResource TertiaryButton}"
            FontSize="13"
            Foreground="{StaticResource PrimaryBrush}"
            HorizontalAlignment="Right"/>
</Grid>
```

#### Bouton de Connexion
```xaml
<Button Content="SE CONNECTER"
        Style="{StaticResource PrimaryButton}"
        FontSize="15"
        FontWeight="SemiBold"
        Height="52"
        Width="100%"
        Margin="0,24,0,0"/>
```
- **Largeur** : 100% (pleine largeur)
- **Hauteur** : 52px
- **Texte** : "SE CONNECTER" (majuscules)
- **Shadow au hover** : Plus prononcée

#### États Visuels

**État Normal** :
- Bordure : #E5E7EB (2px)
- Fond : Blanc

**État Focus** :
- Bordure : #6366F1 (2px)
- Shadow : 0 0 0 4px rgba(99, 102, 241, 0.1)

**État Erreur** :
- Bordure : #EF4444 (2px)
- Message sous le champ : "Email ou mot de passe incorrect"
- Couleur message : #EF4444

---

## 2. 📊 TABLEAU DE BORD PRINCIPAL

### Layout Structure
```
┌─────────────────────────────────────────────────────────────────┐
│ 🚗 Fleet    [Recherche_______________]  🔔(3)  👤 John Doe   ▼ │ Header
├──────┬──────────────────────────────────────────────────────────┤
│      │                                                          │
│ 📊   │  ┌──────┐ ┌──────┐ ┌──────┐ ┌──────┐ ┌──────┐ ┌──────┐│ KPI Cards
│ 🚗   │  │  42  │ │1,250L│ │95,420│ │2,847€│ │38/42 │ │  5   ││
│ ⛽   │  │Véhic.│ │Carbu.│ │  km  │ │Coûts │ │Dispo.│ │Alert.││
│ 📈   │  └──────┘ └──────┘ └──────┘ └──────┘ └──────┘ └──────┘│
│ 👥   │                                                          │
│ ⚙️   │  ┌────────────────────┐ ┌──────────────────────────┐   │ Charts
│      │  │                    │ │                          │   │
│      │  │  Graphique Ligne   │ │   Graphique Barres       │   │
│      │  │  (Consommation)    │ │   (Coûts/Véhicule)       │   │
│      │  │                    │ │                          │   │
│      │  └────────────────────┘ └──────────────────────────┘   │
│      │                                                          │
│      │  ┌──────────────────────────────────────────────────┐   │ Activity
│      │  │  Activités Récentes                              │   │
│      │  │  • VH-001 - Plein carburant - Il y a 2h         │   │
│      │  │  • VH-012 - Maintenance - Hier                   │   │
│      │  └──────────────────────────────────────────────────┘   │
└──────┴──────────────────────────────────────────────────────────┘
```

### Spécifications Détaillées

#### Sidebar Navigation (260px largeur)
```xaml
<Border Background="{StaticResource SurfaceBrush}" 
        BorderBrush="{StaticResource BorderBrush}"
        BorderThickness="0,0,1,0">
    <StackPanel>
        <!-- Logo -->
        <StackPanel Orientation="Horizontal" Padding="24,20">
            <TextBlock Text="🚗" FontSize="28"/>
            <TextBlock Text="Fleet Manager" 
                       FontSize="18" 
                       FontWeight="Bold"
                       Foreground="{StaticResource PrimaryBrush}"
                       Margin="12,0,0,0"/>
        </StackPanel>
        
        <!-- Menu Items -->
        <Button Style="{StaticResource NavButtonActiveStyle}">
            <StackPanel Orientation="Horizontal">
                <TextBlock Text="📊" FontSize="20" Margin="0,0,12,0"/>
                <TextBlock Text="Dashboard" FontSize="14"/>
            </StackPanel>
        </Button>
        
        <Button Style="{StaticResource NavButtonStyle}">
            <StackPanel Orientation="Horizontal">
                <TextBlock Text="🚗" FontSize="20" Margin="0,0,12,0"/>
                <TextBlock Text="Véhicules" FontSize="14"/>
            </StackPanel>
        </Button>
        
        <!-- ... autres items ... -->
    </StackPanel>
</Border>
```

**Items de Navigation** :
- **Hauteur** : 48px chacun
- **Padding** : 16,12px
- **Icône** : 20px, marge droite 12px
- **Texte** : 14px, Medium
- **Bordure gauche active** : 3px indigo
- **Fond actif** : #EEF2FF
- **Fond hover** : #F3F4F6

#### Header (Hauteur 72px)
```xaml
<Grid Background="{StaticResource SurfaceBrush}"
      Height="72"
      Padding="32,0">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="*"/>
        <ColumnDefinition Width="Auto"/>
        <ColumnDefinition Width="Auto"/>
        <ColumnDefinition Width="Auto"/>
    </Grid.ColumnDefinitions>
    
    <!-- Barre de recherche -->
    <Border Grid.Column="0" 
            CornerRadius="10"
            Background="#F9FAFB"
            MaxWidth="480">
        <TextBox PlaceholderText="Rechercher un véhicule, une plaque..."
                 Background="Transparent"
                 BorderThickness="0"
                 Padding="16,12"/>
    </Border>
    
    <!-- Notifications -->
    <Button Grid.Column="1" Style="{StaticResource TertiaryButton}">
        <Grid>
            <TextBlock Text="🔔" FontSize="24"/>
            <Border Background="{StaticResource DangerBrush}"
                    CornerRadius="10"
                    Width="20" Height="20"
                    VerticalAlignment="Top"
                    HorizontalAlignment="Right"
                    Margin="-8,-8,0,0">
                <TextBlock Text="3" 
                           FontSize="11" 
                           FontWeight="Bold"
                           Foreground="White"
                           HorizontalAlignment="Center"
                           VerticalAlignment="Center"/>
            </Border>
        </Grid>
    </Button>
    
    <!-- Profil -->
    <Button Grid.Column="2" Style="{StaticResource TertiaryButton}">
        <StackPanel Orientation="Horizontal">
            <Border Width="36" Height="36" 
                    CornerRadius="18"
                    Background="{StaticResource PrimaryBrush}">
                <TextBlock Text="JD" 
                           Foreground="White"
                           HorizontalAlignment="Center"
                           VerticalAlignment="Center"/>
            </Border>
            <StackPanel Margin="12,0,8,0">
                <TextBlock Text="John Doe" FontSize="14" FontWeight="Medium"/>
                <TextBlock Text="Administrateur" FontSize="12" 
                           Foreground="{StaticResource TextSecondaryBrush}"/>
            </StackPanel>
            <TextBlock Text="▼" FontSize="10" VerticalAlignment="Center"/>
        </StackPanel>
    </Button>
</Grid>
```

#### KPI Cards (6 cartes)
```xaml
<Grid Margin="32,24,32,0">
    <Grid.ColumnDefinitions>
        <ColumnDefinition/>
        <ColumnDefinition/>
        <ColumnDefinition/>
        <ColumnDefinition/>
        <ColumnDefinition/>
        <ColumnDefinition/>
    </Grid.ColumnDefinitions>
    
    <!-- Carte 1 : Total Véhicules -->
    <Border Grid.Column="0" Style="{StaticResource StatCardStyle}">
        <StackPanel>
            <Grid Margin="0,0,0,12">
                <TextBlock Text="VÉHICULES" 
                           Style="{StaticResource CaptionText}"/>
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
                           Style="{StaticResource SecondaryText}"/>
            </StackPanel>
        </StackPanel>
    </Border>
    
    <!-- Carte 2 : Carburant -->
    <Border Grid.Column="1" Style="{StaticResource StatCardStyle}">
        <StackPanel>
            <Grid Margin="0,0,0,12">
                <TextBlock Text="CARBURANT" Style="{StaticResource CaptionText}"/>
                <TextBlock Text="⛽" FontSize="28" HorizontalAlignment="Right"/>
            </Grid>
            <StackPanel Orientation="Horizontal" VerticalAlignment="Center">
                <TextBlock Text="1,250" FontSize="32" FontWeight="Bold"/>
                <TextBlock Text=" L" FontSize="20" FontWeight="Medium" 
                           Foreground="{StaticResource TextSecondaryBrush}"
                           VerticalAlignment="Bottom"
                           Margin="4,0,0,6"/>
            </StackPanel>
            <TextBlock Text="Ce mois" 
                       Style="{StaticResource SecondaryText}"
                       Margin="0,8,0,0"/>
        </StackPanel>
    </Border>
    
    <!-- ... autres cartes similaires ... -->
</Grid>
```

**Dimensions KPI Card** :
- **Largeur** : Flexible (1/6 de la largeur)
- **Hauteur** : 140px
- **Padding** : 20px
- **Border Radius** : 12px
- **Margin entre cartes** : 16px

**Structure interne** :
1. Label + Icône (en haut)
2. Valeur principale (grande, bold)
3. Texte secondaire / Tendance (en bas)

#### Graphiques
```xaml
<Grid Margin="32,24,32,0">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="2*"/>
        <ColumnDefinition Width="20"/>
        <ColumnDefinition Width="3*"/>
    </Grid.ColumnDefinitions>
    
    <!-- Graphique Ligne -->
    <Border Grid.Column="0" Style="{StaticResource CardStyle}">
        <StackPanel>
            <TextBlock Text="Consommation Mensuelle" Style="{StaticResource H4}"/>
            <TextBlock Text="Évolution sur 12 mois" 
                       Style="{StaticResource SecondaryText}"
                       Margin="0,0,0,16"/>
            <!-- LiveCharts ici -->
        </StackPanel>
    </Border>
    
    <!-- Graphique Barres -->
    <Border Grid.Column="2" Style="{StaticResource CardStyle}">
        <StackPanel>
            <TextBlock Text="Coûts par Véhicule" Style="{StaticResource H4}"/>
            <TextBlock Text="Top 10 véhicules" 
                       Style="{StaticResource SecondaryText}"
                       Margin="0,0,0,16"/>
            <!-- LiveCharts ici -->
        </StackPanel>
    </Border>
</Grid>
```

**Dimensions Graphiques** :
- **Hauteur** : 320px
- **Graphique Ligne** : 40% largeur
- **Graphique Barres** : 60% largeur
- **Spacing** : 20px entre les deux

#### Tableau Activités Récentes
```xaml
<Border Style="{StaticResource CardStyle}" Margin="32,24,32,32">
    <StackPanel>
        <Grid Margin="0,0,0,16">
            <TextBlock Text="Activités Récentes" Style="{StaticResource H3}"/>
            <Button Content="Voir tout" 
                    Style="{StaticResource TertiaryButton}"
                    HorizontalAlignment="Right"/>
        </Grid>
        
        <StackPanel>
            <!-- Item 1 -->
            <Grid Height="64" Margin="0,0,0,1">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="Auto"/>
                    <ColumnDefinition Width="*"/>
                    <ColumnDefinition Width="Auto"/>
                </Grid.ColumnDefinitions>
                
                <Border Width="48" Height="48" 
                        CornerRadius="24"
                        Background="#F0F9FF">
                    <TextBlock Text="⛽" 
                               FontSize="24"
                               HorizontalAlignment="Center"
                               VerticalAlignment="Center"/>
                </Border>
                
                <StackPanel Grid.Column="1" Margin="16,0" VerticalAlignment="Center">
                    <TextBlock Text="Plein de carburant - VH-001" 
                               FontSize="14" 
                               FontWeight="Medium"/>
                    <TextBlock Text="42.5L • Station Total • 78.50€" 
                               Style="{StaticResource SecondaryText}"
                               Margin="0,4,0,0"/>
                </StackPanel>
                
                <TextBlock Grid.Column="2" 
                           Text="Il y a 2h"
                           Style="{StaticResource SecondaryText}"
                           VerticalAlignment="Center"/>
            </Grid>
            
            <!-- ... autres items ... -->
        </StackPanel>
    </StackPanel>
</Border>
```

---

## 3. 🚗 MODULE GESTION DES VÉHICULES

### Layout Structure
```
┌─────────────────────────────────────────────────────────────────┐
│  Parc de Véhicules                        [+ Ajouter Véhicule] │ Header
├─────────────────────────────────────────────────────────────────┤
│  [Type ▼] [Statut ▼] [Marque ▼] [Année ▼]    [Rechercher___] │ Filters
├─────────────────────────────────────────────────────────────────┤
│ ┌─────────────────────────────────────────────────────────────┐│
│ │ 🚗  VH-001 | Renault Clio | 2022 | [Disponible] | ... [⚙]││ Row 1
│ ├─────────────────────────────────────────────────────────────┤│
│ │ 🚙  VH-002 | Peugeot 308  | 2021 | [En service]| ... [⚙]││ Row 2
│ ├─────────────────────────────────────────────────────────────┤│
│ │ 🚐  VH-003 | Citroën Berl.| 2023 | [Maintenance]|... [⚙]││ Row 3
│ └─────────────────────────────────────────────────────────────┘│
│                                                                 │
│                     [← 1 2 3 ... 10 →]                         │ Pagination
└─────────────────────────────────────────────────────────────────┘
```

### Spécifications Détaillées

#### Header Section
```xaml
<Grid Padding="32,24,32,24">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="*"/>
        <ColumnDefinition Width="Auto"/>
    </Grid.ColumnDefinitions>
    
    <StackPanel Grid.Column="0">
        <TextBlock Text="Parc de Véhicules" Style="{StaticResource H1}"/>
        <TextBlock Text="Gérez l'ensemble de votre flotte de véhicules" 
                   Style="{StaticResource SecondaryText}"/>
    </StackPanel>
    
    <Button Grid.Column="1" 
            Content="+ Ajouter Véhicule"
            Style="{StaticResource PrimaryButton}"
            FontSize="14"
            Height="48"
            Padding="24,0"
            VerticalAlignment="Center"/>
</Grid>
```

#### Barre de Filtres
```xaml
<Border Background="{StaticResource SurfaceBrush}" 
        BorderBrush="{StaticResource BorderBrush}"
        BorderThickness="0,1"
        Padding="32,16">
    <Grid>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="Auto"/>
            <ColumnDefinition Width="Auto"/>
            <ColumnDefinition Width="Auto"/>
            <ColumnDefinition Width="Auto"/>
            <ColumnDefinition Width="*"/>
            <ColumnDefinition Width="Auto"/>
        </Grid.ColumnDefinitions>
        
        <!-- Type -->
        <ComboBox Grid.Column="0" 
                  PlaceholderText="Type"
                  MinWidth="140"
                  Margin="0,0,12,0"/>
        
        <!-- Statut -->
        <ComboBox Grid.Column="1" 
                  PlaceholderText="Statut"
                  MinWidth="140"
                  Margin="0,0,12,0"/>
        
        <!-- Marque -->
        <ComboBox Grid.Column="2" 
                  PlaceholderText="Marque"
                  MinWidth="140"
                  Margin="0,0,12,0"/>
        
        <!-- Année -->
        <ComboBox Grid.Column="3" 
                  PlaceholderText="Année"
                  MinWidth="120"
                  Margin="0,0,12,0"/>
        
        <!-- Recherche -->
        <Border Grid.Column="5" 
                CornerRadius="8"
                Background="#F9FAFB"
                Width="280">
            <Grid>
                <TextBlock Text="🔍" 
                           FontSize="18"
                           Margin="12,0,0,0"
                           VerticalAlignment="Center"/>
                <TextBox PlaceholderText="Rechercher..."
                         Background="Transparent"
                         BorderThickness="0"
                         Padding="40,10,12,10"/>
            </Grid>
        </Border>
    </Grid>
</Border>
```

#### Tableau des Véhicules
```xaml
<Border Style="{StaticResource CardStyle}" Margin="32,24,32,24">
    <DataGrid ItemsSource="{Binding Vehicles}" 
              RowHeight="72">
        <DataGrid.Columns>
            <!-- Image -->
            <DataGridTemplateColumn Width="80">
                <DataGridTemplateColumn.CellTemplate>
                    <DataTemplate>
                        <Border Width="56" Height="56" 
                                CornerRadius="8"
                                Background="#F3F4F6">
                            <TextBlock Text="🚗" 
                                       FontSize="32"
                                       HorizontalAlignment="Center"
                                       VerticalAlignment="Center"/>
                        </Border>
                    </DataTemplate>
                </DataGridTemplateColumn.CellTemplate>
            </DataGridTemplateColumn>
            
            <!-- Immatriculation -->
            <DataGridTemplateColumn Header="IMMATRICULATION" Width="160">
                <DataGridTemplateColumn.CellTemplate>
                    <DataTemplate>
                        <Border Style="{StaticResource PrimaryBadge}">
                            <TextBlock Text="{Binding RegistrationNumber}" 
                                       Style="{StaticResource PrimaryBadgeText}"
                                       FontWeight="Bold"
                                       FontSize="13"/>
                        </Border>
                    </DataTemplate>
                </DataGridTemplateColumn.CellTemplate>
            </DataGridTemplateColumn>
            
            <!-- Véhicule -->
            <DataGridTemplateColumn Header="VÉHICULE" Width="*">
                <DataGridTemplateColumn.CellTemplate>
                    <DataTemplate>
                        <StackPanel>
                            <TextBlock Text="{Binding DisplayName}" 
                                       FontSize="14" 
                                       FontWeight="Medium"/>
                            <TextBlock Text="{Binding Model}" 
                                       Style="{StaticResource SecondaryText}"
                                       Margin="0,4,0,0"/>
                        </StackPanel>
                    </DataTemplate>
                </DataGridTemplateColumn.CellTemplate>
            </DataGridTemplateColumn>
            
            <!-- Année -->
            <DataGridTextColumn Header="ANNÉE" 
                              Binding="{Binding Year}"
                              Width="100"/>
            
            <!-- Kilométrage -->
            <DataGridTemplateColumn Header="KILOMÉTRAGE" Width="120">
                <DataGridTemplateColumn.CellTemplate>
                    <DataTemplate>
                        <TextBlock>
                            <Run Text="{Binding CurrentMileage, StringFormat=N0}"/>
                            <Run Text=" km" 
                                 Foreground="{StaticResource TextSecondaryBrush}"/>
                        </TextBlock>
                    </DataTemplate>
                </DataGridTemplateColumn.CellTemplate>
            </DataGridTemplateColumn>
            
            <!-- Statut -->
            <DataGridTemplateColumn Header="STATUT" Width="140">
                <DataGridTemplateColumn.CellTemplate>
                    <DataTemplate>
                        <Border Style="{StaticResource SuccessBadge}">
                            <TextBlock Text="{Binding Status}" 
                                       Style="{StaticResource SuccessBadgeText}"/>
                        </Border>
                    </DataTemplate>
                </DataGridTemplateColumn.CellTemplate>
            </DataGridTemplateColumn>
            
            <!-- Actions -->
            <DataGridTemplateColumn Width="120">
                <DataGridTemplateColumn.CellTemplate>
                    <DataTemplate>
                        <StackPanel Orientation="Horizontal" 
                                    HorizontalAlignment="Right">
                            <Button Style="{StaticResource TertiaryButton}"
                                    ToolTip="Voir détails"
                                    Width="36" Height="36"
                                    Margin="0,0,4,0">
                                <TextBlock Text="👁" FontSize="18"/>
                            </Button>
                            <Button Style="{StaticResource TertiaryButton}"
                                    ToolTip="Modifier"
                                    Width="36" Height="36"
                                    Margin="0,0,4,0">
                                <TextBlock Text="✏" FontSize="18"/>
                            </Button>
                            <Button Style="{StaticResource TertiaryButton}"
                                    ToolTip="Supprimer"
                                    Width="36" Height="36">
                                <TextBlock Text="🗑" FontSize="18"/>
                            </Button>
                        </StackPanel>
                    </DataTemplate>
                </DataGridTemplateColumn.CellTemplate>
            </DataGridTemplateColumn>
        </DataGrid.Columns>
    </DataGrid>
</Border>
```

**Spécifications Ligne** :
- **Hauteur** : 72px
- **Image véhicule** : 56×56px, border radius 8px
- **Hover** : Fond #F0F9FF
- **Sélection** : Fond #DBEAFE

#### Pagination
```xaml
<StackPanel Orientation="Horizontal" 
            HorizontalAlignment="Center"
            Margin="0,0,0,32">
    <Button Content="←" Style="{StaticResource SecondaryButton}"
            Width="40" Height="40" Margin="0,0,8,0"/>
    <Button Content="1" Style="{StaticResource PrimaryButton}"
            Width="40" Height="40" Margin="0,0,4,0"/>
    <Button Content="2" Style="{StaticResource TertiaryButton}"
            Width="40" Height="40" Margin="0,0,4,0"/>
    <Button Content="3" Style="{StaticResource TertiaryButton}"
            Width="40" Height="40" Margin="0,0,4,0"/>
    <TextBlock Text="..." VerticalAlignment="Center" Margin="8,0"/>
    <Button Content="10" Style="{StaticResource TertiaryButton}"
            Width="40" Height="40" Margin="4,0,8,0"/>
    <Button Content="→" Style="{StaticResource SecondaryButton}"
            Width="40" Height="40"/>
</StackPanel>
```

---

## 4. ➕ FORMULAIRE AJOUT/MODIFICATION VÉHICULE

### Layout Structure (Modal)
```
┌─────────────────────────────────────────────────────────────────┐
│  Ajouter un Véhicule                                         [×]│ Header
├─────────────────────────────────────────────────────────────────┤
│  ┌─────────────────────────────────────────────────────────┐   │
│  │ 📝 Informations Générales                               │   │ Section 1
│  │                                                          │   │
│  │  [Marque ▼]        [Modèle________]                     │   │
│  │  [Année ▼]         [Immatriculation]                    │   │
│  │  [Type ▼]          [Couleur_______]                     │   │
│  └─────────────────────────────────────────────────────────┘   │
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │ ⚙️ Caractéristiques Techniques                          │   │ Section 2
│  │                                                          │   │
│  │  [Carburant ▼]     [Capacité____L]                      │   │
│  │  [Consommation_L]  [Puissance__ch]                      │   │
│  └─────────────────────────────────────────────────────────┘   │
│                                                                 │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │ 📊 Données Opérationnelles                              │   │ Section 3
│  │                                                          │   │
│  │  [Date acquisition] [Km initial___]                     │   │
│  │  [Km actuel______]  [Statut ▼]                          │   │
│  └─────────────────────────────────────────────────────────┘   │
│                                                                 │
│                          [Annuler] [Enregistrer]                │ Actions
└─────────────────────────────────────────────────────────────────┘
```

### Spécifications Détaillées

#### Modal Container
```xaml
<Border Background="#80000000" HorizontalAlignment="Stretch" VerticalAlignment="Stretch">
    <Border Background="{StaticResource SurfaceBrush}"
            Width="800"
            MaxHeight="720"
            CornerRadius="16"
            VerticalAlignment="Center">
        <Border.Effect>
            <DropShadowEffect BlurRadius="40" ShadowDepth="8" Opacity="0.2"/>
        </Border.Effect>
        
        <!-- Contenu -->
    </Border>
</Border>
```

**Dimensions** :
- **Largeur** : 800px
- **Hauteur max** : 720px (scrollable si nécessaire)
- **Border Radius** : 16px
- **Shadow** : Plus prononcée que les cartes

#### Header Modal
```xaml
<Grid Height="72" Padding="32,0" 
      BorderBrush="{StaticResource BorderBrush}"
      BorderThickness="0,0,0,1">
    <TextBlock Text="Ajouter un Véhicule" Style="{StaticResource H2}"/>
    <Button Content="×" 
            Style="{StaticResource TertiaryButton}"
            FontSize="32"
            Width="48" Height="48"
            HorizontalAlignment="Right"/>
</Grid>
```

#### Sections de Formulaire
```xaml
<ScrollViewer Padding="32,24,32,24">
    <StackPanel>
        <!-- Section 1 : Informations Générales -->
        <Border Style="{StaticResource CardStyle}" Margin="0,0,0,24">
            <StackPanel>
                <StackPanel Orientation="Horizontal" Margin="0,0,0,20">
                    <TextBlock Text="📝" FontSize="24" Margin="0,0,12,0"/>
                    <TextBlock Text="Informations Générales" 
                               Style="{StaticResource H3}"/>
                </StackPanel>
                
                <Grid>
                    <Grid.ColumnDefinitions>
                        <ColumnDefinition Width="*"/>
                        <ColumnDefinition Width="20"/>
                        <ColumnDefinition Width="*"/>
                    </Grid.ColumnDefinitions>
                    <Grid.RowDefinitions>
                        <RowDefinition Height="Auto"/>
                        <RowDefinition Height="16"/>
                        <RowDefinition Height="Auto"/>
                        <RowDefinition Height="16"/>
                        <RowDefinition Height="Auto"/>
                    </Grid.RowDefinitions>
                    
                    <!-- Row 1 : Marque / Modèle -->
                    <StackPanel Grid.Row="0" Grid.Column="0">
                        <Label Content="Marque *"/>
                        <ComboBox ItemsSource="{Binding Brands}"
                                  SelectedItem="{Binding SelectedBrand}"/>
                    </StackPanel>
                    
                    <StackPanel Grid.Row="0" Grid.Column="2">
                        <Label Content="Modèle *"/>
                        <TextBox Text="{Binding Model}"/>
                    </StackPanel>
                    
                    <!-- Row 2 : Année / Immatriculation -->
                    <StackPanel Grid.Row="2" Grid.Column="0">
                        <Label Content="Année *"/>
                        <ComboBox ItemsSource="{Binding Years}"
                                  SelectedItem="{Binding Year}"/>
                    </StackPanel>
                    
                    <StackPanel Grid.Row="2" Grid.Column="2">
                        <Label Content="Immatriculation *"/>
                        <TextBox Text="{Binding RegistrationNumber}"
                                 TextChanged="ValidateRegistration"/>
                        <TextBlock Text="Format : AB-123-CD"
                                   Style="{StaticResource SecondaryText}"
                                   Margin="0,4,0,0"/>
                    </StackPanel>
                    
                    <!-- Row 3 : Type / Couleur -->
                    <StackPanel Grid.Row="4" Grid.Column="0">
                        <Label Content="Type de véhicule *"/>
                        <ComboBox ItemsSource="{Binding VehicleTypes}"
                                  SelectedItem="{Binding VehicleType}"/>
                    </StackPanel>
                    
                    <StackPanel Grid.Row="4" Grid.Column="2">
                        <Label Content="Couleur"/>
                        <TextBox Text="{Binding Color}"/>
                    </StackPanel>
                </Grid>
            </StackPanel>
        </Border>
        
        <!-- Section 2 : Caractéristiques Techniques -->
        <Border Style="{StaticResource CardStyle}" Margin="0,0,0,24">
            <StackPanel>
                <StackPanel Orientation="Horizontal" Margin="0,0,0,20">
                    <TextBlock Text="⚙️" FontSize="24" Margin="0,0,12,0"/>
                    <TextBlock Text="Caractéristiques Techniques" 
                               Style="{StaticResource H3}"/>
                </StackPanel>
                
                <!-- Grille similaire avec champs techniques -->
            </StackPanel>
        </Border>
        
        <!-- Section 3 : Données Opérationnelles -->
        <Border Style="{StaticResource CardStyle}">
            <StackPanel>
                <StackPanel Orientation="Horizontal" Margin="0,0,0,20">
                    <TextBlock Text="📊" FontSize="24" Margin="0,0,12,0"/>
                    <TextBlock Text="Données Opérationnelles" 
                               Style="{StaticResource H3}"/>
                </StackPanel>
                
                <!-- Grille avec champs opérationnels -->
            </StackPanel>
        </Border>
    </StackPanel>
</ScrollViewer>
```

**Champs avec Validation** :

**État Normal** :
```xaml
<TextBox Text="{Binding Value}"/>
```

**État Valide** :
```xaml
<Border BorderBrush="{StaticResource SuccessBrush}" BorderThickness="2">
    <TextBox Text="{Binding Value}"/>
</Border>
<StackPanel Orientation="Horizontal" Margin="0,4,0,0">
    <TextBlock Text="✓" Foreground="{StaticResource SuccessBrush}"/>
    <TextBlock Text="Valide" 
               Foreground="{StaticResource SuccessBrush}"
               Margin="4,0,0,0"/>
</StackPanel>
```

**État Erreur** :
```xaml
<Border BorderBrush="{StaticResource DangerBrush}" BorderThickness="2">
    <TextBox Text="{Binding Value}"/>
</Border>
<StackPanel Orientation="Horizontal" Margin="0,4,0,0">
    <TextBlock Text="✕" Foreground="{StaticResource DangerBrush}"/>
    <TextBlock Text="Format invalide" 
               Foreground="{StaticResource DangerBrush}"
               Margin="4,0,0,0"/>
</StackPanel>
```

#### Footer Actions
```xaml
<Grid Height="80" 
      Padding="32,0"
      BorderBrush="{StaticResource BorderBrush}"
      BorderThickness="0,1,0,0">
    <StackPanel Orientation="Horizontal" HorizontalAlignment="Right">
        <Button Content="Annuler" 
                Style="{StaticResource SecondaryButton}"
                Width="120" Height="48"
                Margin="0,0,12,0"/>
        <Button Content="Enregistrer" 
                Style="{StaticResource PrimaryButton}"
                Width="140" Height="48"/>
    </StackPanel>
</Grid>
```

---

## 5. ⛽ MODULE SUIVI CARBURANT

### Layout Structure
```
┌─────────────────────────────────────────────────────────────────┐
│  Suivi de Carburant                       [+ Nouvelle Entrée]  │
├─────────────────────────────────────────────────────────────────┤
│  [Période: ___→___] [Véhicule ▼] [Type ▼]                     │ Filters
├─────────────────────────────────────────────────────────────────┤
│  ┌──────┐ ┌──────┐ ┌──────┐ ┌──────┐                          │ KPI Cards
│  │2,847€│ │1,250L│ │  7.2 │ │ 42   │                          │
│  │Total │ │Litres│ │L/100 │ │Pleins│                          │
│  └──────┘ └──────┘ └──────┘ └──────┘                          │
├─────────────────────────────────────────────────────────────────┤
│  ┌─────────────────────────────────────────────────────────┐   │
│  │         Graphique Évolution Coûts                       │   │ Chart
│  │                                                          │   │
│  └─────────────────────────────────────────────────────────┘   │
├─────────────────────────────────────────────────────────────────┤
│  Historique des Pleins                                          │
│  ┌─────────────────────────────────────────────────────────┐   │
│  │ 27/11 | VH-001 | 42.5L | 1.85€/L | 78.62€ | Station... │   │ Table
│  │ 26/11 | VH-012 | 38.0L | 1.87€/L | 71.06€ | Station... │   │
│  └─────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────┘
```

Structure similaire aux véhicules avec :
- KPI Cards spécifiques au carburant
- Graphique temporel d'évolution
- Tableau détaillé des entrées
- Actions (Modifier/Supprimer) par ligne

---

## 6. 📏 MODULE KILOMÉTRAGE

Structure similaire au module Carburant avec :
- KPI : Km total, Km moyen/jour, Distance parcourue période
- Graphique évolution kilométrage
- Tableau trajets avec origine/destination
- Calcul automatique distances

---

## 7. 📊 MODULE RAPPORTS ET STATISTIQUES

### Layout avec 3 Zones

```
┌─────────────────────────────────────────────────────────────────┐
│  Rapports & Statistiques                                        │
├────────┬────────────────────────────────────────────────────────┤
│        │  ┌─────────────────────────────────────────────────┐  │
│ PANEL  │  │                                                 │  │
│ CONFIG │  │         Graphiques Comparatifs                  │  │
│        │  │                                                 │  │
│ Period │  └─────────────────────────────────────────────────┘  │
│ [____] │                                                        │
│        │  ┌──────┐ ┌──────┐ ┌──────┐ ┌──────┐                │
│ Metric │  │ KPI  │ │ KPI  │ │ KPI  │ │ KPI  │                │
│ [____] │  └──────┘ └──────┘ └──────┘ └──────┘                │
│        │                                                        │
│ Vehicl │  [📥 PDF] [📥 Excel] [🖨 Imprimer]                   │
│ [____] │                                                        │
└────────┴────────────────────────────────────────────────────────┘
```

Panel gauche pour configuration, zone droite pour visualisation et export.

---

## 8. 👥 MODULE UTILISATEURS (ADMIN)

Structure similaire aux véhicules :
- Tableau avec Avatar + Nom + Email + Rôle + Statut
- Badges colorés pour rôles (Admin = Indigo, User = Gris)
- Badges pour statut (Actif = Vert, Inactif = Gris)
- Actions : Modifier, Désactiver, Supprimer

---

## 🎨 COMPOSANTS RÉUTILISABLES

### 1. Bouton avec Icône
```xaml
<Button Style="{StaticResource PrimaryButton}">
    <StackPanel Orientation="Horizontal">
        <TextBlock Text="➕" FontSize="18" Margin="0,0,8,0"/>
        <TextBlock Text="Ajouter"/>
    </StackPanel>
</Button>
```

### 2. Badge de Statut
```xaml
<Border Style="{StaticResource SuccessBadge}">
    <StackPanel Orientation="Horizontal">
        <Ellipse Width="8" Height="8" 
                 Fill="{StaticResource SuccessBrush}"
                 Margin="0,0,6,0"/>
        <TextBlock Text="Disponible" Style="{StaticResource SuccessBadgeText}"/>
    </StackPanel>
</Border>
```

### 3. Loader / Spinner
```xaml
<Grid Background="#40000000">
    <Border Width="80" Height="80"
            Background="{StaticResource SurfaceBrush}"
            CornerRadius="12">
        <StackPanel HorizontalAlignment="Center" VerticalAlignment="Center">
            <ProgressRing IsActive="True" Width="40" Height="40"/>
            <TextBlock Text="Chargement..." 
                       Style="{StaticResource SecondaryText}"
                       Margin="0,12,0,0"/>
        </StackPanel>
    </Border>
</Grid>
```

### 4. Toast Notification
```xaml
<Border Background="{StaticResource SuccessBrush}"
        CornerRadius="8"
        Padding="16,12"
        HorizontalAlignment="Right"
        VerticalAlignment="Top"
        Margin="0,24,24,0">
    <Border.Effect>
        <DropShadowEffect BlurRadius="16" ShadowDepth="4" Opacity="0.2"/>
    </Border.Effect>
    <StackPanel Orientation="Horizontal">
        <TextBlock Text="✓" Foreground="White" FontSize="18" Margin="0,0,8,0"/>
        <TextBlock Text="Véhicule ajouté avec succès" 
                   Foreground="White" FontSize="14"/>
    </StackPanel>
</Border>
```

---

## 📱 RESPONSIVE BREAKPOINTS

Même si desktop uniquement, prévoir flexibilité :

- **Large (> 1920px)** : Cartes KPI sur 6 colonnes
- **Medium (1280-1920px)** : Cartes KPI sur 3 colonnes (2 lignes)
- **Small (< 1280px)** : Cartes KPI sur 2 colonnes (3 lignes)

---

## 🎬 ANIMATIONS

### Apparition Modal
```xaml
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

### Hover Button
```xaml
<Storyboard>
    <ColorAnimation Storyboard.TargetProperty="(Button.Background).(SolidColorBrush.Color)"
                    To="#4F46E5" Duration="0:0:0.2"/>
</Storyboard>
```

---

**Version** : 1.0  
**Date** : Novembre 2025  
**Application** : Fleet Manager  
**Framework** : WPF .NET 8.0
