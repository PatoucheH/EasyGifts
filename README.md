# EasyGifts

Application de gestion de listes de cadeaux permettant de partager ses souhaits avec famille et amis.

## Description

EasyGifts est une application multiplateforme qui permet aux utilisateurs de :

- Creer et gerer leurs listes de cadeaux personnelles
- Partager leurs listes au sein de groupes (famille, amis)
- Inviter des membres par email
- Voir les cadeaux des autres membres du groupe
- Eviter les doublons en marquant les cadeaux comme achetes

## Architecture

Le projet suit une **Clean Architecture** avec separation claire des responsabilites :

EasyGifts/

- backend/ # API Backend
  - EasyGiftsBackend.Api/ # Controllers, Configuration
  - EasyGiftsBackend.Application/ # Interfaces des services
  - EasyGiftsBackend.Domain/ # Entites, DTOs
  - EasyGiftsBackend.Infrastructure/ # Implementation services, DB
- src/ # Applications Frontend
  - EasyGifts.Shared/ # DTOs partages (client/serveur)
  - EasyGifts.UI/ # Composants Blazor (RCL)
  - EasyGifts.Web/ # Blazor WebAssembly
  - EasyGifts.Maui/ # Application MAUI (mobile/desktop)
- docker-compose.yml # Orchestration des conteneurs

## Technologies

### Backend

| Technologie           | Version | Description              |
| --------------------- | ------- | ------------------------ |
| .NET                  | 10.0    | Framework principal      |
| ASP.NET Core          | 10.0    | API REST                 |
| Entity Framework Core | 10.0    | ORM                      |
| PostgreSQL            | 17      | Base de donnees          |
| ASP.NET Identity      | 10.0    | Gestion des utilisateurs |
| JWT Bearer            | 10.0    | Authentification         |

### Frontend

| Technologie           | Version | Description                 |
| --------------------- | ------- | --------------------------- |
| Blazor WebAssembly    | 10.0    | Application Web SPA         |
| .NET MAUI             | 10.0    | Application multiplateforme |
| Blazored.LocalStorage | 4.5     | Stockage local (token JWT)  |

### Infrastructure

| Technologie    | Description                  |
| -------------- | ---------------------------- |
| Docker         | Conteneurisation             |
| Docker Compose | Orchestration                |
| Nginx          | Serveur web (Blazor WASM)    |
| Mailtrap       | Service SMTP (developpement) |

## Prerequis

- .NET 10 SDK
- Docker Desktop
- PostgreSQL (ou utiliser Docker)

## Installation et Demarrage

### Option 1 : Docker Compose (recommande)

Cloner le repository et lancer:

       git clone https://github.com/votre-username/EasyGifts.git
       cd EasyGifts
       docker-compose up -d

Services disponibles :
| Service | URL | Description |
|---------|-----|-------------|
| API | http://localhost:5000 | Backend REST API |
| Web | http://localhost:8080 | Application Blazor WebAssembly |
| PostgreSQL | localhost:5432 | Base de donnees |

### Option 2 : Developpement local

#### 1. Base de donnees PostgreSQL

Via Docker:

       docker run -d --name easygifts-postgres -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=EasyGiftsDb -p 5432:5432 postgres:17

#### 2. Backend API

       cd backend/EasyGiftsBackend.Api
       dotnet restore
       dotnet run

API disponible sur http://localhost:5000

#### 3. Frontend Web (Blazor WebAssembly)

       cd src/EasyGifts.Web
       dotnet restore
       dotnet run

Application disponible sur https://localhost:7000

#### 4. Application MAUI (optionnel)

       cd src/EasyGifts.Maui
       dotnet restore

       # Windows
       dotnet build -t:Run -f net10.0-windows10.0.19041.0

       # Android
       dotnet build -t:Run -f net10.0-android

## Configuration

### Variables d environnement Backend

| Variable                         | Description                    |
| -------------------------------- | ------------------------------ |
| ConnectionStrings\_\_MyPostgreDB | Chaine de connexion PostgreSQL |
| Jwt\_\_Key                       | Cle secrete JWT                |
| Jwt\_\_Issuer                    | Emetteur JWT                   |
| Jwt\_\_Audience                  | Audience JWT                   |
| Smtp\_\_Host                     | Serveur SMTP                   |
| Smtp\_\_Port                     | Port SMTP                      |
| Smtp\_\_Username                 | Utilisateur SMTP               |
| Smtp\_\_Password                 | Mot de passe SMTP              |

