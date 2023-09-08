namespace RoadStatus.Domain.Aggregates;

public class RoadStatus
{
    public RoadStatus(string roadId, string displayName, string status, string statusDescription)
    {
        RoadId = roadId;
        DisplayName = displayName;
        Status = status;
        StatusDescription = statusDescription;
    }

    public string RoadId { get; }
    public string DisplayName { get; }
    public string Status { get; }
    public string StatusDescription { get; }
}