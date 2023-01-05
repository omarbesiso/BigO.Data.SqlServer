using System.Reflection;
using System.Text;
using JetBrains.Annotations;

namespace BigO.Data.SqlServer;

/// <summary>
///     Represents the properties of a SQL Server instance.
/// </summary>
[PublicAPI]
public class SqlServerProperties
{
    private SqlServerProperties()
    {
    }

    /// <summary>
    ///     The name of the instance.
    /// </summary>
    public string InstanceName { get; private set; } = null!;

    /// <summary>
    ///     The name of the machine that the instance is running on.
    /// </summary>
    public string MachineName { get; private set; } = null!;

    /// <summary>
    ///     The name of the server.
    /// </summary>
    public string ServerName { get; private set; } = null!;

    /// <summary>
    ///     The physical NetBIOS name of the computer.
    /// </summary>
    public string ComputerNamePhysicalNetBIOS { get; private set; } = null!;

    /// <summary>
    ///     The edition ID of the instance.
    /// </summary>
    public long? EditionId { get; private set; }

    /// <summary>
    ///     The edition of the instance.
    /// </summary>
    public string Edition { get; private set; } = null!;

    /// <summary>
    ///     The engine edition of the instance.
    /// </summary>
    public int? EngineEdition { get; private set; }

    /// <summary>
    ///     The version of the common language runtime (CLR) used by the instance.
    /// </summary>
    public string ClrVersion { get; private set; } = null!;

    /// <summary>
    ///     The build number of the product.
    /// </summary>
    public string ProductBuild { get; private set; } = null!;

    /// <summary>
    ///     The build type of the product.
    /// </summary>
    public string ProductBuildType { get; private set; } = null!;

    /// <summary>
    ///     The level of the product.
    /// </summary>
    public string ProductLevel { get; private set; } = null!;

    /// <summary>
    ///     The major version of the product.
    /// </summary>
    public string ProductMajorVersion { get; private set; } = null!;

    /// <summary>
    ///     The minor version of the product.
    /// </summary>
    public string ProductMinorVersion { get; private set; } = null!;

    /// <summary>
    ///     The update level of the product.
    /// </summary>
    public string ProductUpdateLevel { get; private set; } = null!;

    /// <summary>
    ///     The update reference of the product.
    /// </summary>
    public string ProductUpdateReference { get; private set; } = null!;

    /// <summary>
    ///     The version of the product.
    /// </summary>
    public string ProductVersion { get; private set; } = null!;

    /// <summary>
    ///     The collation ID of the instance.
    /// </summary>
    public int? CollationId { get; private set; }

    /// <summary>
    ///     The collation of the instance.
    /// </summary>
    public string Collation { get; private set; } = null!;

    /// <summary>
    ///     The comparison style of the instance.
    /// </summary>
    public int? ComparisonStyle { get; private set; }

    /// <summary>
    ///     The configured level of FileStream support for the instance.
    /// </summary>
    public int? FilestreamConfiguredLevel { get; private set; }

    /// <summary>
    ///     The effective level of FileStream support for the instance.
    /// </summary>
    public int? FilestreamEffectiveLevel { get; private set; }

    /// <summary>
    ///     The name of the FileStream share for the instance.
    /// </summary>
    public string FilestreamShareName { get; private set; } = null!;

    /// <summary>
    ///     Indicates whether Always On Availability Groups is enabled for the instance.
    /// </summary>
    public int? IsHadrEnabled { get; private set; }

    /// <summary>
    ///     The status of the Always On Availability Groups manager for the instance.
    /// </summary>
    public int? HadrManagerStatus { get; private set; }

    /// <summary>
    ///     The default backup path for the instance.
    /// </summary>
    public string InstanceDefaultBackupPath { get; private set; } = null!;

    /// <summary>
    ///     The default data path for the instance.
    /// </summary>
    public string InstanceDefaultDataPath { get; private set; } = null!;

    /// <summary>
    ///     The default log path for the instance.
    /// </summary>
    public string InstanceDefaultLogPath { get; private set; } = null!;

    /// <summary>
    ///     Indicates whether Advanced Analytics is installed on the instance.
    /// </summary>
    public int? IsAdvancedAnalyticsInstalled { get; private set; }

    /// <summary>
    ///     Indicates whether the instance is a big data cluster.
    /// </summary>
    public int? IsBigDataCluster { get; private set; }

    /// <summary>
    ///     Indicates whether the instance is clustered.
    /// </summary>
    public int? IsClustered { get; private set; }

    /// <summary>
    ///     Indicates whether full-text search is installed on the instance.
    /// </summary>
    public int? IsFullTextInstalled { get; private set; }

    /// <summary>
    ///     Indicates whether the instance is configured to use only integrated security.
    /// </summary>
    public int? IsIntegratedSecurityOnly { get; private set; }

    /// <summary>
    ///     Indicates whether the instance is a LocalDB instance.
    /// </summary>
    public int? IsLocalDB { get; private set; }

    /// <summary>
    ///     Indicates whether PolyBase is installed on the instance.
    /// </summary>
    public int? IsPolyBaseInstalled { get; private set; }

    /// <summary>
    ///     Indicates whether the instance is in single-user mode.
    /// </summary>
    public int? IsSingleUser { get; private set; }

    /// <summary>
    ///     Indicates whether the metadata for tempdb is memory-optimized.
    /// </summary>
    public int? IsTempDbMetadataMemoryOptimized { get; private set; }

    /// <summary>
    ///     The LCID of the instance.
    /// </summary>
    public int? LCID { get; private set; }

    /// <summary>
    ///     The type of license for the instance.
    /// </summary>
    public string LicenseType { get; private set; } = null!;

    /// <summary>
    ///     The number of licenses for the instance.
    /// </summary>
    public int? NumLicenses { get; private set; }

    /// <summary>
    ///     The process ID of the instance.
    /// </summary>
    public int? ProcessID { get; private set; }

    /// <summary>
    ///     The date and time that the resource was last updated.
    /// </summary>
    public DateTime? ResourceLastUpdateDateTime { get; private set; }

    /// <summary>
    ///     The version of the resource.
    /// </summary>
    public string ResourceVersion { get; private set; } = null!;

    /// <summary>
    ///     The character set for the instance.
    /// </summary>
    public byte? SqlCharSet { get; private set; }

    /// <summary>
    ///     The name of the character set for the instance.
    /// </summary>
    public string SqlCharSetName { get; private set; } = null!;

    /// <summary>
    ///     The SQL sort order for this instance.
    /// </summary>
    public byte? SqlSortOrder { get; private set; }

    /// <summary>
    ///     The name of the sort order for the instance.
    /// </summary>
    public string SqlSortOrderName { get; private set; } = null!;

    /// <summary>
    ///     The engine edition of the instance, as an enumeration.
    /// </summary>
    public SqlEngineEdition SqlEngineEdition
    {
        get
        {
            var engineEdition = EngineEdition;
            if (engineEdition != null)
            {
                return (SqlEngineEdition)engineEdition;
            }

            return SqlEngineEdition.Unknown;
        }
    }

    /// <summary>
    ///     Returns a string representation of the instance's properties.
    /// </summary>
    /// <returns>A string representation of the instance's properties.</returns>
    public override string ToString()
    {
        var builder = new StringBuilder();

        var properties = GetType().GetRuntimeProperties();
        foreach (var propertyInfo in properties)
        {
            builder.AppendLine(propertyInfo.Name + " : " + propertyInfo.GetValue(this));
        }

        return builder.ToString();
    }
}