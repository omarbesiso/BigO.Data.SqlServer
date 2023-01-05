using System.Collections.Concurrent;
using System.Data;
using BigO.Core.Validation;
using BigO.Data.SqlServer.Extensions;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;

namespace BigO.Data.SqlServer;

/// <summary>
///     Represents a SQL Server database and provides methods to perform operations on the database.
/// </summary>
[PublicAPI]
public partial class SqlDatabase
{
    private static readonly ConcurrentDictionary<string, IEnumerable<SqlParameter>> ParameterCache =
        new();

    /// <summary>
    ///     Initializes a new instance of the <see cref="SqlDatabase" /> class.
    /// </summary>
    /// <param name="connectionString">The connection string to the SQL Server database.</param>
    /// <exception cref="ArgumentNullException"><paramref name="connectionString" /> is <c>null</c> or white space.</exception>
    public SqlDatabase(string connectionString)
    {
        Guard.NotNullOrWhiteSpace(connectionString);
        ConnectionString = connectionString;
    }

    /// <summary>
    ///     Gets the connection string.
    /// </summary>
    public string ConnectionString { get; }

    /// <summary>
    ///     Creates a new <see cref="SqlConnection" /> object to the SQL Server database.
    /// </summary>
    /// <param name="sqlCredential">The optional <see cref="SqlCredential" /> object to use for the connection.</param>
    /// <returns>A new <see cref="SqlConnection" /> object to the SQL Server database.</returns>
    /// <exception cref="ArgumentException">Thrown when the connection string is invalid.</exception>
    /// <remarks>
    ///     This method creates a new <see cref="SqlConnection" /> object to the SQL Server database.
    ///     The <see cref="SqlConnection" /> object can optionally be created using a <see cref="SqlCredential" /> object for
    ///     authentication.
    /// </remarks>
    public SqlConnection CreateConnection(SqlCredential? sqlCredential = null)
    {
        return sqlCredential == null
            ? new SqlConnection(ConnectionString, sqlCredential)
            : new SqlConnection(ConnectionString);
    }

    /// <summary>
    ///     Creates a new <see cref="SqlTransaction" /> object.
    /// </summary>
    /// <param name="isolationLevel">An optional <see cref="IsolationLevel" /> for the transaction.</param>
    /// <param name="transactionName">An optional name for the transaction.</param>
    /// <returns>A <see cref="SqlTransaction" /> object.</returns>
    /// <exception cref="SqlException">
    ///     Thrown when there is an error creating the transaction OR Parallel transactions are not
    ///     allowed when using Multiple Active Result Sets (MARS).
    /// </exception>
    /// <exception cref="InvalidOperationException">Parallel transactions are not supported.</exception>
    /// <remarks>
    ///     This method creates a new <see cref="SqlConnection" /> and begins a transaction on it.
    ///     If <paramref name="transactionName" /> is provided, it is used to name the transaction.
    ///     If <paramref name="isolationLevel" /> is provided, it is used to set the isolation level of the transaction.
    /// </remarks>
    public SqlTransaction CreateTransaction(IsolationLevel? isolationLevel = null, string? transactionName = null)
    {
        var connection = CreateConnection();
        SqlTransaction transaction;
        if (!string.IsNullOrWhiteSpace(transactionName))
        {
            transaction = isolationLevel.HasValue
                ? connection.BeginTransaction(isolationLevel.Value, transactionName)
                : connection.BeginTransaction(transactionName);
        }
        else
        {
            transaction = isolationLevel.HasValue
                ? connection.BeginTransaction(isolationLevel.Value)
                : connection.BeginTransaction();
        }

        return transaction;
    }

    /// <summary>
    ///     Creates a new <see cref="SqlCommand" /> object to execute on the SQL Server database.
    /// </summary>
    /// <param name="commandText">The command text to execute on the database.</param>
    /// <param name="sqlParameterList">The optional list of <see cref="SqlParameter" /> objects to use with the command.</param>
    /// <param name="commandType">
    ///     The optional <see cref="CommandType" /> of the command. Default is
    ///     <see cref="CommandType.StoredProcedure" />.
    /// </param>
    /// <param name="commandTimeout">The optional command timeout in seconds. Default is 30 seconds.</param>
    /// <returns>A new <see cref="SqlCommand" /> object to execute on the SQL Server database.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="commandText" /> is <c>null</c> or white space.</exception>
    /// <remarks>
    ///     This method creates a new <see cref="SqlCommand" /> object to execute on the SQL Server database using the
    ///     specified parameters.
    ///     The <see cref="SqlCommand" /> object can be executed using a <see cref="SqlConnection" /> object.
    /// </remarks>
    public SqlCommand CreateCommand(string commandText, IEnumerable<SqlParameter>? sqlParameterList = null,
        CommandType commandType = CommandType.StoredProcedure, int commandTimeout = 30)
    {
        return SqlCommandFactory.CreateSqlCommand(commandText, sqlParameterList, commandType, commandTimeout);
    }

