using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class AccountController : Controller
{
    private readonly IServiceWrapper _service;

    public AccountController(IServiceWrapper service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Get(int id)
    {
        List<UserDTO> dtoModel = _service.UserService.GetAll().ToList();

        return Ok(dtoModel);
    }
}