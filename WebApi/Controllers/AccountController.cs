using Application.Interfaces;
using Application.Models.Users;
using AutoMapper;
using DevExtreme.AspNet.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
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
    public IActionResult Get()
    {
        var baseResponse = _service.UserService.GetAll();
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }

    [HttpGet("{oid}")]
    public async Task<IActionResult> Get(string oid)
    {
        var baseResponse = await _service.UserService.GetByOid(oid);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateUserDto model)
    {
        UserDto userDto = _mapper.Map<UserDto>(model);
        var baseResponse = await _service.UserService.CreateAsync(userDto, User.Identity.Name);

        if (baseResponse.Success)
            return Ok(baseResponse);
        return BadRequest(baseResponse);
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateUserDto model)
    {
        var baseResponse = await _service.UserService.UpdateUser(model);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return BadRequest(baseResponse);
    }

    [HttpDelete("{oid}")]
    public async Task<IActionResult> Delete(string oid)
    {
        var baseResponse = await _service.UserService.Delete(oid);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }
    
    [HttpGet("index")]
    public async Task<IActionResult> Get([FromQuery] DataSourceLoadOptionsBase loadOptions)
    {
        var loadResult = await _service.UserService.GetLoadResult(loadOptions);
        return Ok(loadResult);
    }

    [HttpGet("current-user")]
    public async Task<IActionResult> GetCurrentUserData()
    {
        var baseResponse = await _service.UserService.GetCurrentUser(User.Identity.Name);
        if (baseResponse.Success)
            return Ok(baseResponse);
        return NotFound(baseResponse);
    }
}