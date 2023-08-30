using System.Text;
using Microsoft.Extensions.Configuration;

namespace Vemahk.Common.Test;

public static class IntegratonTestHelper
{
    public static IConfiguration AsJsonConfiguration(this string json)
    {
        var configuration = new ConfigurationBuilder();
        var bytes = Encoding.UTF8.GetBytes(json);
        using var stream = new MemoryStream(bytes);
        configuration.AddJsonStream(stream);
        return configuration.Build();
    }
}
