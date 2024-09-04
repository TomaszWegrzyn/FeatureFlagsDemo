using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;

namespace FeatureFlagsDemo.Api;

public record WeatherAlert(DateTime Datetime, string AlertText)
{
    
}

[FeatureGate("PremiumWeatherAlerts")]
[Route("[controller]")]
[ApiController]
public class PremiumApiController : ControllerBase
{
    
    [HttpGet("alerts")]
    // [Authorize(Roles = "Premium")] would show 403 if not in the role
    public WeatherAlert[] GetPersonalWeatherAlerts()
    {
        return
        [
            new WeatherAlert(DateTime.Now, "Tornado warning"),
            new WeatherAlert(DateTime.Now.AddHours(1), "Flash flood warning"),
            new WeatherAlert(DateTime.Now.AddHours(2), "Heat advisory")
        ];
    }
}