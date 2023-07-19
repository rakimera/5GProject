using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IServiceWrapper _service;

    public AuthController(IServiceWrapper service)
    {
        _service = service;
    }

    [HttpPost, Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginModel) => 
        Ok(await _service.TokenService.Login(loginModel));
}