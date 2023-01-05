using System.Data;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;

namespace BigO.Data.SqlServer.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="SqlTransaction" /> objects.
/// </summary>
[PublicAPI]
public static class SqlTransactionExtensions
{
    /// <summary>
    ///     Creates a new instance of a <see cref="SqlCommand" /> object with the specified parameters and returns it.
    /// </summary>
    /// <param name="sqlTransaction">The <see cref="SqlTransaction" /> object to associate with the command.</param>
    /// <param name="commandText">The text of the command to execute.</param>
    /// <param name="parameters">A list of <see cref="SqlParameter" /> objects to use with the command. Can be <c>null</c>.</param>
    /// <param name="commandType">The type of command to execute. Default is <see cref="CommandType.StoredProcedure" />.</param>
    /// <param name="commandTimeout">The time in seconds to wait for the command to execute. Default is 30 seconds.</param>
    /// <returns>A new instance of a <see cref="SqlCommand" /> object with the specified parameters.</returns>
    /// <exception cref="ArgumentNullException">
    ///     If <paramref name="sqlTransaction" /> or <paramref name="commandText" /> is
    ///     <c>null</c>.
    /// </exception>
    public static SqlCommand CreateSqlCommand(this SqlTransaction sqlTransaction, string commandText,
        IEnumerable<SqlParameter>? parameters = null, CommandType commandType = CommandType.StoredProcedure,
        int commandTimeout = 30)
    {
        return SqlCommandFactory.CreateSqlCommand(sqlTransaction, commandText, parameters, commandType,
            commandTimeout);
    }
}