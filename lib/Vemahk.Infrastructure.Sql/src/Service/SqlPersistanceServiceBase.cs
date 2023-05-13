using Microsoft.Extensions.Logging;
using Vemahk.Infrastructure.Sql.Interface;
using Vemahk.Kernel.Services;

namespace Vemahk.Infrastructure.Sql.Service
{
    public abstract class SqlPersistanceServiceBase<TService> : ServiceBase<TService>
        where TService : SqlPersistanceServiceBase<TService>
    {
        protected readonly ISqlConnectionProvider _connectionProvider;

        public SqlPersistanceServiceBase(ILogger<TService> logger,
            ISqlConnectionProvider connectionProvider)
            : base(logger)
        {
            _connectionProvider = connectionProvider;
        }
    }
}