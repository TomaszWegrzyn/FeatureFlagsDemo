using Winton.Extensions.Configuration.Consul;

namespace FeatureFlagsDemo.ConfigurationSources;

public static class Extensions
{
    public static WebApplicationBuilder AddConsulConfigurationSource(this WebApplicationBuilder builder)
    {
        builder.Configuration.AddConsul(builder.Environment.ApplicationName, (configSource) =>
        {
            configSource.Optional = true;
            configSource.ReloadOnChange = true;
        });
        return builder;
    }
}