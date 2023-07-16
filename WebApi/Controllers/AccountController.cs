using Application.Interfaces;
using Application.Models;
using Application.Models.Users;
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
    
    
    [HttpGet("{oid}")]
    public async Task<IActionResult> Get(string oid) => 
        Ok(await _service.UserService.GetByOid(oid));

    [HttpPost]
    public async Task<IActionResult> Post(CreateUserDto model)
    {
        UserDto userDto = _mapper.Map<UserDto>(model);
        return Ok(await _service.UserService.CreateAsync(userDto));
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateUserDto model) 
    {
        UserDto userDto = _mapper.Map<UserDto>(model);
        return Ok(await _service.UserService.Update(userDto));
    }
        
    [HttpDelete("{oid}")]
    public async Task<IActionResult> Delete(string oid) =>
        Ok(await _service.UserService.Delete(oid));
}