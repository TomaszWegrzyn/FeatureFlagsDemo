using Microsoft.AspNetCore.Authentication.JwtBearer;
using FeatureFlagsDemo.OpenIdConnect;

namespace FeatureFlagsDemo.Authentication;

public static class Extensions
{
    public static WebApplicationBuilder AddOpenIdConnectJwtBearerAuthentication(this WebApplicationBuilder builder)
    {
        var openIdConnectOptions = builder.Configuration.GetOpenIdConnectOptions();
        
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opts =>
            {
                opts.Authority = openIdConnectOptions.ProviderAddress;
                opts.Audience = openIdConnectOptions.ClientId;
            });
        return builder;
    }
}