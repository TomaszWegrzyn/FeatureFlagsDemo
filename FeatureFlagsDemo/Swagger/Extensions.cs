using Microsoft.Extensions.Options;

namespace FeatureFlagsDemo.Swagger;

public static class Extensions
{
    public static IApplicationBuilder UseSwaggerUi(this WebApplication app)
    {
        var oidcOptions = app.Services.GetRequiredService<IOptions<OpenIdConnectOptions>>().Value;
        return app.UseSwaggerUI(setup =>
        {
            setup.SwaggerEndpoint($"/swagger/v1/swagger.json", "Version 1.0");
            setup.OAuthClientId(oidcOptions.ClientId);
            setup.OAuthAppName("Weather API");
            setup.OAuthScopes("openid");
        });
    }
}