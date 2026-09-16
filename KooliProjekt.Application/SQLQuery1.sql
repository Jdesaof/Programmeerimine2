SELECT 'Olud' AS Tabel, COUNT(*) AS Ridu FROM dbo.Olud
UNION ALL
SELECT 'Partiid', COUNT(*) FROM dbo.Partiid
UNION ALL
SELECT 'Koostisosad', COUNT(*) FROM dbo.Koostisosad
UNION ALL
SELECT 'Maitsmised', COUNT(*) FROM dbo.Maitsmised
UNION ALL
SELECT 'PruulimisLogid', COUNT(*) FROM dbo.PruulimisLogid
UNION ALL
SELECT 'PartiiFotod', COUNT(*) FROM dbo.PartiiFotod;