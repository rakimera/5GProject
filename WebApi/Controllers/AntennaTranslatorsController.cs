using Application.Interfaces;
using Application.Models.AntennaTranslator;
using AutoMapper;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/antenna-translator")]
public class AntennaTranslatorsController : Controller
{

    private readonly IServiceWrapper _service;
    private readonly IMapper _mapper;

    public AntennaTranslatorsController(IServiceWrapper service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var baseResponse = _service.AntennaTranslatorService.GetAll();
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }
    [HttpGet("index")]
    public async Task<IActionResult> Get([FromQuery]DataSourceLoadOptionsBase loadOptions)
    {
        var loadResult = await _service.AntennaTranslatorService.GetLoadResult(loadOptions);
        return Ok(loadResult);
    }

    [HttpGet("getAll/{id}")]
    public IActionResult GetAll(string id)
    {
        var baseResponse = _service.AntennaTranslatorService.GetAllByProjectAntennaId(id);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }
    
    [HttpGet("{oid}")]
    public async Task<IActionResult> Get(string oid)
    {
        var baseResponse = await _service.AntennaTranslatorService.GetByOid(oid);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]CreateAntennaTranslatorDto model)
    {
        AntennaTranslatorDto antennaTranslatorDto = _mapper.Map<AntennaTranslatorDto>(model);
        var baseResponse = await _service.AntennaTranslatorService.CreateAsync(antennaTranslatorDto, User.Identity.Name);
        
        if (baseResponse.Success)
            return Ok(baseResponse);
        return BadRequest(baseResponse);
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateAntennaTranslatorDto model)
    {
        var baseResponse = await _service.AntennaTranslatorService.Update(model);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return BadRequest(baseResponse);
    }

    [HttpDelete("{oid}")]
    public async Task<IActionResult> Delete(string oid)
    {
        var baseResponse = await _service.AntennaTranslatorService.Delete(oid);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }
}