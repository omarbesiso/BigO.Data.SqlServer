using System.Data;
using BigO.Core.Extensions;
using BigO.Data.SqlServer.Extensions;
using JetBrains.Annotations;

// ReSharper disable InconsistentNaming

namespace BigO.Data.SqlServer.Internals;

[PublicAPI]
internal struct MetaType(SqlDbType sqlDbType, DbType dbType, Type dotNetType, Type NullableType)
{
    public bool IsAnsiType = sqlDbType.IsAnsiType();
    public bool IsBinType = sqlDbType.IsBinaryType();
    public bool IsCharType = sqlDbType.IsCharType();
    public bool IsNCharType = sqlDbType.IsNCharType();

    public Type DotNetType { get; set; } = dotNetType;
    public Type NullableDotNetType { get; set; } = NullableType;
    public SqlDbType SqlDbType { get; set; } = sqlDbType;
    public DbType DbType { get; set; } = dbType;
    public string DotNetTypeString { get; set; } = dotNetType.GetTypeAsString();
    public bool RequiresLength { get; set; } = RequiresLengthInternal(sqlDbType);


    internal static readonly MetaType MetaBigInt = new(SqlDbType.BigInt, DbType.Int64, typeof(long), typeof(long?));
    internal static readonly MetaType MetaBinary = new(SqlDbType.Bit, DbType.Boolean, typeof(byte[]), typeof(byte[]));
    internal static readonly MetaType MetaBit = new(SqlDbType.Bit, DbType.Boolean, typeof(bool), typeof(bool?));

    internal static readonly MetaType MetaChar = new(SqlDbType.Char, DbType.AnsiStringFixedLength, typeof(string),
        typeof(string));

    internal static readonly MetaType MetaDate = new(SqlDbType.Date, DbType.Date, typeof(DateTime), typeof(DateTime?));

    internal static readonly MetaType MetaDateTime =
        new(SqlDbType.DateTime, DbType.DateTime, typeof(DateTime), typeof(DateTime?));

    internal static readonly MetaType MetaDateTime2 =
        new(SqlDbType.DateTime2, DbType.DateTime2, typeof(DateTime), typeof(DateTime?));

    internal static readonly MetaType MetaDateTimeOffset = new(SqlDbType.DateTimeOffset, DbType.DateTimeOffset,
        typeof(DateTimeOffset), typeof(DateTimeOffset?));

    internal static readonly MetaType MetaDecimal =
        new(SqlDbType.Decimal, DbType.Decimal, typeof(decimal), typeof(decimal?));

    internal static readonly MetaType MetaFloat = new(SqlDbType.Float, DbType.Double, typeof(double), typeof(double?));
    internal static readonly MetaType MetaImage = new(SqlDbType.Image, DbType.Binary, typeof(byte[]), typeof(byte[]));
    internal static readonly MetaType MetaInt = new(SqlDbType.Int, DbType.Int32, typeof(int), typeof(int?));

    internal static readonly MetaType MetaMoney = new(SqlDbType.Money, DbType.Currency, typeof(decimal),
        typeof(decimal?));

    internal static readonly MetaType MetaNChar = new(SqlDbType.NChar, DbType.StringFixedLength, typeof(string),
        typeof(string));

    internal static readonly MetaType MetaNText = new(SqlDbType.NText, DbType.String, typeof(string), typeof(string));
    internal static readonly MetaType MetaTime = new(SqlDbType.Time, DbType.Time, typeof(TimeSpan), typeof(TimeSpan?));

    internal static readonly MetaType MetaTimestamp =
        new(SqlDbType.Timestamp, DbType.Binary, typeof(byte[]), typeof(byte[]));

    internal static readonly MetaType MetaNVarChar =
        new(SqlDbType.NVarChar, DbType.String, typeof(string), typeof(string));

    internal static readonly MetaType MetaReal = new(SqlDbType.Real, DbType.Single, typeof(float), typeof(float?));

    internal static readonly MetaType MetaSmallDateTime =
        new(SqlDbType.SmallDateTime, DbType.DateTime, typeof(DateTime), typeof(DateTime?));

    internal static readonly MetaType MetaSmallInt =
        new(SqlDbType.SmallInt, DbType.Int16, typeof(short), typeof(short?));

    internal static readonly MetaType MetaSmallMoney =
        new(SqlDbType.SmallMoney, DbType.Currency, typeof(decimal), typeof(decimal?));

    internal static readonly MetaType MetaText = new(SqlDbType.Text, DbType.AnsiString, typeof(string), typeof(string));
    internal static readonly MetaType MetaTinyInt = new(SqlDbType.TinyInt, DbType.Byte, typeof(byte), typeof(byte?));

    internal static readonly MetaType MetaUniqueIdentifier =
        new(SqlDbType.UniqueIdentifier, DbType.Guid, typeof(Guid), typeof(Guid?));

    internal static readonly MetaType MetaVarChar =
        new(SqlDbType.VarChar, DbType.AnsiString, typeof(string), typeof(string));

    internal static readonly MetaType MetaVarBinary =
        new(SqlDbType.VarBinary, DbType.Binary, typeof(byte[]), typeof(byte[]));

    internal static readonly MetaType MetaXml = new(SqlDbType.Xml, DbType.Xml, typeof(string), typeof(string));

