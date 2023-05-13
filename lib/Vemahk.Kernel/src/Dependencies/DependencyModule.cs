using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vemahk.Kernel.Dependencies
{
    public abstract class DependencyModule : ConfiguredDependencyModule
    {
        public override IServiceCollection Load(IServiceCollection services, IConfiguration config) => Load(services);
        public abstract IServiceCollection Load(IServiceCollection services);
    }
}