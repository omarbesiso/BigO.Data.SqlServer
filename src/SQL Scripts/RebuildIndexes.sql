CREATE PROCEDURE [dbo].[RebuildIndexes]
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TableName NVARCHAR(255);
    DECLARE @SqlCommand NVARCHAR(MAX);

    -- Explicitly declare the cursor as LOCAL
    DECLARE [TableCursor] CURSOR LOCAL FAST_FORWARD
    FOR
        SELECT QUOTENAME(SCHEMA_NAME(t.schema_id)) + '.' + QUOTENAME(t.name) AS [TableName]
        FROM sys.tables AS t
        WHERE t.name <> 'database_firewall_rules';

    OPEN [TableCursor];
    FETCH NEXT FROM [TableCursor] INTO @TableName;

    WHILE @@FETCH_STATUS = 0
    BEGIN
        BEGIN TRY
            SET @SqlCommand = 'ALTER INDEX ALL ON ' + @TableName + ' REBUILD WITH (ONLINE = ON)';
            EXEC sp_executesql @SqlCommand = @SqlCommand;
            PRINT ('Successfully rebuilt indexes on ' + @TableName);
        END TRY
        BEGIN CATCH
            PRINT ('Error occurred while rebuilding indexes on ' + @TableName);
        END CATCH

        FETCH NEXT FROM [TableCursor] INTO @TableName;
    END;

    CLOSE [TableCursor];
    DEALLOCATE [TableCursor];
END;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Rebuilds all user defined indexes in the database as part of regular maintenance.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'RebuildIndexes'
GO
