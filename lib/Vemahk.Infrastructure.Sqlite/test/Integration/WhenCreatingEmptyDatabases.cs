using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using Vemahk.Common.Test;
using Vemahk.Infrastructure.Interface;
using Vemahk.Infrastructure.Sqlite.Connections;
using Vemahk.Infrastructure.Sqlite.Extensions;
using Vemahk.Infrastructure.Sqlite.Services;
using Vemahk.Kernel.Services;

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
                "Data Source": "Init.db",
                "Mode": "ReadWriteCreate"
            }
        }
    }
}
""".AsJsonConfiguration();
        var connectionProvider = new SqliteConnectionProvider(config);
        var schema = new TestSchema(new NullLogger<TestSchema>(), connectionProvider);
        var initResult = await schema.Initialize(default);

        Assert.That(initResult.Success, "Failed to init schema", initResult.Message);

        initResult = await schema.Initialize(default);
        Assert.That(initResult.Success, "Reinit failed?", initResult.Message);

        await using var conn = await connectionProvider.OpenConnectionAsync("TESTDB", default);
        const string ExpectedData = "Hello, World!";
        var id = await InsertData(conn, ExpectedData, default);
        var actualData = await GetData(conn, id, default);
        Assert.AreEqual(ExpectedData, actualData);
    }

    [Test]
    public async Task ThenVersionInitializesToOne()
    {
        var config = """
{
    "Connections": {
        "Sqlite": {
            "TESTDB": {
                "Data Source": "Version.db",
                "Mode": "ReadWriteCreate"
            }
        }
    }
}
""".AsJsonConfiguration();

        if (File.Exists("Version.db"))
            File.Delete("Versionl.db");

        var connectionProvider = new SqliteConnectionProvider(config);
        await using var conn = await connectionProvider.OpenConnectionAsync("TESTDB", default);
        var version = await conn.GetDatabaseVersion(default);
        Assert.That(version, Is.EqualTo(0));

        await conn.SetDatabaseVersion(1, default);
        version = await conn.GetDatabaseVersion(default);

        Assert.That(version, Is.EqualTo(1), "version did not update");
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

    private class TestSchema : SchemaDefinition<TestSchema>
    {
        public TestSchema(ILogger<TestSchema> logger, IConnectionProvider<SqliteConnection> connectionProvider)
            : base(logger, connectionProvider)
        {
        }

        protected override string ConnectionName => "TESTDB";

        protected override VersionMigration[] Migrations { get; } =
        {
            SetupSchema
        };

        private static async Task<Result> SetupSchema(SqliteConnection conn, CancellationToken token)
        {
            try
            {
                await using var command = conn.GetTextCommand("""
CREATE TABLE test_table (
    id INTEGER PRIMARY KEY,
    data TEXT NOT NULL
);
""");
                await command.ExecuteNonQueryAsync(default);
                return Result.Pass();
            }
            catch(Exception e)
            {
                return Result.Fail(e.Message);
            }
        }
    }
}