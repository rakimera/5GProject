using System.Security.Claims;
using Application.DataObjects;
using Application.Models;

namespace Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    public Task<BaseResponse<TokenDto>> Refresh(TokenDto tokenApiModel);
    public Task<BaseResponse<bool>> Revoke(string login);
}