using System.Net;
using System.Text;

namespace Vemahk.Common.Client;

public class RequestBuilder
{
    private bool _hasQuery = false;
    private readonly StringBuilder _sb;

    public RequestBuilder(string path)
    {
        _sb = new StringBuilder(path);

        if (path.EndsWith("/"))
            _sb.Remove(_sb.Length - 1, 1);
    }

    public void WithParam(string key, object? value)
    {
        if (string.IsNullOrWhiteSpace(key) || value is null)
            return;

        char ch = '&';
        if (!_hasQuery)
        {
            ch = '?';
            _hasQuery = true;
        }

        _sb.Append(ch)
            .Append(WebUtility.UrlEncode(key))
            .Append('=')
            .Append(WebUtility.UrlEncode(value.ToString()))
            ;
    }

    public HttpRequestMessage BuildGet() => Build(HttpMethod.Get);
    public HttpRequestMessage BuildPost() => Build(HttpMethod.Post);
    public HttpRequestMessage BuildPut() => Build(HttpMethod.Put);
    public HttpRequestMessage BuildDelete() => Build(HttpMethod.Delete);
    public HttpRequestMessage BuildHead() => Build(HttpMethod.Head);
    public HttpRequestMessage BuildOptions() => Build(HttpMethod.Options);
    public HttpRequestMessage Build(HttpMethod method) => new HttpRequestMessage(method, _sb.ToString());
}