    /// <summary>
    ///     Creates a new <see cref="SqlCommand" /> object to execute on the SQL Server database.
    /// </summary>
    /// <param name="sqlConnection">The <see cref="SqlConnection" /> object to use with the command.</param>
    /// <param name="commandText">The command text to execute on the database.</param>
    /// <param name="sqlParameterList">The optional list of <see cref="SqlParameter" /> objects to use with the command.</param>
    /// <param name="commandType">
    ///     The optional <see cref="CommandType" /> of the command. Default is
    ///     <see cref="CommandType.StoredProcedure" />.
    /// </param>
    /// <param name="commandTimeout">The optional command timeout in seconds. Default is 30 seconds.</param>
    /// <returns>A new <see cref="SqlCommand" /> object to execute on the SQL Server database.</returns>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="sqlConnection" /> or <paramref name="commandText" /> is
    ///     <c>null</c> or white space.
    /// </exception>
    /// <remarks>
    ///     This method creates a new <see cref="SqlCommand" /> object to execute on the SQL Server database using the
    ///     specified parameters.
    ///     The <see cref="SqlCommand" /> object is associated with the specified <see cref="SqlConnection" /> object.
    /// </remarks>
    public SqlCommand CreateCommand(SqlConnection sqlConnection, string commandText,
        IEnumerable<SqlParameter>? sqlParameterList = null, CommandType commandType = CommandType.StoredProcedure,
        int commandTimeout = 30)
    {
        return SqlCommandFactory.CreateSqlCommand(sqlConnection, commandText, sqlParameterList, commandType,
            commandTimeout);
    }

    /// <summary>
    ///     Creates a new <see cref="SqlCommand" /> object that is associated with the <see cref="SqlTransaction" /> provided.
    /// </summary>
    /// <param name="sqlTransaction">
    ///     The <see cref="SqlTransaction" /> to associate the new <see cref="SqlCommand" /> object
    ///     with.
    /// </param>
    /// <param name="commandText">The text command to run.</param>
    /// <param name="sqlParameterList">The list of <see cref="SqlParameter" /> objects to use with the command.</param>
    /// <param name="commandType">The type of command to execute. The default is <see cref="CommandType.StoredProcedure" />.</param>
    /// <param name="commandTimeout">The time in seconds to wait for the command to execute. The default is 30 seconds.</param>
    /// <exception cref="ArgumentNullException"><paramref name="sqlTransaction" /> is <c>null</c> or white space.</exception>
    /// <returns>A new <see cref="SqlCommand" /> object that is associated with the <see cref="SqlTransaction" /> provided.</returns>
    public SqlCommand CreateCommand(SqlTransaction sqlTransaction, string commandText,
        IEnumerable<SqlParameter>? sqlParameterList = null, CommandType commandType = CommandType.StoredProcedure,
        int commandTimeout = 30)
    {
        return SqlCommandFactory.CreateSqlCommand(sqlTransaction, commandText, sqlParameterList, commandType,
            commandTimeout);
    }

    /// <summary>
    ///     Retrieves the edition of the SQL Server engine being used.
    /// </summary>
    /// <returns>The edition of the SQL Server engine being used.</returns>
    /// <remarks>
    ///     This method uses the <see cref="SqlServerUtilities.GetEngineEdition" /> method to retrieve the edition of the SQL
    ///     Server engine being used.
    /// </remarks>
    public SqlEngineEdition GetEngineEdition()
    {
        return SqlServerUtilities.GetEngineEdition(ConnectionString);
    }

    /// <summary>
    ///     Returns the server properties of the database.
    /// </summary>
    /// <returns>The server properties of the database.</returns>
    public SqlServerProperties? GetSqlServerProperties()
    {
        return SqlServerUtilities.GetSqlServerProperties(ConnectionString);
    }

    /// <summary>
    ///     Gets a list of parameters for a stored procedure from the database server.
    ///     This method is useful for obtaining the list of parameters for a stored procedure
    ///     without having to know the parameter names or types in advance.
    /// </summary>
    /// <param name="procedureName">
    ///     The name of the stored procedure to get parameters for.
    ///     This value cannot be <c>null</c> or whitespace.
    /// </param>
    /// <param name="includeReturnValueParameter">
    ///     A value indicating whether to include the return value parameter in the list of parameters.
    ///     The default value is <c>false</c>, meaning that the return value parameter is not included.
    /// </param>
    /// <returns>A list of <see cref="SqlParameter" /> objects representing the parameters of the stored procedure.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown if the <paramref name="procedureName" /> is <c>null</c> or whitespace.
    /// </exception>
    /// <exception cref="SqlException">
    ///     Thrown if there is a problem connecting to the database or if the stored procedure does not exist.
    /// </exception>
    public SqlParameterList GetProcedureParameters(string procedureName, bool includeReturnValueParameter = false)
    {
        Guard.NotNullOrWhiteSpace(procedureName, nameof(procedureName));

        if (!ParameterCache.ContainsKey(procedureName))
        {
            using var connection = CreateConnection();
            using var command =
                connection.CreateSqlCommand(procedureName, commandType: CommandType.StoredProcedure);

            connection.Open();
            SqlCommandBuilder.DeriveParameters(command);
            connection.Close();

            if (!includeReturnValueParameter)
            {
                command.Parameters.RemoveAt(0);
            }

            var derivedParameters = new SqlParameter[command.Parameters.Count];
            command.Parameters.CopyTo(derivedParameters, 0);
            foreach (var derivedParameter in derivedParameters)
            {
                derivedParameter.Value = DBNull.Value;
            }

            ParameterCache[procedureName] = derivedParameters;
        }

        var cachedParameters = ParameterCache[procedureName].ToArray();
        var outputParameters = new SqlParameter[cachedParameters.Length];
        cachedParameters.CopyTo(outputParameters, 0);
        var list = new SqlParameterList(outputParameters);
        return list;
    }
}