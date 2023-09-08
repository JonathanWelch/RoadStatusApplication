using System.Net;

namespace RoadStatusSpecs;

public class FakeHttpMessageHandler : HttpMessageHandler
{
    private readonly HttpStatusCode _statusCode;
    private readonly string _responseContent;

    public FakeHttpMessageHandler(HttpStatusCode statusCode, string responseContent = "")
    {
        _statusCode = statusCode;
        _responseContent = responseContent;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var response = new HttpResponseMessage(_statusCode)
        {
            Content = new StringContent(_responseContent),
        };

        return await Task.FromResult(response);
    }
}