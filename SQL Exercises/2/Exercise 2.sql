/* 
	1. Create a stored procedure called 'sp_Tristan_GetCompressorMonthlyEntryCount'. The result set must be the answer #4 in Exercise #1
*/


CREATE Procedure sp_Tristan_GetCompressorMonthlyEntryCount 
AS
BEGIN
	SELECT 
	c.CompressorName,
	DATEFROMPARTS(YEAR(ceh.RecordDate), MONTH(ceh.RecordDate), 1) AS EntryMonth,
	COUNT(*) AS EntryCount
	FROM 
	Compressor c
	INNER JOIN CompressorEntryHeader ceh on c.ID = ceh.CompressorID
	WHERE CompressorName in ('TestComp1', 'MC109')
	GROUP BY c.CompressorName, month(ceh.RecordDate), year(ceh.RecordDate)
	ORDER BY c.CompressorName, EntryMonth;
END

SELECT * 
FROM sys.procedures 
WHERE name = 'sp_Tristan_GetCompressorMonthlyEntryCount';

EXEC sp_Tristan_GetCompressorMonthlyEntryCount;







/*
	2. Select the CompressorName and latest oil change for the following compressors:  'TestComp1' and 'MC109'

	Hint: You can use either CTE, sub-query or temporaty table to fetch latest oil change.
*/

SELECT 
	c.CompressorName,
	(SELECT MAX(StartDate)
	From OilChange
	Where CompressorID = c.ID and IsActive =1 ) AS LatestOilChange
FROM Compressor c
WHERE c.CompressorName IN ('TestComp1', 'MC109') 





/*
	3. Based on the submitted entries. Select CompressorName, data point name and the highest (and latest) reading with the entry date for each data point for the following compressor: 'TestComp1'. 
	Must include data points that only accepts numeric values (Data point type 'Engine Data'  and 'Compressor Data')

	Hint: Data point readings are stored in the table CompressorEntryDetail for each submitted entries. Must use CAST to convert reading from NVARCHAR to FLOAT
		- https://learn.microsoft.com/en-us/sql/t-sql/functions/cast-and-convert-transact-sql?view=sql-server-ver16
*/

/*
CompressorName from Compressor tb
DataPoint name from DataPoint tb
DataPoint readings (highest and lowest) from CompressorEntryDetail tb
Entry Date from CompressorEntryHeader tb

*/

-- 1'st way of doing question 3

/*
QUERY EXPLANATION

		JOIN CompressorEntryHeader ceh ON c.ID = ceh.CompressorID:
			We Join these two to get the entry headers related to the compressor as we need the 'Testcomp1' and its id which correlates to the header table
		JOIN CompressorEntryDetail ced ON ceh.ID = ced.CompressorEntryHeaderID:
			We join these two to get the readings, entry detail inherits from entry header, "the compressor id" and entry detail contains the readings
		JOIN DataPoint dp ON ced.DataPointID = dp.ID
			Purpose: Links detailed readings (CompressorEntryDetail) to their respective data points (DataPoint table).
			Role:
				Provides context for the readings by associating them with their corresponding data points (e.g., RPM, Min RPM).
				Allows filtering based on data point properties.
		JOIN DataPointType dpt ON dp.DataPointTypeID = dpt.ID
			Purpose: Links each data point (DataPoint) to its type (DataPointType table).
			Role:
				Filters readings to include only specific types of data points (Engine Data, Compressor Data).
				Provides additional context about the nature of each data point.

Sub-Query Explanation:
		Purpose: Ensures the date associated with the maximum reading is the most recent.
		Role:
			Subquery finds the latest date for each maximum reading for each data point.
			Matches the date in the main query to this latest date to ensure correctness.
		Example:
		If TestComp1 has an RPM reading of 1500 recorded on multiple dates, this subquery ensures we get the latest date for the reading (e.g., 2022-10-04).

		Inner Select:
			SELECT MAX(ceh2.RecordDate): Finds the latest date for each maximum reading.
		Joins:
			JOIN CompressorEntryHeader ceh2 ON ced2.CompressorEntryHeaderID = ceh2.ID: Links entry details to their headers within the subquery.
		Conditions:
			ceh2.CompressorID = c.ID: Matches the compressor in the subquery with the current compressor in the main query.
			ced2.DataPointID = dp.ID: Matches the data point in the subquery with the current data point in the main query.
			CAST(ced2.Reading AS FLOAT) = CAST(ced.Reading AS FLOAT): Ensures the subquery is finding the latest date for the same maximum reading as in the main query.
		Result
			This subquery returns the latest date for the maximum reading for each data point. The main query then filters the records to only include those with this latest date (ceh.RecordDate =).
*/
SELECT
    c.CompressorName,
    dp.Name AS DataPointName,
    CAST(ced.Reading AS FLOAT) AS MaxReading,
    ceh.RecordDate AS EntryDate
FROM
    Compressor c
    JOIN CompressorEntryHeader ceh ON c.ID = ceh.CompressorID
    JOIN CompressorEntryDetail ced ON ceh.ID = ced.CompressorEntryHeaderID
    JOIN DataPoint dp ON ced.DataPointID = dp.ID
    JOIN DataPointType dpt ON dp.DataPointTypeID = dpt.ID
