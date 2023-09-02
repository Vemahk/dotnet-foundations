using System.Web;

namespace Vemahk.Common.Client;

public abstract class ClientBase
{
    protected ClientBase(HttpClient client)
    {
        Client = client;
    }

    public HttpClient Client { get; }

    protected RequestBuilder PrepareRequest(string pathRoot) => new RequestBuilder(pathRoot);

    protected string UrlEncode(string str) => HttpUtility.UrlEncode(str);
}
