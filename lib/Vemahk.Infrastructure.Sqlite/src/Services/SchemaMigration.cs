using System.Diagnostics;

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

using Vemahk.Infrastructure.Interface;
using Vemahk.Infrastructure.Sqlite.Extensions;
using Vemahk.Kernel.Services;

namespace Vemahk.Infrastructure.Sqlite.Services;

public abstract class SchemaDefinition<T> where T : SchemaDefinition<T>
{
    protected delegate Task<Result> VersionMigration(SqliteConnection conn, CancellationToken token);
    protected static VersionMigration Noop = (_, _) => Task.FromResult(Result.Pass());

    private readonly ILogger<T> _logger;
    private readonly IConnectionProvider<SqliteConnection> _connectionProvider;

    protected SchemaDefinition(ILogger<T> logger, IConnectionProvider<SqliteConnection> connectionProvider)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }

    protected abstract string ConnectionName { get; }
    protected abstract VersionMigration[] Migrations { get; }

    public async Task<Result> Initialize(CancellationToken token)
    {
#if NET7_0_OR_GREATER
        await
#endif
        using var conn = await _connectionProvider.OpenConnectionAsync(ConnectionName, token);
        var targetVersion = Migrations.Length;
        var currentVersion = await conn.GetDatabaseVersion(token);

        _logger.LogDebug("Database {db} Initial Version: {currentVersion}", ConnectionName, currentVersion);

        if(currentVersion == targetVersion)
            return Result.Pass();

        _logger.LogInformation("Database is {versionBehind} versions behind.", targetVersion - currentVersion);

        while (currentVersion < targetVersion)
        {
            _logger.LogInformation("Migrating from {currentVersion} to {nextVersion}...", currentVersion, currentVersion + 1);

            var sw = Stopwatch.StartNew();
            var upgradeResult = await DoMigration(conn, currentVersion, token);
            sw.Stop();

            if (!upgradeResult.Success)
            {
                _logger.LogCritical("Failed to migrate {db} from {currentVersion} to {nextVersion}... {message}",
                    ConnectionName, currentVersion, currentVersion + 1, upgradeResult.Message);
                return Result.Fail(upgradeResult);
            }
            
            _logger.LogDebug("Migration successful. Took {time}.", sw.Elapsed);

            currentVersion = upgradeResult.Data;
        }

        _logger.LogInformation("All migrations sucessful.");

        return Result.Pass();
    }

    private async Task<Result<int>> DoMigration(SqliteConnection conn, int version, CancellationToken token)
    {
#if NET7_0_OR_GREATER
        await using var tran = await conn.BeginTransactionAsync(token);
#else
        using var tran = conn.BeginTransaction();
#endif

        var result = await Migrations[version](conn, token);

        if (!result.Success)
        {
#if NET7_0_OR_GREATER
            await tran.RollbackAsync(token);
#else
            tran.Rollback();
#endif
            return Result.Fail(result);
        }

        var newVersion = version + 1;
        await conn.SetDatabaseVersion(newVersion, token);
#if NET7_0_OR_GREATER
        await tran.CommitAsync(token);
#else
        tran.Commit();
#endif
        return Result.Pass(newVersion);
    }
}
