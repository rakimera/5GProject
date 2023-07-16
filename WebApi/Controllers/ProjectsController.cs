using Application.Interfaces;
using Application.Models.Projects;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/projects")]
public class ProjectsController : Controller
{
    private readonly IServiceWrapper _service;
    private readonly IMapper _mapper;

    public ProjectsController(IServiceWrapper service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Get() =>
        Ok(_service.ProjectService.GetAll());
    
    [HttpGet("{oid}")]
    public async Task<IActionResult> Get(string oid) => 
        Ok(await _service.ProjectService.GetByOid(oid));
    
    [HttpPost]
    public async Task<IActionResult> Post(CreateProjectDto model)
    {
        ProjectDto projectDto = _mapper.Map<ProjectDto>(model);
        return Ok(await _service.ProjectService.CreateAsync(projectDto));
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateProjectDto model) 
    {
        ProjectDto projectDto = _mapper.Map<ProjectDto>(model);
        return Ok(await _service.ProjectService.Update(projectDto));
    }

    [HttpDelete("{oid}")]
    public async Task<IActionResult> Delete(string oid) =>
        Ok(await _service.ProjectService.Delete(oid));
}