WHERE
    c.CompressorName = 'TestComp1'
    AND dpt.Name IN ('Engine Data', 'Compressor Data')
    AND ISNUMERIC(ced.Reading) = 1
    AND CAST(ced.Reading AS FLOAT) = (
        SELECT MAX(CAST(ced2.Reading AS FLOAT))
        FROM CompressorEntryDetail ced2
        JOIN CompressorEntryHeader ceh2 ON ced2.CompressorEntryHeaderID = ceh2.ID
        JOIN DataPoint dp2 ON ced2.DataPointID = dp2.ID
        JOIN DataPointType dpt2 ON dp2.DataPointTypeID = dpt2.ID
        WHERE
            ceh2.CompressorID = c.ID
            AND dp2.ID = dp.ID
            AND dpt2.Name IN ('Engine Data', 'Compressor Data')
            AND ISNUMERIC(ced2.Reading) = 1
    )
    AND ceh.RecordDate = (
        SELECT MAX(ceh2.RecordDate)
        FROM CompressorEntryDetail ced2
        JOIN CompressorEntryHeader ceh2 ON ced2.CompressorEntryHeaderID = ceh2.ID
        WHERE
            ceh2.CompressorID = c.ID
            AND ced2.DataPointID = dp.ID
            AND CAST(ced2.Reading AS FLOAT) = CAST(ced.Reading AS FLOAT)
    )
ORDER BY
    c.CompressorName,
    dp.Name;


-- 2nd way of doing question 3, also solves question 2
WITH LatestOilChange AS (
	SELECT CompressorID, MAX(StartDate) AS 'OilChangeDate'
	FROM dbo.OilChange
	GROUP BY CompressorID
)

SELECT CompressorName, OilChangeDate
FROM dbo.Compressor c JOIN LatestOilChange l ON c.ID = l.CompressorID
WHERE CompressorName = 'TestComp1' OR CompressorName = 'MC109';
WITH TestComp1Info AS ( --merged table of compressor and datapoint info
	SELECT CompressorName, dp.CompressorID, ced.DataPointID, dp.Name AS dpName, RecordDate, Reading AS Reading
	FROM dbo.Compressor c
	JOIN dbo.DataPoint dp ON c.ID = dp.CompressorID
	JOIN dbo.DataPointType dpt ON dp.DataPointTypeID = dpt.ID
	JOIN dbo.CompressorEntryDetail ced ON dp.ID = ced.DataPointID
	JOIN dbo.CompressorEntryHeader ceh ON ced.CompressorEntryHeaderID = ceh.ID
	WHERE CompressorName = 'TestComp1' AND (dpt.Name = 'Engine Data' OR dpt.Name = 'Compressor Data')
), HighestReading AS ( --highest reading for each datapoint name
	SELECT CompressorName, dpName, MAX(CAST(Reading AS float)) AS 'ReadingHighest'
	FROM TestComp1Info
	GROUP BY CompressorName, dpName
), AllMaxes AS ( --table with duplicate dates for max reading in the same datapoint name
SELECT hr.CompressorName, hr.dpName AS 'dpName', Reading, RecordDate FROM TestComp1Info
CROSS JOIN HighestReading hr
WHERE Reading = CAST(ReadingHighest AS nvarchar)
), MaxDates AS ( --table with max date for each datapoint name from AllMaxes
SELECT CompressorName, dpName, MAX(RecordDate) AS MaxRecordDate FROM AllMaxes
GROUP BY CompressorName, dpName)
SELECT a.CompressorName, a.dpName, Reading, RecordDate FROM AllMaxes a
JOIN MaxDates m ON a.CompressorName = m.CompressorName AND a.dpName = m.dpName
WHERE RecordDate = MaxRecordDate;



--3rd way of doing question 3
WITH TestComp1Info AS ( --merged table of compressor and datapoint info
	SELECT CompressorName, dp.CompressorID, ced.DataPointID, dp.Name AS dpName, RecordDate, Reading AS Reading
	FROM dbo.Compressor c
	JOIN dbo.DataPoint dp ON c.ID = dp.CompressorID
	JOIN dbo.DataPointType dpt ON dp.DataPointTypeID = dpt.ID
	JOIN dbo.CompressorEntryDetail ced ON dp.ID = ced.DataPointID
	JOIN dbo.CompressorEntryHeader ceh ON ced.CompressorEntryHeaderID = ceh.ID
	WHERE CompressorName = 'TestComp1' AND (dpt.Name = 'Engine Data' OR dpt.Name = 'Compressor Data')
), HighestReading AS ( --highest reading for each datapoint name
	SELECT CompressorName, dpName, MAX(CAST(Reading AS float)) AS 'ReadingHighest'
	FROM TestComp1Info
	GROUP BY CompressorName, dpName)
SELECT DISTINCT hr.CompressorName, hr.dpName AS 'dpName', Reading, MAX(RecordDate) OVER (PARTITION BY hr.dpName) FROM TestComp1Info
CROSS JOIN HighestReading hr
WHERE Reading = CAST(ReadingHighest AS nvarchar)