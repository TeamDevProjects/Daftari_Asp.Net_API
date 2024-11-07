-----------------------------------------
-----------------[ Views ]---------------
-----------------------------------------
Alter view SectorsView as

SELECT        Sectors.SectorId, Sectors.SectorName,SectorTypes.SectorTypeName
FROM            Sectors INNER JOIN
                         SectorTypes ON Sectors.SectorTypeId = SectorTypes.SectorTypeId Group by SectorTypeName,SectorId,SectorName

select * from SectorsView order by SectorTypeName

select SectorTypeName,SectorId,SectorName from SectorsView Group by SectorTypeName,SectorId,SectorName