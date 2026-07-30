using RequirementAI.Contract.Settings;

namespace RequirementAI.Business.Providers.LLM;

internal static class HttpLLMProviderHelper
{
    public static readonly TimeSpan RequestTimeout = TimeSpan.FromMinutes(60);

    public static Uri BuildUri(LLMProviderSettings provider, string path)
    {
        if (string.IsNullOrWhiteSpace(provider.BaseUrl))
            throw new InvalidOperationException("The provider BaseUrl is missing.");

        return new Uri($"{provider.BaseUrl.TrimEnd('/')}/{path.TrimStart('/')}");
    }

    public static async Task<string> ReadResponse(
        HttpResponseMessage response,
        string providerName,
        CancellationToken ct)
    {
        var payload = await response.Content.ReadAsStringAsync(ct);
        if (response.IsSuccessStatusCode)
            return payload;

        const int maxErrorLength = 2048;
        var error = payload.Length > maxErrorLength ? payload[..maxErrorLength] : payload;
        throw new HttpRequestException(
            $"{providerName} request failed with status {(int)response.StatusCode}: {error}",
            null,
            response.StatusCode);
    }
}
