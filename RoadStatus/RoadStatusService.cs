using RoadStatus.Domain.Interfaces;

namespace RoadStatus;

public class RoadStatusService
{
    private readonly IRoadService _roadService;
    private readonly IOutput _output;

    public RoadStatusService(IRoadService roadService, IOutput output)
    {
        _roadService = roadService;
        _output = output;
    }

    public async Task<bool> CheckStatus(string roadId)
    {
        var roadStatus = await _roadService.GetRoadStatusAsync(roadId);

        if (roadStatus is null)
        {
            OutputForInvalidRoad(roadId);
            return false;
        }

        OutputForValidRoad(roadStatus);
        return true;
    }

    private void OutputForInvalidRoad(string roadId)
    {
        _output.WriteLine($"{roadId} is not a valid road");
    }

    private void OutputForValidRoad(Domain.Aggregates.RoadStatus roadStatus)
    {
        _output.WriteLine($"The status of the {roadStatus.DisplayName} is as follows");
        _output.WriteLine($"\tRoad Status is {roadStatus.Status}");
        _output.WriteLine($"\tRoad Status Description is {roadStatus.StatusDescription}");
    }
}