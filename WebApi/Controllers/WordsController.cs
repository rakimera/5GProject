using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/words")]
public class WordsController : Controller
{
    private readonly IServiceWrapper _service;

    public WordsController(IServiceWrapper service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        // await _service.WordService.GetLoadXlsx();
        var baseResponse = await _service.WordService.ProjectWord();
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }
}