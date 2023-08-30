using System.Data.SqlClient;

using Microsoft.Extensions.DependencyInjection;

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