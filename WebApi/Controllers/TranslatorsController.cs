using Application.Interfaces;
using Application.Models.Projects.ProjectAntennas;
using Application.Models.TranslatorSpecs;
using AutoMapper;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/translators")]
public class TranslatorsController : Controller
{
    private readonly IServiceWrapper _service;
    private readonly IMapper _mapper;

    public TranslatorsController(IServiceWrapper service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var baseResponse = _service.TranslatorSpecsService.GetAll();
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }

    [HttpGet("getAll/{id}")]
    public IActionResult GetAll(string id)
    {
        var baseResponse = _service.TranslatorSpecsService.GetAllByProjectId(id);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }
    
    [HttpGet("{oid}")]
    public async Task<IActionResult> Get(string oid)
    {
        var baseResponse = await _service.TranslatorSpecsService.GetByOid(oid);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]CreateTranslatorSpecsDto model)
    {
        TranslatorSpecsDto projectAntennaDto = _mapper.Map<TranslatorSpecsDto>(model);
        var baseResponse = await _service.TranslatorSpecsService.CreateAsync(projectAntennaDto, User.Identity.Name);
        
        if (baseResponse.Success)
            return Ok(baseResponse);
        return BadRequest(baseResponse);
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateTranslatorSpecsDto model)
    {
        var baseResponse = await _service.TranslatorSpecsService.Update(model);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return BadRequest(baseResponse);
    }

    [HttpDelete("{oid}")]
    public async Task<IActionResult> Delete(string oid)
    {
        var baseResponse = await _service.TranslatorSpecsService.Delete(oid);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }
}