using System.Text.Json;

using Vemahk.Kernel.Services;

namespace Vemahk.Kernel.Test.Services;

[TestFixture]
public class WhenSerializingResponses
{
    [Test]
    public void ThenSuccessResponseSerializesAndDeserializes()
    {
        var response = Response.Pass();

        var json = JsonSerializer.Serialize(response);
        var newResponse = JsonSerializer.Deserialize<Response>(json);

        Assert.That(response.Success, Is.EqualTo(newResponse.Success), nameof(Response.Success));
        Assert.That(response.Message, Is.EqualTo(newResponse.Message), nameof(Response.Message));

        const string message = "Bad times.";
        response = Response.Fail(message);

        json = JsonSerializer.Serialize(response);
        newResponse = JsonSerializer.Deserialize<Response>(json);

        Assert.That(response.Success, Is.EqualTo(newResponse.Success), nameof(Response.Success));
        Assert.That(response.Message, Is.EqualTo(newResponse.Message), nameof(Response.Message));
    }

    [Test]
    public void ThenResponseWithDataSerializesAndDeserializes()
    {
        var response = Response.Pass(5);

        var json = JsonSerializer.Serialize(response);
        var newResponse = JsonSerializer.Deserialize<Response<int>>(json);

        Assert.That(response.Success, Is.EqualTo(newResponse.Success), nameof(Response<int>.Success));
        Assert.That(response.Message, Is.EqualTo(newResponse.Message), nameof(Response<int>.Message));
        Assert.That(response.Data, Is.EqualTo(newResponse.Data), nameof(Response<int>.Data));

        const string message = "Bad times.";
        response = Response.Fail(message);

        json = JsonSerializer.Serialize(response);
        newResponse = JsonSerializer.Deserialize<Response<int>>(json);

        Assert.That(response.Success, Is.EqualTo(newResponse.Success), nameof(Response<int>.Success));
        Assert.That(response.Message, Is.EqualTo(newResponse.Message), nameof(Response<int>.Message));
        Assert.That(response.Data, Is.EqualTo(newResponse.Data), nameof(Response<int>.Data));
    }
}
