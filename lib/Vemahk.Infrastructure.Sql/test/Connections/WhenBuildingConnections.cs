using Vemahk.Infrastructure.Sql.Connections;

namespace Vemahk.Infrastructure.Sql.Test.Connections;

[TestFixture, Category("Manual"), Parallelizable(ParallelScope.All)]
public class WhenBuildingConnections
{
    [Test]
    public async Task ThenSufficientConfigurationWorks()
    {
        var configJson = """
{
    "Connections":{
        "Sql": {
            "Default.Application Name": "Vemahk.Infrastructure.Sql.Test.Connections",
            "Default.Data Source": "localhost",
            "Default.Initial Catalog": "master",
        }
    }
}
""";

        var config = TestHelper.BuildConfigFromJson(configJson);

        var provider = new SqlConnectionProvider(config);
        await using var conn = await provider.OpenConnectionAsync(null, default);
    }

    [Test]
    public async Task ThenNamedConnectionWorks()
    {
        var configJson = """
{
    "Connections":{
        "Sql": {
            "Default.Application Name": "Vemahk.Infrastructure.Sql.Test.Connections",
            "LOCAL": {
                "Data Source": "localhost",
                "Initial Catalog": "master",
            }
        }
    }
}
""";

        var config = TestHelper.BuildConfigFromJson(configJson);
        var provider = new SqlConnectionProvider(config);
        await using var conn = await provider.OpenConnectionAsync("LOCAL", default);
    }
}