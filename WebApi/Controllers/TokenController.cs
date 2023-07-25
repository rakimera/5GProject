using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IServiceWrapper _service;

    public TokenController(IServiceWrapper service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> Refresh(TokenDto tokenApiModel) => 
        Ok(await _service.TokenService.Refresh(tokenApiModel));

    [HttpPost, Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    [Route("revoke")]
    public async Task<IActionResult> Revoke()
    {
        return Ok(await _service.TokenService.Revoke(User.Identity.Name));
    }
}