using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vemahk.Kernel.Dependencies;

public abstract class ConfiguredDependencyModule
{
    public abstract IServiceCollection Load(IServiceCollection services, IConfiguration config);
}