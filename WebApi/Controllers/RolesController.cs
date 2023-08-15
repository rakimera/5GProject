using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/roles")]
public class RolesController : Controller
{
    private readonly IServiceWrapper _service;

    public RolesController(IServiceWrapper service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var baseResponse = _service.UserService.GetRoles();
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }
}