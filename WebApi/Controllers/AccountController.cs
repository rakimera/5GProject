using Application.DataObjects;
using Application.DataObjects;
using Application.Interfaces;
using Application.Models.Users;
using AutoMapper;
using DevExtreme.AspNet.Data;
using Domain.Entities;
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
    public async Task<IActionResult> Get([FromQuery]DataSourceLoadOptionsBase loadOptions)
    {
        try
        {
            BaseResponse<IQueryable<User>?> usersResponse = _service.UserService.GetAllQueryable();

            if (!usersResponse.Success)
            {
                return StatusCode(usersResponse.StatusCode, new { Message = usersResponse.Messages });
            }
    
            IQueryable<User>? users = usersResponse.Result;
            var result = await DataSourceLoader.LoadAsync(users, loadOptions);
            return Ok(result);
        }
        catch (Exception e)
        {
            return StatusCode(500, new { Message = e.Message });
        }
    }
}