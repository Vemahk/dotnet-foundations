using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;
using Vemahk.Infrastructure.Interface;
using Vemahk.Infrastructure.Sql.Connections;
using Vemahk.Infrastructure.Sql.Interface;

namespace Vemahk.Infrastructure.Sql.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection WithSql(this IServiceCollection services)
        {
            services.AddSingleton<ISqlConnectionProvider, SqlConnectionProvider>();
            services.AddSingleton<IConnectionProvider<SqlConnection>, SqlConnectionProvider>();

            return services;
        }
    }
}