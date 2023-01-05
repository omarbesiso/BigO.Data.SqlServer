CREATE PROCEDURE [dbo].[GetDatabaseSize]
AS
BEGIN
    SET NOCOUNT ON;

    SELECT SUM([dm_db_partition_stats].[reserved_page_count]) * 8.0 / 1024 AS "size in MB"
    FROM [sys].[dm_db_partition_stats];
END;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Get the size of the database on which it is executed.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'GetDatabaseSize'
GO


