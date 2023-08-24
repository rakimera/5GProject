using Application.Interfaces;
using Application.Models;
using Application.Models.Address;
using AutoMapper;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]

public class TownsController : Controller
{
    private readonly IServiceWrapper _service;
    private readonly IMapper _mapper;

    public TownsController(IServiceWrapper service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }
    
    [HttpGet]
    public IActionResult Get()
    {
        var baseResponse = _service.TownService.GetAll();
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }
    
    
    [HttpGet("{oid}")]
    public async Task<IActionResult> Get(string oid)
    {
        var baseResponse = await _service.TownService.GetByOid(oid);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateTownDto model)
    {
        TownDto townDto = _mapper.Map<TownDto>(model);
        var baseResponse = await _service.TownService.CreateAsync(townDto, User.Identity.Name);
        
        if (baseResponse.Success)
            return Ok(baseResponse);
        return BadRequest(baseResponse);
    }

    [HttpDelete("{oid}")]
    public async Task<IActionResult> Delete(string oid)
    {
        var baseResponse = await _service.TownService.Delete(oid);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }
    
    [HttpGet("index"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> Get([FromQuery]DataSourceLoadOptionsBase loadOptions)
    {
        var loadResult = await _service.TownService.GetLoadResult(loadOptions);
        return Ok(loadResult);
    }
}