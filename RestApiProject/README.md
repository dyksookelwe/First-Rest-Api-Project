# Simple REST API on ASP.NET Core

This is my practice project where I build a simple CRUD system:
- backend on ASP.NET Core (Minimal API, no controllers)
- frontend on HTML + JavaScript + fetch()
>  Frontend notice  
This UI is based on a learning example.  
I am currently focusing on Backend (ASP.NET + APIs) and learning frontend step-by-step.  
I reviewed and understood the logic and will gradually replace it with my own version.
- in-memory list instead of database

### Features
- View all users
- Add new user
- Edit user data
- Delete user
- Get user by ID
- Regex-based routing (learning step)
- Static files served from `wwwroot`

### Tech stack
- ASP.NET Core
- C#
- HTML + JS (fetch API)
- In-memory data (no DB yet)

### How to run

```bash
dotnet run

Project structure:
Program.cs        – API logic
Models/Person.cs  – user model
wwwroot/
   index.html     – UI
   js/scripts.js  – client logic

