using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Application.Common;
using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class TokenService : ITokenService
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public TokenService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public Task<string> GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var secretKey = AuthenticationOptions.GetSymmetricSecurityKey();
        var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
        var tokeOptions = new JwtSecurityToken(
            issuer: AuthenticationOptions.ISSUER,
            audience: AuthenticationOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.Now.AddMinutes(AuthenticationOptions.LIFETIME),
            signingCredentials: signinCredentials
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        return Task.FromResult(tokenString);
    }
    
    public Task<string> GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Task.FromResult(Convert.ToBase64String(randomNumber));
        }
    }
    
    public Task<ClaimsPrincipal> GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = AuthenticationOptions.GetSymmetricSecurityKey(),
            ValidateLifetime = false 
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
        return Task.FromResult(principal);
    }

    public async Task<BaseResponse<TokenDto>>Login(LoginDto loginModel)
    {
        if (loginModel is null)
        {
            return new BaseResponse<TokenDto>(
                Result: null,
                Messages: new List<string>{"Данные пусты"},
                Success: false,
                StatusCode: 404);
        }
        var user = await _repositoryWrapper.UserRepository.GetByCondition
            (x => x.Login == loginModel.Login && x.Password == loginModel.Password);
        if (user is null)
            return new BaseResponse<TokenDto>(
                Result: null,
                Messages: new List<string>{"Такого пользователя не существует"},
                Success: false,
                StatusCode: 404);
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, loginModel.Login),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var accessToken = await GenerateAccessToken(claims);
        var refreshToken = await GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(AuthenticationOptions.LIFETIMEREFRESHTOKEN);
        _repositoryWrapper.UserRepository.Update(user);
        await _repositoryWrapper.Save();
        TokenDto tokenDto = new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        return new BaseResponse<TokenDto>(
            Result: tokenDto,
            Messages: new List<string>{"Пользователь успешно авторизован"},
            Success: true,
            StatusCode: 200);
    }
}