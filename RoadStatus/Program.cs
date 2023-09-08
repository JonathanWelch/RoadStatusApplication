using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RoadStatus.Domain.Interfaces;
using RoadStatus.Infrastructure;
using RoadStatus.Infrastructure.TfL;

namespace RoadStatus;

internal class Program
{
    static async Task<int> Main(string[] args)
    {
        try
        {
            if (args.Length == 0)
            {
                Console.WriteLine("A road must be specified.");
                return 1;
            }

            var roadId = string.Join(" ", args);

            using var host = BuildHost(args);
            var roadStatusService = host.Services.GetRequiredService<RoadStatusService>();

            var roadStatusCheckSuccessful = await roadStatusService.CheckStatus(roadId);

            return roadStatusCheckSuccessful ? 0 : 1;

        }
        catch (Exception)
        {
            Console.WriteLine("An error has occurred running the application.");
            return 1;
        }
    }

    private static IHost BuildHost(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.AddTransient<RoadStatusService>();
                services.AddHttpClient<IRoadService, RoadStatusApiClient>()
                    .ConfigureHttpClient(client =>
                    {
                        client.BaseAddress = new Uri(GetUriString(context));
                    });
                services.AddTransient<IOutput, ConsoleOutput>();
                services.AddOptions<RoadStatusApiConfiguration>()
                    .Configure<IConfiguration>((options, configuration) =>
                    {
                        configuration.GetSection("AppSettings").Bind(options);
                    });

            })
            .Build();
    }

    private static string GetUriString(HostBuilderContext context)
    {
        const string appSetting = "AppSettings:ApiUri";

        var uri = context.Configuration[appSetting];

        return uri ?? throw new Exception($"Configuration setting value for '{appSetting}' can not be found.");
    }
}