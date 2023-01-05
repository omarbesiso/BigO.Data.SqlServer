using System.Data;
using BigO.Core.Validation;
using BigO.Data.SqlServer.Extensions;
using Microsoft.Data.SqlClient;

namespace BigO.Data.SqlServer;

public partial class SqlDatabase
{
    /// <summary>
    ///     Executes a Transact-SQL statement against the connection and returns the number of rows affected.
    /// </summary>
    /// <param name="commandText">The Transact-SQL statement or stored procedure to execute at the data source.</param>
    /// <param name="sqlParameterList">
    ///     An array of <see cref="SqlParameter" /> objects to use with the Transact-SQL statement or stored procedure.
    /// </param>
    /// <param name="commandType">One of the <see cref="CommandType" /> values.</param>
    /// <param name="commandTimeout">
    ///     The wait time in seconds before terminating the attempt to execute a command and
    ///     generating an error.
    /// </param>
    /// <returns>The number of rows affected.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="commandText" /> is a <c>null</c> or white-space. </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    ///     <paramref name="commandTimeout" /> is less than zero.
    /// </exception>
    /// <exception cref="SqlException">An error occurred while executing the command.</exception>
    public int ExecuteNonQuery(string commandText, SqlParameterList? sqlParameterList = null,
        CommandType commandType = CommandType.StoredProcedure, int commandTimeout = 30)
    {
        Guard.NotNullOrWhiteSpace(commandText);

        using var sqlConnection = CreateConnection();
        using var command = sqlConnection.CreateSqlCommand(commandText, sqlParameterList, commandType, commandTimeout);
        sqlConnection.Open();
        return command.ExecuteNonQuery();
    }

    /// <summary>
    ///     Executes a Transact-SQL statement against the connection and returns the number of rows affected.
    /// </summary>
    /// <param name="sqlConnection">The connection to the database.</param>
    /// <param name="commandText">The Transact-SQL statement or stored procedure to execute.</param>
    /// <param name="sqlParameterList">The parameters to use for the Transact-SQL statement or stored procedure.</param>
    /// <param name="commandType">The type of the command. Default value is <see cref="CommandType.StoredProcedure" />.</param>
    /// <param name="commandTimeout">The time in seconds to wait for the command to execute. Default value is 30 seconds.</param>
    /// <returns>The number of rows affected.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="sqlConnection" /> or <paramref name="commandText" /> is <c>null</c>.
    /// </exception>
    /// <remarks>
    ///     This method opens the connection to the database if it is not already open, and closes it after the command has
    ///     been executed.
    /// </remarks>
    public static int ExecuteNonQuery(SqlConnection sqlConnection, string commandText,
        IEnumerable<SqlParameter>? sqlParameterList = null, CommandType commandType = CommandType.StoredProcedure,
        int commandTimeout = 30)
    {
        Guard.NotNull(sqlConnection);
        Guard.NotNullOrWhiteSpace(commandText);

        using var command = sqlConnection.CreateSqlCommand(commandText, sqlParameterList, commandType, commandTimeout);
        var connectionWasOpened = sqlConnection.OpenIfNot();
        var result = command.ExecuteNonQuery();
        if (connectionWasOpened)
        {
            sqlConnection.Close();
        }

        return result;
    }

    /// <summary>
    ///     Executes a Transact-SQL statement against the connection and returns the number of rows affected.
    /// </summary>
    /// <param name="sqlTransaction">The transaction to use for the command.</param>
    /// <param name="commandText">The Transact-SQL statement or stored procedure to execute.</param>
    /// <param name="sqlParameterList">The parameters to use for the Transact-SQL statement or stored procedure.</param>
    /// <param name="commandType">The type of the command. Default value is <see cref="CommandType.StoredProcedure" />.</param>
    /// <param name="commandTimeout">The time in seconds to wait for the command to execute. Default value is 30 seconds.</param>
    /// <returns>The number of rows affected.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="sqlTransaction" /> or <paramref name="commandText" /> is <c>null</c> or white-space.
    /// </exception>
    /// <remarks>
    ///     This method uses the provided transaction to execute the command.
    /// </remarks>
    public static int ExecuteNonQuery(SqlTransaction sqlTransaction, string commandText,
        IEnumerable<SqlParameter>? sqlParameterList = null, CommandType commandType = CommandType.StoredProcedure,
        int commandTimeout = 30)
    {
        Guard.NotNull(sqlTransaction);
        Guard.NotNullOrWhiteSpace(commandText);

        using var command = sqlTransaction.CreateSqlCommand(commandText, sqlParameterList, commandType, commandTimeout);
        return command.ExecuteNonQuery();
    }
}