# Instructions pour publier sur GitHub

## ✅ Ce qui a été fait

1. ✅ Dépôt Git initialisé
2. ✅ Fichier `.gitignore` configuré (ignore `appsettings.json` et fichiers sensibles)
3. ✅ Fichier `appsettings.example.json` créé (template pour la configuration)
4. ✅ Fichier `README.md` créé avec documentation complète
5. ✅ Tous les fichiers ajoutés et commités

## 🚀 Étapes pour publier sur GitHub

### 1. Créer un nouveau dépôt sur GitHub

1. Allez sur [GitHub.com](https://github.com)
2. Cliquez sur le bouton **"+"** en haut à droite → **"New repository"**
3. Remplissez les informations :
   - **Repository name** : `FleetManager` (ou le nom de votre choix)
   - **Description** : "Application de gestion de parc automobile en C# WPF"
   - **Visibilité** : Public ou Private (selon votre préférence)
   - ⚠️ **NE COCHEZ PAS** "Initialize this repository with a README" (on a déjà un README)
4. Cliquez sur **"Create repository"**

### 2. Connecter votre dépôt local à GitHub

Exécutez ces commandes dans PowerShell (remplacez `VOTRE_USERNAME` par votre nom d'utilisateur GitHub) :

```powershell
cd "C:\Users\smith\Documents\PROJET_BTS\Fleet_Manager\FleetManager"

# Ajouter le remote GitHub
git remote add origin https://github.com/VOTRE_USERNAME/FleetManager.git

# Renommer la branche principale en 'main' (si nécessaire)
git branch -M main

# Pousser le code sur GitHub
git push -u origin main
```

### 3. Si vous utilisez l'authentification par token

Si GitHub vous demande des identifiants :
1. Allez dans GitHub → Settings → Developer settings → Personal access tokens → Tokens (classic)
2. Créez un nouveau token avec les permissions `repo`
3. Utilisez ce token comme mot de passe lors du `git push`

### 4. Alternative : Utiliser GitHub CLI

Si vous avez GitHub CLI installé :

```powershell
gh repo create FleetManager --public --source=. --remote=origin --push
```

## 📝 Vérification

Après le push, vérifiez que tout est bien en ligne :
- Allez sur `https://github.com/VOTRE_USERNAME/FleetManager`
- Vérifiez que tous les fichiers sont présents
- Vérifiez que le README s'affiche correctement

## 🔒 Sécurité

✅ Le fichier `appsettings.json` (contenant la chaîne de connexion) est **ignoré** par Git
✅ Seul `appsettings.example.json` (template) sera sur GitHub
✅ Les fichiers compilés (`bin/`, `obj/`) sont ignorés
✅ Les fichiers sensibles sont protégés

## 📌 Commandes Git utiles

```powershell
# Voir l'état des fichiers
git status

# Ajouter des modifications
git add .

# Faire un commit
git commit -m "Description des modifications"

# Pousser sur GitHub
git push

# Récupérer les dernières modifications
git pull
```

---

**Note** : Si vous rencontrez des problèmes, vérifiez que vous avez bien configuré Git :
```powershell
git config --global user.name "Votre Nom"
git config --global user.email "votre.email@example.com"
```

