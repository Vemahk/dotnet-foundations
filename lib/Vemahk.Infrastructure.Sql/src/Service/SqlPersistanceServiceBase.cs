using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Vemahk.Infrastructure.Interface;
using Vemahk.Kernel.Services;

namespace Vemahk.Infrastructure.Sql.Service;

public abstract class SqlPersistanceServiceBase<TService> : ServiceBase<TService>
    where TService : SqlPersistanceServiceBase<TService>
{
    protected readonly IConnectionProvider<SqlConnection> _connectionProvider;

    public SqlPersistanceServiceBase(
        ILogger<TService> logger,
        IConnectionProvider<SqlConnection> connectionProvider)
        : base(logger)
    {
        _connectionProvider = connectionProvider;
    }
}