using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Serilog.Sinks.OpenObserve;

public class HttpClient
{
    private const string AuthScheme = "Basic";
    private const string UrlApi = "api";
    private const string UrlMulti = "_multi";
    private const string MediaType = "application/json";
    private readonly System.Net.Http.HttpClient _httpClient;
    private readonly string _endpointUrl;

    private HttpClient(string url, string organization, string authToken, string streamName = "default")
    {
        _httpClient = new System.Net.Http.HttpClient
        {
            BaseAddress = new Uri(url),
            DefaultRequestHeaders =
            {
                Authorization = new AuthenticationHeaderValue(AuthScheme, authToken)
            }
        };
        _endpointUrl = BuildEndpointUrl(organization, streamName);
    }

    public HttpClient(string url, string organization, string key, string password, string streamName = "default")
        : this(url, organization, ToBase64String($"{key}:{password}"), streamName)
    {
    }

    public async Task<HttpClientResponse> Send(string data)
    {
        var content = new StringContent(data, Encoding.UTF8, MediaType);
        var responseObject = await _httpClient.PostAsync(_endpointUrl, content);
        return await JsonSerializer.DeserializeAsync<HttpClientResponse>(
            await responseObject.Content.ReadAsStreamAsync());
    }

    private static string BuildEndpointUrl(string organization, string streamName) =>
        $"/{UrlApi}/{Cleanup(organization)}/{Cleanup(streamName)}/{UrlMulti}";

    private static string Cleanup(string value) => value.Trim('/');

    private static string ToBase64String(string authenticationString) =>
        Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
}