using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Vemahk.Infrastructure.Interface;

namespace Vemahk.Infrastructure.Sql.Interface
{
    public interface ISqlConnectionProvider : IConnectionProvider<SqlConnection>
    {
        string GetConnectionString(string connectionName);
        Task<SqlConnection> OpenConnectionAsync(string connectionName, CancellationToken token);
    }
}
