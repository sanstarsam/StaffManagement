# Staff Management App

“Staff Management” using Asp.net Web API and JavaScript frameworks Angular as front-end.  

---

## Table of Contents

1. [Features](#features)  
2. [Tech Stack](#tech-stack)  
3. [Project Structure](#project-structure)  
4. [Setup](#setup)  
5. [Backend](#backend)  
6. [Frontend](#frontend)  
7. [Testing](#testing)  
8. [Continuous Integration (CI)](#continuous-integration-ci)  
9. [Usage](#usage)  
10. [License](#license)  

---

## Features

Function:
- Staff Management (Add, Edit, Delete, Search), input with the information as below:
	o Staff ID: String (8)
	o Full name: String (100)
	o Birthday: Date
	o Gender: Int (with 1: Male, 2: Female)
- Advanced search form: Search for staffs by criteria:
	o Staff ID
	o Gender
	o Birthday (from date to date)
- Reports: after getting the results on Advanced search, we can export Excel file or PDF file

---

## Tech Stack

| Layer | Technology |
|-------|------------|
| Backend | ASP.NET Core 8, C# |
| Frontend | Angular 16, TypeScript |
| Database / Storage | XML file (for simplicity) |
| Testing | xUnit, Moq, FluentAssertions, Cypress |
| CI| GitHub Actions |

## Setup

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- [Node.js 20+](https://nodejs.org/en/download/)  
- Angular CLI `npm install -g @angular/cli`  

---

## Backend
### 1. Navigate to the backend folder

```bash
cd backend
```

### 2. Restore dependencies and build

```bash
dotnet restore
dotnet build
```

### 3. Run the API

```bash
dotnet run
```

- API will be available at `https://localhost:5001` (or `http://localhost:5000`)  

### 4. Test backend

```bash
dotnet test
```

- Runs **unit and integration tests**  

---

## Frontend

### 1. Navigate to the frontend folder

```bash
cd frontend
```

### 2. Install dependencies

```bash
npm ci
```

### 3. Serve Angular app

```bash
ng serve
```

- App runs at `http://localhost:4200`  
- Make sure the **environment.ts** points to the backend API:  

```ts
export const environment = {
  production: false,
  apiUrl: 'https://localhost:5001/api'
};
```

### 4. Build for production

```bash
ng build --configuration production
```

---

## Testing

### Backend

- **Unit & Integration Tests:** `dotnet test`
- **E2E Tests (C#):** Uses `WebApplicationFactory<Program>` + `HttpClient`

---

## Continuous Integration (CI)

- GitHub Actions pipeline configured to:
  - Build backend (`dotnet build`)  
  - Run backend unit & integration tests  
  - Build Angular frontend (`ng build`)  
  - Run frontend unit tests (`ng test`)  
  - Run E2E tests with Cypress  

---

## Usage

1. Open the Angular app in the browser: `http://localhost:4200`  
2. Add a staff member via the UI.  
3. Use **Advanced Search** to filter staff by ID, Gender, or Birthday.  
4. Export results to **Excel** or **PDF** using the export buttons.  
5. Backend API endpoints for reference:  

| Method | Endpoint | Description |
|--------|----------|------------|
| GET | `/api/staffs` | Get all staff (with optional filters) |
| GET | `/api/staffs/{id}` | Get staff by ID |
| POST | `/api/staffs` | Create new staff |
| PUT | `/api/staffs` | Update staff |
| DELETE | `/api/staffs/{id}` | Delete staff |

---
