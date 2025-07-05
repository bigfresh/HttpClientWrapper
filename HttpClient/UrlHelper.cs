using System.Collections.Specialized;
using System.Web;

namespace HttpClientwrapper;

public static class UrlHelper
{
    public static string AddQueryString(string baseUrl, NameValueCollection queryParams)
    {
        var uriBuilder = new UriBuilder(baseUrl);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        foreach (string key in queryParams)
        {
            query[key] = queryParams[key];
        }

        uriBuilder.Query = query.ToString();
        return uriBuilder.ToString();
    }
}