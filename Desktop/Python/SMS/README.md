# Sales Management System (SMS)

Full-stack assessment project built with Angular, .NET Core Web API and MS SQL Server.

## Stack

- **Frontend:** Angular 17, DevExtreme (data grid + charts)
- **Backend:** .NET 8 Web API, EF Core, JWT auth
- **Database:** MS SQL Server

## Folder structure

```
SMS/
 ├─ database/           SQL scripts (schema + seed data)
 ├─ backend/SMS.API/     .NET Core Web API
 ├─ frontend/sms-frontend/   Angular app
 └─ docs/                notes / screenshots
```

## 1. Database setup

1. Open SSMS (or Azure Data Studio) connected to your local SQL Server instance.
2. Run `database/01_CreateDatabase.sql` — creates `SalesManagementDB` and all five tables.
3. Run `database/02_SeedData.sql` — inserts a default admin user, a handful of customers/items,
   and three sample invoices so the dashboard/report aren't empty on first run.

Default login: **admin / Admin@123**

> The seeded password hash was generated with BCrypt (work factor 11). If you want to regenerate
> it yourself, run this in a `dotnet-script`/LINQPad snippet:
> `BCrypt.Net.BCrypt.HashPassword("Admin@123")` and swap the hash in `02_SeedData.sql`.

## 2. Backend (SMS.API)

```bash
cd backend/SMS.API
dotnet restore
```

Update `appsettings.json`:
- `ConnectionStrings:DefaultConnection` — point it at your SQL Server instance.
- `JwtSettings:Key` — replace with a real random secret before you run this anywhere but localhost.

Since the schema is created directly via the SQL scripts above (not EF migrations), you can run
the API straight away:

```bash
dotnet run
```

The API comes up on `https://localhost:7199` (see `Properties/launchSettings.json`) with Swagger
at `/swagger`. CORS is already configured to allow `http://localhost:4200` for the Angular dev
server — change `Cors:AllowedOrigins` in appsettings if you serve the frontend elsewhere.

If you'd rather manage the schema through EF Core migrations instead of the raw SQL scripts:

```bash
dotnet tool install --global dotnet-ef   # if you don't have it already
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 3. Frontend (sms-frontend)

```bash
cd frontend/sms-frontend
npm install
npm start
```

Runs on `http://localhost:4200`. `src/environments/environment.ts` points at
`https://localhost:7199/api` by default — update it if your API runs on a different port.

## What's implemented

- JWT login screen, route guard + HTTP interceptor that attaches the token and bounces to
  `/login` on a 401.
- Customer Master and Item Master — list + search + slide-over form for add/edit/soft-delete.
  Item Master supports a photo upload with a live preview (stored as base64 for simplicity;
  swap for blob storage + URLs if this goes anywhere near production).
- Sales Invoice Entry — the Top/Prior/Next/Bottom/Search/List navigation bar plus
  Add/Edit/Delete/Save/Cancel, backed by a composite (InvoiceType, InvoiceNumber) key on the
  server. The line-items grid auto-calculates row amount and the invoice total as you type.
- Interactive Sales Report using a DevExtreme grid — drag any column into the group panel to
  group by Customer, Category or Brand, with per-group and grand totals on Sale Amount and
  Gross Profit.
- Dashboard with KPI cards (Total Sales, Total Gross Profit, Top Selling Item) plus a sales
  trend bar chart and a sales-by-category donut chart.

## What I'd do with more time

- Real image storage (Azure Blob / S3) instead of base64 in the DB for item photos.
- Pagination on the Customer/Item lists — fine for the seeded data, not fine for a real catalog.
- Optimistic concurrency handling on invoice Save (currently last-write-wins).
- Unit tests around the gross profit / dashboard aggregation logic — that's the part most worth
  protecting from a bad refactor.
- Swap the invoice list/search modals for something less 2005 (a proper DevExtreme grid, same
  as the report screen).

## Submission notes

`database/`, `backend/`, `frontend/` are laid out exactly like they'd sit in the repo root, so
this folder can be zipped and submitted as-is, or pushed straight to a new GitHub repo.
