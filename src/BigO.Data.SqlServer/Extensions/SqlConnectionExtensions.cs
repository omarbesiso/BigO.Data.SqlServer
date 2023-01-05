using System.Data;
using BigO.Core.Validation;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;

namespace BigO.Data.SqlServer.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="SqlConnection" /> objects.
/// </summary>
[PublicAPI]
public static class SqlConnectionExtensions
{
    /// <summary>
    ///     Determines if the current <see cref="ConnectionState" /> of the <paramref name="connection" /> is within the given
    ///     <paramref name="states" />.
    /// </summary>
    /// <param name="connection">The <see cref="SqlConnection" /> object to check the state of. Cannot be <c>null</c>.</param>
    /// <param name="states">The <see cref="ConnectionState" /> values to check against. Can be <c>null</c> or empty.</param>
    /// <returns><c>true</c> if the current state is within the given states, <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="connection" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     If <paramref name="states" /> is <c>null</c> or empty, the method returns <c>false</c>.
    /// </remarks>
    public static bool StateIsWithin(this SqlConnection connection, params ConnectionState[]? states)
    {
        Guard.NotNull(connection, nameof(connection));
        return states is { Length: > 0 } && states.Any(x => (connection.State & x) == x);
    }

    /// <summary>
    ///     Determines if the current <see cref="ConnectionState" /> of the <paramref name="connection" /> is the given
    ///     <paramref name="state" />.
    /// </summary>
    /// <param name="connection">The <see cref="SqlConnection" /> object to check the state of. Cannot be <c>null</c>.</param>
    /// <param name="state">The <see cref="ConnectionState" /> value to check against.</param>
    /// <returns><c>true</c> if the current state is the given state, <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="connection" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method is a convenience wrapper around the <see cref="StateIsWithin(SqlConnection, ConnectionState[])" />
    ///     method.
    /// </remarks>
    public static bool IsInState(this SqlConnection connection, ConnectionState state)
    {
        return connection.StateIsWithin(state);
    }

    /// <summary>
    ///     Determines if the current <see cref="ConnectionState" /> of the <paramref name="connection" /> is not the given
    ///     <paramref name="state" />.
    /// </summary>
    /// <param name="connection">The <see cref="SqlConnection" /> object to check the state of. Cannot be <c>null</c>.</param>
    /// <param name="state">The <see cref="ConnectionState" /> value to check against.</param>
    /// <returns><c>true</c> if the current state is not the given state, <c>false</c> otherwise.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="connection" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     This method is a convenience wrapper around the <see cref="IsInState(SqlConnection, ConnectionState)" /> method.
    /// </remarks>
    public static bool IsNotInState(this SqlConnection connection, ConnectionState state)
    {
        return !connection.IsInState(state);
    }

    /// <summary>
    ///     Opens the <paramref name="connection" /> if it is not already open.
    /// </summary>
    /// <param name="connection">The <see cref="SqlConnection" /> object to open. Cannot be <c>null</c>.</param>
    /// <returns><c>true</c> si the connection was not open and was open; otherwise <c>false</c>.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="connection" /> is <c>null</c>.</exception>
    /// <exception cref="SqlException">Thrown when there is an error opening the connection.</exception>
    /// <remarks>This method is a convenience wrapper around the <see cref="SqlConnection.Open()" /> method.</remarks>
    public static bool OpenIfNot(this SqlConnection connection)
    {
        if (connection.IsNotInState(ConnectionState.Open))
        {
            connection.Open();
            return true;
        }

        return false;
    }

    /// <summary>
    ///     Creates a new <see cref="SqlCommand" /> object with the specified command text, parameters, command type, and
    ///     command timeout.
    /// </summary>
    /// <param name="sqlConnection">The <see cref="SqlConnection" /> to associate the command with.</param>
    /// <param name="commandText">The command text to execute.</param>
    /// <param name="parameters">The <see cref="SqlParameter" /> objects to use with the command. Default is <c>null</c>.</param>
    /// <param name="commandType">The <see cref="CommandType" /> to use. Default is <see cref="CommandType.StoredProcedure" />.</param>
    /// <param name="commandTimeout">The time in seconds to wait for the command to execute. Default is 30 seconds.</param>
    /// <returns>A new <see cref="SqlCommand" /> object.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="sqlConnection" /> or <paramref name="commandText" /> is <c>null</c> or consists only of
    ///     whitespace characters.
    /// </exception>
    public static SqlCommand CreateSqlCommand(this SqlConnection sqlConnection, string commandText,
        IEnumerable<SqlParameter>? parameters = null, CommandType commandType = CommandType.StoredProcedure,
        int commandTimeout = 30)
    {
        return SqlCommandFactory.CreateSqlCommand(sqlConnection, commandText, parameters, commandType,
            commandTimeout);
    }
}