using Microsoft.Extensions.Configuration;
using Vemahk.Infrastructure.Extensions;

namespace Vemahk.Infrastructure.Sql.Extensions
{
    public static class ConfigExtensions
    {
        public static IConfigurationSection GetSqlConnectionsSection(this IConfiguration config)
        {
            var connections = config.GetConnectionsSection();
            if(connections == null)
                return null;

            var sqlConnections = connections.GetSection("Sql");
            return sqlConnections;
        }
    }
}