using System.Data;
using BigO.Core.Validation;
using BigO.Data.SqlServer.Extensions;
using Microsoft.Data.SqlClient;

namespace BigO.Data.SqlServer;

public partial class SqlDatabase
{
    /// <summary>
    ///     Executes a command and returns the first column of the first row of the result set.
    /// </summary>
    /// <param name="commandText">The command text to execute.</param>
    /// <param name="sqlParameterList">An optional <see cref="SqlParameterList" /> to use for the command.</param>
    /// <param name="commandType">The type of the command. The default is <see cref="CommandType.StoredProcedure" />.</param>
    /// <param name="commandTimeout">The number of seconds to wait before terminating the command. The default is 30 seconds.</param>
    /// <returns>The value of the first column of the first row of the result set, or <c>null</c> if there are no rows.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="commandText" /> is <c>null</c> or white space.</exception>
    /// <exception cref="SqlException">Thrown when there is an error executing the command.</exception>
    /// <remarks>
    ///     This method creates a new <see cref="SqlConnection" />, opens it, and closes it after executing the command.
    /// </remarks>
    public object ExecuteScalar(string commandText, SqlParameterList? sqlParameterList = null,
        CommandType commandType = CommandType.StoredProcedure, int commandTimeout = 30)
    {
        Guard.NotNullOrWhiteSpace(commandText);

        using var sqlConnection = CreateConnection();
        using var command = sqlConnection.CreateSqlCommand(commandText, sqlParameterList, commandType, commandTimeout);
        sqlConnection.Open();
        return command.ExecuteScalar();
    }

    /// <summary>
    ///     Executes a command and returns the first column of the first row of the result set.
    /// </summary>
    /// <param name="sqlConnection">The <see cref="SqlConnection" /> to use for the command.</param>
    /// <param name="commandText">The command text to execute.</param>
    /// <param name="sqlParameterList">An optional collection of <see cref="SqlParameter" /> objects to use for the command.</param>
    /// <param name="commandType">The type of the command. The default is <see cref="CommandType.StoredProcedure" />.</param>
    /// <param name="commandTimeout">The number of seconds to wait before terminating the command. The default is 30 seconds.</param>
    /// <returns>The value of the first column of the first row of the result set, or <c>null</c> if there are no rows.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="sqlConnection" /> or
    ///     <paramref name="commandText" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="SqlException">Thrown when there is an error executing the command.</exception>
    /// <remarks>
    ///     This method opens the <paramref name="sqlConnection" /> if it is not already open, and closes it if it was opened
    ///     by this method.
    /// </remarks>
    public object ExecuteScalar(SqlConnection sqlConnection, string commandText,
        IEnumerable<SqlParameter>? sqlParameterList = null, CommandType commandType = CommandType.StoredProcedure,
        int commandTimeout = 30)
    {
        Guard.NotNull(sqlConnection);
        Guard.NotNullOrWhiteSpace(commandText);

        using var command = sqlConnection.CreateSqlCommand(commandText, sqlParameterList, commandType, commandTimeout);
        var connectionWasOpened = sqlConnection.OpenIfNot();
        var result = command.ExecuteScalar();
        if (connectionWasOpened)
        {
            sqlConnection.Close();
        }

        return result;
    }

    /// <summary>
    ///     Executes a command and returns the first column of the first row of the result set.
    /// </summary>
    /// <param name="sqlTransaction">The <see cref="SqlTransaction" /> to use for the command.</param>
    /// <param name="commandText">The command text to execute.</param>
    /// <param name="sqlParameterList">An optional <see cref="SqlParameterList" /> to use for the command.</param>
    /// <param name="commandType">The type of the command. The default is <see cref="CommandType.StoredProcedure" />.</param>
    /// <param name="commandTimeout">The number of seconds to wait before terminating the command. The default is 30 seconds.</param>
    /// <returns>The value of the first column of the first row of the result set, or <c>null</c> if there are no rows.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when <paramref name="sqlTransaction" /> or
    ///     <paramref name="commandText" /> is <c>null</c>.
    /// </exception>
    /// <exception cref="SqlException">Thrown when there is an error executing the command.</exception>
    /// <remarks>
    ///     This method opens the <see cref="SqlTransaction.Connection" /> if it is not already open, and closes it if it was
    ///     opened by this method.
    /// </remarks>
    public object ExecuteScalar(SqlTransaction sqlTransaction, string commandText,
        SqlParameterList? sqlParameterList = null, CommandType commandType = CommandType.StoredProcedure,
        int commandTimeout = 30)
    {
        using var command =
            sqlTransaction.CreateSqlCommand(commandText, sqlParameterList, commandType, commandTimeout);
        var connectionWasOpened = sqlTransaction.Connection.OpenIfNot();
        var result = command.ExecuteScalar();
        if (connectionWasOpened)
        {
            sqlTransaction.Connection.Close();
        }

        return result;
    }
}