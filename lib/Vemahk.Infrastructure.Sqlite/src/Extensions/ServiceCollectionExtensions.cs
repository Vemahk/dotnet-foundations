using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

using Vemahk.Infrastructure.Interface;
using Vemahk.Infrastructure.Sqlite.Connections;

namespace Vemahk.Infrastructure.Sqlite.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection WithSqlite(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionProvider<SqliteConnection>, SqliteConnectionProvider>();
        services.AddSingleton<SqliteConnectionProvider>();

        return services;
    }
}