using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vemahk.Kernel.Dependencies;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection LoadModule<T>(this IServiceCollection services) where T : DependencyModule, new()
    {
        return new T().Load(services);
    }

    public static IServiceCollection LoadModule<T>(this IServiceCollection services, IConfiguration config) where T : ConfiguredDependencyModule, new()
    {
        return new T().Load(services, config);
    }
}