@echo off
REM Build and run the .NET Framework OpenTelemetry sample

echo Building the project...
cd OtelFrameworkSample
dotnet build

if %errorlevel% neq 0 (
    echo Build failed!
    pause
    exit /b %errorlevel%
)

echo.
echo Build succeeded!
echo.

REM Set the OTLP endpoint if not already set
if "%OTEL_EXPORTER_OTLP_ENDPOINT%"=="" (
    set OTEL_EXPORTER_OTLP_ENDPOINT=http://localhost:4317
    echo Using default OTLP endpoint: http://localhost:4317
) else (
    echo Using configured OTLP endpoint: %OTEL_EXPORTER_OTLP_ENDPOINT%
)

echo.
echo Running the application...
echo.

cd bin\Debug\net48
OtelFrameworkSample.exe

cd ..\..\..\..
pause
