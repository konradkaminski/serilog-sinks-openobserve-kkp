using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Serilog.Sinks.OpenObserve;

public class HttpClient
{
    private const string AUTH_SCHEME = "Basic";
    private const string URL_API = "api";
    private const string URL_JSON = "_json";
    private readonly System.Net.Http.HttpClient _httpClient;
    private readonly string _endpointUrl;

    private HttpClient(string url, string organization, string authToken, string streamName = "default")
    {
        _httpClient = new System.Net.Http.HttpClient
        {
            BaseAddress = new Uri(url),
            DefaultRequestHeaders = {
                Authorization = new AuthenticationHeaderValue(AUTH_SCHEME, authToken)
            }
        };
        _endpointUrl = BuildEndpointUrl(organization, streamName);
    }
    public HttpClient(string url, string organization, string login, string password, string streamName = "default")
        : this(url, organization, ToBase64String($"{login}:{password}"), streamName) 
    {
    }
    public async Task<HttpClientResponse> Send(IEnumerable<LogEntry> batch)
    {
        var responseObject = await _httpClient.PostAsJsonAsync(_endpointUrl, batch);
        return JsonSerializer.Deserialize<HttpClientResponse>(await responseObject.Content.ReadAsStringAsync());
    }
    private static string BuildEndpointUrl(string organization, string streamName) => $"/{URL_API}/{Cleanup(organization)}/{Cleanup(streamName)}/{URL_JSON}";
    private static string Cleanup(string value) => value.Trim('/');
    private static string ToBase64String(string authenticationString) => Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(authenticationString));

    
}