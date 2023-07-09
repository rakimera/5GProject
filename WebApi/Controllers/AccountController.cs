using Application.Interfaces;
using Application.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/users")]
public class AccountController : Controller
{
    private readonly IServiceWrapper _service;

    public AccountController(IServiceWrapper service)
    {
        _service = service;
    }

    [HttpGet]
    public IActionResult Get() => 
        Ok(_service.UserService.GetAll());
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid oid) => 
        Ok(await _service.UserService.GetByOid(oid));

    [HttpPost]
    public async Task<IActionResult> Post(UserDTO model) =>
        Ok(await _service.UserService.CreateAsync(model));

    [HttpPut]
    public async Task<IActionResult> Put(UserDTO model) =>
        Ok(await _service.UserService.Update(model));

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid oid) =>
        Ok(await _service.UserService.Delete(oid));
}