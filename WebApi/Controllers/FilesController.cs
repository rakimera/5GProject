using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/files")]
public class FilesController : Controller
{
    private readonly IServiceWrapper _service;

    public FilesController(IServiceWrapper service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get(string id)
    {
        // await _service.WordService.GetLoadXlsx();
        var baseResponse = await _service.FileService.ProjectWord(id);
        // var baseResponse = await _service.FileService.CreateGrafic();
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }
}