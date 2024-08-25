namespace FeatureFlagsDemo;

public class OpenIdConnectOptions
{
    public const string OpenIdConnect = "OpenIdConnect";
    
    public required string ClientId { get; set; }
    public required string ProviderAddress { get; set; }
}