using Polly;
using Polly.Retry;

namespace HttpClientwrapper;

public static class PollyPolicies
{
    public static readonly AsyncRetryPolicy<HttpResponseMessage> RetryPolicy = Policy
        .Handle<HttpRequestException>()
        .OrResult<HttpResponseMessage>(r => (int)r.StatusCode >= 500)
        .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)),
            onRetry: (outcome, timespan, retryAttempt, context) =>
            {
                Console.WriteLine($"[Retry {retryAttempt}] Waiting {timespan.TotalSeconds}s due to: " +
                                  $"{outcome.Exception?.Message ?? outcome.Result.StatusCode.ToString()}");
            });
}