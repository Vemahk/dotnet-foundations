using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Xml;

namespace Vemahk.Common.Client;

public static class ClientExtensions
{
    public static Uri GetRequiredUri(this IConfiguration config, string key)
    {
        var configValue = config.GetRequiredSection(key).Value ?? string.Empty;
        return new Uri(configValue);
    }

    public static Action<HttpClient> ConfigureClient(this IConfiguration config, string key)
    {
        var uri = config.GetRequiredUri(key);
        return client => client.BaseAddress = uri;
    }

    public static async Task<T> SendAndReceiveAsync<T>(this HttpClient client, HttpRequestMessage request, CancellationToken token)
    {
        var response = await client.SendAsync(request, token);
        response.EnsureSuccessStatusCode();
        var mediaType = response.Content.Headers.ContentType.MediaType;

        if (mediaType == "application/json")
        {
            var content = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<T>(content, cancellationToken: token)
                ?? throw new ApplicationException("Failed to parse json response from http response");
        }

        throw new ApplicationException($"Unacceptable content type: {mediaType}");
    }
}