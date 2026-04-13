# Joc de Șah (SahApp)

O aplicație web full-stack pentru a juca șah, care dispune de un frontend în React și un backend ASP.NET Core Web API ce aplică regulile jocului și validarea mutărilor.

## Funcționalități
- **Gameplay Interactiv:** O interfață de șah complet funcțională, construită cu React.
- **Validare pe Server:** Toate validările mutărilor, actualizările stării tablei de joc și detectările situațiilor de șah (`verifyCheck`) sunt gestionate în siguranță pe backend.
- **Design Orientat pe Obiect (OOP):** Backend-ul utilizează șabloane de proiectare precum Factory (`PieceFactory`) pentru a instanția și gestiona comportamentele specifice ale pieselor (Rege, Cal etc.).
- **Jurnalizare (Logging):** Sistemul personalizat de logging (`IGameLogger`) urmărește mutările, avertismentele și stările de șah în timpul jocului.

## Tehnologii Utilizate
- **Frontend:** React, Vite, JavaScript (`sahapp.client`)
- **Backend:** C# 12, .NET 8, ASP.NET Core Web API (`SahApp.Server`)

## Structura Proiectului
- **/SahApp.Server:** Conține controllerele API (ex. `VerifyController`), modelele jocului și regulile logice. 
- **/sahapp.client:** Conține componentele UI de frontend (`App.jsx`), configurația (`vite.config.js`) și dependențele pachetelor.

## Ghid de Pornire

### Cerințe preliminare
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (pentru clientul React)

### Instrucțiuni de Configurare
1. **Clonează repository-ul:**
2. **Configurarea Backend-ului:**
   - Deschide folderul `SahApp.Server` în Visual Studio.
   - Rulează proiectul pentru a porni API-ul (se va folosi fișierul `launchSettings.json`).
3. **Configurarea Frontend-ului:**
   - Deschide un terminal și navighează către folderul `sahapp.client`.
   - Instalează dependențele rulând:
     ```bash
     npm install
     ```
   - Pornește serverul de dezvoltare Vite:
     ```bash
     npm run dev
     ```
