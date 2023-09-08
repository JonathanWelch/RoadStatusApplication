using RoadStatus.Infrastructure.TfL;
using System.Net;
using Microsoft.Extensions.Options;

namespace RoadStatusTests.Infrastructure.TfL
{
    [TestFixture]
    public class RoadStatusApiClientTests
    {
        private const string Uri = "http://www.test.com";

        [Test]
        public async Task CanGetRoadStatus()
        {
            const string roadId = "a2";
            const string displayName = "A2";
            const string status = "Good";
            const string statusDescription = "No Exceptional Delays";

            var handler = new FakeHttpMessageHandler(HttpStatusCode.OK, BuildSuccessfulResponse(roadId, displayName, status, statusDescription));

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(Uri)
            };

            var apiClient = new RoadStatusApiClient(httpClient, Options.Create(new RoadStatusApiConfiguration()));

            var roadStatus = await apiClient.GetRoadStatusAsync(displayName);

            Assert.Multiple(() =>
            {
                Assert.That(roadStatus?.RoadId, Is.EqualTo(roadId));
                Assert.That(roadStatus?.DisplayName, Is.EqualTo(displayName));
                Assert.That(roadStatus?.Status, Is.EqualTo(status));
                Assert.That(roadStatus?.StatusDescription, Is.EqualTo(statusDescription));
            });
        }

        [Test]
        public async Task CanHandleInvalidRoad()
        {
            const string roadId = "A233";
            var handler = new FakeHttpMessageHandler(HttpStatusCode.NotFound, BuildNotFoundResponse(roadId));

            var httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(Uri)
            };

            var apiClient = new RoadStatusApiClient(httpClient, Options.Create(new RoadStatusApiConfiguration()));

            var roadStatus = await apiClient.GetRoadStatusAsync(roadId);

            Assert.That(roadStatus, Is.EqualTo(null));
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
"""  ;
        }

        private static string BuildNotFoundResponse(string roadId)
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
"""  ;
        }
    }
}
