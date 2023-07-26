using Application.Interfaces;
using Application.Models.ContrAgents;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ContrAgentsController : Controller
{
    
    private readonly IServiceWrapper _service;
    private readonly IMapper _mapper;

    public ContrAgentsController(IMapper mapper, IServiceWrapper service)
    {
        _mapper = mapper;
        _service = service;
    }

    [HttpGet]
    public IActionResult Get() => 
        Ok(_service.ContrAgentService.GetAll());
    
    
    [HttpGet("{oid}")]
    public async Task<IActionResult> Get(string oid) => 
        Ok(await _service.ContrAgentService.GetByOid(oid));

    [HttpPost]
    public async Task<IActionResult> Post(CreateContrAgentDto model)
    {
        ContrAgentDto contrAgentDto = _mapper.Map<ContrAgentDto>(model);
        return Ok(await _service.ContrAgentService.CreateAsync(contrAgentDto));
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateContrAgentDto model)
    {
        ContrAgentDto contrAgentDto = _mapper.Map<ContrAgentDto>(model);
        return Ok(await _service.ContrAgentService.Update(contrAgentDto));
    }
        
    [HttpDelete("{oid}")]
    public async Task<IActionResult> Delete(string oid) =>
        Ok(await _service.ContrAgentService.Delete(oid));
}