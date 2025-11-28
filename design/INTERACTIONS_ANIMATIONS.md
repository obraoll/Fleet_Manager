# 🎬 Spécifications des Interactions et Animations

## Vue d'ensemble

Ce document détaille toutes les interactions et animations de l'application Fleet Manager, conformément aux meilleures pratiques UI/UX 2025.

---

## ⏱️ Durées et Easing

### Durées Standard

| Type d'animation | Durée | Usage |
|-----------------|-------|-------|
| **Rapide** | 150ms | Hover, focus |
| **Standard** | 200ms | Transitions générales |
| **Moyenne** | 300ms | Modales, apparitions |
| **Lente** | 500ms | Transitions complexes |

### Easing Functions

| Fonction | Usage | XAML |
|----------|-------|------|
| **EaseOut** | Apparitions, entrées | `CubicEase EasingMode="EaseOut"` |
| **EaseIn** | Disparitions, sorties | `CubicEase EasingMode="EaseIn"` |
| **EaseInOut** | Transitions complexes | `CubicEase EasingMode="EaseInOut"` |
| **Linear** | Progress bars, loaders | `LinearEase` |

---

## 🔘 Interactions des Boutons

### Bouton Primaire

**État Normal** :
- Fond : `#6366F1`
- Ombre : `ShadowSm`
- Transition : 200ms

**État Hover** :
```xml
<Storyboard>
    <ColorAnimation Storyboard.TargetProperty="(Button.Background).(SolidColorBrush.Color)"
                    To="#4F46E5" Duration="0:0:0.2"/>
    <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(TranslateTransform.Y)"
                    To="-1" Duration="0:0:0.2"/>
    <DoubleAnimation Storyboard.TargetProperty="Effect.(DropShadowEffect.BlurRadius)"
                    To="8" Duration="0:0:0.2"/>
</Storyboard>
```

**État Active (Clic)** :
- Transform : `translateY(0)`
- Durée : 100ms

**État Disabled** :
- Opacité : 50%
- Cursor : `NotAllowed`

### Bouton Secondaire

**Hover** :
- Fond : `#F9FAFB`
- Bordure : `#6366F1` (2px)
- Transition : 200ms

### Bouton Tertiaire

**Hover** :
- Fond : `#F9FAFB`
- Transition : 200ms

---

## 🎴 Interactions des Cartes

### Carte Standard

**Hover** :
```xml
<Storyboard>
    <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(TranslateTransform.Y)"
                    To="-2" Duration="0:0:0.2">
        <DoubleAnimation.EasingFunction>
            <CubicEase EasingMode="EaseOut"/>
        </DoubleAnimation.EasingFunction>
    </DoubleAnimation>
    <DoubleAnimation Storyboard.TargetProperty="Effect.(DropShadowEffect.BlurRadius)"
                    To="8" Duration="0:0:0.2"/>
    <DoubleAnimation Storyboard.TargetProperty="Effect.(DropShadowEffect.ShadowDepth)"
                    To="4" Duration="0:0:0.2"/>
</Storyboard>
```

**Carte de Statistiques** :
- Même animation hover
- Effet supplémentaire : Légère pulsation de l'icône (scale 1.05)

---

## 📝 Interactions des Formulaires

### Input Focus

**Animation Focus** :
```xml
<Storyboard>
    <ColorAnimation Storyboard.TargetProperty="(Border.BorderBrush).(SolidColorBrush.Color)"
                    To="#6366F1" Duration="0:0:0.2"/>
    <DoubleAnimation Storyboard.TargetProperty="Effect.(DropShadowEffect.BlurRadius)"
                    From="0" To="4" Duration="0:0:0.2"/>
    <ColorAnimation Storyboard.TargetProperty="Effect.(DropShadowEffect.Color)"
                    To="#6366F1" Duration="0:0:0.2"/>
</Storyboard>
```

**Ombre Focus** :
- `BlurRadius="4"`
- `ShadowDepth="0"`
- `Color="#6366F1"`
- `Opacity="0.1"`

### Validation en Temps Réel

**État Valide** :
```xml
<Storyboard>
    <ColorAnimation Storyboard.TargetProperty="(Border.BorderBrush).(SolidColorBrush.Color)"
                    To="#10B981" Duration="0:0:0.3"/>
    <!-- Icône ✓ apparaît avec fade in -->
    <DoubleAnimation Storyboard.TargetProperty="Opacity"
                    From="0" To="1" Duration="0:0:0.3"/>
</Storyboard>
```

**État Erreur** :
```xml
<Storyboard>
    <ColorAnimation Storyboard.TargetProperty="(Border.BorderBrush).(SolidColorBrush.Color)"
                    To="#EF4444" Duration="0:0:0.2"/>
    <!-- Animation shake -->
    <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(TranslateTransform.X)"
                    Values="-10;10;-10;10;0" Duration="0:0:0.5">
        <DoubleAnimation.EasingFunction>
            <CubicEase EasingMode="EaseOut"/>
        </DoubleAnimation.EasingFunction>
    </DoubleAnimation>
</Storyboard>
```

---

## 🪟 Interactions des Modales

