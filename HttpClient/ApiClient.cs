using System.Collections.Specialized;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace HttpClientwrapper;

public class ApiClient
{
    private readonly HttpClient _httpClient = HttpClientProvider.Client;

    public async Task<T> GetAsync<T>(string url, NameValueCollection? queryParams = null)
    {
        try
        {
            url = queryParams != null
                ? UrlHelper.AddQueryString(url, queryParams)
                : url;
            
            var response = await PollyPolicies.RetryPolicy.ExecuteAsync(() =>
                _httpClient.GetAsync(url));

            if (!response.IsSuccessStatusCode)
            {
                var raw = await response.Content.ReadAsStringAsync();
                LogError("GET", url, response.StatusCode, raw);
                response.EnsureSuccessStatusCode(); // Will throw
            }

            return await response.Content.ReadAsAsync<T>();
            using var stream = await response.Content.ReadAsStreamAsync();
            using var sr = new StreamReader(stream);
            using var reader = new JsonTextReader(sr);
            var serializer = new JsonSerializer();
            return serializer.Deserialize<T>(reader);
            

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Exception] GET {url} → {ex.Message}");
            throw;
        }
    }

    public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data)
    {
        HttpResponseMessage response = null;

        try
        {
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            response = await PollyPolicies.RetryPolicy.ExecuteAsync(() =>
                _httpClient.PostAsync(url, content));

            if (!response.IsSuccessStatusCode)
            {
                var raw = await response.Content.ReadAsStringAsync();
                LogError("POST", url, response.StatusCode, raw);
                response.EnsureSuccessStatusCode(); // Will throw
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Exception] POST {url} → {ex.Message}");
            throw;
        }
    }

    private void LogError(string method, string url, HttpStatusCode statusCode, string rawContent)
    {
        Console.WriteLine($"[HTTP Error] {method} {url}");
        Console.WriteLine($"Status Code: {(int)statusCode} ({statusCode})");
        Console.WriteLine("Raw Response:");
        Console.WriteLine(rawContent);
        Console.WriteLine("---- End of Raw Response ----");
    }
}
