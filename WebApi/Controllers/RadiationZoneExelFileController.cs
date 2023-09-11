using Application.Interfaces;
using Application.Models.RadiationZone.RadiationZoneExelFile;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/radiationZone-files")]
[Authorize]
public class RadiationZoneExelFileController : Controller
{
    private readonly IServiceWrapper _service;
    private readonly IMapper _mapper;

    public RadiationZoneExelFileController(IServiceWrapper service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }
    
    [HttpGet("{id}")]
    public IActionResult Get(string id)
    {
        var baseResponse = _service.RadiationZoneExelFileService.GetAllById(id);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromForm]RadiationZoneExelFileDto model, [FromForm]IFormFile uploadedFile)
    {
        var saveFileResponse = await _service.RadiationZoneExelFileService.ConvertExel(model, uploadedFile);
        if (saveFileResponse.Success)
        {
            var baseResponse = await _service.RadiationZoneExelFileService.CreateAsync(saveFileResponse.Result, User.Identity.Name);
            
            return Ok(baseResponse);
        }
        
        return Ok(saveFileResponse);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var baseResponse = await _service.RadiationZoneExelFileService.Delete(id);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }
}
