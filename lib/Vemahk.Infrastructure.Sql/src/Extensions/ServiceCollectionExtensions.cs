using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;
using Vemahk.Infrastructure.Interface;
using Vemahk.Infrastructure.Sql.Connections;

namespace Vemahk.Infrastructure.Sql.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection WithSql(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionProvider<SqlConnection>, SqlConnectionProvider>();

        return services;
    }
}