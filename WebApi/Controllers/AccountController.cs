using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class AccountController : Controller
{
    private readonly IServiceWrapper _service;
    private readonly IMapper _mapper;

    public AccountController(IServiceWrapper service, IMapper mapper)
    {
        _service = service;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult Get() => 
        Ok(_service.UserService.GetAll());
    
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id) => 
        Ok(await _service.UserService.GetByOid(id));

    [HttpPost]
    public async Task<IActionResult> Post(CreateUserDto model)
    {
        UserDTO userDto = _mapper.Map<UserDTO>(model);
        return Ok(await _service.UserService.CreateAsync(userDto));
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateUserDto model) 
    {
        UserDTO userDto = _mapper.Map<UserDTO>(model);
        return Ok(await _service.UserService.Update(userDto));
    }
        
        

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid oid) =>
        Ok(await _service.UserService.Delete(oid));
}