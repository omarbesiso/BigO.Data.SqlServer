using System.Data;
using BigO.Data.SqlServer.Extensions;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;

namespace BigO.Data.SqlServer.Utilities;

/// <summary>
///     A static class that provides methods for executing SQL commands and reading their results into a list of
///     <see cref="DataTable" /> objects.
/// </summary>
[PublicAPI]
public static class SqlResultReader
{
    /// <summary>
    /// Executes the given SQL command and returns a <see cref="SqlResultReaderResponse"/> containing the results.
    /// </summary>
    /// <param name="connectionString">The connection string for the SQL Server database.</param>
    /// <param name="commandText">The SQL command text to be executed.</param>
    /// <param name="commandType">The type of the SQL command.</param>
    /// <param name="parameters">An optional collection of <see cref="SqlParameter"/> objects to be used in the SQL command.</param>
    /// <returns>A <see cref="SqlResultReaderResponse"/> containing the results of the SQL command execution.</returns>
    /// <exception cref="SqlException">Thrown when there is an error executing the SQL command.</exception>
    /// <remarks>
    /// This method executes the given SQL command using the provided connection string and parameters.
    /// It reads the results into a list of <see cref="DataTable"/> objects.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// var connectionString = "your_connection_string";
    /// var commandText = "SELECT * FROM Employees";
    /// var commandType = CommandType.Text;
    /// var response = SqlResultReader.GetSqlResults(connectionString, commandText, commandType);
    /// if (response.Success)
    /// {
    ///     Console.WriteLine("Results:");
    ///     foreach (var dataTable in response.SchemaDataTables)
    ///     {
    ///         // Process data table
    ///     }
    /// }
    /// else
    /// {
    ///     Console.WriteLine("Error:");
    ///     foreach (var error in response.Errors)
    ///     {
    ///         Console.WriteLine(error.Message);
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public static SqlResultReaderResponse GetSqlResults(string connectionString, string commandText, CommandType commandType,
        IEnumerable<SqlParameter>? parameters = null)
    {
        using var connection = new SqlConnection(connectionString);
        return GetSqlResults(connection, commandText, commandType, parameters);
    }

    /// <summary>
    /// Executes the given SQL command using an existing <see cref="SqlConnection"/> and returns a <see cref="SqlResultReaderResponse"/> containing the results.
    /// </summary>
    /// <param name="connection">The <see cref="SqlConnection"/> to use for the SQL command execution.</param>
    /// <param name="commandText">The SQL command text to be executed.</param>
    /// <param name="commandType">The type of the SQL command.</param>
    /// <param name="parameters">An optional collection of <see cref="SqlParameter"/> objects to be used in the SQL command.</param>
    /// <returns>A <see cref="SqlResultReaderResponse"/> containing the results of the SQL command execution.</returns>
    /// <exception cref="SqlException">Thrown when there is an error executing the SQL command.</exception>
    /// <remarks>
    /// This method executes the given SQL command using the provided <see cref="SqlConnection"/> and parameters.
    /// It reads the results into a list of <see cref="DataTable"/> objects.
    /// </remarks>
    /// <example>
    /// <code><![CDATA[
    /// using var connection = new SqlConnection("your_connection_string");
    /// connection.Open();
    /// var commandText = "SELECT * FROM Employees";
    /// var commandType = CommandType.Text;
    /// var response = SqlResultReader.GetSqlResults(connection, commandText, commandType);
    /// if (response.Success)
    /// {
    ///     Console.WriteLine("Results:");
    ///     foreach (var dataTable in response.SchemaDataTables)
    ///     {
    ///         // Process data table
    ///     }
    /// }
    /// else
    /// {
    ///     Console.WriteLine("Error:");
    ///     foreach (var error in response.Errors)
    ///     {
    ///         Console.WriteLine(error.Message);
    ///     }
    /// }
    /// ]]></code>
    /// </example>
    public static SqlResultReaderResponse GetSqlResults(SqlConnection connection, string commandText,
        CommandType commandType, IEnumerable<SqlParameter>? parameters = null)
    {
        SqlResultReaderResponse output = new SqlResultReaderResponse();
        SqlTransaction? transaction = null;
        SqlDataReader? reader = null;

        try
        {
            bool requireClose = connection.OpenIfNot();

            SqlParameterList? sqlParameterList = null;
            if (parameters != null)
            {
                var sqlParameters = parameters.ToList();
                if (sqlParameters.Any())
                {
                    sqlParameterList = new SqlParameterList(sqlParameters);
                }
            }

            using var command = SqlCommandFactory.CreateSqlCommand(connection, commandText, sqlParameterList, commandType);

            reader = command.ExecuteReader(CommandBehavior.SchemaOnly);
            var dataTable = new DataTable();
            dataTable.Load(reader);
            output.SchemaDataTables.Add(dataTable);
            while (!reader.IsClosed)
            {
                dataTable = new DataTable();
                dataTable.Load(reader);
                output.SchemaDataTables.Add(dataTable);
            }

            if (requireClose)
            {
                connection.Close();
            }
        }
        catch (Exception e)
        {
            output.Success = false;
            output.Errors = new[] { e };
        }
        finally
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction.Dispose();
            }

            reader?.Dispose();
        }

        return output;
    }
}