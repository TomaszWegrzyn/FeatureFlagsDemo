using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using FeatureFlagsDemo;
using FeatureFlagsDemo.Api;
using FeatureFlagsDemo.Authentication;
using FeatureFlagsDemo.ConfigurationSources;
using FeatureFlagsDemo.OpenIdConnect;
using FeatureFlagsDemo.Swagger;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder();
builder.AddConsulConfigurationSource();

builder.AddOpenIdConnectOptions();
builder.AddOpenIdConnectJwtBearerAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<TrackingUserDataMiddleware>();

// builder.Services.AddScopedFeatureManagement(); // Use this to make feature filters use scoped services

builder.Services
    .AddFeatureManagement()
    .AddFeatureFilter<ABTestFilter>()
    .AddFeatureFilter<PremiumFilter>()
    .WithTargeting();

builder.Services.AddControllers();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUi();
}

app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();

app.UseMiddlewareForFeature<TrackingUserDataMiddleware>("TrackingUserData");

app.UseHttpsRedirection();
app.AddWeatherApi();

app.Run();