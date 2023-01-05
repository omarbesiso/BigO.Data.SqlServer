using System.Data;
using BigO.Data.SqlServer.Internals;
using JetBrains.Annotations;

namespace BigO.Data.SqlServer.Extensions;

/// <summary>
///     Contains useful utility/extensions methods for transforming <see cref="SqlDbType" /> enums.
/// </summary>
[PublicAPI]
public static class SqlDbTypeExtensions
{
    /// <summary>
    ///     Converts a SqlDbType value to its equivalent System.Type value.
    /// </summary>
    /// <param name="sqlDbType">The SqlDbType value to be converted.</param>
    /// <param name="nullableType">
    ///     Indicates whether the resulting Type value should be a nullable type. Default value is
    ///     false.
    /// </param>
    /// <returns>The System.Type value that corresponds to the specified SqlDbType value.</returns>
    /// <exception cref="NotSupportedException">Thrown when the conversion is not supported.</exception>
    public static Type GetDotNetType(this SqlDbType sqlDbType, bool nullableType = false)
    {
        Type? type;

        if (nullableType)
        {
            SqlTypeMappers.SqlDbDotNetTypeNullableMap.TryGetValue(sqlDbType, out type);
        }
        else
        {
            SqlTypeMappers.SqlDbDotNetTypeMap.TryGetValue(sqlDbType, out type);
        }

        if (type != null)
        {
            return type;
        }

        throw new NotSupportedException($"The {sqlDbType} could not be converted to a .NET type.");
    }

    /// <summary>
    ///     Converts a SqlDbType value to its equivalent DbType value.
    /// </summary>
    /// <param name="sqlDbType">The SqlDbType value to be converted.</param>
    /// <returns>The DbType value that corresponds to the specified SqlDbType value.</returns>
    /// <exception cref="NotSupportedException">Thrown when the conversion is not supported.</exception>
    public static DbType GetDbType(this SqlDbType sqlDbType)
    {
        if (SqlTypeMappers.SqlDbTypeMap.TryGetValue(sqlDbType, out var type))
        {
            return type;
        }

        throw new NotSupportedException($"The {sqlDbType} could not be converted to a .NET type.");
    }
}