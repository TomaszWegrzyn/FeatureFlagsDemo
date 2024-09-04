namespace FeatureFlagsDemo;

public class TrackingUserDataMiddleware : IMiddleware
{
    private readonly ILogger<TrackingUserDataMiddleware> _logger;

    // ReSharper disable once ConvertToPrimaryConstructor
    public TrackingUserDataMiddleware(ILogger<TrackingUserDataMiddleware> logger)
    {
        _logger = logger;
    }

    public Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        _logger.LogInformation("Imaginary tracking user data middleware");
        return next(context);
    }
}