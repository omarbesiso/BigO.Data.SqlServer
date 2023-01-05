using System.Data;
using BigO.Core.Validation;
using BigO.Data.SqlServer.Extensions;
using BigO.Data.SqlServer.Internals;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;

namespace BigO.Data.SqlServer;

/// <summary>
///     Class with useful Sql Server utilities.
/// </summary>
[PublicAPI]
public static class SqlServerUtilities
{
    /// <summary>
    ///     Drops a database if it exists.
    /// </summary>
    /// <param name="masterDatabaseConnectionString">The connection string to the <c>master</c> database.</param>
    /// <param name="databaseName">The name of the database to be dropped.</param>
    /// <exception cref="SqlException">Thrown when there is an error executing the SQL command.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the connection is already open.</exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="masterDatabaseConnectionString" /> or
    ///     <paramref name="databaseName" /> is <c>null</c> or white space.
    /// </exception>
    /// <remarks>
    ///     This method will set the database to single user mode and then drop it.
    ///     If the database does not exist, this method will do nothing.
    /// </remarks>
    public static void DropDatabaseIfExists(string masterDatabaseConnectionString, string databaseName)
    {
        Guard.NotNullOrWhiteSpace(masterDatabaseConnectionString);
        Guard.NotNullOrWhiteSpace(databaseName);

        using var connection = new SqlConnection(masterDatabaseConnectionString);
        var commandText = string.Format(SqlCommands.DropDatabaseCommand, databaseName);
        using var command = connection.CreateSqlCommand(commandText, commandType: CommandType.Text);
        connection.Open();
        command.ExecuteNonQuery();
    }

    /// <summary>
    ///     Closes all active connections to the specified database.
    /// </summary>
    /// <param name="masterDatabaseConnectionString">The connection string to the <c>master</c> database.</param>
    /// <param name="databaseName">The name of the database to close active connections to.</param>
    /// <exception cref="SqlException">Thrown when there is an error executing the SQL command.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the connection is already open.</exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="masterDatabaseConnectionString" /> or
    ///     <paramref name="databaseName" /> is <c>null</c> or white space.
    /// </exception>
    /// <remarks>
    ///     This method will set the database to single user mode, which will close all active connections to the database.
    ///     If the database does not exist, this method will do nothing.
    /// </remarks>
    public static void CloseActiveDatabaseConnections(string masterDatabaseConnectionString, string databaseName)
    {
        Guard.NotNullOrWhiteSpace(masterDatabaseConnectionString);
        Guard.NotNullOrWhiteSpace(databaseName);

        using var connection = new SqlConnection(masterDatabaseConnectionString);
        var commandText = string.Format(SqlCommands.CloseDatabaseConnectionsCommand, databaseName);
        using var command = connection.CreateSqlCommand(commandText, commandType: CommandType.Text);
        connection.Open();
        command.ExecuteNonQuery();
    }

    /// <summary>
    ///     Determines whether the specified database exists.
    /// </summary>
    /// <param name="masterDatabaseConnectionString">The connection string to the <c>master</c> database.</param>
    /// <param name="databaseName">The name of the database to check for existence.</param>
    /// <returns>True if the database exists, false otherwise.</returns>
    /// <exception cref="SqlException">Thrown when there is an error executing the SQL command.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the connection is already open.</exception>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="masterDatabaseConnectionString" /> or
    ///     <paramref name="databaseName" /> is <c>null</c> or white space.
    /// </exception>
    /// <remarks>
    ///     This method executes a SQL query to check for the existence of the specified database.
    /// </remarks>
    public static bool DatabaseExists(string masterDatabaseConnectionString, string databaseName)
    {
        Guard.NotNullOrWhiteSpace(masterDatabaseConnectionString);
        Guard.NotNullOrWhiteSpace(databaseName);

        using var connection = new SqlConnection(masterDatabaseConnectionString);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = $"SELECT Count(*) FROM sys.databases WHERE [name]='{databaseName}'";
        connection.Open();
        var count = Convert.ToInt32(command.ExecuteScalar());
        return count > 0;
    }

    /// <summary>
    ///     Gets the edition of the SQL Server engine.
    /// </summary>
    /// <param name="connectionString">The connection string to the SQL Server instance.</param>
    /// <returns>The <see cref="SqlEngineEdition" /> of the SQL Server instance.</returns>
    /// <exception cref="SqlException">Thrown when there is an error executing the SQL command.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the connection is already open.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="connectionString" /> is <c>null</c> or white space.</exception>
    /// <remarks>
    ///     This method executes a SQL command to get the edition of the SQL Server engine.
    ///     If the edition cannot be parsed, the method will return <see cref="SqlEngineEdition.Unknown" />.
    /// </remarks>
    public static SqlEngineEdition GetEngineEdition(string connectionString)
    {
        Guard.NotNullOrWhiteSpace(connectionString);

        int engineEdition;
        using (var connection = new SqlConnection(connectionString))
        {
            using var command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT ServerProperty('EngineEdition') AS EngineEdition";
            connection.Open();
            engineEdition = Convert.ToInt32(command.ExecuteScalar());
        }

        if (!Enum.TryParse(engineEdition.ToString(), true, out SqlEngineEdition sqlEngineEdition))
        {
            sqlEngineEdition = SqlEngineEdition.Unknown;
        }

        return sqlEngineEdition;
    }

    /// <summary>
    ///     Gets the properties of the SQL Server instance.
    /// </summary>
    /// <param name="connectionString">The connection string to the SQL Server instance.</param>
    /// <returns>The <see cref="SqlServerProperties" /> of the SQL Server instance.</returns>
    /// <exception cref="SqlException">Thrown when there is an error executing the SQL command.</exception>
    /// <exception cref="InvalidOperationException">Thrown when the connection is already open.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="connectionString" /> is <c>null</c> or white space.</exception>
    /// <remarks>
    ///     This method executes a SQL command to get the properties of the SQL Server instance.
    /// </remarks>
    public static SqlServerProperties? GetSqlServerProperties(string connectionString)
    {
        Guard.NotNullOrWhiteSpace(connectionString);

        SqlServerProperties? sqlServerProperties = null;
        using var connection = new SqlConnection(connectionString);
        using var command = connection.CreateCommand();
        command.CommandType = CommandType.Text;
        command.CommandText = SqlCommands.GetServerProperties;
        connection.Open();
        var reader = command.ExecuteReader(CommandBehavior.CloseConnection);
        if (reader.Read())
        {
            sqlServerProperties = reader.MapObjectFromReader<SqlServerProperties>();
        }

        return sqlServerProperties;
    }
}