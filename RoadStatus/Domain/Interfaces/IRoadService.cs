namespace RoadStatus.Domain.Interfaces;

public interface IRoadService
{
    public Task<Aggregates.RoadStatus?> GetRoadStatusAsync(string roadId);
}