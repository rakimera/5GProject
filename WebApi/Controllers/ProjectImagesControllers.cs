using Application.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/projectImages")]
[Authorize]
public class ProjectImagesControllers : Controller
{
    private readonly IServiceWrapper _service;
    private readonly IMapper _mapper;

    public ProjectImagesControllers(IServiceWrapper service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    /*[HttpGet]
    public IActionResult Get()
    {
        var baseResponse = _service.ImageService.GetAll();
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }*/
}