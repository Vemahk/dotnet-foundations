using Microsoft.Extensions.Configuration;

using Vemahk.Infrastructure.Extensions;

namespace Vemahk.Infrastructure.Sqlite.Extensions;

public static class ConfigExtensions
{
    public static IConfigurationSection? GetSqliteConnectionsSection(this IConfiguration config) => config.GetConnectionsSection()?.GetSection("Sqlite");
}