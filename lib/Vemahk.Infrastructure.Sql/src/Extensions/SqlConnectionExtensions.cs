using System.Data;
using System.Data.SqlClient;

namespace Vemahk.Infrastructure.Sql.Extensions
{
    public static class SqlConnectionExtensions
    {
        public static SqlCommand GetStoredProcedureCommand(this SqlConnection connection, string sprocName)
        {
            var command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = sprocName;

            return command;
        }
    }
}