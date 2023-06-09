﻿using Microsoft.Extensions.Configuration;
using System.Text;

namespace Vemahk.Infrastructure.Sql.Test;

public static class TestHelper
{
    public static IConfiguration BuildConfigFromJson(string json)
    {
        var jsonBytes = Encoding.UTF8.GetBytes(json);
        var jsonStream = new MemoryStream(jsonBytes);
        return new ConfigurationBuilder()
            .AddJsonStream(jsonStream)
            .Build();
    }

}