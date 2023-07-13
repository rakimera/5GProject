using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        return NotFound();
    }
}