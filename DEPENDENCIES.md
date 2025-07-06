
# Dependencies

This document lists all key dependencies across the project folders for easier reviewer setup.

---

## Task 1 — Backend (TaskApi)

.NET SDK 8 (tested with 8.0.6)

**NuGet Packages:**

| Package                      | Version |
| ---------------------------- | ------- |
| Azure.Identity               | 1.14.1  |
| Microsoft.AspNetCore.OpenApi | 8.0.6   |
| Microsoft.Graph              | 5.84.0  |
| Microsoft.Identity.Client    | 4.73.1  |
| Swashbuckle.AspNetCore       | 6.4.0   |

## Secrets Configuration

This project uses the **Microsoft identity client credentials flow** to connect to Microsoft Graph.  
For local development, you must configure the following secrets using the [dotnet user-secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) feature:

```bash
dotnet user-secrets set "AzureAd:TenantId" "<your-tenant-id>"
dotnet user-secrets set "AzureAd:ClientId" "<your-client-id>"
dotnet user-secrets set "AzureAd:ClientSecret" "<your-client-secret>"
```
You can verify the secrets with:
```bash
dotnet user-secrets list
```
---

## Task 1 — Frontend (TaskUi)

**Node.js**: tested with Node 18.x  
**NPM**: tested with npm 10.x

Install dependencies with:

```bash
npm install
```

**Key packages:**

| Package                | Version   |
| ---------------------- | --------- |
| react                  | 19.1.0    |
| react-dom              | 19.1.0    |
| react-router-dom       | 7.6.3     |
| axios                  | 1.10.0    |
| typescript             | 4.9.5     |
| @testing-library/react | 16.3.0    |
| @types/react           | 19.1.8    |
| @types/node            | 16.18.126 |
| react-scripts          | 5.0.1     |

---

## Task 2 — SharePoint Framework (ProjectDashboard)

**Node.js**: tested with Node 14.x  
**NPM**: tested with npm 6.x  
**SharePoint Framework**: 1.13.1

Restore packages with:

```bash
npm install
```

**Key packages:**

| Package                     | Version |
| --------------------------- | ------- |
| @microsoft/sp-core-library  | 1.13.1  |
| @microsoft/sp-webpart-base  | 1.13.1  |
| @microsoft/sp-build-web     | 1.13.1  |
| @microsoft/sp-property-pane | 1.13.1  |
| @pnp/sp                     | 2.14.0  |
| @fluentui/react             | 8.123.1 |
| office-ui-fabric-react      | 7.174.1 |
| react                       | 16.13.1 |
| react-dom                   | 16.13.1 |
| gulp                        | 4.0.2   |

---

## Task 3 — CodeReview.cs

No dependencies.  
This is a standalone C# file with inline review comments and does not need to be built or run.

---

# How to install

- Each folder is self-contained.
  - for TaskApi run:
    ```bash
    dotnet restore
    dotnet build
    dotnet run
    ```
  - for TaskUi run:
    ```bash
    npm install
    npm start
    ```
  - for ProjectDashboard run:
    ```bash
    npm install
    gulp serve
    ```

# Author

Elitzur Bahir
