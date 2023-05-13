using Microsoft.Extensions.Configuration;

namespace Vemahk.Infrastructure.Extensions
{
    public static class ConfigExtensions
    {
        public static IConfigurationSection GetConnectionsSection(this IConfiguration config)
        {
            if (config == null)
                return null;

            var section = config.GetSection("Connections");
            return section;
        }
    }
}