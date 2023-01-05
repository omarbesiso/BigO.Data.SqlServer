using System.Data;

namespace BigO.Data.SqlServer.Internals;

internal static class SqlTypeMappers
{
    public static Dictionary<SqlDbType, Type> SqlDbDotNetTypeMap = new()
    {
        { SqlDbType.BigInt, typeof(long) },
        { SqlDbType.Binary, typeof(byte[]) },
        { SqlDbType.Bit, typeof(bool) },
        { SqlDbType.Char, typeof(string) },
        { SqlDbType.Date, typeof(DateTime) },
        { SqlDbType.DateTime, typeof(DateTime) },
        { SqlDbType.DateTime2, typeof(DateTime) },
        { SqlDbType.DateTimeOffset, typeof(DateTimeOffset) },
        { SqlDbType.Decimal, typeof(decimal) },
        { SqlDbType.Float, typeof(double) },
        { SqlDbType.Image, typeof(byte[]) },
        { SqlDbType.Int, typeof(int) },
        { SqlDbType.Money, typeof(decimal) },
        { SqlDbType.NChar, typeof(string) },
        { SqlDbType.NText, typeof(string) },
        { SqlDbType.NVarChar, typeof(string) },
        { SqlDbType.Real, typeof(float) },
        { SqlDbType.SmallDateTime, typeof(DateTime) },
        { SqlDbType.SmallInt, typeof(short) },
        { SqlDbType.SmallMoney, typeof(decimal) },
        { SqlDbType.Text, typeof(string) },
        { SqlDbType.Time, typeof(TimeSpan) },
        { SqlDbType.Timestamp, typeof(byte[]) },
        { SqlDbType.TinyInt, typeof(byte) },
        { SqlDbType.UniqueIdentifier, typeof(Guid) },
        { SqlDbType.VarBinary, typeof(byte[]) },
        { SqlDbType.VarChar, typeof(string) },
        { SqlDbType.Variant, typeof(object) },
        { SqlDbType.Xml, typeof(string) }
    };

    public static Dictionary<SqlDbType, Type> SqlDbDotNetTypeNullableMap = new()
    {
        {
            SqlDbType.BigInt, typeof(long?)
        },
        { SqlDbType.Binary, typeof(byte[]) },
        { SqlDbType.Bit, typeof(bool?) },
        { SqlDbType.Char, typeof(string) },
        { SqlDbType.Date, typeof(DateTime?) },
        { SqlDbType.DateTime, typeof(DateTime?) },
        { SqlDbType.DateTime2, typeof(DateTime?) },
        { SqlDbType.DateTimeOffset, typeof(DateTimeOffset?) },
        { SqlDbType.Decimal, typeof(decimal?) },
        { SqlDbType.Float, typeof(double) },
        { SqlDbType.Image, typeof(byte[]) },
        { SqlDbType.Int, typeof(int?) },
        { SqlDbType.Money, typeof(decimal?) },
        { SqlDbType.NChar, typeof(string) },
        { SqlDbType.NText, typeof(string) },
        { SqlDbType.NVarChar, typeof(string) },
        { SqlDbType.Real, typeof(float?) },
        { SqlDbType.SmallDateTime, typeof(DateTime?) },
        { SqlDbType.SmallInt, typeof(short?) },
        { SqlDbType.SmallMoney, typeof(decimal?) },
        { SqlDbType.Text, typeof(string) },
        { SqlDbType.Time, typeof(TimeSpan?) },
        { SqlDbType.TinyInt, typeof(byte?) },
        { SqlDbType.UniqueIdentifier, typeof(Guid?) },
        { SqlDbType.VarBinary, typeof(byte[]) },
        { SqlDbType.VarChar, typeof(string) },
        { SqlDbType.Variant, typeof(object) },
        { SqlDbType.Xml, typeof(string) }
    };


    public static Dictionary<SqlDbType, DbType> SqlDbTypeMap = new()
    {
        { SqlDbType.BigInt, DbType.Int64 },
        { SqlDbType.Binary, DbType.Binary },
        { SqlDbType.Bit, DbType.Boolean },
        { SqlDbType.Char, DbType.AnsiStringFixedLength },
        { SqlDbType.Date, DbType.Date },
        { SqlDbType.DateTime, DbType.DateTime },
        { SqlDbType.DateTime2, DbType.DateTime2 },
        { SqlDbType.DateTimeOffset, DbType.DateTimeOffset },
        { SqlDbType.Decimal, DbType.Decimal },
        { SqlDbType.Float, DbType.Double },
        { SqlDbType.Image, DbType.Binary },
        { SqlDbType.Int, DbType.Int32 },
        { SqlDbType.Money, DbType.Decimal },
        { SqlDbType.NChar, DbType.String },
        { SqlDbType.NText, DbType.String },
        { SqlDbType.NVarChar, DbType.String },
        { SqlDbType.Real, DbType.Single },
        { SqlDbType.SmallDateTime, DbType.DateTime },
        { SqlDbType.SmallInt, DbType.Int16 },
        { SqlDbType.SmallMoney, DbType.Decimal },
        { SqlDbType.Text, DbType.String },
        { SqlDbType.Time, DbType.Time },
        { SqlDbType.TinyInt, DbType.Byte },
        { SqlDbType.Timestamp, DbType.Binary },
        { SqlDbType.UniqueIdentifier, DbType.Guid },
        { SqlDbType.VarBinary, DbType.Binary },
        { SqlDbType.VarChar, DbType.AnsiString },
        { SqlDbType.Variant, DbType.Object },
        { SqlDbType.Xml, DbType.Xml }
    };
}