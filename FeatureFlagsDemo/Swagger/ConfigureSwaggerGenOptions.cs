using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using FeatureFlagsDemo.OpenIdConnect;

namespace FeatureFlagsDemo;

public class ConfigureSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions>
{
    private const string SecuritySchemeName = "OpenIdConnect";
    private readonly IConfiguration _configuration;

    public ConfigureSwaggerGenOptions(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(SwaggerGenOptions options)
    {
        var oidcOptions = _configuration.GetOpenIdConnectOptions();
        var oidcScheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OpenIdConnect,
            OpenIdConnectUrl = new Uri($"{oidcOptions.ProviderAddress}/.well-known/openid-configuration"),
            
            // use id_token instead of access_token
            Extensions = new Dictionary<string, IOpenApiExtension>
            {
                { "x-tokenName", new OpenApiString("id_token") } 
            },
            
            Description = "OpenId Security Scheme"
        };
        options.AddSecurityDefinition(SecuritySchemeName, oidcScheme);

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme, 
                        Id = SecuritySchemeName
                    }
                },
                new List<string> { }
            }
        });
    }
}