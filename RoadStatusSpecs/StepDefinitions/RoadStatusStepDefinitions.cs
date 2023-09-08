using System.Net;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using RoadStatus;
using RoadStatus.Infrastructure.TfL;
using TechTalk.SpecFlow.Assist;

namespace RoadStatusSpecs.StepDefinitions;

[Binding]
public class RoadStatusStepDefinitions
{
    private readonly ScenarioContext _scenarioContext;
    private const string Uri = "http://www.test.com";
    private List<string> _lines;
    private bool _roadStatusFound;

    public RoadStatusStepDefinitions(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [Given(@"we have the following valid roads")]
    public void GivenWeHaveTheFollowingValidRoads(Table validRoads)
    {
        var roads = validRoads.CreateSet<ExampleValidRoad>();

        foreach (var validRoad in roads)
        {
            _scenarioContext[validRoad.RoadId] = validRoad;
        }
    }

    [Given(@"a valid road id of (.*) is specified")]
    public void GivenAValidRoadIdIsSpecified(string roadId)
    {
        _scenarioContext["RoadId"] = roadId;
    }

    [Given(@"an invalid road id of (.*) is specified")]
    public void GivenAnInvalidRoadIdIsSpecified(string roadId)
    {
        _scenarioContext["RoadId"] = roadId;
    }

    [When(@"the client is run")]
    public async Task WhenTheClientIsRun()
    {
        FakeHttpMessageHandler handler;
        var roadId = _scenarioContext["RoadId"] as string ?? throw new InvalidOperationException();

        if (IsValidRoad(roadId))
        {
            var roadStatus = _scenarioContext[roadId] as ExampleValidRoad ?? throw new InvalidOperationException();

            handler = new FakeHttpMessageHandler(HttpStatusCode.OK,
                BuildSuccessfulResponse(roadStatus.RoadId, roadStatus.DisplayName, roadStatus.Status,
                    roadStatus.StatusDescription));
        }
        else
        {
            handler = new FakeHttpMessageHandler(HttpStatusCode.NotFound, BuildFailureResponse(roadId));
        }

        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri(Uri)
        };

        var roadService = new RoadStatusApiClient(httpClient, Options.Create(new RoadStatusApiConfiguration()));
        var fakeConsoleOutput = new FakeConsoleOutput();
        var roadStatusService = new RoadStatusService(roadService, fakeConsoleOutput);

        _roadStatusFound = await roadStatusService.CheckStatus(roadId);

        _lines = fakeConsoleOutput.Lines;
    }

    private bool IsValidRoad(string roadId)
    {
        return _scenarioContext.ContainsKey(roadId);
    }

    [Then(@"the road (.*) should be displayed")]
    public void ThenTheRoadDisplayNameShouldBeDisplayed(string roadDisplayName)
    {
        var expectedOutput = @$"The status of the {roadDisplayName} is as follows";

        Assert.That(_lines[0], Is.EqualTo(expectedOutput));
    }

    [Then(@"the road status should be (.*)")]
    public void ThenTheRoadStatusShouldBe(string roadStatus)
    {
        var expectedOutput = $"\tRoad Status is {roadStatus}";
        Assert.That(_lines[1], Is.EqualTo(expectedOutput));
    }

    [Then(@"the road status description is (.*)")]
    public void ThenTheRoadStatusDescriptionIs(string roadStatusDescription)
    {
        var expectedOutput = $"\tRoad Status Description is {roadStatusDescription}";
        Assert.That(_lines[2], Is.EqualTo(expectedOutput));
    }

    [Then(@"the application should return an informative error")]
    public void ThenTheApplicationShouldReturnAnInformativeError()
    {
        var roadId = _scenarioContext["RoadId"] as string ?? throw new InvalidOperationException();
        var expectedOutput = $"{roadId} is not a valid road";
        Assert.That(_lines.Count, Is.EqualTo(1));
        Assert.That(_lines[0], Is.EqualTo(expectedOutput));
    }

    [Then(@"the application should exit with a non-zero System Error code")]
    public void ThenTheApplicationShouldExitWithANon_ZeroSystemErrorCode()
    {
        Assert.That(_roadStatusFound, Is.EqualTo(false));
    }

    private static string BuildSuccessfulResponse(string roadId, string displayName, string status, string statusDescription)
    {
        return $$""" 
[   
    {
        "$type": "Tfl.Api.Presentation.Entities.RoadCorridor, Tfl.Api.Presentation.Entities",  
        "id": "{{roadId}}", 
        "displayName": "{{displayName}}",
        "statusSeverity": "{{status}}",
        "statusSeverityDescription": "{{statusDescription}}",
        "bounds": "[[-0.0857,51.44091],[0.17118,51.49438]]",
        "envelope": "[[-0.0857,51.44091],[-0.0857,51.49438],[0.17118,51.49438],[0.17118,51.44091],[-0.0857,51.44091]]",
        "url": "/Road/a2" 
    }         
] 
""";
    }

    private static string BuildFailureResponse(string roadId)
    {
        return $$""" 
{  
  "$type": "Tfl.Api.Presentation.Entities.ApiError, Tfl.Api.Presentation.Entities",  
  "timestampUtc": "2017-11-21T14:37:39.7206118Z",  
  "exceptionType": "EntityNotFoundException",  
  "httpStatusCode": 404,  
  "httpStatus": "NotFound",  
  "relativeUri": "/Road/{{roadId}}",  
  "message": "The following road id is not recognised: {{roadId}}"
} 
""";

    }
}

public class ExampleValidRoad
{
    public string RoadId { get; set; }
    public string DisplayName { get; set; }
    public string Status { get; set; }
    public string StatusDescription { get; set; }
}