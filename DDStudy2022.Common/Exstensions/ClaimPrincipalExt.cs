using System.Security.Claims;

namespace DDStudy2022.Common.Exstensions;

public static class ClaimPrincipalExt
{
    public static T? GetClaimValue<T>(this ClaimsPrincipal user, string claim)
    {
        var value = user.Claims.FirstOrDefault(it => it.Type == claim)?.Value;
        if (value==null)
        {
            return default;
        }

        return Utils.Convert<T>(value);
    }
}