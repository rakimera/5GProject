using Application.Interfaces;
using Application.Models.Projects.ProjectImages;
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

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(string id)
    {
        var baseResponse = await _service.ProjectImageService.GetByOid(id);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody]ProjectImageDto model, IFormFile uploadedFile)
    {
        var saveFileResponse = await _service.ProjectImageService.SaveFile(model, uploadedFile);
        if (saveFileResponse.Success)
        {
            var baseResponse = await _service.ProjectImageService.CreateAsync(model, User.Identity.Name);
        
            if (baseResponse.Success)
                return Ok(baseResponse);
            return BadRequest(baseResponse);
        }
        
        return BadRequest(saveFileResponse);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var baseResponse = await _service.ProjectImageService.Delete(id);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }
}