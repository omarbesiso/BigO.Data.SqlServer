using System.ComponentModel;
using JetBrains.Annotations;

namespace BigO.Data.SqlServer;

/// <summary>
///     Indicates the 'Database Engine' edition of the instance of SQL Server installed on the server.
/// </summary>
[PublicAPI]
public enum SqlEngineEdition
{
    /// <summary>
    ///     Indicates that the database engine is unknown.
    /// </summary>
    [Description("Database engine edition is unknown")]
    Unknown = 0,

    /// <summary>
    ///     Indicates a personal or desktop SQL engine.
    /// </summary>
    [Description("Personal or Desktop Engine")]
    Personal = 1,

    /// <summary>
    ///     Indicates SQL Server Standard edition.
    /// </summary>
    /// <remarks>
    ///     This is returned for Standard, Web, and Business Intelligence.
    /// </remarks>
    [Description("Standard (This is returned for Standard, Web, and Business Intelligence)")]
    Standard = 2,

    /// <summary>
    ///     Indicates SQL Server Enterprise edition.
    /// </summary>
    [Description("Enterprise (This is returned for Evaluation, Developer, and Enterprise editions)")]
    Enterprise = 3,

    /// <summary>
    ///     Indicates SQL Server Express edition.
    /// </summary>
    /// <remarks>
    ///     This is returned for Express, Express with Tools, and Express with Advanced Services.
    /// </remarks>
    [Description("Express (This is returned for Express, Express with Tools, and Express with Advanced Services)")]
    Express = 4,

    /// <summary>
    ///     Indicates a Azure SQL Database.
    /// </summary>
    [Description("Azure SQL Database")] AzureDatabase = 5,

    /// <summary>
    ///     Indicates a Microsoft Azure Synapse Analytics.
    /// </summary>
    /// <remarks>
    ///     Formerly known as SQL Data Warehouse.
    /// </remarks>
    [Description("Microsoft Azure Synapse Analytics (formerly SQL Data Warehouse)")]
    AzureSynapseAnalytics = 6,

    /// <summary>
    ///     Indicates Azure SQL Managed Instance.
    /// </summary>
    [Description("Azure SQL Managed Instance")]
    AzureManagedInstance = 7,

    /// <summary>
    ///     Indicates Azure SQL Edge.
    /// </summary>
    /// <remarks>
    ///     This is returned for both editions of Azure SQL Edge
    /// </remarks>
    [Description("Azure SQL Edge (this is returned for both editions of Azure SQL Edge)")]
    AzureEdge = 8
}