using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace UserInterface.Controllers;

[ApiController]
[Route("api/users")]
public class AccountController : Controller
{
    private readonly UserService _userService;

    public AccountController(UserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int id)
    {
        UserDTO? dtoModel = await _userService.GetByIdAsync(id);
        return Ok(dtoModel);
    }
}