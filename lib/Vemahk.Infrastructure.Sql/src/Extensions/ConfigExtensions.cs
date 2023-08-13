using Microsoft.Extensions.Configuration;
using Vemahk.Infrastructure.Extensions;

namespace Vemahk.Infrastructure.Sql.Extensions;

public static class ConfigExtensions
{
    public static IConfigurationSection? GetSqlConnectionsSection(this IConfiguration config) => config.GetConnectionsSection()?.GetSection("Sql");
}