    internal static readonly MetaType MetaVariant =
        new(SqlDbType.Variant, DbType.Object, typeof(object), typeof(object));

    internal static readonly MetaType MetaStructured =
        new(SqlDbType.Structured, DbType.Object, typeof(object), typeof(object));

    internal static readonly MetaType MetaUdt = new(SqlDbType.Udt, DbType.Object, typeof(object), typeof(object));

    internal static MetaType GetMetaTypeFromSqlDbType(SqlDbType sqlDbType)
    {
        switch (sqlDbType)
        {
            case SqlDbType.BigInt: return MetaBigInt;
            case SqlDbType.Binary: return MetaBinary;
            case SqlDbType.Bit: return MetaBit;
            case SqlDbType.Char: return MetaChar;
            case SqlDbType.Date: return MetaDate;
            case SqlDbType.DateTime: return MetaDateTime;
            case SqlDbType.DateTime2: return MetaDateTime2;
            case SqlDbType.DateTimeOffset: return MetaDateTimeOffset;
            case SqlDbType.Decimal: return MetaDecimal;
            case SqlDbType.Float: return MetaFloat;
            case SqlDbType.Image: return MetaImage;
            case SqlDbType.Int: return MetaInt;
            case SqlDbType.Money: return MetaMoney;
            case SqlDbType.NChar: return MetaNChar;
            case SqlDbType.NText: return MetaNText;
            case SqlDbType.NVarChar: return MetaNVarChar;
            case SqlDbType.Real: return MetaReal;
            case SqlDbType.SmallDateTime: return MetaSmallDateTime;
            case SqlDbType.SmallInt: return MetaSmallInt;
            case SqlDbType.SmallMoney: return MetaSmallMoney;
            case SqlDbType.Text: return MetaText;
            case SqlDbType.Time: return MetaTime;
            case SqlDbType.Timestamp: return MetaTimestamp;
            case SqlDbType.TinyInt: return MetaTinyInt;
            case SqlDbType.VarBinary: return MetaVarBinary;
            case SqlDbType.VarChar: return MetaVarChar;
            case SqlDbType.Variant: return MetaVariant;
            case SqlDbType.Xml: return MetaXml;
            case SqlDbType.UniqueIdentifier: return MetaUniqueIdentifier;
            case SqlDbType.Udt: return MetaUdt;
            case SqlDbType.Structured: return MetaStructured;
            default:
                throw new ArgumentOutOfRangeException(
                    $"The {typeof(SqlDbType).FullName} : '{sqlDbType}' is not supported.");
        }
    }

    private static bool RequiresLengthInternal(SqlDbType sqlDataType)
    {
        switch (sqlDataType)
        {
            case SqlDbType.BigInt:
            case SqlDbType.Bit:
            case SqlDbType.Date:
            case SqlDbType.DateTime:
            case SqlDbType.DateTime2:
            case SqlDbType.DateTimeOffset:
            case SqlDbType.Float:
            case SqlDbType.Image:
            case SqlDbType.Int:
            case SqlDbType.Money:
            case SqlDbType.NText:
            case SqlDbType.Real:
            case SqlDbType.SmallDateTime:
            case SqlDbType.SmallInt:
            case SqlDbType.SmallMoney:
            case SqlDbType.Text:
            case SqlDbType.Time:
            case SqlDbType.Timestamp:
            case SqlDbType.TinyInt:
            case SqlDbType.Variant:
            case SqlDbType.Xml:
            case SqlDbType.UniqueIdentifier:
            case SqlDbType.Udt:
            case SqlDbType.Structured:
                return false;
            case SqlDbType.Binary:
            case SqlDbType.Char:
            case SqlDbType.Decimal:
            case SqlDbType.NChar:
            case SqlDbType.NVarChar:
            case SqlDbType.VarChar:
            case SqlDbType.VarBinary:
                return true;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    internal static MetaType GetMetaTypeFromDbType(DbType dbType)
    {
        switch (dbType)
        {
            case DbType.AnsiString: return MetaVarChar;
            case DbType.AnsiStringFixedLength: return MetaChar;
            case DbType.Binary: return MetaVarBinary;
            case DbType.Byte: return MetaTinyInt;
            case DbType.Boolean: return MetaBit;
            case DbType.Currency: return MetaMoney;
            case DbType.Date:
            case DbType.DateTime: return MetaDateTime;
            case DbType.Decimal: return MetaDecimal;
            case DbType.Double: return MetaFloat;
            case DbType.Guid: return MetaUniqueIdentifier;
            case DbType.Int16: return MetaSmallInt;
            case DbType.Int32: return MetaInt;
            case DbType.Int64: return MetaBigInt;
            case DbType.Object: return MetaVariant;
            case DbType.Single: return MetaReal;
            case DbType.String: return MetaNVarChar;
            case DbType.StringFixedLength: return MetaNChar;
            case DbType.Time: return MetaDateTime;
            case DbType.Xml: return MetaXml;
            case DbType.DateTime2: return MetaDateTime2;
            case DbType.DateTimeOffset: return MetaDateTimeOffset;
            case DbType.SByte: // unsupported
            case DbType.UInt16:
            case DbType.UInt32:
            case DbType.UInt64:
            case DbType.VarNumeric:
            default:
                throw new ArgumentOutOfRangeException($"The {typeof(DbType).FullName} : '{dbType}' is not supported.");
        }
    }
}