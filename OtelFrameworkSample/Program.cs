using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Threading;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace OtelFrameworkSample
{
    class Program
    {
        private static readonly ActivitySource ActivitySource = new ActivitySource("OtelFrameworkSample");
        private static readonly Meter Meter = new Meter("OtelFrameworkSample", "1.0.0");
        private static readonly Counter<long> RequestCounter = Meter.CreateCounter<long>("sample.requests");
        private static readonly Histogram<double> RequestDuration = Meter.CreateHistogram<double>("sample.request.duration");

        static void Main(string[] args)
        {
            Console.WriteLine("=== .NET Framework OpenTelemetry Sample ===");
            Console.WriteLine();

            // Configure OpenTelemetry resource
            var resourceBuilder = ResourceBuilder.CreateDefault()
                .AddService("OtelFrameworkSample", serviceVersion: "1.0.0");

            // Configure Tracing
            using var tracerProvider = Sdk.CreateTracerProviderBuilder()
                .SetResourceBuilder(resourceBuilder)
                .AddSource("OtelFrameworkSample")
                .AddConsoleExporter()
                .AddOtlpExporter(options =>
                {
                    // Read from environment variable or use default
                    options.Endpoint = new Uri(
                        Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") 
                        ?? "http://localhost:4317");
                })
                .Build();

            // Configure Metrics
            using var meterProvider = Sdk.CreateMeterProviderBuilder()
                .SetResourceBuilder(resourceBuilder)
                .AddMeter("OtelFrameworkSample")
                .AddConsoleExporter()
                .AddOtlpExporter(options =>
                {
                    options.Endpoint = new Uri(
                        Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") 
                        ?? "http://localhost:4317");
                })
                .Build();

            // Configure Logging
            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddOpenTelemetry(options =>
                {
                    options.SetResourceBuilder(resourceBuilder);
                    options.AddConsoleExporter();
                    options.AddOtlpExporter(otlpOptions =>
                    {
                        otlpOptions.Endpoint = new Uri(
                            Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") 
                            ?? "http://localhost:4317");
                    });
                });
            });

            var logger = loggerFactory.CreateLogger<Program>();

            Console.WriteLine("OpenTelemetry configured successfully!");
            Console.WriteLine($"OTLP Endpoint: {Environment.GetEnvironmentVariable("OTEL_EXPORTER_OTLP_ENDPOINT") ?? "http://localhost:4317"}");
            Console.WriteLine();

            // Generate sample telemetry data
            Console.WriteLine("Generating sample telemetry...");
            Console.WriteLine();

            var random = new Random();
            for (int i = 1; i <= 5; i++)
            {
                // Create a trace span
                using (var activity = ActivitySource.StartActivity($"SampleOperation-{i}"))
                {
                    activity?.SetTag("iteration", i);
                    activity?.SetTag("sample.attribute", "test-value");

                    // Log some information
                    logger.LogInformation("Processing request {RequestId} at {Timestamp}", 
                        i, DateTime.UtcNow);

                    // Simulate some work
                    var duration = random.Next(100, 500);
                    Thread.Sleep(duration);

                    // Record metrics
                    RequestCounter.Add(1, 
                        new KeyValuePair<string, object>("operation", $"sample-op-{i}"),
                        new KeyValuePair<string, object>("status", "success"));

                    RequestDuration.Record(duration, 
                        new KeyValuePair<string, object>("operation", $"sample-op-{i}"));

                    logger.LogInformation("Completed request {RequestId} in {Duration}ms", 
                        i, duration);
                }

                Console.WriteLine($"[{i}/5] Request processed");
            }

            Console.WriteLine();
            Console.WriteLine("Telemetry generation complete!");
            Console.WriteLine();

            // Log summary
            logger.LogInformation("Sample application completed successfully. Total requests: {Count}", 5);

            // Give time for exporters to flush
            Console.WriteLine("Waiting for exporters to flush...");
            Thread.Sleep(2000);

            Console.WriteLine();
            Console.WriteLine("Done! Press any key to exit...");
            Console.ReadKey();
        }
    }
}
