using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[Route("api/energy-flows")]
[ApiController]
[Authorize]
public class EnergyFlowsController : Controller
{
    private readonly IServiceWrapper _serviceWrapper;

    public EnergyFlowsController(IServiceWrapper serviceWrapper)
    {
        _serviceWrapper = serviceWrapper;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var response = await _serviceWrapper.EnergyFlowService.CreateAsync(id, User.Identity.Name);
        return Ok(response);
    }
}