using Microsoft.Extensions.Configuration;

namespace Vemahk.Infrastructure.Extensions;

public static class ConfigExtensions
{
    public static IConfigurationSection GetConnectionsSection(this IConfiguration config) => config.GetSection("Connections");
}