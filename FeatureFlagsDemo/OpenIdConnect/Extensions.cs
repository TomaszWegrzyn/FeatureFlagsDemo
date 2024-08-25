namespace FeatureFlagsDemo.OpenIdConnect;

public static class Extensions
{
    public static WebApplicationBuilder AddOpenIdConnectOptions(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<OpenIdConnectOptions>(
            builder.Configuration.GetSection(OpenIdConnectOptions.OpenIdConnect)
        );
        return builder;
    }
    
    public static OpenIdConnectOptions GetOpenIdConnectOptions(this IConfiguration configuration)
    {
        return configuration
            .GetRequiredSection(OpenIdConnectOptions.OpenIdConnect)
            .Get<OpenIdConnectOptions>()!;
    }
}