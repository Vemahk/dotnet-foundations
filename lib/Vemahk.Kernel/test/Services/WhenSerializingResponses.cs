using System.Net;
using System.Text.Json;

using Newtonsoft.Json.Bson;

using Vemahk.Kernel.Services;

namespace Vemahk.Kernel.Test.Services;

[TestFixture]
public class WhenSerializingResponses
{
    [Test]
    public void ThenSuccessResponseSerializesAndDeserializes()
    {
        var response = Response.Pass();

        var newResponse = JsonCopy(response);

        Assert.That(response.Success, Is.EqualTo(newResponse.Success), nameof(Response.Success));
        Assert.That(response.Message, Is.EqualTo(newResponse.Message), nameof(Response.Message));

        const string message = "Bad times.";
        response = Response.Fail(message);
        newResponse = JsonCopy(response);

        Assert.That(response.Success, Is.EqualTo(newResponse.Success), nameof(Response.Success));
        Assert.That(response.Message, Is.EqualTo(newResponse.Message), nameof(Response.Message));
    }

    [Test]
    public void ThenResponseWithDataSerializesAndDeserializes()
    {
        var response = Response.Pass(5);
        var newResponse = JsonCopy(response);
        CompareResponses(newResponse, response);

        const string message = "Bad times.";
        response = Response.Fail(message);
        newResponse = JsonCopy(response);
        CompareResponses(newResponse, response);
    }

    [Test]
    public void ThenResponseWithNullableReferenceTypeComplies()
    {
        Response<Optional<string>> response = Response.None<string>();
        var newResponse = JsonCopy(response);
        CompareResponses(newResponse, response);

        const string data = "Hello, World!";
        response = Response.Some(data);
        newResponse = JsonCopy(response);
        CompareResponses(newResponse, response);
    }

    [Test]
    public void ThenResponseWithNullableValueTypeComplies()
    {
        Response<Optional<int>> response = Response.None<int>();
        Assert.That(response.Data.HasValue, Is.False, "respone.Data 1");

        var newResponse = JsonCopy(response);
        CompareResponses(newResponse, response);

        const int data = 5;
        response = Response.Some(data);
        newResponse = JsonCopy(response);
        CompareResponses(newResponse, response);
    }

    private T JsonCopy<T>(T data)
    {
        var json = JsonSerializer.Serialize(data);
        Console.WriteLine(json);
        var newData = JsonSerializer.Deserialize<T>(json);
        Assert.NotNull(newData);
        return newData!;
    }

    private void CompareResponses<T>(Response<T> actual, Response<T> expected)
    {
        Assert.That(actual.Success, Is.EqualTo(expected.Success), nameof(Response<T>.Success));
        Assert.That(actual.Message, Is.EqualTo(expected.Message), nameof(Response<T>.Message));
        Assert.That(actual.Data, Is.EqualTo(expected.Data), nameof(Response<T>.Data));
    }
}
