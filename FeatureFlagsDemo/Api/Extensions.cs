using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;

namespace FeatureFlagsDemo.Api;

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public static class Extensions
{
    public static WebApplication AddWeatherApi(this WebApplication app)
    {
        var summaries1 = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering",
            "Scorching"
        };
        
        var summaries2 = new[]
        {
            "Ice Cube", "Fridge Mode", "Chilly", "Cool-ish", "Comfy", "Toasty", "T-shirt Time", "Roasting", "Melting", "Inferno"
        };
        app.MapGet("/weatherforecast", async ( IFeatureManager featureManager, ILoggerFactory loggerFactory) =>
            {
                var aiForecastEnabled = await featureManager
                    .IsEnabledAsync("AiBasedForecasting");
                var experimentalSummariesEnabled = await featureManager
                    .IsEnabledAsync("ExperimentalSummaries");
                
                var summariesToUse = experimentalSummariesEnabled ? summaries2 : summaries1; 
                if (aiForecastEnabled)
                {
                    return AmazingAiBasedForecast(summariesToUse, loggerFactory.CreateLogger("GetWeatherForecast"));
                }
                else
                {
                    return LegacyForecast(summariesToUse, loggerFactory.CreateLogger("GetWeatherForecast"));
                }
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi()
            .RequireAuthorization();
        
        app.MapGet("/weatherhistory", async ([FromQuery, Range(1, 5000)] ulong pastDays, IFeatureManager featureManager, ILoggerFactory loggerFactory) =>
            {
                var experimentalSummariesEnabled = await featureManager
                    .IsEnabledAsync("ExperimentalSummaries");
                
                var limitRetrievingReallyOldData = await featureManager
                    .IsEnabledAsync("LimitRetrievingReallyOldData");
                if (limitRetrievingReallyOldData)
                {
                    if (pastDays > 365)
                    {
                        return Results.BadRequest("You can only retrieve up to 365 days of weather history");
                    }
                }
                
                var summariesToUse = experimentalSummariesEnabled ? summaries2 : summaries1; 
                return Results.Ok(
                    Enumerable.Range(1, (int)pastDays).Select(index =>
                        new WeatherForecast
                        (
                            DateOnly.FromDateTime(DateTime.Now.AddDays(-index)),
                            Random.Shared.Next(-20, 55),
                            summariesToUse[Random.Shared.Next(summariesToUse.Length)]
                        ))
                    .ToArray()
                );
            })
            .WithName("GetWeatherHistory")
            .WithOpenApi()
            .RequireAuthorization();

        return app;
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