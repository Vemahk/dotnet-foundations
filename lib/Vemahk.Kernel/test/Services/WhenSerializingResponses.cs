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
        var response = Result.Pass();

        var newResponse = JsonCopy(response);

        Assert.That(response.Success, Is.EqualTo(newResponse.Success), nameof(Result.Success));
        Assert.That(response.Message, Is.EqualTo(newResponse.Message), nameof(Result.Message));

        const string message = "Bad times.";
        response = Result.Fail(message);
        newResponse = JsonCopy(response);

        Assert.That(response.Success, Is.EqualTo(newResponse.Success), nameof(Result.Success));
        Assert.That(response.Message, Is.EqualTo(newResponse.Message), nameof(Result.Message));
    }

    [Test]
    public void ThenResponseWithDataSerializesAndDeserializes()
    {
        var response = Result.Pass(5);
        var newResponse = JsonCopy(response);
        CompareResponses(newResponse, response);

        const string message = "Bad times.";
        response = Result.Fail(message);
        newResponse = JsonCopy(response);
        CompareResponses(newResponse, response);
    }

    [Test]
    public void ThenResponseWithNullableReferenceTypeComplies()
    {
        Result<Optional<string>> response = Result.None<string>();
        var newResponse = JsonCopy(response);
        CompareResponses(newResponse, response);

        const string data = "Hello, World!";
        response = Result.Some(data);
        newResponse = JsonCopy(response);
        CompareResponses(newResponse, response);
    }

    [Test]
    public void ThenResponseWithNullableValueTypeComplies()
    {
        Result<Optional<int>> response = Result.None<int>();
        Assert.That(response.Data.HasValue, Is.False, "respone.Data 1");

        var newResponse = JsonCopy(response);
        CompareResponses(newResponse, response);

        const int data = 5;
        response = Result.Some(data);
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

    private void CompareResponses<T>(Result<T> actual, Result<T> expected)
    {
        Assert.That(actual.Success, Is.EqualTo(expected.Success), nameof(Result<T>.Success));
        Assert.That(actual.Message, Is.EqualTo(expected.Message), nameof(Result<T>.Message));
        Assert.That(actual.Data, Is.EqualTo(expected.Data), nameof(Result<T>.Data));
    }
}