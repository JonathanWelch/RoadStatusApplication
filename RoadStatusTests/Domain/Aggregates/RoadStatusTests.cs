namespace RoadStatusTests.Domain.Aggregates;

[TestFixture]
public class RoadStatusTests
{
    [Test]
    public void CanBeCreated()
    {
        const string roadId = "a2";
        const string displayName = "A2";
        const string status = "Good";
        const string statusDescription = "No Exceptional Delays";

        var roadStatus = new RoadStatus.Domain.Aggregates.RoadStatus(roadId, displayName, status, statusDescription);

        Assert.Multiple(() =>
        {
            Assert.That(roadStatus.RoadId, Is.EqualTo(roadId));
            Assert.That(roadStatus.DisplayName, Is.EqualTo(displayName));
            Assert.That(roadStatus.Status, Is.EqualTo(status));
            Assert.That(roadStatus.StatusDescription, Is.EqualTo(statusDescription));
        });
    }
}