## API Endpoints

### Authentification (/api/auth)

| Methode | Endpoint  | Description                 | Auth |
| ------- | --------- | --------------------------- | ---- |
| POST    | /register | Inscription                 | Non  |
| POST    | /login    | Connexion (retourne JWT)    | Non  |
| GET     | /me       | Profil utilisateur connecte | Oui  |

### Cadeaux (/api/gift)

| Methode | Endpoint     | Description                          | Auth |
| ------- | ------------ | ------------------------------------ | ---- |
| POST    | /create      | Creer un cadeau                      | Oui  |
| POST    | /delete      | Supprimer un cadeau                  | Oui  |
| POST    | /update      | Modifier un cadeau                   | Oui  |
| GET     | /getByGiftId | Obtenir un cadeau par ID             | Oui  |
| GET     | /getByUserId | Obtenir les cadeaux d un utilisateur | Oui  |
| GET     | /myGifts     | Obtenir mes cadeaux                  | Oui  |

### Groupes (/api/groups)

| Methode | Endpoint                    | Description            | Auth |
| ------- | --------------------------- | ---------------------- | ---- |
| POST    | /                           | Creer un groupe        | Oui  |
| GET     | /{groupId}                  | Obtenir un groupe      | Oui  |
| DELETE  | /{groupId}                  | Supprimer un groupe    | Oui  |
| POST    | /{groupId}/invite           | Inviter un utilisateur | Oui  |
| GET     | /me                         | Obtenir mes groupes    | Oui  |
| GET     | /{groupId}/members          | Obtenir les membres    | Oui  |
| DELETE  | /{groupId}/members/{userId} | Retirer un membre      | Oui  |

## Modele de donnees

### Entites principales

**User**: Id, IdentityId, Username, Email, Gifts

**Gift**: Id, Name, Description, Price, Url, ImageUrl, IsPurchased, UserId

**Group**: Id, Name, AdminId, GroupUsers

**GroupInvitation**: Id, GroupId, Email, Token, ExpiresAt, Accepted

## Structure Frontend

### Composants Blazor (EasyGifts.UI)

- Auth/LoginPage.razor - Page de connexion
- Auth/RegisterPage.razor - Page d inscription
- Gifts/GiftListPage.razor - Liste des cadeaux
- Gifts/UserGiftsPage.razor - Cadeaux d un utilisateur
- Groups/GroupPage.razor - Gestion des groupes
- Layout/MainLayout.razor - Layout principal
- Home.razor - Page d accueil

### Services Frontend

| Service                        | Description                          |
| ------------------------------ | ------------------------------------ |
| ApiClient                      | Client HTTP pour l API               |
| JwtAuthenticationStateProvider | Gestion de l etat d authentification |
| IAuthService                   | Interface authentification           |
| IGiftService                   | Interface gestion des cadeaux        |
| IGroupService                  | Interface gestion des groupes        |

## Plateformes supportees

### EasyGifts.Web (Blazor WebAssembly)

- Tous les navigateurs modernes (Chrome, Firefox, Edge, Safari)

### EasyGifts.Maui

| Plateforme | Version minimale |
| ---------- | ---------------- |
| Android    | 7.0 (API 24)     |
| iOS        | 15.0             |
| macOS      | 15.0 (Catalyst)  |
| Windows    | 10.0.17763       |

## Developpement

### Migrations Entity Framework

       cd backend/EasyGiftsBackend.Api
       dotnet ef migrations add NomDeLaMigration -p ../EasyGiftsBackend.Infrastructure
       dotnet ef database update -p ../EasyGiftsBackend.Infrastructure

### Build Docker

       # Backend uniquement
       docker build -t easygifts-api -f backend/Dockerfile ./backend

       # Frontend Web uniquement
       docker build -t easygifts-web -f src/EasyGifts.Web/Dockerfile .

       # Tout le stack
       docker-compose build

## Tests

       # Backend
       cd backend && dotnet test

       # Frontend
       cd src && dotnet test

## Contribution

1.  Fork le projet
2.  Creer une branche (git checkout -b feature/nouvelle-fonctionnalite)
3.  Commit les changements
4.  Push la branche
5.  Ouvrir une Pull Request

## Licence

Ce projet est sous licence MIT.

