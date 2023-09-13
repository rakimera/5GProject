using Application.Interfaces;
using Application.Models.RadiationZone;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/radiationZones")]
public class RadiationZoneController : Controller
{
    private readonly IServiceWrapper _service;
    private readonly IMapper _mapper;

    public RadiationZoneController(
        IServiceWrapper service, 
        IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }
    
    // [HttpGet, Authorize(Roles = "Admin")]
    [HttpGet,]
    public IActionResult Get()
    {
        var baseResponse = _service.RadiationZoneService.GetAll();
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }
    
    
    [HttpGet("{oid}")]
    public async Task<IActionResult> Get(string oid)
    {
        var baseResponse = await _service.RadiationZoneService.GetByOid(oid);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateRadiationZoneDto model)
    {
        RadiationZoneDto radiationZoneDto = _mapper.Map<RadiationZoneDto>(model);
        var baseResponse = await _service.RadiationZoneService.CreateAsync(radiationZoneDto, User.Identity.Name);
        
        if (baseResponse.Success)
            return Ok(baseResponse);
        return BadRequest(baseResponse);
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateRadiationZoneDto model)
    {
        var baseResponse = await _service.RadiationZoneService.Update(model, User.Identity.Name);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return BadRequest(baseResponse);
    }
        
    [HttpDelete("{oid}")]
    public async Task<IActionResult> Delete(string oid)
    {
        var baseResponse = await _service.RadiationZoneService.Delete(oid);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }
}