CREATE VIEW [vViewColumns]
AS
SELECT [V].[object_id] AS [ViewId],
       [V].[name] AS [ViewName],
       [T].[object_id] AS [TableId],
       [T].[name] AS [TableName],
       [C].[column_id] AS [ColumnId],
       [C].[name] AS [ColumnName],
       [TY].[name] AS [ColumnDataType],
       [C].[max_length] AS [ColumnMaxLength],
       [EP].[value] AS [Description]
FROM [sys].[views] AS [V]
    INNER JOIN [sys].[sql_expression_dependencies] AS [D]
        ON [V].[object_id] = [D].[referencing_id]
    INNER JOIN [sys].[objects] AS [T]
        ON [T].[object_id] = [D].[referenced_id]
    INNER JOIN [sys].[columns] AS [C]
        ON [C].[object_id] = [T].[object_id]
           --AND [C].[column_id] = [D].[referenced_minor_id]
    INNER JOIN [sys].[types] AS [TY]
        ON [C].[system_type_id] = [TY].[user_type_id]
    LEFT JOIN [sys].[extended_properties] [EP]
        ON [T].[object_id] = [EP].[major_id]
           AND [C].[column_id] = [EP].[minor_id]
           AND [EP].[name] = 'MS_Description';


