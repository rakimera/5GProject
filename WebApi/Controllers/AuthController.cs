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
    public IActionResult Login([FromBody] LoginDto loginModel) => 
        Ok(_service.TokenService.Login(loginModel));
}