using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.FeatureManagement;

namespace FeatureFlagsDemo;

// [FilterAlias("DifferentName")]
public class ABTestFilter : IFeatureFilter
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    // ReSharper disable once ConvertToPrimaryConstructor
    public ABTestFilter(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }

    public Task<bool> EvaluateAsync(FeatureFilterEvaluationContext context)
    {
        var user = _httpContextAccessor.HttpContext!.User;
        var userId = user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;

        return Task.FromResult(IsInABTestGroup(userId));
    }
    
    private static bool IsInABTestGroup(string userIdentifier)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(userIdentifier));
        var hashInt = BitConverter.ToInt32(hashBytes, 0);
        return Math.Abs(hashInt) % 2 == 0;
    }
}