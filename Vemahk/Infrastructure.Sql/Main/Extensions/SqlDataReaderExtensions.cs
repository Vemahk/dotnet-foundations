using System.Data.SqlClient;

namespace Vemahk.Infrastructure.Sql.Extensions
{
    public static class SqlDataReaderExtensions
    {
        public static int GetInt32(this SqlDataReader reader, string columnName)
        {
            var ordinal = reader.GetOrdinal(columnName);
            return reader.GetInt32(ordinal);
        }
    }
}