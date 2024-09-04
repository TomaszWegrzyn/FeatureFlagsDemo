using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using FeatureFlagsDemo;
using FeatureFlagsDemo.Api;
using FeatureFlagsDemo.Authentication;
using FeatureFlagsDemo.OpenIdConnect;
using FeatureFlagsDemo.Swagger;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.AddOpenIdConnectOptions();

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerGenOptions>();
builder.Services.AddSwaggerGen();

builder.AddOpenIdConnectJwtBearerAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddHttpContextAccessor();

builder.Services
    .AddFeatureManagement()    
    .AddFeatureFilter<ABTestFilter>();
builder.Services.AddHttpContextAccessor();

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