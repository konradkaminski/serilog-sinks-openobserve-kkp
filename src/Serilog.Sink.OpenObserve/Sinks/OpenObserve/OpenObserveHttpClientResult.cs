using System.Net.Http;

namespace Serilog.Sinks.OpenObserve;

public class OpenObserveHttpClientResult
{
    private OpenObserveHttpClientResult(HttpResponseMessage responseMessage)
    {
        IsSuccess = responseMessage.IsSuccessStatusCode;
        if (IsSuccess)
        {
            Message = responseMessage.Content.ReadAsStringAsync().Result;
        }
    }
    public bool IsSuccess { get; init; }
    public string Message { get; init; } = string.Empty;
    public static OpenObserveHttpClientResult FromResult(HttpResponseMessage responseMessage) =>
        new OpenObserveHttpClientResult(responseMessage);
}