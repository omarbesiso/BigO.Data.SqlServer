using System.Data;
using BigO.Core.Extensions;
using BigO.Core.Validation;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;

// ReSharper disable PossibleMultipleEnumeration

namespace BigO.Data.SqlServer;

/// <summary>
///     Factory for creating <see cref="SqlCommand" /> instances.
/// </summary>
[PublicAPI]
public static class SqlCommandFactory
{
    /// <summary>
    ///     Creates a new <see cref="SqlCommand" /> with the specified command text, list of parameters, command type, and
    ///     command timeout.
    /// </summary>
    /// <param name="commandText">
    ///     The command text for the <see cref="SqlCommand" />. This must not be <c>null</c> or
    ///     whitespace.
    /// </param>
    /// <param name="sqlParameterList">
    ///     The list of parameters for the <see cref="SqlCommand" />. This can be <c>null</c> or
    ///     empty.
    /// </param>
    /// <param name="commandType">
    ///     The command type for the <see cref="SqlCommand" />. This defaults to
    ///     <see cref="CommandType.StoredProcedure" />.
    /// </param>
    /// <param name="commandTimeout">The command timeout for the <see cref="SqlCommand" />. This defaults to 30 seconds.</param>
    /// <returns>
    ///     A new <see cref="SqlCommand" /> with the specified command text, list of parameters, command type, and command
    ///     timeout.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="commandText" /> is <c>null</c> or whitespace.</exception>
    public static SqlCommand CreateSqlCommand(string commandText, IEnumerable<SqlParameter>? sqlParameterList = null,
        CommandType commandType = CommandType.StoredProcedure, int commandTimeout = 30)
    {
        Guard.NotNullOrWhiteSpace(commandText, nameof(commandText));

        var command = new SqlCommand(commandText) { CommandType = commandType, CommandTimeout = commandTimeout };

        if (sqlParameterList.IsNotNullOrEmpty())
        {
            command.Parameters.AddRange(sqlParameterList.ToArray());
        }

        return command;
    }

    /// <summary>
    ///     Creates a new instance of a <see cref="SqlCommand" /> object with the specified command text and optional parameter
    ///     list, and connects it to the specified <see cref="SqlConnection" />.
    /// </summary>
    /// <param name="sqlConnection">The <see cref="SqlConnection" /> to use with the new <see cref="SqlCommand" />.</param>
    /// <param name="commandText">The text of the command. The default is an empty string.</param>
    /// <param name="sqlParameterList">
    ///     An <see cref="IEnumerable{SqlParameter}" /> of <see cref="SqlParameter" /> objects. The
    ///     default is <c>null</c>.
    /// </param>
    /// <param name="commandType">
    ///     One of the <see cref="CommandType" /> values. The default is
    ///     <see cref="CommandType.StoredProcedure" />.
    /// </param>
    /// <param name="commandTimeout">The time in seconds to wait for the command to execute. The default is 30 seconds.</param>
    /// <returns>
    ///     A new <see cref="SqlCommand" /> with the specified command text, list of parameters, command type, and command
    ///     timeout.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="sqlConnection" /> is <c>null</c>
    ///     <paramref name="commandText" /> is <c>null</c> or whitespace.
    /// </exception>
    public static SqlCommand CreateSqlCommand(SqlConnection sqlConnection, string commandText,
        IEnumerable<SqlParameter>? sqlParameterList = null, CommandType commandType = CommandType.StoredProcedure,
        int commandTimeout = 30)
    {
        Guard.NotNull(sqlConnection, nameof(sqlConnection));
        Guard.NotNullOrWhiteSpace(commandText, nameof(commandText));

        var command = sqlConnection.CreateCommand();
        command.CommandType = commandType;
        command.CommandText = commandText;
        command.CommandTimeout = commandTimeout;

        if (sqlParameterList.IsNotNullOrEmpty())
        {
            command.Parameters.AddRange(sqlParameterList.ToArray());
        }

        return command;
    }

    /// <summary>
    ///     Creates a new <see cref="SqlCommand" /> object that is associated with the specified <see cref="SqlTransaction" />.
    /// </summary>
    /// <param name="sqlTransaction">The <see cref="SqlTransaction" /> to associate the command with.</param>
    /// <param name="commandText">The text command to execute.</param>
    /// <param name="sqlParameterList">An optional list of <see cref="SqlParameter" /> objects to use with the command.</param>
    /// <param name="commandType">The type of command to execute. Default is <see cref="CommandType.StoredProcedure" />.</param>
    /// <param name="commandTimeout">The time in seconds to wait for the command to execute. Default is 30 seconds.</param>
    /// <returns>A new <see cref="SqlCommand" /> object.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="sqlTransaction" /> or <paramref name="commandText" />
    ///     is <c>null</c>.
    /// </exception>
    public static SqlCommand CreateSqlCommand(SqlTransaction sqlTransaction, string commandText,
        IEnumerable<SqlParameter>? sqlParameterList = null, CommandType commandType = CommandType.StoredProcedure,
        int commandTimeout = 30)
    {
        Guard.NotNull(sqlTransaction, nameof(sqlTransaction));
        Guard.NotNullOrWhiteSpace(commandText, nameof(commandText));

        var command = sqlTransaction.Connection.CreateCommand();
        command.CommandType = commandType;
        command.CommandText = commandText;
        command.Transaction = sqlTransaction;
        command.CommandTimeout = commandTimeout;

        if (sqlParameterList.IsNotNullOrEmpty())
        {
            command.Parameters.AddRange(sqlParameterList.ToArray());
        }

        return command;
    }
}