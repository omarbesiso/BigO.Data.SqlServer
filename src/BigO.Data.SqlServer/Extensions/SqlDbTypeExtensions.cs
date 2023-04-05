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
        var meta = MetaType.GetMetaTypeFromSqlDbType(sqlDbType);
        var type = nullableType ? meta.NullableDotNetType : meta.DotNetType;
        return type;
    }

    /// <summary>
    ///     Converts a SqlDbType value to its equivalent DbType value.
    /// </summary>
    /// <param name="sqlDbType">The SqlDbType value to be converted.</param>
    /// <returns>The DbType value that corresponds to the specified SqlDbType value.</returns>
    /// <exception cref="NotSupportedException">Thrown when the conversion is not supported.</exception>
    public static DbType GetDbType(this SqlDbType sqlDbType)
    {
        var meta = MetaType.GetMetaTypeFromSqlDbType(sqlDbType);
        return meta.DbType;
    }

    /// <summary>
    ///     Determines whether the specified <see cref="SqlDbType" /> is an ANSI type.
    /// </summary>
    /// <param name="type">The <see cref="SqlDbType" /> to evaluate.</param>
    /// <returns>
    ///     Returns <c>true</c> if the specified <paramref name="type" /> is an ANSI type; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when an invalid <see cref="SqlDbType" /> is provided.</exception>
    /// <example>
    ///     <code><![CDATA[
    /// using System.Data;
    /// 
    /// SqlDbType sqlType = SqlDbType.VarChar;
    /// bool isAnsiType = sqlType.IsAnsiType();
    /// Console.WriteLine($"Is Ansi Type: {isAnsiType}"); // Output: Is Ansi Type: true
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     The method considers <see cref="SqlDbType.Char" />, <see cref="SqlDbType.VarChar" />, and
    ///     <see cref="SqlDbType.Text" /> as ANSI types.
    ///     It is an extension method and can be called directly on any <see cref="SqlDbType" /> value.
    /// </remarks>
    public static bool IsAnsiType(this SqlDbType type)
    {
        return type is SqlDbType.Char or SqlDbType.VarChar or SqlDbType.Text;
    }

    /// <summary>
    ///     Determines whether the specified <see cref="SqlDbType" /> is a character type.
    /// </summary>
    /// <param name="type">The <see cref="SqlDbType" /> to evaluate.</param>
    /// <returns>
    ///     Returns <c>true</c> if the specified <paramref name="type" /> is a character type; otherwise, <c>false</c>.
    /// </returns>
    /// <exception cref="ArgumentException">Thrown when an invalid <see cref="SqlDbType" /> is provided.</exception>
    /// <example>
    ///     <code><![CDATA[
    /// using System.Data;
    /// 
    /// SqlDbType sqlType = SqlDbType.NVarChar;
    /// bool isCharType = sqlType.IsCharType();
    /// Console.WriteLine($"Is Char Type: {isCharType}"); // Output: Is Char Type: true
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     The method considers <see cref="SqlDbType.NChar" />, <see cref="SqlDbType.NVarChar" />,
    ///     <see cref="SqlDbType.NText" />,
    ///     <see cref="SqlDbType.Char" />, <see cref="SqlDbType.VarChar" />, <see cref="SqlDbType.Text" />, and
    ///     <see cref="SqlDbType.Xml" /> as character types.
    ///     It is an extension method and can be called directly on any <see cref="SqlDbType" /> value.
    /// </remarks>
    public static bool IsCharType(this SqlDbType type)
    {
        return type is SqlDbType.NChar or SqlDbType.NVarChar or SqlDbType.NText or SqlDbType.Char or SqlDbType.VarChar
            or SqlDbType.Text or SqlDbType.Xml;
    }

    /// <summary>
    ///     Determines if the specified <see cref="SqlDbType" /> is a binary type.
    /// </summary>
    /// <param name="type">The <see cref="SqlDbType" /> to check.</param>
    /// <returns><c>true</c> if the <paramref name="type" /> is a binary type; otherwise, <c>false</c>.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// SqlDbType sqlType = SqlDbType.VarBinary;
    /// bool isBinary = IsBinaryType(sqlType);
    /// Console.WriteLine(isBinary); // Output: True
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     The <see cref="IsBinaryType" /> method checks if the provided <see cref="SqlDbType" /> is one of the following
    ///     binary types:
    ///     <list type="bullet">
    ///         <item>
    ///             <see cref="SqlDbType.Image" />
    ///         </item>
    ///         <item>
    ///             <see cref="SqlDbType.Binary" />
    ///         </item>
    ///         <item>
    ///             <see cref="SqlDbType.VarBinary" />
    ///         </item>
    ///         <item>
    ///             <see cref="SqlDbType.Timestamp" />
    ///         </item>
    ///         <item>
    ///             <see cref="SqlDbType.Udt" />
    ///         </item>
    ///         <item>SqlSmallVarBinary (24)</item>
    ///     </list>
    ///     This method can be useful when you need to determine whether a specific <see cref="SqlDbType" /> should be treated
    ///     as a binary type during data processing or database operations.
    /// </remarks>
    public static bool IsBinaryType(this SqlDbType type)
    {
        return type is SqlDbType.Image or SqlDbType.Binary or SqlDbType.VarBinary or SqlDbType.Timestamp
                   or SqlDbType.Udt ||
               (int)type == 24 /*SqlSmallVarBinary*/;
    }

    /// <summary>
    ///     Determines if the specified <see cref="SqlDbType" /> is an NChar type.
    /// </summary>
    /// <param name="type">The <see cref="SqlDbType" /> to check.</param>
    /// <returns><c>true</c> if the <paramref name="type" /> is an NChar type; otherwise, <c>false</c>.</returns>
    /// <example>
    ///     <code><![CDATA[
    /// SqlDbType sqlType = SqlDbType.NVarChar;
    /// bool isNCharType = sqlType.IsNCharType();
    /// Console.WriteLine(isNCharType); // Output: True
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     The <see cref="IsNCharType" /> method checks if the provided <see cref="SqlDbType" /> is one of the following NChar
    ///     types:
    ///     <list type="bullet">
    ///         <item>
    ///             <see cref="SqlDbType.NChar" />
    ///         </item>
    ///         <item>
    ///             <see cref="SqlDbType.NVarChar" />
    ///         </item>
    ///         <item>
    ///             <see cref="SqlDbType.NText" />
    ///         </item>
    ///         <item>
    ///             <see cref="SqlDbType.Xml" />
    ///         </item>
    ///     </list>
    ///     This method can be useful when you need to determine whether a specific <see cref="SqlDbType" /> should be treated
    ///     as an NChar type during data processing or database operations.
    /// </remarks>
    public static bool IsNCharType(this SqlDbType type)
    {
        return type is SqlDbType.NChar or SqlDbType.NVarChar or SqlDbType.NText or SqlDbType.Xml;
    }

    /// <summary>
    ///     Determines if the specified <see cref="SqlDbType" /> requires a declaration length.
    /// </summary>
    /// <param name="sqlDataType">The <see cref="SqlDbType" /> to check.</param>
    /// <returns><c>true</c> if the <paramref name="sqlDataType" /> requires a declaration length; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when an unsupported <see cref="SqlDbType" /> is encountered.</exception>
    /// <example>
    ///     <code><![CDATA[
    /// SqlDbType sqlType = SqlDbType.NVarChar;
    /// bool requiresDeclarationLength = sqlType.RequiresDeclarationLength();
    /// Console.WriteLine(requiresDeclarationLength); // Output: True
    /// ]]></code>
    /// </example>
    /// <remarks>
    ///     The <see cref="RequiresDeclarationLength" /> method checks if the provided <see cref="SqlDbType" /> requires a
    ///     declaration length when defining a column or parameter in a SQL Server database. It returns <c>true</c> for the
    ///     following data types:
    ///     <list type="bullet">
    ///         <item>
    ///             <see cref="SqlDbType.Binary" />
    ///         </item>
    ///         <item>
    ///             <see cref="SqlDbType.Char" />
    ///         </item>
    ///         <item>
    ///             <see cref="SqlDbType.Decimal" />
    ///         </item>
    ///         <item>
    ///             <see cref="SqlDbType.NChar" />
    ///         </item>
    ///         <item>
    ///             <see cref="SqlDbType.NVarChar" />
    ///         </item>
    ///         <item>
    ///             <see cref="SqlDbType.VarChar" />
    ///         </item>
    ///         <item>
    ///             <see cref="SqlDbType.VarBinary" />
    ///         </item>
    ///     </list>
    ///     It returns <c>false</c> for other supported data types.
    /// </remarks>
    public static bool RequiresDeclarationLength(this SqlDbType sqlDataType)
    {
        return MetaType.GetMetaTypeFromSqlDbType(sqlDataType).RequiresLength;
    }
}