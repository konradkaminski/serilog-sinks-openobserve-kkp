using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Serilog.Events;

namespace Serilog.Sinks.OpenObserve;

public class OpenObserveHttpClient
{
    private const string AUTH_SCHEME = "Basic";
    private readonly HttpClient _httpClient;
    private readonly string _endpointUrl;

    private OpenObserveHttpClient(string url, string organization, string authToken, string streamName)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(url),
            DefaultRequestHeaders = {
                Authorization = new AuthenticationHeaderValue(AUTH_SCHEME, authToken)
            }
        };
        _endpointUrl = BuildEndpointUrl(organization, streamName);
    }
    public OpenObserveHttpClient(string url, string organization, string login, string password, string streamName) 
        : this(url, organization, ToBase64String($"{login}:{password}"), streamName) 
    {
    }
    public async Task<OpenObserveHttpClientResult> Send(IEnumerable<LogEvent> bath)
    {
        var response = await _httpClient.PostAsJsonAsync(_endpointUrl, bath);
        return OpenObserveHttpClientResult.FromResult(response);
    }
    private static string BuildEndpointUrl(string organization, string streamName) => $"/api/{Cleanup(organization)}/{Cleanup(streamName)}/_json";
    private static string Cleanup(string value) => value.Trim('/');
    private static string ToBase64String(string authenticationString) => Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(authenticationString));

    
}