WITH LatestWell AS
(
    SELECT
        w.*,
        ROW_NUMBER() OVER (
            PARTITION BY w.PlatformId
            ORDER BY w.UpdatedAt DESC
        ) AS RowNum
    FROM Wells w
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
JOIN Platforms p
    ON lw.PlatformId = p.Id
WHERE lw.RowNum = 1;