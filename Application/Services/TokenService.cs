using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Application.Common;
using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models;
using Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services;

public class TokenService : ITokenService
{
    private readonly IRepositoryWrapper _repositoryWrapper;

    public TokenService(IRepositoryWrapper repositoryWrapper)
    {
        _repositoryWrapper = repositoryWrapper;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var signinCredentials = new SigningCredentials(AuthenticationOptions.GetSymmetricSecurityKey(),
            SecurityAlgorithms.HmacSha256);
        var tokeOptions = new JwtSecurityToken(
            issuer: AuthenticationOptions.ISSUER,
            audience: AuthenticationOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.Now.AddMinutes(AuthenticationOptions.LIFETIME),
            signingCredentials: signinCredentials
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        return tokenString;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
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
        if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid token");
        return principal;
    }

    public async Task<BaseResponse<TokenDto>> Refresh(TokenDto tokenApiModel)
    {
        if (tokenApiModel is null)
            return new BaseResponse<TokenDto>(
                Result: null,
                Messages: new List<string> { "Данные пусты" },
                Success: false);

        var principal = GetPrincipalFromExpiredToken(tokenApiModel.AccessToken);
        var user = await _repositoryWrapper.UserRepository.GetByCondition(x => x.Login == principal.Identity.Name);
        var refreshToken = _repositoryWrapper.TokenRepository.GetAll()
            .FirstOrDefault(x => x.Token == tokenApiModel.RefreshToken);

        if (refreshToken.RefreshTokenExpiryTime <= DateTime.Now)
        {
            _repositoryWrapper.TokenRepository.Delete(refreshToken);
        }

        if (user is null
            || refreshToken is null
            || refreshToken.Token != tokenApiModel.RefreshToken)
            return new BaseResponse<TokenDto>(
                Result: null,
                Messages: new List<string> { "Неверный запрос" },
                Success: false);

        var newRefreshToken = GenerateRefreshToken();
        RefreshToken refToken = new RefreshToken
        {
            Id = refreshToken.Id,
            Token = newRefreshToken,
            UserId = user.Id,
            RefreshTokenExpiryTime = DateTime.Now.AddMinutes(AuthenticationOptions.LIFETIMEREFRESHTOKEN)
        };
        _repositoryWrapper.TokenRepository.Update(refToken);
        await _repositoryWrapper.Save();

        var tokenDto = new TokenDto()
        {
            AccessToken = GenerateAccessToken(principal.Claims),
            RefreshToken = newRefreshToken
        };

        return new BaseResponse<TokenDto>(
            Result: tokenDto,
            Success: true,
            Messages: new List<string> { "Авторизация прошла успешно" });
    }


    public async Task<BaseResponse<bool>> Revoke(string login)
    {
        var user = await _repositoryWrapper.UserRepository.GetByCondition(x => x.Login == login);
        if (user == null)
            return new BaseResponse<bool>(
                Result: false,
                Messages: new List<string> { "Такого пользователя нет" },
                Success: false);
        await _repositoryWrapper.Save();
        return new BaseResponse<bool>(
            Result: true,
            Messages: new List<string> { "Операция произведена успешно" },
            Success: true);
    }
}