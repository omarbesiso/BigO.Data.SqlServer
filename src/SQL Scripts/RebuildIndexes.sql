CREATE PROCEDURE [dbo].[RebuildIndexes]
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @TableName VARCHAR(255);

    DECLARE [TableCursor] CURSOR LOCAL FORWARD_ONLY READ_ONLY FOR(
    SELECT '[' + [IST].[TABLE_SCHEMA] + '].[' + [IST].[TABLE_NAME] + ']' AS [TableName]
    FROM [INFORMATION_SCHEMA].[TABLES] AS [IST]
    WHERE [IST].[TABLE_NAME] <> 'database_firewall_rules'
          AND ([IST].[TABLE_TYPE] = 'BASE TABLE'
              --OR [IST].[TABLE_TYPE] = 'VIEW'
              ));

    OPEN [TableCursor];
    FETCH NEXT FROM [TableCursor]
    INTO @TableName;
    WHILE @@FETCH_STATUS = 0
    BEGIN
        PRINT ('Rebuilding Indexes on ' + @TableName);
        DECLARE @sqlCommand NVARCHAR(100) = N'ALTER INDEX ALL ON ' + @TableName + N' REBUILD';
        EXECUTE [sys].[sp_executesql]  @stmt = @sqlCommand;
        FETCH NEXT FROM [TableCursor]
        INTO @TableName;
    END;

    CLOSE [TableCursor];
    DEALLOCATE [TableCursor];
END;