using Application.DataObjects;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IServiceWrapper _service;
    private readonly IMapper _mapper;

    public TokenController(IServiceWrapper service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("refresh")]
    public IActionResult Refresh(TokenDto tokenApiModel)
    {
        if (tokenApiModel is null)
            return BadRequest("Invalid client request");
        string accessToken = tokenApiModel.AccessToken;
        string refreshToken = tokenApiModel.RefreshToken;
        var principal = _service.TokenService.GetPrincipalFromExpiredToken(accessToken);
        var username = principal.Identity.Name;
        var user = _service.UserService.GetByLogin(username).Result.Result;
        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return BadRequest("Неверный запрос");
        var newAccessToken = _service.TokenService.GenerateAccessToken(principal.Claims);
        var newRefreshToken = _service.TokenService.GenerateRefreshToken();
        user.RefreshToken = newRefreshToken;
        UserDto UserDto = _mapper.Map<UserDto>(user);
        _service.UserService.Update(UserDto);
        return Ok(new BaseResponse<TokenDto>(
            new TokenDto()
            {
            AccessToken = accessToken,
            RefreshToken = refreshToken 
            }, 
            true, 200, "Авторизация прошла успешно"));
    }
    [HttpPost, Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    [Route("revoke")]
    public IActionResult Revoke()
    {
        var username = User.Identity.Name;
        var user = _service.UserService.GetByLogin(username).Result.Result;
        if (user == null) return BadRequest();
        user.RefreshToken = null;
        UserDto UserDto = _mapper.Map<UserDto>(user);
        _service.UserService.Update(UserDto);
        return NoContent();
    }
}