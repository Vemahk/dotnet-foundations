using Microsoft.Data.Sqlite;

using SQLitePCL;

using Vemahk.Common.Test;
using Vemahk.Infrastructure.Interface;
using Vemahk.Infrastructure.Sqlite.Connections;
using Vemahk.Infrastructure.Sqlite.Extensions;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Vemahk.Infrastructure.Sqlite.Test.Integration;

[TestFixture]
public class WhenCreatingEmptyDatabases
{
    [Test]
    public async Task ThenDatabaseIsCreated()
    {
        var config = """
{
    "Connections": {
        "Sqlite": {
            "TESTDB": {
                "Data Source": ":memory:",
                "Mode": "Memory"
            }
        }
    }
}
""".AsJsonConfiguration();
        var connectionProvider = new SqliteConnectionProvider(config);
        await using var conn = await connectionProvider.OpenConnectionAsync("TESTDB");
        await SetupSchema(conn, default);

        const string ExpectedData = "Hello, World!";
        var id = await InsertData(conn, ExpectedData, default);
        var actualData = await GetData(conn, id, default);
        Assert.AreEqual(ExpectedData, actualData);
    }

    private static async Task SetupSchema(SqliteConnection conn, CancellationToken token)
    {
        await using var command = conn.GetTextCommand("""
CREATE TABLE IF NOT EXISTS test_table (
    id INTEGER PRIMARY KEY,
    data TEXT NOT NULL
);
""");

        await command.ExecuteNonQueryAsync(default);
    }

    public static async Task<long> InsertData(SqliteConnection conn, string data, CancellationToken token)
    {
        await using var command = conn.GetTextCommand("""
INSERT INTO test_table (data) VALUES (@data);
SELECT last_insert_rowid();
""");
        command.Parameters.AddWithValue("@data", data);
        var result = await command.ExecuteScalarAsync(token);
        return Convert.ToInt64(result);
    }

    public static async Task<string?> GetData(SqliteConnection conn, long id, CancellationToken token)
    {
        await using var command = conn.GetTextCommand("""
SELECT data FROM test_table WHERE id = @id;
""");
        command.Parameters.AddWithValue("@id", id);
        await using var reader = await command.ExecuteReaderAsync(token);
        if (!reader.Read())
            return null;

        return reader.GetString(0);
    }
}
