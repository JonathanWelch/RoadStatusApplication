using System.Net.Http.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using RoadStatus.Domain.Interfaces;

namespace RoadStatus.Infrastructure.TfL;

public class RoadStatusApiClient : IRoadService
{
    private readonly IOptions<RoadStatusApiConfiguration> _configuration;

    private readonly HttpClient _httpClient;

    public RoadStatusApiClient(HttpClient httpClient, IOptions<RoadStatusApiConfiguration> configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<Domain.Aggregates.RoadStatus?> GetRoadStatusAsync(string roadId)
    {
        try
        {
            var requestUri = $"/Road/{roadId}?app_id={_configuration.Value.ApplicationId}app_key={_configuration.Value.ApiKey}";
            var roadCorridors = await _httpClient.GetFromJsonAsync<List<RoadCorridor>>(requestUri);

            if (roadCorridors == null || !roadCorridors.Any())
                return default;

            return MapRoadStatus(roadCorridors);
        }
        catch (HttpRequestException)
        {
            return default;
        }
    }

    private static Domain.Aggregates.RoadStatus MapRoadStatus(IEnumerable<RoadCorridor> roadCorridors)
    {
        var roadCorridor = roadCorridors.First();

        return new Domain.Aggregates.RoadStatus(roadCorridor.Id,
            roadCorridor.DisplayName,
            roadCorridor.StatusSeverity,
            roadCorridor.StatusSeverityDescription);
    }

    public record RoadCorridor
    {
        [JsonPropertyName("id")] public string Id { get; set; }
        [JsonPropertyName("displayName")] public string DisplayName { get; set; }
        [JsonPropertyName("statusSeverity")] public string StatusSeverity { get; set; }
        [JsonPropertyName("statusSeverityDescription")] public string StatusSeverityDescription { get; set; }
    }
}