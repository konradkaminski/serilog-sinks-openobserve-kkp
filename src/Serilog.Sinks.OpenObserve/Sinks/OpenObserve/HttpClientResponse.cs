using System.Text.Json.Serialization;

namespace Serilog.Sinks.OpenObserve;

public class HttpClientResponse
{
    [JsonPropertyName("code")]
    public int Code { get; set; }
    [JsonPropertyName("error")]
    public int Error { get; set; }
    [JsonPropertyName("status")]
    public HttpClientResponseStatus[] Status { get; set; }
    public bool IsSuccess => Code == 200;
}
