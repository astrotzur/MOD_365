# MOD_365

A demonstration and learning project showcasing Microsoft 365–related development skills.  
The repository is organized into multiple tasks, each focusing on a different Microsoft 365–relevant technology or practice.

## Tasks

- **Task 1: Task Management System**
  - `TaskApi`: .NET Web API backend using Microsoft Graph and SharePoint to manage tasks.
  - `TaskUi`: React front-end to display and create tasks (note: Tailwind CSS integration is missing and currently uses plain CSS instead).
  - This project uses the **Microsoft identity client credentials flow** to connect to Microsoft Graph.  
For local development, you must configure the secrets using the [dotnet user-secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) feature. Read DEPENDENCIES.md for details.

- **Task 2: Project Dashboard Web Part**
  - `ProjectDashboard`: An SPFx web part, implemented using class-based React components instead of modern hooks, to display a simple SharePoint project dashboard.

- **Task 3: Code Review**
  - `CodeReview.cs`: A standalone file containing code provided for review, with inline comments explaining identified issues and proposed improvements.

- **Task 4**  
  This task was explicitly omitted due to time constraints.

## Repository Structure

- TaskApi → Backend for Task 1 (ASP.NET Core + Microsoft Graph)
- TaskUi → Frontend for Task 1 (React)
- ProjectDashboard → Task 2 (SPFx class-based web part)
- CodeReview.cs → Task 3 (standalone code review)
- (no Task 4)

## Requirements

- .NET 8 SDK
- Node.js (LTS recommended, Node 18 for Task 1 and Node 14 for Task 2)
- Visual Studio Code (or other code editor)
- Microsoft 365 Developer account (for Graph API and SharePoint data)

## Notes

- **Task 1** shows end-to-end integration: SharePoint list → Graph API → ASP.NET API → React front-end.  
- **Task 2** is a SharePoint Framework web part using older class-based components.  
- **Task 3** demonstrates static code review with documented improvements.  
- Task 4 is explicitly omitted.

---

**Author:** Elitzur Bahir  
**License:** MIT
