using System.Security.Claims;
using Application.Common;
using Application.Interfaces;
using Application.Models;
using Application.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IServiceWrapper _service;
    private readonly IMapper _mapper;

    public AuthController(IServiceWrapper service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpPost, Route("login")]
    public IActionResult Login([FromBody] LoginDto loginModel)
    {
        if (loginModel is null)
            return BadRequest("Invalid client request");

        var user = _service.UserService.GetAuthorizedUser(loginModel.Login, loginModel.Password).Result;
        if (user is null)
            return Unauthorized();
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, loginModel.Login),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var accessToken = _service.TokenService.GenerateAccessToken(claims);
        var refreshToken = _service.TokenService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(AuthenticationOptions.LIFETIMEREFRESHTOKEN);
        
        UserDto UserDto = _mapper.Map<UserDto>(user);
        _service.UserService.Update(UserDto);
        
        TokenDto tokenDto = new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
        
        return Ok(tokenDto);
    }
}