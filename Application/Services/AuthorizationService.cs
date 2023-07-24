using System.Security.Claims;
using Application.Common;
using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models;
using Domain.Entities;

namespace Application.Services;

public class AuthorizationService : IAuthorizationService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ITokenService _tokenService;

    public AuthorizationService(IRepositoryWrapper repositoryWrapper, ITokenService tokenService)
    {
        _repositoryWrapper = repositoryWrapper;
        _tokenService = tokenService;
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
        var refresh = _tokenService.GenerateRefreshToken();
        RefreshToken refreshToken = new RefreshToken
        {
            Token = refresh,
            UserId = user.Id,
            RefreshTokenExpiryTime = DateTime.Now.AddMinutes(AuthenticationOptions.LIFETIMEREFRESHTOKEN)
        };
        await _repositoryWrapper.TokenRepository.CreateAsync(refreshToken);

        await _repositoryWrapper.Save();
        TokenDto tokenDto = new TokenDto
        {
            AccessToken = _tokenService.GenerateAccessToken(claims),
            RefreshToken = refresh
        };
        return new BaseResponse<TokenDto>(
            Result: tokenDto,
            Messages: new List<string>{"Пользователь успешно авторизован"},
            Success: true,
            StatusCode: 200);
    }
}