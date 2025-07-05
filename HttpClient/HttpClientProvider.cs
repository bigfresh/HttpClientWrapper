namespace HttpClientwrapper;

public static class HttpClientProvider
{
    private static readonly Lazy<System.Net.Http.HttpClient> HttpClient = new(CreateHttpClient);
    public static System.Net.Http.HttpClient Client => HttpClient.Value;

    private static System.Net.Http.HttpClient CreateHttpClient()
    {
        var client = new System.Net.Http.HttpClient
        {
            Timeout = TimeSpan.FromSeconds(30)
        };
        client.DefaultRequestHeaders.Add("Accept", "application/json");
        return client;
    }
}