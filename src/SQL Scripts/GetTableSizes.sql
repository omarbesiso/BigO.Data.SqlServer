CREATE PROCEDURE [dbo].[GetTableSizes]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT [T].[name] AS [TableName],
           SUM([P].[reserved_page_count]) * 8.0 AS "size in KB",
           (SUM([P].[reserved_page_count]) * 8.0) / 1024 AS "size in MB"
    FROM [sys].[tables] AS [T]
        INNER JOIN [sys].[dm_db_partition_stats] AS [P]
            ON [P].[object_id] = [T].[object_id]
    WHERE NOT (LEFT([T].[name], 3) = 'sys')
          AND NOT (LEFT([T].[name], 10) = 'filestream')
          AND NOT (LEFT([T].[name], 9) = 'filetable')
          AND NOT (LEFT([T].[name], 5) = 'queue')
          AND NOT (LEFT([T].[name], 8) = 'sqlagent')
    GROUP BY [T].[schema_id],
             [T].[name]
    ORDER BY "size in MB" DESC;
END;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Get a list of all the tables in the database and their sizes.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'GetTableSizes'
GO


