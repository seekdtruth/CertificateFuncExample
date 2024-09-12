using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WindowsServer.TelemetryChannel;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static void Main(string[] args)
    {
        var host = new HostBuilder()
        .ConfigureFunctionsWebApplication()
        .ConfigureServices(services =>
        {
            services.AddSingleton(new CertService());

            services.AddApplicationInsightsTelemetryWorkerService();
            //services.ConfigureFunctionsApplicationInsights();

            services.Configure<TelemetryConfiguration>((config) =>
            {
                AdaptiveSamplingTelemetryProcessor adaptiveSamplingProcessor = null;

                foreach (var processor in config.DefaultTelemetrySink.TelemetryProcessors)
                {
                    if (processor is AdaptiveSamplingTelemetryProcessor adaptiveProcessor)
                    {
                        adaptiveSamplingProcessor = adaptiveProcessor;
                        break; // Exit the loop once the first match is found
                    }
                }

                if (adaptiveSamplingProcessor != null)
                {
                    adaptiveSamplingProcessor.MinSamplingPercentage = 100;
                }
            });
        })
        .Build();

        host.Run();
    }
}