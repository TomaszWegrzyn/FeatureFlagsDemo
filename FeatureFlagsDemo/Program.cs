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
builder.Services
    .AddFeatureManagement()    
    .AddFeatureFilter<ABTestFilter>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUi();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.AddWeatherApi();

app.Run();