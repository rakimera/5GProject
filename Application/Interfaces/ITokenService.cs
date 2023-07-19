using System.Security.Claims;
using Application.DataObjects;
using Application.Models;

namespace Application.Interfaces;

public interface ITokenService
{
    Task<string> GenerateAccessToken(IEnumerable<Claim> claims);
    Task<string> GenerateRefreshToken();
    Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token);
    public Task<BaseResponse<TokenDto>> Login(LoginDto loginModel);
}