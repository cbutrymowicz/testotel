# Build and run the .NET Framework OpenTelemetry sample

Write-Host "Building the project..." -ForegroundColor Cyan
Set-Location OtelFrameworkSample
dotnet build

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit $LASTEXITCODE
}

Write-Host ""
Write-Host "Build succeeded!" -ForegroundColor Green
Write-Host ""

# Set the OTLP endpoint if not already set
if (-not $env:OTEL_EXPORTER_OTLP_ENDPOINT) {
    $env:OTEL_EXPORTER_OTLP_ENDPOINT = "http://localhost:4317"
    Write-Host "Using default OTLP endpoint: http://localhost:4317" -ForegroundColor Yellow
} else {
    Write-Host "Using configured OTLP endpoint: $env:OTEL_EXPORTER_OTLP_ENDPOINT" -ForegroundColor Yellow
}

Write-Host ""
Write-Host "Running the application..." -ForegroundColor Cyan
Write-Host ""

Set-Location bin\Debug\net48
.\OtelFrameworkSample.exe

Set-Location ..\..\..\..
Read-Host "Press Enter to exit"
