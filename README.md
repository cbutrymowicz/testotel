# testotel

This repository contains a sample .NET Framework application demonstrating OpenTelemetry (OTEL) integration.

## Projects

### OtelFrameworkSample

A .NET Framework 4.8 console application that demonstrates:
- OpenTelemetry SDK integration
- Sending logs via OpenTelemetry
- Sending metrics via OpenTelemetry
- Exporting telemetry data via OTLP (OpenTelemetry Protocol)

See [OtelFrameworkSample/README.md](OtelFrameworkSample/README.md) for detailed information.

## Quick Start

### Option 1: Using Helper Scripts (Windows)

**Command Prompt:**
```cmd
run-sample.bat
```

**PowerShell:**
```powershell
.\run-sample.ps1
```

### Option 2: Manual Steps

1. Build the solution:
   ```cmd
   cd OtelFrameworkSample
   dotnet build
   ```

2. Run the application:
   ```cmd
   cd bin\Debug\net48
   OtelFrameworkSample.exe
   ```

## OpenTelemetry Collector (Optional)

To receive and view the exported telemetry data, you can run an OpenTelemetry Collector:

1. Download the OpenTelemetry Collector from: https://github.com/open-telemetry/opentelemetry-collector/releases

2. Use the provided configuration file:
   ```cmd
   otelcol --config=otel-collector-config.yaml
   ```

3. Run the sample application (it will export to `http://localhost:4317` by default)

## Environment Variables

- `OTEL_EXPORTER_OTLP_ENDPOINT`: Set the OTLP endpoint (default: `http://localhost:4317`)

Example:
```cmd
set OTEL_EXPORTER_OTLP_ENDPOINT=http://collector.example.com:4317
```

For more details, see the [OtelFrameworkSample README](OtelFrameworkSample/README.md).