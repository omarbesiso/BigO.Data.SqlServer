using System.Collections.ObjectModel;
using System.Data;
using System.Runtime.CompilerServices;
using BigO.Core.Extensions;
using BigO.Core.Validation;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;
using Microsoft.Data.SqlClient.Server;

// ReSharper disable PossibleMultipleEnumeration

namespace BigO.Data.SqlServer;

/// <summary>
///     A custom collection of SQL parameters.
/// </summary>
[PublicAPI]
[Serializable]
public class SqlParameterList : Collection<SqlParameter>
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="SqlParameterList" /> class.
    /// </summary>
    public SqlParameterList()
    {
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="SqlParameterList" /> class.
    /// </summary>
    /// <param name="sqlParameters">The SQL parameters.</param>
    public SqlParameterList(IList<SqlParameter> sqlParameters) : base(sqlParameters)
    {
    }

    /// <summary>
    ///     Adds a new <see cref="SqlParameter" /> object to the collection and returns it.
    /// </summary>
    /// <param name="parameterName">The name of the parameter. Cannot be null or whitespace.</param>
    /// <param name="parameterType">The <see cref="SqlDbType" /> of the parameter.</param>
    /// <param name="parameterValue">The value of the parameter. Can be <c>null</c>.</param>
    /// <param name="parameterDirection">
    ///     The <see cref="ParameterDirection" /> of the parameter. Default is
    ///     <see cref="ParameterDirection.Input" />.
    /// </param>
    /// <param name="parameterSize">The size of the parameter. Can be <c>null</c>.</param>
    /// <param name="parameterPrecision">The precision of the parameter. Can be <c>null</c>.</param>
    /// <param name="parameterScale">The scale of the parameter. Can be <c>null</c>.</param>
    /// <returns>The added <see cref="SqlParameter" /> object.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="parameterName" /> is null or whitespace.</exception>
    /// <remarks>
    ///     If the <paramref name="parameterValue" /> is <c>null</c>, it is set to <see cref="DBNull.Value" />.
    ///     The size, precision, and scale of the parameter are only set if their respective parameters are not <c>null</c>.
    /// </remarks>
    public SqlParameter AddParameter(string parameterName, SqlDbType parameterType, object? parameterValue,
        ParameterDirection parameterDirection = ParameterDirection.Input, int? parameterSize = null,
        byte? parameterPrecision = null, byte? parameterScale = null)
    {
        Guard.NotNullOrWhiteSpace(parameterName, nameof(parameterName));

        var sqlParameter = new SqlParameter
        {
            ParameterName = parameterName,
            Value = parameterValue ?? DBNull.Value,
            SqlDbType = parameterType,
            Direction = parameterDirection
        };

        SetParameterInfo(sqlParameter, parameterSize, parameterPrecision, parameterScale);

        Add(sqlParameter);

        return sqlParameter;
    }

    /// <summary>
    ///     Adds a new table-valued <see cref="SqlParameter" /> object to the collection and returns it.
    /// </summary>
    /// <param name="parameterName">The name of the parameter. Cannot be null or whitespace.</param>
    /// <param name="typeName">The name of the user-defined type of the parameter.</param>
    /// <param name="records">The data records of the parameter. Can be an empty enumerable, but not <c>null</c>.</param>
    /// <param name="parameterSize">The size of the parameter. Can be <c>null</c>.</param>
    /// <param name="parameterPrecision">The precision of the parameter. Can be <c>null</c>.</param>
    /// <param name="parameterScale">The scale of the parameter. Can be <c>null</c>.</param>
    /// <returns>The added <see cref="SqlParameter" /> object.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="parameterName" /> is null or whitespace.</exception>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="records" /> is <c>null</c>.</exception>
    /// <remarks>
    ///     If the <paramref name="records" /> enumerable is empty, the value of the parameter is set to
    ///     <see cref="DBNull.Value" />.
    ///     Otherwise, the value is set to a <see cref="DataTable" /> created from the records.
    ///     The size, precision, and scale of the parameter are only set if their respective parameters are not <c>null</c>.
    /// </remarks>
    public SqlParameter AddTableValuedParameter(string parameterName, string typeName,
        IEnumerable<SqlDataRecord> records, int? parameterSize = null,
        byte? parameterPrecision = null, byte? parameterScale = null)
    {
        var sqlParameter = new SqlParameter(parameterName, SqlDbType.Structured)
        {
            TypeName = typeName,
            Direction = ParameterDirection.Input
        };

        if (records.IsNullOrEmpty())
        {
            sqlParameter.Value = DBNull.Value;
        }
        else
        {
            var dt = new DataTable();
            var record = records.First();
            var columnCount = record.FieldCount;
            for (var i = 0; i < columnCount; i++)
            {
                dt.Columns.Add(record.GetName(i), record.GetFieldType(i));
            }

            foreach (var sqlDataRecord in records)
            {
                var values = new object[columnCount];
                sqlDataRecord.GetValues(values);
                dt.Rows.Add(values);
            }

            sqlParameter.Value = dt;
        }

        SetParameterInfo(sqlParameter, parameterSize, parameterPrecision, parameterScale);

        Add(sqlParameter);
        return sqlParameter;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetParameterInfo(SqlParameter sqlParameter, int? parameterSize, byte? parameterPrecision,
        byte? parameterScale)
    {
        if (parameterSize.HasValue)
        {
            sqlParameter.Size = parameterSize.Value;
        }

        if (parameterScale.HasValue)
        {
            sqlParameter.Scale = parameterScale.Value;
        }

        if (parameterPrecision.HasValue)
        {
            sqlParameter.Precision = parameterPrecision.Value;
        }
    }

    /// <summary>
    ///     Adds a new output <see cref="SqlParameter" /> object to the collection and returns it.
    /// </summary>
    /// <param name="parameterName">The name of the parameter. Cannot be null or whitespace.</param>
    /// <param name="parameterType">The <see cref="SqlDbType" /> of the parameter.</param>
    /// <param name="value">The value of the parameter. Can be <c>null</c>. Default is <c>null</c>.</param>
    /// <returns>The added <see cref="SqlParameter" /> object.</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="parameterName" /> is null or whitespace.</exception>
    /// <remarks>
    ///     The <see cref="SqlParameter.Direction" /> property is set to <see cref="ParameterDirection.Output" />.
    /// </remarks>
    public SqlParameter AddOutputParameter(string parameterName, SqlDbType parameterType, object? value = null)
    {
        return AddParameter(parameterName, parameterType, value, ParameterDirection.Output);
    }
}