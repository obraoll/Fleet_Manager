# 🎬 Fleet Manager - Spécifications d'Interactions et Animations

## 📋 Table des Matières
1. [Principes d'Animation](#principes-danimation)
2. [Transitions Standard](#transitions-standard)
3. [Micro-interactions](#micro-interactions)
4. [Feedback Utilisateur](#feedback-utilisateur)
5. [États des Composants](#états-des-composants)
6. [Skeleton Loaders](#skeleton-loaders)
7. [Flow d'Utilisation](#flow-dutilisation)

---

## 🎨 Principes d'Animation

### Timing et Easing

**Durées Standard** :
- **Ultra rapide** : 100ms → Micro-feedback (hover bouton)
- **Rapide** : 200ms → Transitions UI (changement couleur)
- **Moyen** : 300ms → Animations standard (modals, slides)
- **Lent** : 500ms → Animations complexes (graphiques)

**Easing Functions** :
```csharp
// Ease Out (recommandé pour entrées)
CubicEase { EasingMode = EaseOut }

// Ease In Out (recommandé pour loops)
QuadraticEase { EasingMode = EaseInOut }

// Bounce (pour feedback succès)
BounceEase { EasingMode = EaseOut, Bounces = 2 }
```

### Règles d'Or
1. **Subtilité** : Animations discrètes, jamais distrayantes
2. **Cohérence** : Même timing pour interactions similaires
3. **Performance** : 60 FPS minimum, pas de lag
4. **But** : Chaque animation a un objectif (guider, informer, rassurer)

---

## 🔄 Transitions Standard

### 1. Navigation entre Vues

**Slide avec Fade** :
```xaml
<Storyboard x:Key="PageTransition">
    <!-- Fade Out ancienne vue -->
    <DoubleAnimation Storyboard.TargetName="OldView"
                    Storyboard.TargetProperty="Opacity"
                    From="1" To="0" 
                    Duration="0:0:0.2"/>
    
    <!-- Slide In nouvelle vue -->
    <DoubleAnimation Storyboard.TargetName="NewView"
                    Storyboard.TargetProperty="(UIElement.RenderTransform).(TranslateTransform.X)"
                    From="40" To="0" 
                    Duration="0:0:0.3">
        <DoubleAnimation.EasingFunction>
            <CubicEase EasingMode="EaseOut"/>
        </DoubleAnimation.EasingFunction>
    </DoubleAnimation>
    
    <!-- Fade In nouvelle vue -->
    <DoubleAnimation Storyboard.TargetName="NewView"
                    Storyboard.TargetProperty="Opacity"
                    From="0" To="1" 
                    Duration="0:0:0.3"
                    BeginTime="0:0:0.1"/>
</Storyboard>
```

**Timing** : 300ms  
**Direction** : De droite vers gauche (40px)  
**Quand** : Changement de module dans la navigation

---

### 2. Apparition de Modal/Dialog

**Scale + Fade avec Backdrop** :
```xaml
<Storyboard x:Key="ModalAppear">
    <!-- Backdrop Fade In -->
    <DoubleAnimation Storyboard.TargetName="Backdrop"
                    Storyboard.TargetProperty="Opacity"
                    From="0" To="1" 
                    Duration="0:0:0.2"/>
    
    <!-- Modal Scale + Fade -->
    <DoubleAnimation Storyboard.TargetName="ModalContent"
                    Storyboard.TargetProperty="(UIElement.RenderTransform).(ScaleTransform.ScaleX)"
                    From="0.85" To="1" 
                    Duration="0:0:0.3">
        <DoubleAnimation.EasingFunction>
            <CubicEase EasingMode="EaseOut"/>
        </DoubleAnimation.EasingFunction>
    </DoubleAnimation>
    
    <DoubleAnimation Storyboard.TargetName="ModalContent"
                    Storyboard.TargetProperty="(UIElement.RenderTransform).(ScaleTransform.ScaleY)"
                    From="0.85" To="1" 
                    Duration="0:0:0.3">
        <DoubleAnimation.EasingFunction>
            <CubicEase EasingMode="EaseOut"/>
        </DoubleAnimation.EasingFunction>
    </DoubleAnimation>
    
    <DoubleAnimation Storyboard.TargetName="ModalContent"
                    Storyboard.TargetProperty="Opacity"
                    From="0" To="1" 
                    Duration="0:0:0.3"
                    BeginTime="0:0:0.05"/>
</Storyboard>
```

**Timing** : 300ms  
**Scale** : 0.85 → 1.0  
**Backdrop** : Fade noir 50% opacité  
**Quand** : Ouverture formulaire ajout véhicule, confirmations

---

### 3. Apparition de Cartes au Chargement

**Staggered Fade + Slide Up** :
```xaml
<!-- Card 1 -->
<Storyboard x:Key="Card1Appear">
    <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(TranslateTransform.Y)"
                    From="20" To="0" 
                    Duration="0:0:0.4"
                    BeginTime="0:0:0">
        <DoubleAnimation.EasingFunction>
            <CubicEase EasingMode="EaseOut"/>
        </DoubleAnimation.EasingFunction>
    </DoubleAnimation>
    <DoubleAnimation Storyboard.TargetProperty="Opacity"
                    From="0" To="1" 
                    Duration="0:0:0.4"
                    BeginTime="0:0:0"/>
</Storyboard>

<!-- Card 2 -->
<Storyboard x:Key="Card2Appear">
    <!-- Même animation mais BeginTime="0:0:0.05" -->
</Storyboard>

<!-- Card 3 : BeginTime="0:0:0.1" -->
<!-- Card 4 : BeginTime="0:0:0.15" -->
<!-- etc. -->
```

**Timing** : 400ms par carte  
**Delay** : 50ms entre chaque carte  
**Direction** : Slide de 20px vers le haut  
**Quand** : Chargement page Dashboard avec KPI cards

---

## ⚡ Micro-interactions

### 1. Hover Bouton Primary

```xaml
<ControlTemplate.Triggers>
    <Trigger Property="IsMouseOver" Value="True">
        <!-- Changement couleur background -->
        <Trigger.EnterActions>
            <BeginStoryboard>
                <Storyboard>
                    <ColorAnimation Storyboard.TargetProperty="(Button.Background).(SolidColorBrush.Color)"
                                  To="#4F46E5" 
                                  Duration="0:0:0.15"/>
                    
                    <!-- Élévation ombre -->
                    <DoubleAnimation Storyboard.TargetProperty="(Button.Effect).(DropShadowEffect.BlurRadius)"
                                   To="16" 
                                   Duration="0:0:0.15"/>
                    <DoubleAnimation Storyboard.TargetProperty="(Button.Effect).(DropShadowEffect.ShadowDepth)"
                                   To="6" 
                                   Duration="0:0:0.15"/>
                </Storyboard>
            </BeginStoryboard>
        </Trigger.EnterActions>
        
        <Trigger.ExitActions>
            <BeginStoryboard>
                <Storyboard>
                    <ColorAnimation Storyboard.TargetProperty="(Button.Background).(SolidColorBrush.Color)"
                                  To="#6366F1" 
                                  Duration="0:0:0.15"/>
                    <DoubleAnimation Storyboard.TargetProperty="(Button.Effect).(DropShadowEffect.BlurRadius)"
                                   To="8" 
                                   Duration="0:0:0.15"/>
                    <DoubleAnimation Storyboard.TargetProperty="(Button.Effect).(DropShadowEffect.ShadowDepth)"
                                   To="2" 
                                   Duration="0:0:0.15"/>
                </Storyboard>
            </BeginStoryboard>
        </Trigger.ExitActions>
    </Trigger>
</ControlTemplate.Triggers>
```

**Timing** : 150ms  
**Effet** : Assombrissement + élévation ombre  
**Feedback** : Bouton cliquable confirmé

---

### 2. Hover Ligne DataGrid

```xaml
<Style TargetType="DataGridRow">
    <Style.Triggers>
        <Trigger Property="IsMouseOver" Value="True">
            <Trigger.EnterActions>
                <BeginStoryboard>
                    <Storyboard>
                        <ColorAnimation Storyboard.TargetProperty="(DataGridRow.Background).(SolidColorBrush.Color)"
                                      To="#F0F9FF" 
                                      Duration="0:0:0.1"/>
                    </Storyboard>
                </BeginStoryboard>
            </Trigger.EnterActions>
            <Trigger.ExitActions>
                <BeginStoryboard>
                    <Storyboard>
                        <ColorAnimation Storyboard.TargetProperty="(DataGridRow.Background).(SolidColorBrush.Color)"
                                      To="#FFFFFF" 
                                      Duration="0:0:0.1"/>
                    </Storyboard>
                </BeginStoryboard>
            </Trigger.ExitActions>
        </Trigger>
    </Style.Triggers>
</Style>
```

**Timing** : 100ms  
**Effet** : Fond bleu très clair  
**Feedback** : Ligne sélectionnable

---

### 3. Click Bouton (Ripple Effect)

```xaml
<!-- Simulation effet ripple -->
<ControlTemplate.Triggers>
    <Trigger Property="IsPressed" Value="True">
        <Trigger.EnterActions>
            <BeginStoryboard>
                <Storyboard>
                    <!-- Scale légèrement -->
                    <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(ScaleTransform.ScaleX)"
                                   To="0.98" 
                                   Duration="0:0:0.08"/>
                    <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(ScaleTransform.ScaleY)"
                                   To="0.98" 
                                   Duration="0:0:0.08"/>
                </Storyboard>
            </BeginStoryboard>
        </Trigger.EnterActions>
        <Trigger.ExitActions>
            <BeginStoryboard>
                <Storyboard>
                    <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(ScaleTransform.ScaleX)"
                                   To="1" 
                                   Duration="0:0:0.15">
                        <DoubleAnimation.EasingFunction>
                            <BounceEase EasingMode="EaseOut" Bounces="1"/>
                        </DoubleAnimation.EasingFunction>
                    </DoubleAnimation>
                    <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(ScaleTransform.ScaleY)"
                                   To="1" 
                                   Duration="0:0:0.15">
                        <DoubleAnimation.EasingFunction>
                            <BounceEase EasingMode="EaseOut" Bounces="1"/>
                        </DoubleAnimation.EasingFunction>
                    </DoubleAnimation>
                </Storyboard>
            </BeginStoryboard>
        </Trigger.ExitActions>
    </Trigger>
</ControlTemplate.Triggers>
```

**Timing** : 80ms compression, 150ms release  
**Effet** : Scale 0.98 puis bounce back à 1.0  
**Feedback** : Action confirmée

---

### 4. Focus Input Field

```xaml
<Style TargetType="TextBox">
    <Style.Triggers>
        <Trigger Property="IsFocused" Value="True">
            <Trigger.EnterActions>
                <BeginStoryboard>
                    <Storyboard>
                        <!-- Bordure indigo -->
                        <ColorAnimation Storyboard.TargetProperty="(TextBox.BorderBrush).(SolidColorBrush.Color)"
                                      To="#6366F1" 
                                      Duration="0:0:0.2"/>
                        
                        <!-- Shadow glow -->
                        <DoubleAnimation Storyboard.TargetProperty="(TextBox.Effect).(DropShadowEffect.BlurRadius)"
                                       To="12" 
                                       Duration="0:0:0.2"/>
                        <DoubleAnimation Storyboard.TargetProperty="(TextBox.Effect).(DropShadowEffect.Opacity)"
                                       To="0.3" 
                                       Duration="0:0:0.2"/>
                    </Storyboard>
                </BeginStoryboard>
            </Trigger.EnterActions>
            <Trigger.ExitActions>
                <BeginStoryboard>
                    <Storyboard>
                        <ColorAnimation Storyboard.TargetProperty="(TextBox.BorderBrush).(SolidColorBrush.Color)"
                                      To="#E5E7EB" 
                                      Duration="0:0:0.2"/>
                        <DoubleAnimation Storyboard.TargetProperty="(TextBox.Effect).(DropShadowEffect.BlurRadius)"
                                       To="0" 
                                       Duration="0:0:0.2"/>
                        <DoubleAnimation Storyboard.TargetProperty="(TextBox.Effect).(DropShadowEffect.Opacity)"
                                       To="0" 
                                       Duration="0:0:0.2"/>
                    </Storyboard>
                </BeginStoryboard>
            </Trigger.ExitActions>
        </Trigger>
    </Style.Triggers>
</Style>
```

**Timing** : 200ms  
**Effet** : Bordure indigo + glow shadow  
**Feedback** : Champ actif

---

### 5. Toggle CheckBox

```xaml
<Style TargetType="CheckBox">
    <Style.Triggers>
        <Trigger Property="IsChecked" Value="True">
            <Trigger.EnterActions>
                <BeginStoryboard>
                    <Storyboard>
                        <!-- Checkmark apparaît avec scale -->
                        <DoubleAnimation Storyboard.TargetName="CheckMark"
                                       Storyboard.TargetProperty="(UIElement.RenderTransform).(ScaleTransform.ScaleX)"
                                       From="0" To="1" 
                                       Duration="0:0:0.2">
                            <DoubleAnimation.EasingFunction>
                                <BackEase EasingMode="EaseOut" Amplitude="0.3"/>
                            </DoubleAnimation.EasingFunction>
                        </DoubleAnimation>
                        <DoubleAnimation Storyboard.TargetName="CheckMark"
                                       Storyboard.TargetProperty="(UIElement.RenderTransform).(ScaleTransform.ScaleY)"
                                       From="0" To="1" 
                                       Duration="0:0:0.2">
                            <DoubleAnimation.EasingFunction>
                                <BackEase EasingMode="EaseOut" Amplitude="0.3"/>
                            </DoubleAnimation.EasingFunction>
                        </DoubleAnimation>
                        
                        <!-- Background color -->
                        <ColorAnimation Storyboard.TargetProperty="(Border.Background).(SolidColorBrush.Color)"
                                      To="#EEF2FF" 
                                      Duration="0:0:0.15"/>
                    </Storyboard>
                </BeginStoryboard>
            </Trigger.EnterActions>
        </Trigger>
    </Style.Triggers>
</Style>
```

**Timing** : 200ms  
**Effet** : Checkmark scale avec back easing  
**Feedback** : État coché confirmé

---

## 📢 Feedback Utilisateur

### 1. Toast Notification (Succès)

```xaml
<!-- Toast Container -->
<Border x:Name="Toast"
        Background="{StaticResource SuccessBrush}"
        CornerRadius="8"
        Padding="16,12"
        HorizontalAlignment="Right"
        VerticalAlignment="Top"
        Margin="0,24,24,0"
        Opacity="0">
    <Border.Effect>
        <DropShadowEffect BlurRadius="16" ShadowDepth="4" Opacity="0.2"/>
    </Border.Effect>
    <Border.RenderTransform>
        <TranslateTransform X="0" Y="-40"/>
    </Border.RenderTransform>
    
    <StackPanel Orientation="Horizontal">
        <TextBlock Text="✓" Foreground="White" FontSize="18" Margin="0,0,8,0"/>
        <TextBlock Text="Véhicule ajouté avec succès" 
                   Foreground="White" FontSize="14"/>
    </StackPanel>
</Border>

<!-- Animation Apparition -->
<Storyboard x:Key="ToastAppear">
    <DoubleAnimation Storyboard.TargetName="Toast"
                    Storyboard.TargetProperty="(UIElement.RenderTransform).(TranslateTransform.Y)"
                    From="-40" To="0" 
                    Duration="0:0:0.3">
        <DoubleAnimation.EasingFunction>
            <BackEase EasingMode="EaseOut" Amplitude="0.3"/>
        </DoubleAnimation.EasingFunction>
    </DoubleAnimation>
    
    <DoubleAnimation Storyboard.TargetName="Toast"
                    Storyboard.TargetProperty="Opacity"
                    From="0" To="1" 
                    Duration="0:0:0.3"/>
</Storyboard>

<!-- Animation Disparition (après 3s) -->
<Storyboard x:Key="ToastDisappear" BeginTime="0:0:3">
    <DoubleAnimation Storyboard.TargetName="Toast"
                    Storyboard.TargetProperty="Opacity"
                    From="1" To="0" 
                    Duration="0:0:0.3"/>
</Storyboard>
```

**Timing** :
- Apparition : 300ms (slide down + fade)
- Affichage : 3 secondes
- Disparition : 300ms (fade out)

**Types** :
- Succès : Vert (#10B981) + ✓
- Erreur : Rouge (#EF4444) + ✕
- Warning : Orange (#F59E0B) + ⚠
- Info : Indigo (#6366F1) + ℹ

---

### 2. Loader Inline (Bouton)

```xaml
<Button Style="{StaticResource PrimaryButton}">
    <Grid>
        <!-- Texte normal -->
        <TextBlock x:Name="ButtonText" 
                   Text="Enregistrer"
                   Opacity="1"/>
        
        <!-- Spinner (caché par défaut) -->
        <StackPanel x:Name="LoadingSpinner" 
                    Orientation="Horizontal"
                    Opacity="0">
            <ProgressRing IsActive="True" 
                         Width="16" Height="16"
                         Foreground="White"
                         Margin="0,0,8,0"/>
            <TextBlock Text="Enregistrement..." Foreground="White"/>
        </StackPanel>
    </Grid>
</Button>

<!-- Animation Loading State -->
<Storyboard x:Key="ButtonLoading">
    <DoubleAnimation Storyboard.TargetName="ButtonText"
                    Storyboard.TargetProperty="Opacity"
                    To="0" 
                    Duration="0:0:0.2"/>
    
    <DoubleAnimation Storyboard.TargetName="LoadingSpinner"
                    Storyboard.TargetProperty="Opacity"
                    To="1" 
                    Duration="0:0:0.2"
                    BeginTime="0:0:0.1"/>
</Storyboard>
```

**Timing** : 200ms transition  
**État** : Bouton désactivé pendant loading  
**Quand** : Soumission formulaire

---

### 3. Progress Bar Linéaire

```xaml
<Border Height="4" 
        Background="#E5E7EB" 
        CornerRadius="2"
        Margin="0,16,0,0">
    <Border x:Name="ProgressFill"
            Background="{StaticResource PrimaryBrush}"
            HorizontalAlignment="Left"
            Width="0"
            CornerRadius="2"/>
</Border>

<!-- Animation Progress -->
<Storyboard x:Key="ProgressAnimation">
    <DoubleAnimation Storyboard.TargetName="ProgressFill"
                    Storyboard.TargetProperty="Width"
                    From="0" To="{Binding ActualWidth, ElementName=ProgressBar}" 
                    Duration="0:0:2">
        <DoubleAnimation.EasingFunction>
            <CubicEase EasingMode="EaseInOut"/>
        </DoubleAnimation.EasingFunction>
    </DoubleAnimation>
</Storyboard>
```

**Quand** : Upload fichier, génération rapport

---

## 🎭 États des Composants

### Bouton - 5 États Visuels

| État | Background | Border | Shadow | Cursor | Duration |
|------|-----------|--------|--------|--------|----------|
| **Default** | #6366F1 | None | Blur:8 Depth:2 | Default | - |
| **Hover** | #4F46E5 | None | Blur:16 Depth:6 | Hand | 150ms |
| **Active/Press** | #4338CA | None | Blur:4 Depth:1 | Hand | 80ms |
| **Disabled** | #6366F1 | None | Blur:8 Depth:2 | Arrow | - |
| **Loading** | #6366F1 | None | Blur:8 Depth:2 | Arrow | - |

**Opacités** :
- Default : 100%
- Disabled : 50%
- Loading : 100% (avec spinner)

---

### Input Field - 6 États Visuels

| État | Border Color | Border Width | Shadow | Background |
|------|-------------|--------------|--------|-----------|
| **Default** | #E5E7EB | 1px | None | White |
| **Hover** | #818CF8 | 1px | None | White |
| **Focus** | #6366F1 | 2px | Glow Indigo | White |
| **Valid** | #10B981 | 2px | Glow Green | White |
| **Error** | #EF4444 | 2px | None | White |
| **Disabled** | #E5E7EB | 1px | None | #F3F4F6 |

**Transitions** : 200ms

---

### Badge - 4 Types avec États

**Succès (Disponible)** :
- Background : #D1FAE5
- Text : #065F46
- Hover : Background → #A7F3D0

**Warning (En service)** :
- Background : #FEF3C7
- Text : #92400E
- Hover : Background → #FDE68A

**Danger (Hors service)** :
- Background : #FEE2E2
- Text : #991B1B
- Hover : Background → #FECACA

**Primary (Info)** :
- Background : #EEF2FF
- Text : #3730A3
- Hover : Background → #E0E7FF

---

## 💀 Skeleton Loaders

### Card KPI Skeleton

```xaml
<Border Style="{StaticResource StatCardStyle}">
    <StackPanel>
        <!-- Label Skeleton -->
        <Border Width="80" Height="12" 
                Background="#E5E7EB" 
                CornerRadius="6"
                Margin="0,0,0,12"/>
        
        <!-- Value Skeleton -->
        <Border Width="60" Height="32" 
                Background="#E5E7EB" 
                CornerRadius="6"
                Margin="0,0,0,8"/>
        
        <!-- Secondary Text Skeleton -->
        <Border Width="100" Height="10" 
                Background="#F3F4F6" 
                CornerRadius="5"/>
    </StackPanel>
</Border>

<!-- Animation Pulse -->
<Storyboard x:Key="SkeletonPulse" RepeatBehavior="Forever">
    <DoubleAnimation Storyboard.TargetProperty="Opacity"
                    From="1" To="0.5" 
                    Duration="0:0:1"
                    AutoReverse="True"/>
</Storyboard>
```

**Effet** : Pulse opacity 1.0 ↔ 0.5 en boucle  
**Quand** : Chargement données Dashboard

---

### Table Row Skeleton

```xaml
<Border Height="72" Background="White" Margin="0,0,0,1">
    <Grid Padding="16,0">
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="80"/>
            <ColumnDefinition Width="*"/>
            <ColumnDefinition Width="*"/>
            <ColumnDefinition Width="*"/>
        </Grid.ColumnDefinitions>
        
        <!-- Image Placeholder -->
        <Border Grid.Column="0" 
                Width="56" Height="56" 
                Background="#E5E7EB" 
                CornerRadius="8"/>
        
        <!-- Text Placeholders -->
        <StackPanel Grid.Column="1" VerticalAlignment="Center">
            <Border Width="120" Height="12" Background="#E5E7EB" CornerRadius="6"/>
            <Border Width="80" Height="10" Background="#F3F4F6" CornerRadius="5" Margin="0,8,0,0"/>
        </StackPanel>
        
        <!-- ... autres colonnes ... -->
    </Grid>
</Border>
```

**Nombre** : 5-10 lignes skeleton  
**Quand** : Chargement liste véhicules

---

## 🔄 Flow d'Utilisation

### Scénario 1 : Ajouter un Véhicule

```
1. Utilisateur clique "+ Ajouter Véhicule"
   → Hover animation (150ms)
   → Click ripple (80ms)

2. Modal apparaît
   → Backdrop fade in (200ms)
   → Modal scale + fade (300ms)

3. Utilisateur remplit formulaire
   → Focus animations sur champs (200ms)
   → Validation temps réel (bordure verte/rouge)

4. Utilisateur clique "Enregistrer"
   → Bouton → État Loading (200ms)
   → Spinner apparaît

5. Requête API réussie
   → Modal disparaît (300ms fade out)
   → Toast succès apparaît en haut-droite (300ms slide down)
   → Liste véhicules se rafraîchit avec stagger (400ms)

Total : ~2-3 secondes avec feedback constant
```

---

### Scénario 2 : Navigation Dashboard → Véhicules

```
1. Utilisateur clique "Véhicules" dans sidebar
   → Hover effect (100ms)
   → Item actif change visuellement (200ms)

2. Vue Dashboard fade out
   → Opacity 1 → 0 (200ms)

3. Vue Véhicules slide + fade in
   → Translate X 40 → 0 (300ms)
   → Opacity 0 → 1 (300ms)
   → Delay 100ms après fade out

4. Données se chargent
   → Skeleton rows apparaissent immédiatement
   → Pulse animation

5. Données arrivent
   → Skeletons → vraies données (fade 200ms)
   → Rows stagger (50ms delay entre chaque)

Total : ~1 seconde transition + temps chargement
```

---

### Scénario 3 : Validation Formulaire avec Erreur

```
1. Utilisateur soumet formulaire incomplet
   → Bouton désactivé (instant)

2. Champs invalides identifiés
   → Shake animation sur champs (300ms)
   → Bordure rouge (200ms)
   → Message erreur fade in sous champ (200ms)

3. Focus automatique sur premier champ invalide
   → Scroll smooth vers champ (300ms)
   → Focus glow animation (200ms)

Total : ~1 seconde pour feedback complet
```

**Shake Animation** :
```xaml
<Storyboard x:Key="ShakeAnimation">
    <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(TranslateTransform.X)"
                    Duration="0:0:0.05"
                    From="0" To="-10"
                    AutoReverse="True"
                    RepeatBehavior="3x"/>
</Storyboard>
```

---

## 🎯 Checklist Performance

### Animations à Optimiser

✅ **Utiliser** :
- Opacity (très performant)
- TranslateTransform X/Y (GPU accelerated)
- ScaleTransform (GPU accelerated)
- RotateTransform (GPU accelerated)

⚠️ **Éviter** :
- Width/Height animés (provoque reflow)
- Margin animé (provoque reflow)
- Trop d'animations simultanées (>10)

### Tests de Performance

| Animation | FPS Minimum | Durée Max | Devices |
|-----------|------------|-----------|---------|
| Modal Open | 60 | 300ms | Tous |
| Page Transition | 55 | 400ms | Tous |
| Hover Button | 60 | 150ms | Tous |
| Table Load | 50 | 600ms | Tous |

---

## 📚 Ressources Code

### Animation Helper Class

```csharp
public static class AnimationHelper
{
    public static void FadeIn(UIElement element, double duration = 0.3)
    {
        var storyboard = new Storyboard();
        var animation = new DoubleAnimation
        {
            From = 0,
            To = 1,
            Duration = TimeSpan.FromSeconds(duration),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        
        Storyboard.SetTarget(animation, element);
        Storyboard.SetTargetProperty(animation, new PropertyPath("Opacity"));
        storyboard.Children.Add(animation);
        storyboard.Begin();
    }
    
    public static void SlideIn(UIElement element, double fromX, double duration = 0.3)
    {
        element.RenderTransform = new TranslateTransform();
        
        var storyboard = new Storyboard();
        var animation = new DoubleAnimation
        {
            From = fromX,
            To = 0,
            Duration = TimeSpan.FromSeconds(duration),
            EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
        };
        
        Storyboard.SetTarget(animation, element);
        Storyboard.SetTargetProperty(animation, 
            new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
        storyboard.Children.Add(animation);
        storyboard.Begin();
    }
    
    public static void ShowToast(string message, ToastType type = ToastType.Success)
    {
        // Implémentation toast notification
    }
}

public enum ToastType
{
    Success,
    Error,
    Warning,
    Info
}
```

---

**Version** : 1.0  
**Date** : Novembre 2025  
**Application** : Fleet Manager  
**Framework** : WPF .NET 8.0
