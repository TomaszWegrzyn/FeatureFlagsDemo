using Microsoft.FeatureManagement;

namespace FeatureFlagsDemo;

// [FilterAlias("DifferentName")]
public class PremiumFilter : IFeatureFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    // ReSharper disable once ConvertToPrimaryConstructor
    public PremiumFilter(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        return Task.FromResult(user.IsInRole("Premium"));
    }
}