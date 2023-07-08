using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class AccountController : Controller
{
    private readonly IServiceWrapper _service;
    private readonly IEnergyFlowSummation _energyFlowSummation;

    public AccountController(IEnergyFlowSummation energyFlowSummation, IServiceWrapper service)
    {
        _energyFlowSummation = energyFlowSummation;
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int id)
    {
        var dtoModel =  _service.UserService;

        return Ok(dtoModel);
    }
}