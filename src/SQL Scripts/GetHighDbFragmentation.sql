CREATE PROCEDURE [dbo].[GetHighDbFragmentation]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT DB_NAME() AS [DBName],
           OBJECT_NAME([ps].[object_id]) AS [TableName],
           [i].[name] AS [IndexName],
           [ips].[index_type_desc],
           [ips].[avg_fragmentation_in_percent]
    FROM [sys].[dm_db_partition_stats] AS [ps]
        INNER JOIN [sys].[indexes] AS [i]
            ON [ps].[object_id] = [i].[object_id]
               AND [ps].[index_id] = [i].[index_id]
        CROSS APPLY [sys].[dm_db_index_physical_stats](DB_ID(), [ps].[object_id], [ps].[index_id], NULL, 'LIMITED') AS [ips]
    WHERE [ips].[avg_fragmentation_in_percent] > 10
    ORDER BY [ips].[avg_fragmentation_in_percent] DESC;
END;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Gets the tables in the database with high fragmentation. (Higher than 10 percent).' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'GetHighDbFragmentation'
GO


