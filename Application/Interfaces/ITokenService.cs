using System.Security.Claims;

namespace Application.Interfaces;

public interface ITokenService
{
    Task<string> GenerateAccessToken(IEnumerable<Claim> claims);
    Task<string> GenerateRefreshToken();
    Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
}