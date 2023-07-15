using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.Common;
using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class AccountController : Controller
{
    private readonly IServiceWrapper _service;
    private readonly IMapper _mapper;

    public AccountController(IServiceWrapper service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Get() =>
        Ok(_service.UserService.GetAll());


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id) =>
        Ok(await _service.UserService.GetByOid(id));

    [HttpPost]
    public async Task<IActionResult> Post(CreateUserDto model)
    {
        UserDto userDto = _mapper.Map<UserDto>(model);
        return Ok(await _service.UserService.CreateAsync(userDto));
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateUserDto model)
    {
        UserDto userDto = _mapper.Map<UserDto>(model);
        return Ok(await _service.UserService.Update(userDto));
    }


    [HttpDelete("{oid}")]
    public async Task<IActionResult> Delete(Guid oid) =>
        Ok(await _service.UserService.Delete(oid));

    [HttpPost("/token")]
    public IActionResult Token(string login, string password)
    {
        var identity = GetIdentity(login, password);
        if (identity == null)
        {
            return BadRequest(new { errorText = "Неправильное имя пользователя или пароль." });
        }

        var now = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(
            issuer: AuthenticationOptions.ISSUER,
            audience: AuthenticationOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(TimeSpan.FromMinutes(AuthenticationOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(AuthenticationOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        var response = new
        {
            access_token = encodedJwt,
            login = identity.Name
        };
        return Ok(response);
    }

    private ClaimsIdentity GetIdentity(string login, string password)
    {
        try
        {
            UserDto user = _service.UserService.GetAuthorizedUser(login, password).Result;
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}