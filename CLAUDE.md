# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Contexte

TP étudiant (ESP Dakar, Master 2 GLSI, cours « Enseignement .NET »). L'utilisateur débute en C# :
réponds en **français**, explique ce que tu fais, garde un code simple et commenté en français, sans architecture superflue.
Les noms `POOAspEntity`, `P5AspEntity` et `P8Maui` sont imposés par l'enseignant : ne les renomme pas.
Le brief complet du TP se trouve dans le dossier parent (`../claude.md`, `../TP .NET ... .pdf`).

## Environnement

- `global.json` fixe le **SDK .NET 10** (la machine a aussi .NET 11). Le workload MAUI est installé pour la bande 10.0.400 :
  lance toujours `dotnet` depuis ce dépôt pour garder le bon SDK.
- Cible : .NET 10 LTS, et non .NET 8 comme dans le sujet (choix validé par l'utilisateur, .NET 8 n'étant plus supporté).
- `dotnet-ef` 10.0.15 est installé globalement dans `%USERPROFILE%\.dotnet\tools`. Ce dossier n'est peut-être pas dans le PATH de la session.
- Le SDK Android n'est **pas** encore installé : la cible `net10.0-android`, et donc `dotnet build` sur la solution entière,
  échouent avec XA5300 tant que l'utilisateur n'a pas lancé `InstallAndroidDependencies` (voir README).
  Pour vérifier le code MAUI sans Android : `dotnet build P8Maui -f net10.0-windows10.0.19041.0`.

## Commandes

```bash
dotnet build P5AspEntity
cd P5AspEntity && dotnet ef migrations add <Nom> && dotnet ef database update
dotnet run --project P5AspEntity --launch-profile http     # http://localhost:5112, Swagger sur /swagger
dotnet build P8Maui -f net10.0-windows10.0.19041.0 -t:Run  # lance l'appli sous Windows
```

Il n'y a pas de projet de tests. On vérifie l'API avec `P5AspEntity/P5AspEntity.http` ou avec curl.
Arrête le processus `P5AspEntity` avant `dotnet ef` ou un rebuild, sinon la DLL reste verrouillée.

## Architecture

**P5AspEntity** (API minimale) : tout est dans `Program.cs` (configuration et routes `Map*`).
`PizzaDb` (DbContext) est injecté directement dans les lambdas des routes. Le fournisseur se choisit dans `Program.cs` :
SQLite est actif (chaîne `ConnectionStrings:Pizzas` dans `appsettings.json`), et la ligne InMemory est volontairement
gardée en commentaire, car elle correspond à l'étape 1 du module Learn.
Toute modification d'un modèle dans `Models/` demande une nouvelle migration EF.

**P8Maui** : chaîne d'injection de dépendances définie dans `MauiProgram.cs`, soit
`PersonRepository` (singleton, chemin `FileSystem.AppDataDirectory/people.db3`) → `MainViewModel` (singleton) → `MainPage` (transient).
Shell crée les pages des onglets (`AppShell.xaml`, `TabBar`) via le conteneur ; une page qui reçoit des dépendances
dans son constructeur doit donc être enregistrée dans `MauiProgram`.
- La page Personnes est en MVVM : `INotifyPropertyChanged` écrit à la main et `Command` de MAUI (pas de CommunityToolkit).
  Les liaisons sont compilées (`x:DataType` obligatoire, car `MauiXamlInflator=SourceGen`).
- `PhonewordPage` reprend volontairement le style du module 1 (événements `Clicked` dans le code-behind).
- Les styles nommés partagés (`PageTitle`, `PrimaryButton`, `DangerButton`, `Card`...) sont dans `App.xaml`.
  Les couleurs viennent de `Resources/Styles/Colors.xaml`.
- SQLite : `sqlite-net-pcl` 1.11 apporte lui-même SQLitePCLRaw 3. N'ajoute **pas** `SQLitePCLRaw.bundle_green`
  (version 2.1.x vulnérable, et en conflit avec la version 3).
- Seules les cibles Android et Windows existent : les dossiers `Platforms/iOS` et `Platforms/MacCatalyst` ont été supprimés.

## Rendu

À rendre : le lien du dépôt GitHub, et la capture d'écran Microsoft Learn dans `docs/capture-learn.png` (faite par l'utilisateur).
Demande avant de créer le dépôt GitHub ou de pousser.
