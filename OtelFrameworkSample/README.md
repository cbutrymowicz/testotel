# .NET Framework OpenTelemetry Sample

This is a sample .NET Framework 4.8 console application that demonstrates how to use OpenTelemetry SDK to send logs, traces, and metrics via OTLP (OpenTelemetry Protocol) exporters.

## Features

- **Traces**: Creates activity spans to track operations
- **Metrics**: Records counters and histograms for request metrics
- **Logs**: Structured logging with OpenTelemetry integration
- **OTLP Exporter**: Sends telemetry data to an OpenTelemetry collector or compatible backend
- **Console Exporter**: Displays telemetry data in the console for debugging

## Prerequisites

- .NET Framework 4.8 or later
- Visual Studio 2019/2022 or .NET SDK with .NET Framework support (Windows only)
- (Optional) OpenTelemetry Collector or compatible backend for OTLP export

## Building the Application

### On Windows with Visual Studio:
```bash
cd OtelFrameworkSample
dotnet build
```

### Or using MSBuild:
```bash
cd OtelFrameworkSample
msbuild OtelFrameworkSample.csproj
```

## Running the Application

### Basic Run (Console Export Only):
```bash
cd OtelFrameworkSample\bin\Debug\net48
OtelFrameworkSample.exe
```

### With OTLP Exporter Configuration:
Set the `OTEL_EXPORTER_OTLP_ENDPOINT` environment variable to point to your OpenTelemetry collector:

```bash
# Windows Command Prompt
set OTEL_EXPORTER_OTLP_ENDPOINT=http://localhost:4317
cd OtelFrameworkSample\bin\Debug\net48
OtelFrameworkSample.exe

# Windows PowerShell
$env:OTEL_EXPORTER_OTLP_ENDPOINT = "http://localhost:4317"
cd OtelFrameworkSample\bin\Debug\net48
.\OtelFrameworkSample.exe
```

## Setting up OpenTelemetry Collector (Optional)

If you want to see the telemetry data being exported via OTLP, you can run an OpenTelemetry Collector:

1. Download the OpenTelemetry Collector from: https://github.com/open-telemetry/opentelemetry-collector/releases

2. Create a `config.yaml` file:
```yaml
receivers:
  otlp:
    protocols:
      grpc:
        endpoint: 0.0.0.0:4317
      http:
        endpoint: 0.0.0.0:4318

processors:
  batch:

exporters:
  logging:
    loglevel: debug

service:
  pipelines:
    traces:
      receivers: [otlp]
      processors: [batch]
      exporters: [logging]
    metrics:
      receivers: [otlp]
      processors: [batch]
      exporters: [logging]
    logs:
      receivers: [otlp]
      processors: [batch]
      exporters: [logging]
```

3. Run the collector:
```bash
otelcol --config=config.yaml
```

## What the Application Does

The sample application:

1. Configures OpenTelemetry with:
   - TracerProvider for distributed tracing
   - MeterProvider for metrics
   - LoggerFactory with OpenTelemetry integration

2. Generates sample telemetry by:
   - Creating 5 trace spans with custom attributes
   - Recording request counts and duration metrics
   - Logging structured information messages

3. Exports telemetry to:
   - Console (for immediate visibility)
   - OTLP endpoint (configurable via environment variable)

## NuGet Packages Used

- **OpenTelemetry** (v1.9.0): Core OpenTelemetry SDK
- **OpenTelemetry.Exporter.Console** (v1.9.0): Console exporter for debugging
- **OpenTelemetry.Exporter.OpenTelemetryProtocol** (v1.9.0): OTLP exporter
- **OpenTelemetry.Extensions.Hosting** (v1.9.0): Extensions for logging integration

## Configuration

The application reads the OTLP endpoint from the `OTEL_EXPORTER_OTLP_ENDPOINT` environment variable. If not set, it defaults to `http://localhost:4317`.

## Troubleshooting

- **Build errors**: Ensure you have .NET Framework 4.8 Developer Pack installed
- **Runtime errors**: This application must run on Windows as .NET Framework is Windows-only
- **OTLP export failures**: Check that your OpenTelemetry collector is running and accessible at the configured endpoint

## References

- [OpenTelemetry .NET](https://github.com/open-telemetry/opentelemetry-dotnet)
- [OpenTelemetry Specification](https://opentelemetry.io/docs/specs/otel/)
- [OTLP Protocol](https://opentelemetry.io/docs/specs/otlp/)
