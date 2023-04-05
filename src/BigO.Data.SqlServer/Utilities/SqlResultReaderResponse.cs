using System.Data;

namespace BigO.Data.SqlServer.Utilities;

/// <summary>
/// Represents the response from a SQL result reader operation.
/// </summary>
public class SqlResultReaderResponse
{
    /// <summary>
    /// Gets or sets the list of <see cref="DataTable"/> objects that contain the schema and data returned by the query.
    /// </summary>
    public List<DataTable> SchemaDataTables { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether the SQL result reader operation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Gets or sets the collection of <see cref="Exception"/> objects that occurred during the SQL result reader operation, if any.
    /// </summary>
    public IEnumerable<Exception>? Errors { get; set; }
}