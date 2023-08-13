using System.Data.Common;

namespace Vemahk.Infrastructure.Interface;

public interface IConnectionProvider<T> where T : DbConnection
{
    string GetConnectionString(string? connectionName);
    T GetConnection(string connectionString);
}

public static class ConnectionProviderExtensions 
{
    /// <summary>
    /// Open a connection by connection name. Caller assumes the responsibility of closing the connection.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="connectionProvider"></param>
    /// <param name="connectionName">The name of the connection definition, not the connection string itself.</param>
    /// <returns></returns>
    public static async Task<T> OpenConnectionAsync<T>(this IConnectionProvider<T> connectionProvider, string? connectionName) where T : DbConnection
    {
        var connectionString = connectionProvider.GetConnectionString(connectionName);
        var conn = connectionProvider.GetConnection(connectionString);
        await conn.OpenAsync();
        return conn;
    }
}