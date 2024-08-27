using Microsoft.FeatureManagement;

namespace FeatureFlagsDemo.Api;

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public static class Extensions
{
    public static RouteHandlerBuilder AddWeatherApi(this WebApplication app)
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering",
            "Scorching"
        };
        return app.MapGet("/weatherforecast", async ( IFeatureManager featureManager, ILoggerFactory LoggerFactory) =>
            {
                var aiForecastEnabled = await featureManager
                    .IsEnabledAsync("AmazingAiBasedForecasting");
                if (aiForecastEnabled)
                {
                    return AmazingAiBasedForecast(summaries, LoggerFactory.CreateLogger("GetWeatherForecast"));
                }
                else
                {
                    return LegacyForecast(summaries, LoggerFactory.CreateLogger("GetWeatherForecast"));
                }
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi()
            .RequireAuthorization();
    }

    private static WeatherForecast[] LegacyForecast(string[] summaries, ILogger logger)
    {
        logger.LogInformation("Using legacy forecast");
        return Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
    }
    
    private static WeatherForecast[] AmazingAiBasedForecast(string[] summaries, ILogger logger)
    {
        logger.LogInformation("Using amazing AI-based forecast");
        return Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
    }
}