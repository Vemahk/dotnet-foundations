using System.Collections.Concurrent;

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

using Vemahk.Infrastructure.Interface;
using Vemahk.Infrastructure.Sqlite.Extensions;
using Vemahk.Kernel.Exceptions;

namespace Vemahk.Infrastructure.Sqlite.Connections;

public class SqliteConnectionProvider : IConnectionProvider<SqliteConnection>
{
    private static string? _defaultConnectionString;
    private static readonly ConcurrentDictionary<string, string> _connectionStringCache = new ConcurrentDictionary<string, string>();

    private readonly IConfiguration _config;

    public SqliteConnectionProvider(IConfiguration config)
    {
        _config = config;
    }

    public string GetConnectionString(string? connectionName)
    {
        var useDefault = string.IsNullOrWhiteSpace(connectionName);

        // Try the cache.
        if (useDefault)
        {
            if (_defaultConnectionString != null)
                return _defaultConnectionString;
        }
        else if (_connectionStringCache.TryGetValue(connectionName!, out var cachedConnectionString))
            return cachedConnectionString;

        var connectionString = CreateConnectionString(connectionName);

        // Save the build connection string.
        if (useDefault)
            _defaultConnectionString = connectionString;
        else
            _connectionStringCache.TryAdd(connectionName!, connectionString);

        return connectionString;
    }

    public SqliteConnection GetConnection(string connectionString) => new SqliteConnection(connectionString);

    private string CreateConnectionString(string? connectionName)
    {
        // Load configs.
        var sqlSection = _config.GetSqliteConnectionsSection();
        if (sqlSection == null || !sqlSection.Exists())
            throw new InsufficientConfigurationException($"Sql Connections configuration does not exist.");

        IConfigurationSection? connSection = null;
        if (!string.IsNullOrWhiteSpace(connectionName))
        {
            connSection = sqlSection.GetSection(connectionName!);
            if (connSection != null && !connSection.Exists())
                connSection = null;
        }

        // Build the ConnectionString
        var builder = new SqliteConnectionStringBuilder();

        foreach (var key in SqlConnectionKey.All)
        {
            var val = key.DefaultValue;

            var defaultConfigValue = sqlSection[$"Default.{key.KeyName}"];
            if (!string.IsNullOrWhiteSpace(defaultConfigValue))
                val = defaultConfigValue;

            if (connSection != null)
            {
                var configValue = connSection[key.KeyName];
                if (!string.IsNullOrWhiteSpace(configValue))
                    val = configValue;
            }

            if (val == null)
                throw new InsufficientConfigurationException($"Sql Connection {connectionName} did not have the required key {key.KeyName} defined.");

            builder[key.KeyName] = val;
        }

        return builder.ToString();
    }
}

internal class SqlConnectionKey
{
    //https://learn.microsoft.com/en-us/dotnet/api/microsoft.data.sqlclient.sqlconnection.connectionstring?view=sqlclient-dotnet-standard-5.1
    public static readonly SqlConnectionKey[] All =
    {
        new SqlConnectionKey("Data Source", null),
        new SqlConnectionKey("Mode", "ReadOnly"),
        new SqlConnectionKey("Cache", "Default"),
    };

    private SqlConnectionKey(string keyName, string? defaultValue)
    {
        KeyName = keyName;
        DefaultValue = defaultValue;
    }

    public string KeyName { get; }
    public string? DefaultValue { get; }
}