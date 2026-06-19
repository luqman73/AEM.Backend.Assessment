# AEM Backend Assessment

## Requirements

* .NET 8 SDK
* SQL Server LocalDB

---

## Setup

1. Clone the repository
2. Restore packages
3. Run database migration:

```
Update-Database
```

4. Run the application

---

## Run the API

Open Swagger:

```
https://localhost:<port>/swagger
```

---

## Sync Data

Call the endpoints:

```
POST /api/DataSync/sync-platform-well
POST /api/DataSync/sync-platform-well-dummy
```

---

## SQL Query

Get latest updated well per platform:

```sql
WITH LatestWell AS
(
    SELECT *,
           ROW_NUMBER() OVER (PARTITION BY PlatformId ORDER BY UpdatedAt DESC) AS rn
    FROM Wells
)
SELECT
    p.UniqueName AS PlatformName,
    lw.ExternalId AS Id,
    p.ExternalId AS PlatformId,
    lw.UniqueName,
    lw.Latitude,
    lw.Longitude,
    lw.CreatedAt,
    lw.UpdatedAt
FROM LatestWell lw
JOIN Platforms p ON p.Id = lw.PlatformId
WHERE lw.rn = 1;
```

---

## Run Notes

* Run `Update-Database` before starting the app
* Use Swagger to test API endpoints
