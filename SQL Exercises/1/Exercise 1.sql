/* 
	1. Select the ID and CompressorName for the following compressors: 'TestComp1' and 'MC109'

	Hint: Filters are done with WHERE clause
*/

SELECT ID, CompressorName 
FROM Compressor
WHERE CompressorName IN ('TestComp1', 'MC109');


/* 
	2. Select the CompressorName for the following compressors with their assigned data points: 'TestComp1' and 'MC109' 
	From the data points, must select ID, [Name] and OrderIndex. Must include only active data points. Result must be sorted by OrderIndex

	Hint: Must do a INNER JOIN to fetch data points related to the compressors
*/
SELECT 
c.CompressorName,
dp.CompressorID,
dp.Name,
dp.OrderIndex
FROM 
Compressor c 
INNER JOIN DataPoint dp ON c.ID = dp.CompressorID
WHERE c.CompressorName IN ('TestComp1', 'MC109') AND dp.IsActive = 1
ORDER BY dp.OrderIndex;

/*
	3. Select the CompressorName and the RecordDate of the submitted entries for the following compressor: 'TestComp1'
	
	Hint: Submitted entries are stored in the CompressorEntryHeader table
*/

SELECT 
c.CompressorName,
ceh.RecordDate
FROM 
Compressor c
INNER JOIN 
CompressorEntryHeader ceh ON c.ID = ceh.CompressorID
WHERE 
c.CompressorName = 'TestComp1';







/*
	4. Select the CompressorName for the following compressor: 'TestComp1', 'MC109'. 
	Based on the submitted entries each compressors has, must include how many entries were submitted per month. Result must be sorted by CompressorName and month

	Hint: Must use GROUP BY clause. SQL Server has functions called YEAR, MONTH and DATEFROMPARTS. Refer to the following links for more information
		- https://learn.microsoft.com/en-us/sql/t-sql/functions/date-and-time-data-types-and-functions-transact-sql?view=sql-server-ver16#GetDateandTimeParts
		- https://learn.microsoft.com/en-us/sql/t-sql/functions/date-and-time-data-types-and-functions-transact-sql?view=sql-server-ver16#fromParts
*/

SELECT 
c.CompressorName,
DATEFROMPARTS(YEAR(ceh.RecordDate), MONTH(ceh.RecordDate), 1) AS EntryMonth,
COUNT(*) AS EntryCount
FROM 
Compressor c
INNER JOIN CompressorEntryHeader ceh on c.ID = ceh.CompressorID
WHERE CompressorName in ('TestComp1', 'MC109')
GROUP BY c.CompressorName, month(ceh.RecordDate), year(ceh.RecordDate)
ORDER BY c.CompressorName, EntryMonth




