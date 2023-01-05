using BigO.Core.Validation;
using JetBrains.Annotations;
using Microsoft.Data.SqlClient;

namespace BigO.Data.SqlServer.Extensions;

/// <summary>
///     Provides a set of useful extension methods for working with <see cref="SqlException" /> objects.
/// </summary>
[PublicAPI]
public static class SqlExceptionExtensions
{
    /// <summary>
    ///     Determines if the given <paramref name="sqlException" /> is caused by a unique constraint violation.
    /// </summary>
    /// <param name="sqlException">The <see cref="SqlException" /> to check. Can be <c>null</c>.</param>
    /// <returns><c>true</c> if the exception is caused by a unique constraint violation, <c>false</c> otherwise.</returns>
    /// <remarks>
    ///     This method checks the <see cref="SqlError.Number" /> property of each error in the
    ///     <see cref="SqlException.Errors" /> collection.
    ///     If any of the numbers are 2627 or 2601, the method returns <c>true</c>.
    /// </remarks>
    public static bool IsUniqueConstraintException(this SqlException? sqlException)
    {
        return sqlException != null &&
               sqlException.Errors.Cast<SqlError>()
                   .Any(sqlError => sqlError.Number is 2627 or 2601);
    }

    /// <summary>
    ///     Determines if the given <paramref name="sqlException" /> is caused by a foreign key violation.
    /// </summary>
    /// <param name="sqlException">The <see cref="SqlException" /> to check. Can be <c>null</c>.</param>
    /// <returns><c>true</c> if the exception is caused by a foreign key violation, <c>false</c> otherwise.</returns>
    /// <remarks>
    ///     This method checks the <see cref="SqlError.Number" /> property of each error in the
    ///     <see cref="SqlException.Errors" /> collection.
    ///     If any of the numbers are 547, the method returns <c>true</c>.
    /// </remarks>
    public static bool IsForeignKeyException(this SqlException sqlException)
    {
        return sqlException.Errors.Cast<SqlError>().Any(sqlError => sqlError.Number == 547);
    }