### Apparition Modal

**Overlay** :
```xml
<Storyboard>
    <DoubleAnimation Storyboard.TargetProperty="Opacity"
                    From="0" To="1" Duration="0:0:0.3"/>
</Storyboard>
```

**Container Modal** :
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
    <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(ScaleTransform.ScaleY)"
                    From="0.9" To="1" Duration="0:0:0.3">
        <DoubleAnimation.EasingFunction>
            <CubicEase EasingMode="EaseOut"/>
        </DoubleAnimation.EasingFunction>
    </DoubleAnimation>
</Storyboard>
```

**Disparition** :
- Animation inverse
- Durée : 200ms (plus rapide)

### Fermeture Modal

**Clic sur Overlay** :
- Animation de fermeture
- Délai : 100ms avant fermeture effective

**Bouton Fermer** :
- Rotation de l'icône × (0° → 90°)
- Durée : 200ms

---

## 📊 Interactions des Tableaux

### Hover sur Ligne

```xml
<Storyboard>
    <ColorAnimation Storyboard.TargetProperty="(Border.Background).(SolidColorBrush.Color)"
                    To="#F0F9FF" Duration="0:0:0.15"/>
</Storyboard>
```

### Sélection de Ligne

```xml
<Storyboard>
    <ColorAnimation Storyboard.TargetProperty="(Border.Background).(SolidColorBrush.Color)"
                    To="#EEF2FF" Duration="0:0:0.2"/>
    <ColorAnimation Storyboard.TargetProperty="(Border.BorderBrush).(SolidColorBrush.Color)"
                    To="#6366F1" Duration="0:0:0.2"/>
</Storyboard>
```

### Tri de Colonne

**Icône de Tri** :
- Rotation : 0° → 180° (ascendant)
- Rotation : 180° → 0° (descendant)
- Durée : 300ms

---

## 🧭 Interactions de Navigation

### Sidebar

**Item Hover** :
```xml
<Storyboard>
    <ColorAnimation Storyboard.TargetProperty="(Border.Background).(SolidColorBrush.Color)"
                    To="#F9FAFB" Duration="0:0:0.2"/>
</Storyboard>
```

**Item Active** :
- Bordure gauche : Apparition progressive (0px → 3px)
- Fond : `#EEF2FF`
- Durée : 200ms

**Réduction Sidebar** :
```xml
<Storyboard>
    <DoubleAnimation Storyboard.TargetProperty="Width"
                    From="260" To="70" Duration="0:0:0.3">
        <DoubleAnimation.EasingFunction>
            <CubicEase EasingMode="EaseInOut"/>
        </DoubleAnimation.EasingFunction>
    </DoubleAnimation>
</Storyboard>
```

---

## 🔔 Interactions des Notifications

### Toast Notification

**Apparition** :
```xml
<Storyboard>
    <!-- Slide from right -->
    <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(TranslateTransform.X)"
                    From="400" To="0" Duration="0:0:0.3">
        <DoubleAnimation.EasingFunction>
            <CubicEase EasingMode="EaseOut"/>
        </DoubleAnimation.EasingFunction>
    </DoubleAnimation>
    <!-- Fade In -->
    <DoubleAnimation Storyboard.TargetProperty="Opacity"
                    From="0" To="1" Duration="0:0:0.3"/>
</Storyboard>
```

**Disparition** :
- Animation inverse
- Délai automatique : 5 secondes

### Badge de Notification

**Pulsation** :
```xml
<Storyboard RepeatBehavior="Forever">
    <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(ScaleTransform.ScaleX)"
                    Values="1;1.2;1" Duration="0:0:1.5">
        <DoubleAnimation.EasingFunction>
            <CubicEase EasingMode="EaseInOut"/>
        </DoubleAnimation.EasingFunction>
    </DoubleAnimation>
    <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(ScaleTransform.ScaleY)"
                    Values="1;1.2;1" Duration="0:0:1.5">
        <DoubleAnimation.EasingFunction>
            <CubicEase EasingMode="EaseInOut"/>
        </DoubleAnimation.EasingFunction>
    </DoubleAnimation>
</Storyboard>
```

---

## 📈 Interactions des Graphiques

### Hover sur Point de Données

**Tooltip** :
- Apparition : Fade in + scale (0.9 → 1)
- Durée : 200ms
- Position : Suit le curseur

**Point de Donnée** :
- Scale : 1 → 1.2
- Durée : 200ms

### Zoom sur Graphique

**Animation de Zoom** :
```xml
<Storyboard>
    <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(ScaleTransform.ScaleX)"
                    To="1.1" Duration="0:0:0.3"/>
    <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(ScaleTransform.ScaleY)"
                    To="1.1" Duration="0:0:0.3"/>
</Storyboard>
```

---

## 🔄 Interactions de Chargement

### Skeleton Loader

**Animation de Pulsation** :
```xml
<Storyboard RepeatBehavior="Forever">
    <DoubleAnimation Storyboard.TargetProperty="Opacity"
                    Values="0.3;0.7;0.3" Duration="0:0:1.5">
        <DoubleAnimation.EasingFunction>
            <CubicEase EasingMode="EaseInOut"/>
        </DoubleAnimation.EasingFunction>
    </DoubleAnimation>
</Storyboard>
```

