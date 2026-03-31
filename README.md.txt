Countries API  - Liquid Labs Assignment

My Approach

For this assignment, I focused on keeping the design simple and clean while following the given requirements.

I used a layered structure (Controller → Service → Repository) to separate concerns. Instead of using an ORM, I implemented raw SQL queries using ADO.NET as required.

The caching logic was implemented using a cache-aside pattern, which ensures that repeated requests do not hit the external API unnecessarily.

I also tested the API endpoints using a browser and verified data persistence directly in SQL Server. 

The focus was on clarity, maintainability and meeting the assignment requirements without over-engineering the solution.
---

 Features

- Fetch country data from a public API
- Store country data in SQL Server
- Retrieve all countries from the database
- Retrieve a country by ID
- Retrieve a country by code (with caching)
- Uses raw SQL queries (no ORM)

---

 Technologies Used

- .NET 8 (ASP.NET Core Web API)
- SQL Server
- ADO.NET (`Microsoft.Data.SqlClient`)
- REST Countries API (https://restcountries.com/)

---

 Project Structure


CountriesApi/
├── Controllers/       → API endpoints
├── Services/          → Business logic (caching)
├── Repositories/      → Database access (SQL queries)
├── ExternalApis/      → Third-party API integration
├── Models/            → Data models
├── Database/          → SQL schema file


---

 Database Setup

1. Open SQL Server Management Studio (SSMS)
2. Connect to your local SQL Server
3. Run the following command to create the database:

CREATE DATABASE CountriesDb;

4. Then open the file `Database/schema.sql` and run it against `CountriesDb`

---

  Configuration

Open appsettings.json and update the connection string:

{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=CountriesDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}

Replace YOUR_SERVER with:
- LocalDB: (localdb)\MSSQLLocalDB
- SQL Express: .\SQLEXPRESS
- Full SQL Server: localhost

---

  How to Run

1. Clone the repository:
git clone https://github.com/HasithaKW/CountriesApi.git

2. Navigate to the project folder:
cd CountriesApi

3. Restore packages:
dotnet restore

4. Run the API:
dotnet run

The API will start at http://localhost:5120


---
  API Endpoints

 GET /api/countries
Returns all cached countries from the database.

**Response:**
```json
[
  {
    "id": 1,
    "code": "LK",
    "name": "Sri Lanka",
    "capital": "Sri Jayawardenepura Kotte",
    "population": 21763170,
    "region": "Asia",
    "createdAt": "2026-03-31T11:30:45.123"
  },
  {
    "id": 2,
    "code": "US",
    "name": "United States",
    "capital": "Washington, D.C.",
    "population": 331893745,
    "region": "Americas",
    "createdAt": "2026-03-31T11:31:20.456"
  }
]
```

 GET /api/countries/{id}
Returns a single country by its database ID.

**Example:** `/api/countries/1`

**Response:**
```json
{
  "id": 1,
  "code": "LK",
  "name": "Sri Lanka",
  "capital": "Sri Jayawardenepura Kotte",
  "population": 21763170,
  "region": "Asia",
  "createdAt": "2026-03-31T11:30:45.123"
}
```

 GET /api/countries/code/{code}
Returns a country by 2-letter country code. Checks DB first, calls REST Countries API if not cached.

**Example:** `/api/countries/code/LK`

**Response:**
```json
{
  "id": 1,
  "code": "LK",
  "name": "Sri Lanka",
  "capital": "Sri Jayawardenepura Kotte",
  "population": 21763170,
  "region": "Asia",
  "createdAt": "2026-03-31T11:30:45.123"
}
```

 Error Response (404 Not Found)

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "Not Found",
  "status": 404,
  "detail": "Country with code 'XX' was not found"
}
```
  


---

 Libraries Used

**Microsoft.Data.SqlClient**
I used this library to connect to SQL Server and run raw SQL queries
using SqlConnection and SqlCommand. The assignment required avoiding
ORMs, so this gave me direct control over the SQL execution.

*System.Text.Json**
This is built into .NET 8 so no extra package was needed. I used it
to deserialize the JSON response from the REST Countries API into
C# model classes.

---
 Caching Behavior

* First request
  * Fetches data from REST Countries API
  * Saves data to SQL Server
  * Returns response

* Subsequent requests
  * Retrieves data directly from database
  * No external API call

---

 Design Decisions

 * No ORM Used

The assignment explicitly required avoiding ORMs, so ADO.NET (`SqlConnection`, `SqlCommand`) was used for direct SQL execution.

 * Repository Pattern

Separates database logic from business logic, improving maintainability and testability.

 * Service Layer

Handles caching logic and external API interaction, keeping controllers clean.

 * HttpClientFactory

Used for calling external APIs to ensure efficient resource management and avoid socket exhaustion.

---

 Error Handling

* Returns `404 Not Found` when a country is not available
* Handles external API failures gracefully
* Prevents SQL injection using parameterized queries

---

 Notes

* No API key is required for the REST Countries API
* Ensure SQL Server is running before starting the application
* The application is designed for local development and testing

---

  Author 
 Hasitha Wijesooriya


