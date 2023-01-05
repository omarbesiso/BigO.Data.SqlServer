CREATE PROCEDURE [dbo].[GetAllForeignKeys]
AS
BEGIN

    SET NOCOUNT ON;

    SELECT [PKTABLE_QUALIFIER] = CONVERT(sysname, DB_NAME()),
           [PKTABLE_OWNER] = CONVERT(sysname, SCHEMA_NAME([O1].[schema_id])),
           [PKTABLE_NAME] = CONVERT(sysname, [O1].[name]),
           [PKCOLUMN_NAME] = CONVERT(sysname, [C1].[name]),
           [FKTABLE_QUALIFIER] = CONVERT(sysname, DB_NAME()),
           [FKTABLE_OWNER] = CONVERT(sysname, SCHEMA_NAME([O2].[schema_id])),
           [FKTABLE_NAME] = CONVERT(sysname, [O2].[name]),
           [FKCOLUMN_NAME] = CONVERT(sysname, [C2].[name]),
                                                  -- Force the column to be non-nullable (see SQL BU 325751)
                                                  --KEY_SEQ             = isnull(convert(smallint,k.constraint_column_id), sysconv(smallint,0)),
           [UPDATE_RULE] = CONVERT(   SMALLINT,
                                      CASE OBJECTPROPERTY([F].[object_id], 'CnstIsUpdateCascade')
                                          WHEN 1 THEN
                                              0
                                          ELSE
                                              1
                                      END
                                  ),
           [DELETE_RULE] = CONVERT(   SMALLINT,
                                      CASE OBJECTPROPERTY([F].[object_id], 'CnstIsDeleteCascade')
                                          WHEN 1 THEN
                                              0
                                          ELSE
                                              1
                                      END
                                  ),
           [FK_NAME] = CONVERT(sysname, OBJECT_NAME([F].[object_id])),
           [PK_NAME] = CONVERT(sysname, [I].[name]),
           [DEFERRABILITY] = CONVERT(SMALLINT, 7) -- SQL_NOT_DEFERRABLE
    FROM [sys].[all_objects] AS [O1]
        INNER JOIN [sys].[foreign_keys] AS [F]
            ON [O1].[object_id] = [F].[referenced_object_id]
        INNER JOIN [sys].[all_objects] AS [O2]
            ON [O2].[object_id] = [F].[parent_object_id]
        INNER JOIN [sys].[all_columns] AS [C1]
            ON [C1].[object_id] = [F].[referenced_object_id]
        INNER JOIN [sys].[all_columns] AS [C2]
            ON [C2].[object_id] = [F].[parent_object_id]
        INNER JOIN [sys].[foreign_key_columns] AS [K]
            ON ([K].[constraint_object_id] = [F].[object_id])
        INNER JOIN [sys].[indexes] AS [I]
            ON (
                   [F].[referenced_object_id] = [I].[object_id]
                   AND [F].[key_index_id] = [I].[index_id]
               )
    WHERE [O1].[object_id] = [F].[referenced_object_id]
          AND [O2].[object_id] = [F].[parent_object_id]
          AND [C1].[object_id] = [F].[referenced_object_id]
          AND [C2].[object_id] = [F].[parent_object_id]
          AND [C1].[column_id] = [K].[referenced_column_id]
          AND [C2].[column_id] = [K].[parent_column_id];

END;
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Retrieves all the foreign keys in the database.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'PROCEDURE',@level1name=N'GetAllForeignKeys'
GO


