using Application.Interfaces;
using Application.Models.Users;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet, Authorize(Roles = "Admin")]
    public IActionResult Get()
    {
        var baseResponse = _service.UserService.GetAll();
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse.Messages);
    }


    [HttpGet("{oid}")]
    public async Task<IActionResult> Get(string oid)
    {
        var baseResponse = await _service.UserService.GetByOid(oid);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse.Messages);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateUserDto model)
    {
        UserDto userDto = _mapper.Map<UserDto>(model);
        var baseResponse = await _service.UserService.CreateAsync(userDto);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return BadRequest(baseResponse);
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateUserDto model)
    {
        UserDto userDto = _mapper.Map<UserDto>(model);
        var baseResponse = await _service.UserService.Update(userDto);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return BadRequest(baseResponse.Messages);
    }

    [HttpDelete("{oid}")]
    public async Task<IActionResult> Delete(string oid)
    {
        var baseResponse = await _service.UserService.Delete(oid);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse.Messages);
    }
}