using System.Data;
using BigO.Core.Validation;
using BigO.Data.SqlServer.Extensions;
using Microsoft.Data.SqlClient;

namespace BigO.Data.SqlServer;

public partial class SqlDatabase
{
    /// <summary>
    ///     Executes the Transact-SQL statement or stored procedure and returns a data reader.
    /// </summary>
    /// <param name="commandText">The Transact-SQL statement or stored procedure to execute.</param>
    /// <param name="sqlParameterList">The parameters to use for the Transact-SQL statement or stored procedure.</param>
    /// <param name="commandType">The type of the command. Default value is <see cref="CommandType.StoredProcedure" />.</param>
    /// <param name="commandTimeout">The time in seconds to wait for the command to execute. Default value is 30 seconds.</param>
    /// <returns>A <see cref="SqlDataReader" /> object that can be used to read the result set.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="commandText" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="SqlException">
    ///     Thrown if an error occurs while executing the command.
    /// </exception>
    /// <remarks>
    ///     This method opens a connection to the database, creates a command using the provided parameters, and returns a data
    ///     reader.
    ///     The connection is closed when the reader is closed.
    /// </remarks>
    public SqlDataReader ExecuteReader(string commandText, SqlParameterList? sqlParameterList = null,
        CommandType commandType = CommandType.StoredProcedure, int commandTimeout = 30)
    {
        Guard.NotNullOrWhiteSpace(commandText);

        using var sqlConnection = CreateConnection();
        sqlConnection.Open();
        using var command = sqlConnection.CreateSqlCommand(commandText, sqlParameterList, commandType, commandTimeout);
        return command.ExecuteReader(CommandBehavior.CloseConnection);
    }

    /// <summary>
    ///     Executes the Transact-SQL statement or stored procedure and returns a data reader.
    /// </summary>
    /// <param name="sqlConnection">The <see cref="SqlConnection" /> to use for the command.</param>
    /// <param name="commandText">The Transact-SQL statement or stored procedure to execute.</param>
    /// <param name="sqlParameterList">The parameters to use for the Transact-SQL statement or stored procedure.</param>
    /// <param name="commandType">The type of the command. Default value is <see cref="CommandType.StoredProcedure" />.</param>
    /// <param name="commandTimeout">The time in seconds to wait for the command to execute. Default value is 30 seconds.</param>
    /// <returns>A <see cref="SqlDataReader" /> object that can be used to read the result set.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="sqlConnection" /> or <paramref name="commandText" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="SqlException">
    ///     Thrown if an error occurs while executing the command.
    /// </exception>
    /// <remarks>
    ///     This method opens the <paramref name="sqlConnection" /> if it is not already open, and closes it when the
    ///     <see cref="SqlDataReader" /> is closed.
    /// </remarks>
    public static SqlDataReader ExecuteReader(SqlConnection sqlConnection, string commandText,
        SqlParameterList? sqlParameterList = null, CommandType commandType = CommandType.StoredProcedure,
        int commandTimeout = 30)
    {
        Guard.NotNull(sqlConnection);
        Guard.NotNullOrWhiteSpace(commandText);

        using var command = sqlConnection.CreateSqlCommand(commandText, sqlParameterList, commandType, commandTimeout);
        var connectionWasOpened = sqlConnection.OpenIfNot();
        var reader = connectionWasOpened
            ? command.ExecuteReader(CommandBehavior.CloseConnection)
            : command.ExecuteReader();
        return reader;
    }

    /// <summary>
    ///     Executes a command and returns a SqlDataReader.
    /// </summary>
    /// <param name="sqlTransaction">The <see cref="SqlTransaction" /> to use for the command.</param>
    /// <param name="commandText">The command text to execute.</param>
    /// <param name="sqlParameterList">An optional <see cref="SqlParameterList" /> to use for the command.</param>
    /// <param name="commandType">The type of the command. The default is <see cref="CommandType.StoredProcedure" />.</param>
    /// <param name="commandTimeout">The number of seconds to wait before terminating the command. The default is 30 seconds.</param>
    /// <returns>A <see cref="SqlDataReader" /> object.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="sqlTransaction" /> or
    ///     <paramref name="commandText" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="SqlException">Thrown when there is an error executing the command.</exception>
    /// <remarks>
    ///     This method opens the <see cref="SqlTransaction.Connection" /> if it is not already open, and closes it when the
    ///     <see cref="SqlDataReader" /> is closed.
    /// </remarks>
    public static SqlDataReader ExecuteReader(SqlTransaction sqlTransaction, string commandText,
        SqlParameterList? sqlParameterList = null, CommandType commandType = CommandType.StoredProcedure,
        int commandTimeout = 30)
    {
        Guard.NotNull(sqlTransaction);
        Guard.NotNullOrWhiteSpace(commandText);

        using var command =
            sqlTransaction.CreateSqlCommand(commandText, sqlParameterList, commandType, commandTimeout);
        var connectionWasOpened = sqlTransaction.Connection.OpenIfNot();
        var reader = connectionWasOpened
            ? command.ExecuteReader(CommandBehavior.CloseConnection)
            : command.ExecuteReader();
        return reader;
    }
}