### Spinner de Chargement

**Rotation** :
```xml
<Storyboard RepeatBehavior="Forever">
    <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(RotateTransform.Angle)"
                    From="0" To="360" Duration="0:0:1">
        <DoubleAnimation.EasingFunction>
            <LinearEase/>
        </DoubleAnimation.EasingFunction>
    </DoubleAnimation>
</Storyboard>
```

---

## 🎯 Interactions Spécifiques

### Drag & Drop (Upload de Fichiers)

**Zone de Drop** :
- Bordure : Pointillés animés
- Fond : `#EEF2FF` (Primary Light)
- Animation : Pulsation de la bordure

**Fichier en Cours de Drag** :
- Opacité : 50%
- Scale : 0.95

### Pagination

**Changement de Page** :
- Fade out : 150ms
- Fade in : 200ms
- Délai entre les deux : 50ms

### Filtres

**Application de Filtre** :
- Animation de la table : Fade out → Fade in
- Durée totale : 300ms

---

## 📱 Interactions Tactiles (si applicable)

### Tap

- Feedback visuel : Scale 0.95
- Durée : 100ms

### Swipe

- Détection : 50px minimum
- Animation : Suit le doigt

---

## ⚡ Performance

### Bonnes Pratiques

1. **Utiliser GPU** : `RenderTransform` au lieu de `LayoutTransform`
2. **Limiter les animations simultanées** : Max 3-4
3. **Désactiver les animations** : Si `SystemParameters.PowerLineStatus == PowerLineStatus.Offline`
4. **Utiliser `BeginAnimation`** : Au lieu de `Storyboard` pour les animations simples

### Optimisations

```csharp
// Désactiver les animations si performance faible
if (SystemParameters.PowerLineStatus == PowerLineStatus.Offline)
{
    // Utiliser des transitions instantanées
}
```

---

## 🎨 Exemples de Code Complets

### Bouton avec Hover Animé

```xml
<Button Content="Enregistrer" Style="{StaticResource PrimaryButton}">
    <Button.RenderTransform>
        <TranslateTransform/>
    </Button.RenderTransform>
    <Button.Style.Triggers>
        <Trigger Property="IsMouseOver" Value="True">
            <Trigger.EnterActions>
                <BeginStoryboard>
                    <Storyboard>
                        <ColorAnimation Storyboard.TargetProperty="(Button.Background).(SolidColorBrush.Color)"
                                        To="#4F46E5" Duration="0:0:0.2"/>
                        <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(TranslateTransform.Y)"
                                        To="-1" Duration="0:0:0.2"/>
                    </Storyboard>
                </BeginStoryboard>
            </Trigger.EnterActions>
            <Trigger.ExitActions>
                <BeginStoryboard>
                    <Storyboard>
                        <ColorAnimation Storyboard.TargetProperty="(Button.Background).(SolidColorBrush.Color)"
                                        To="#6366F1" Duration="0:0:0.2"/>
                        <DoubleAnimation Storyboard.TargetProperty="(UIElement.RenderTransform).(TranslateTransform.Y)"
                                        To="0" Duration="0:0:0.2"/>
                    </Storyboard>
                </BeginStoryboard>
            </Trigger.ExitActions>
        </Trigger>
    </Button.Style.Triggers>
</Button>
```

### Modal avec Animation

```xml
<Border x:Name="ModalOverlay" Background="#80000000" Opacity="0">
    <Border x:Name="ModalContainer" 
            Background="White" 
            Width="800" 
            CornerRadius="16"
            Opacity="0"
            RenderTransformOrigin="0.5,0.5">
        <Border.RenderTransform>
            <ScaleTransform ScaleX="0.9" ScaleY="0.9"/>
        </Border.RenderTransform>
        <!-- Contenu modal -->
    </Border>
</Border>
```

```csharp
// Code-behind pour animer l'ouverture
private void ShowModal()
{
    var overlayAnim = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
    var containerFade = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
    var containerScale = new DoubleAnimation(0.9, 1, TimeSpan.FromMilliseconds(300))
    {
        EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
    };
    
    ModalOverlay.BeginAnimation(UIElement.OpacityProperty, overlayAnim);
    ModalContainer.BeginAnimation(UIElement.OpacityProperty, containerFade);
    ModalContainer.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, containerScale);
    ModalContainer.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, containerScale);
}
```

---

## ✅ Checklist d'Implémentation

- [ ] Toutes les animations respectent les durées standard
- [ ] Les easing functions sont cohérentes
- [ ] Les animations GPU sont utilisées (`RenderTransform`)
- [ ] Les états hover/focus/active sont tous implémentés
- [ ] Les animations de chargement sont présentes
- [ ] Les feedbacks visuels sont immédiats (< 200ms)
- [ ] Les animations complexes sont optimisées
- [ ] Les animations peuvent être désactivées pour l'accessibilité

---

**Version** : 1.0.0  
**Date** : 2025  
**Auteur** : Fleet Manager Design Team