    /// <summary>
    ///     Determines whether the specified <see cref="Exception" /> is transient.
    /// </summary>
    /// <remarks>
    ///     <para>Usually used with Sql Azure. Sql Error numbers checked are:</para>
    ///     <list type="table">
    ///         <listheader>
    ///             <term>Error_Number</term>
    ///             <description>Description</description>
    ///         </listheader>
    ///         <item>
    ///             <term>11001</term>
    ///             <description>
    ///                 A network-related or instance-specific error occurred while establishing a connection to SQL Server.
    ///                 The server was not found or was not accessible. Verify that the instance name is correct and that SQL
    ///                 Server is configured to allow remote connections. (provider: TCP Provider, error: 0 - No such host is
    ///                 known.)
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>40501</term>
    ///             <description>The service is currently busy. Retry the request after 10 seconds.</description>
    ///         </item>
    ///         <item>
    ///             <term>10922</term>
    ///             <description>%ls failed. Rerun the statement.</description>
    ///         </item>
    ///         <item>
    ///             <term>10928</term>
    ///             <description>The limit for the database has been reached.</description>
    ///         </item>
    ///         <item>
    ///             <term>10929</term>
    ///             <description>
    ///                 The server is currently too busy to support the minimum guarantee of requests for this
    ///                 database.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>10936</term>
    ///             <description>
    ///                 Resource ID : %d. The request limit for the elastic pool is %d and has been reached.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>10053</term>
    ///             <description>
    ///                 A transport-level error has occurred when receiving results from the server.
    ///                 An established connection was aborted by the software in your host machine.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>10054</term>
    ///             <description>
    ///                 A transport-level error has occurred when sending the request to the server.
    ///                 (provider: TCP Provider, error: 0 - An existing connection was forcibly closed by the remote host.)
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>10060</term>
    ///             <description>
    ///                 A network-related or instance-specific error occurred while establishing a connection to SQL Server.
    ///                 The server was not found or was not accessible. Verify that the instance name is correct and that SQL
    ///                 Server
    ///                 is configured to allow remote connections. (provider: TCP Provider, error: 0 - A connection attempt
    ///                 failed
    ///                 because the connected party did not properly respond after a period of time, or established connection
    ///                 failed
    ///                 because connected host has failed to respond.)"
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>10060</term>
    ///             <description>The service has encountered an error processing your request. Please try again.</description>
    ///         </item>
    ///         <item>
    ///             <term>14355</term>
    ///             <description>The MSSQLServerADHelper service is busy. Retry this operation later.</description>
    ///         </item>
    ///         <item>
    ///             <term>17197</term>
    ///             <description>
    ///                 Login failed due to timeout; the connection has been closed. This error may indicate heavy
    ///                 server load. Reduce the load on the server and retry login.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>20041</term>
    ///             <description>Transaction rolled back. Could not execute trigger. Retry your transaction.</description>
    ///         </item>
    ///         <item>
    ///             <term>40501</term>
    ///             <description>
    ///                 The service is currently busy. Retry the request after 10 seconds. Code: (reason code to be
    ///                 decoded).
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>40540</term>
    ///             <description>The service has encountered an error processing your request. Please try again.</description>
    ///         </item>
    ///         <item>
    ///             <term>40613</term>
    ///             <description>
    ///                 Database is not currently available. Please retry the connection later. If the problem persists,
    ///                 contact customer
    ///                 support, and provide them the session tracing ID.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>40143</term>
    ///             <description>The service has encountered an error processing your request. Please try again.</description>
    ///         </item>
    ///         <item>
    ///             <term>49920</term>
    ///             <description>
    ///                 The service is busy processing multiple requests for this subscription. Requests are currently
    ///                 blocked for resource optimization. Query sys.dm_operation_status for operation status. Wait until
    ///                 pending requests are complete or delete one of your pending requests and retry your request later.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>49919</term>
    ///             <description>
    ///                 Cannot process create or update request. Too many create or update operations in progress for
    ///                 subscription "%ld". The service is busy processing multiple create or update requests for your
    ///                 subscription or server. Requests are currently blocked for resource optimization. Query
    ///                 sys.dm_operation_status for pending operations. Wait till pending create or update requests are
    ///                 complete or delete one of your pending requests and retry your request later.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>49918</term>
    ///             <description>
    ///                 Cannot process request. Not enough resources to process request. The service is currently busy.Please
    ///                 retry the request later.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>41839</term>
    ///             <description>
    ///                 Transaction exceeded the maximum number of commit dependencies.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>41325</term>
    ///             <description>
    ///                 The current transaction failed to commit due to a serializable validation failure.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>41305</term>
    ///             <description>
    ///                 The current transaction failed to commit due to a repeatable read validation failure.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>41302</term>
    ///             <description>
    ///                 The current transaction attempted to update a record that has been updated since the transaction
    ///                 started.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>41301</term>
    ///             <description>
    ///                 Dependency failure: a dependency was taken on another transaction that later failed to commit.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>9515</term>
    ///             <description>
    ///                 An XML schema has been altered or dropped, and the query plan is no longer valid. Please rerun the
    ///                 query batch.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>8651</term>
    ///             <description>
    ///                 Could not perform the operation because the requested memory grant was not available in resource pool
    ///                 '%ls' (%ld). Rerun the query, reduce the query load, or check resource governor configuration setting.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>8645</term>
    ///             <description>
    ///                 A timeout occurred while waiting for memory resources to execute the query in resource pool '%ls'
    ///                 (%ld). Rerun the query.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>8628</term>
    ///             <description>
    ///                 A time out occurred while waiting to optimize the query. Rerun the query.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>4221</term>
    ///             <description>
    ///                 Login to read-secondary failed due to long wait on 'HADR_DATABASE_WAIT_FOR_TRANSITION_TO_VERSIONING'.
    ///                 The replica is not available for login because row versions are missing for transactions that were
    ///                 in-flight when the replica was recycled. The issue can be resolved by rolling back or committing the
    ///                 active transactions on the primary replica. Occurrences of this condition can be minimized by avoiding
    ///                 long write transactions on the primary.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>3966</term>
    ///             <description>
    ///                 Transaction is rolled back when accessing version store. It was earlier marked as victim when the
    ///                 version store was shrunk due to insufficient space in tempdb. This transaction was marked as a victim
    ///                 earlier because it may need the row version(s) that have already been removed to make space in tempdb.
    ///                 Retry the transaction.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>3960</term>
    ///             <description>
    ///                 Snapshot isolation transaction aborted due to update conflict. You cannot use snapshot isolation to
    ///                 access table '%.*ls' directly or indirectly in database '%.*ls' to update, delete, or insert the row
    ///                 that has been modified or deleted by another transaction. Retry the transaction or change the isolation
    ///                 level for the update/delete statement.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>3935</term>
    ///             <description>
    ///                 A FILESTREAM transaction context could not be initialized. This might be caused by a resource shortage.
    ///                 Retry the operation.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>1807</term>
    ///             <description>
    ///                 Could not obtain exclusive lock on database 'model'. Retry the operation later.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>1221</term>
    ///             <description>
    ///                 The Database Engine is attempting to release a group of locks that are not currently held by the
    ///                 transaction. Retry the transaction. If the problem persists, contact your support provider.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>1205</term>
    ///             <description>
    ///                 Deadlock.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>1204</term>
    ///             <description>
    ///                 The instance of the SQL Server Database Engine cannot obtain a LOCK resource at this time. Rerun your
    ///                 statement when there are fewer active users. Ask the database administrator to check the lock and
    ///                 memory configuration for this instance, or to check for long-running transactions.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>1203</term>
    ///             <description>
    ///                 Process ID %d attempted to unlock a resource it does not own: %.*ls. Retry the transaction, because
    ///                 this error may be caused by a timing condition. If the problem persists, contact the database
    ///                 administrator.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>997</term>
    ///             <description>
    ///                 A connection was successfully established with the server, but then an error occurred during the login
    ///                 process. (provider: Named Pipes Provider, error: 0 - Overlapped I/O operation is in progress).
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>921</term>
    ///             <description>
    ///                 Database '%.*ls' has not been recovered yet. Wait and try again.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>669</term>
    ///             <description>
    ///                 The row object is inconsistent. Please rerun the query.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>617</term>
    ///             <description>
    ///                 Descriptor for object ID %ld in database ID %d not found in the hash table during attempt to un-hash
    ///                 it. A work table is missing an entry. Rerun the query. If a cursor is involved, close and reopen the
    ///                 cursor.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>601</term>
    ///             <description>
    ///                 Could not continue scan with NOLOCK due to data movement.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>233</term>
    ///             <description>
    ///                 The client was unable to establish a connection because of an error during connection initialization
    ///                 process before login.
    ///                 Possible causes include the following: the client tried to connect to an unsupported version of SQL
    ///                 Server; the server was too busy
    ///                 to accept new connections; or there was a resource limitation (insufficient memory or maximum allowed
    ///                 connections) on the server.
    ///                 (provider: TCP Provider, error: 0 - An existing connection was forcibly closed by the remote host.)
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>121</term>
    ///             <description>
    ///                 The semaphore timeout period has expired.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>64</term>
    ///             <description>
    ///                 A connection was successfully established with the server, but then an error occurred during
    ///                 the login process.
    ///             </description>
    ///         </item>
    ///         <item>
    ///             <term>20</term>
    ///             <description>The instance of SQL Server you attempted to connect to does not support encryption.</description>
    ///         </item>
    ///     </list>
    ///     <para></para>
    /// </remarks>
    /// <param name="exception">The <see cref="Exception" /> instance.</param>
    /// <returns>
    ///     <c>true</c> if the specified <see cref="Exception" /> is transient; otherwise, <c>false</c>.
    /// </returns>
    /// <sqlException cref="ArgumentNullException">Thrown when the <paramref name="exception" /> is null. </sqlException>
    public static bool IsTransient(this Exception exception)
    {
        Guard.NotNull(exception);

        if (exception is not SqlException sqlException)
        {
            if (exception is TimeoutException)
            {
                return true;
            }
        }
        else
        {
            foreach (SqlError sqlError in sqlException.Errors)
            {
                switch (sqlError.Number)
                {
                    case 11001:
                    case 10922:
                    case 10928:
                    case 10929:
                    case 10936:
                    case 10053:
                    case 10054:
                    case 10060:
                    case 14355:
                    case 17197:
                    case 20041:
                    case 40197:
                    case 40501:
                    case 40540:
                    case 40613:
                    case 40143:
                    case 49920:
                    case 49919:
                    case 49918:
                    case 41839:
                    case 41325:
                    case 41305:
                    case 41302:
                    case 41301:
                    case 9515:
                    case 8651:
                    case 8645:
                    case 8628:
                    case 4221:
                    case 3966:
                    case 3960:
                    case 3935:
                    case 1807:
                    case 1205:
                    case 1204:
                    case 1203:
                    case 997:
                    case 921:
                    case 669:
                    case 617:
                    case 601:
                    case 233:
                    case 121:
                    case 64:
                    case 20:
                        return true;
                }
            }
        }

        return false;
    }
}