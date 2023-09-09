using Moq;
using RoadStatus;
using RoadStatus.Domain.Interfaces;

namespace RoadStatusTests;

[TestFixture]
public class RoadStatusServiceTests
{
    private Mock<IRoadService> _roadService;
    private Mock<IOutput> _output;
    private RoadStatusService _service;
    private List<string> _lines;

    [SetUp]
    public void Setup()
    {
        _roadService = new Mock<IRoadService>();
        _output = new Mock<IOutput>();

        _service = new RoadStatusService(_roadService.Object, _output.Object);

        _lines = new List<string>();
        _output.Setup(m => m.WriteLine(Capture.In(_lines)));
    }

    [Test]
    public async Task CanCheckStatusOfValidRoad()
    {
        const string validRoadId = "a2";
        var roadStatus = new RoadStatus.Domain.Aggregates.RoadStatus("a2", "A2", "Good", "No Exceptional Delays");

        _roadService.Setup(r =>r.GetRoadStatusAsync(validRoadId)).Returns(
            Task.FromResult<RoadStatus.Domain.Aggregates.RoadStatus?>(roadStatus)
        );

        var result = await _service.CheckStatus(validRoadId);

        _roadService.Verify(r => r.GetRoadStatusAsync(validRoadId), Times.Once);
        
        var expectedLines = new List<string>
        {
            $"The status of the {roadStatus.DisplayName} is as follows",
            $"\tRoad Status is {roadStatus.Status}",
            $"\tRoad Status Description is {roadStatus.StatusDescription}"
        };

        Assert.Multiple(() =>
        {
            Assert.That(_lines.SequenceEqual(expectedLines));
            Assert.That(result, Is.EqualTo(true));
        });
    }

    [Test]
    public async Task CanCheckStatusOfInvalidRoad()
    {
        const string invalidRoadId = "Invalid road Id";
        _roadService.Setup(r =>r.GetRoadStatusAsync(invalidRoadId)).Returns(
            Task.FromResult<RoadStatus.Domain.Aggregates.RoadStatus?>(null)
        );

        var result = await _service.CheckStatus(invalidRoadId);
        
        _roadService.Verify(r => r.GetRoadStatusAsync(invalidRoadId), Times.Once);

        var expectedLines = new List<string>
        {
            $"{invalidRoadId} is not a valid road"
        };

        Assert.Multiple(() =>
        {
            Assert.That(_lines.SequenceEqual(expectedLines));
            Assert.That(result, Is.EqualTo(false));
        });
    }
}