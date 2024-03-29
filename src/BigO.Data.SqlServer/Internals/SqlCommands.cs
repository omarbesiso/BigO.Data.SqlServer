﻿namespace BigO.Data.SqlServer.Internals;

internal static class SqlCommands
{
    public const string DropDatabaseCommand = @"
EXECUTE sp_executesql N'IF EXISTS (SELECT name FROM [master].dbo.sysdatabases WHERE   name = ''{0}'' ) 
    BEGIN 
        ALTER DATABASE [{0}] 
        SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
        DROP DATABASE [{0}]; 
   END'";

    public const string CloseDatabaseConnectionsCommand = @"
EXECUTE sp_executesql N'IF EXISTS (SELECT name FROM [master].dbo.sysdatabases WHERE   name = ''{0}'' ) 
    BEGIN 
        ALTER DATABASE [{0}] 
        SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
   END'";

    public const string GetServerProperties = @"
SELECT CONVERT(NVARCHAR(128), SERVERPROPERTY('InstanceName')) AS [InstanceName],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('MachineName')) AS [MachineName],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('ServerName')) AS [ServerName],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('ComputerNamePhysicalNetBIOS')) AS [ComputerNamePhysicalNetBIOS],
       CONVERT(BIGINT, SERVERPROPERTY('EditionID')) AS [EditionId],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('Edition')) AS [Edition],
       CONVERT(INT, SERVERPROPERTY('EngineEdition')) AS [EngineEdition],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('BuildClrVersion')) AS [ClrVersion],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('ProductBuild')) AS [ProductBuild],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('ProductBuildType')) AS [ProductBuildType],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('ProductLevel')) AS [ProductLevel],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('ProductMajorVersion')) AS [ProductMajorVersion],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('ProductMinorVersion')) AS [ProductMinorVersion],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('ProductUpdateLevel')) AS [ProductUpdateLevel],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('ProductUpdateReference')) AS [ProductUpdateReference],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('ProductVersion')) AS [ProductVersion],
       CONVERT(INT, SERVERPROPERTY('CollationID')) AS [CollationId],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('Collation')) AS [Collation],
       CONVERT(INT, SERVERPROPERTY('ComparisonStyle')) AS [ComparisonStyle],
       CONVERT(INT, SERVERPROPERTY('FilestreamConfiguredLevel')) AS [FilestreamConfiguredLevel],
       CONVERT(INT, SERVERPROPERTY('FilestreamEffectiveLevel')) AS [FilestreamEffectiveLevel],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('FilestreamShareName')) AS [FilestreamShareName],
       CONVERT(INT, SERVERPROPERTY('IsHadrEnabled')) AS [IsHadrEnabled],
       CONVERT(INT, SERVERPROPERTY('HadrManagerStatus')) AS [HadrManagerStatus],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('InstanceDefaultBackupPath')) AS [InstanceDefaultBackupPath],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('InstanceDefaultDataPath')) AS [InstanceDefaultDataPath],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('InstanceDefaultLogPath')) AS [InstanceDefaultLogPath],
       CONVERT(INT, SERVERPROPERTY('IsAdvancedAnalyticsInstalled')) AS [IsAdvancedAnalyticsInstalled],
       CONVERT(INT, SERVERPROPERTY('IsBigDataCluster')) AS [IsBigDataCluster],
       CONVERT(INT, SERVERPROPERTY('IsClustered')) AS [IsClustered],
       CONVERT(INT, SERVERPROPERTY('IsFullTextInstalled')) AS [IsFullTextInstalled],
       CONVERT(INT, SERVERPROPERTY('IsIntegratedSecurityOnly	')) AS [IsIntegratedSecurityOnly],
       CONVERT(INT, SERVERPROPERTY('IsLocalDB')) AS [IsLocalDB],
       CONVERT(INT, SERVERPROPERTY('IsPolyBaseInstalled')) AS [IsPolyBaseInstalled],
       CONVERT(INT, SERVERPROPERTY('IsSingleUser')) AS [IsSingleUser],
       CONVERT(INT, SERVERPROPERTY('IsTempDbMetadataMemoryOptimized')) AS [IsTempDbMetadataMemoryOptimized],
       CONVERT(INT, SERVERPROPERTY('LCID	')) AS [LCID],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('LicenseType')) AS [LicenseType],
       CONVERT(INT, SERVERPROPERTY('NumLicenses')) AS [NumLicenses],
       CONVERT(INT, SERVERPROPERTY('ProcessID')) AS [ProcessID],
       CONVERT(DATETIME2, SERVERPROPERTY('ResourceLastUpdateDateTime')) AS [ResourceLastUpdateDateTime],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('ResourceVersion')) AS [ResourceVersion],
       CONVERT(TINYINT, SERVERPROPERTY('SqlCharSet')) AS [SqlCharSet],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('SqlCharSetName')) AS [SqlCharSetName],
       CONVERT(TINYINT, SERVERPROPERTY('SqlSortOrder')) AS [SqlSortOrder],
       CONVERT(NVARCHAR(128), SERVERPROPERTY('SqlSortOrderName')) AS [SqlSortOrderName]
";
}