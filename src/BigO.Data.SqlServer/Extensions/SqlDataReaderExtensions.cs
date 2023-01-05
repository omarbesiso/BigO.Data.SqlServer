using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using BigO.Core.Extensions;
using BigO.Core.Validation;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;

namespace BigO.Data.SqlServer.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="SqlDataReader" /> objects.
/// </summary>
[PublicAPI]
public static class SqlDataReaderExtensions
{
    private static readonly ConcurrentDictionary<Type, IEnumerable<PropertyInfo>> PropertyCache =
        new();

    /// <summary>
    ///     Determines whether the specified <see cref="SqlDataReader" /> object has a column with the given name.
    /// </summary>
    /// <param name="sqlDataReader">The <see cref="SqlDataReader" /> object to check for the column name.</param>
    /// <param name="columnName">The name of the column to check for.</param>
    /// <returns>
    ///     True if the specified <see cref="SqlDataReader" /> object has a column with the given name; otherwise, false.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="sqlDataReader" /> is <c>null</c>.</exception>
    public static bool HasColumnName(this SqlDataReader sqlDataReader, string columnName)
    {
        Guard.NotNull(sqlDataReader, nameof(sqlDataReader));

        for (var i = 0; i < sqlDataReader.FieldCount; i++)
        {
            if (sqlDataReader.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    ///     Maps the values of a <see cref="SqlDataReader" /> to the properties of an object.
    /// </summary>
    /// <typeparam name="T">The type of the object to map the values to.</typeparam>
    /// <param name="sqlDataReader">The <see cref="SqlDataReader" /> to get the values from.</param>
    /// <param name="target">The target object to map the values to.</param>
    /// <returns>The target object with its properties mapped to the values from the <see cref="SqlDataReader" />.</returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown when either <paramref name="sqlDataReader" /> or <paramref name="target" /> is <c>null</c>.
    /// </exception>
    public static T MapObjectFromReader<T>(this SqlDataReader sqlDataReader, T target) where T : class
    {
        var targetObjectType = target.GetType();
        IEnumerable<PropertyInfo> properties;
        if (!PropertyCache.ContainsKey(targetObjectType))
        {
            PropertyCache[targetObjectType] = properties =
                targetObjectType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }
        else
        {
            properties = PropertyCache[targetObjectType];
        }

        foreach (var propertyInfo in properties)
        {
            var columnName = propertyInfo.Name;

            var columnNameAttribute = propertyInfo.GetCustomAttribute<ColumnAttribute>();
            if (columnNameAttribute != null && !string.IsNullOrWhiteSpace(columnNameAttribute.Name))
            {
                columnName = columnNameAttribute.Name;
            }

            if (!sqlDataReader.HasColumnName(columnName))
            {
                continue;
            }

            var ordinal = sqlDataReader.GetOrdinal(propertyInfo.Name);
            var value = sqlDataReader.GetValue(ordinal);
            propertyInfo.SetValue(target, value != DBNull.Value ? value : propertyInfo.PropertyType.DefaultValue());
        }

        return target;
    }

    /// <summary>
    ///     Maps the values of the columns in the current row of the specified <see cref="SqlDataReader" /> to the properties
    ///     of the specified object.
    /// </summary>
    /// <typeparam name="T">The type of the object to map the values to.</typeparam>
    /// <param name="sqlDataReader">The <see cref="SqlDataReader" /> to map the values from.</param>
    /// <returns>
    ///     The object with its properties mapped to the values of the columns in the current row of the
    ///     <see cref="SqlDataReader" />.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    ///     Thrown if <paramref name="sqlDataReader" /> is <c>null</c>.
    /// </exception>
    public static T MapObjectFromReader<T>(this SqlDataReader sqlDataReader) where T : class
    {
        var instance = (T)Activator.CreateInstance(typeof(T), true)!;
        return sqlDataReader.MapObjectFromReader(instance);
    }
}