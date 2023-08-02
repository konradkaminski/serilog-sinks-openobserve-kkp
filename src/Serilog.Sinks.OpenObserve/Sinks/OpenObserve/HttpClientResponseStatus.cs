using System.Text.Json.Serialization;

namespace Serilog.Sinks.OpenObserve;

public class HttpClientResponseStatus 
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("successful")]
    public int Successful { get; set; }
    [JsonPropertyName("failed")]
    public int Failed { get; set; }
    [JsonPropertyName("error")]
    public int Error { get; set; }
}
