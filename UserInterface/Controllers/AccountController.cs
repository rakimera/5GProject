using Application.Interfaces;
using Application.Models;
using Application.Validation;
using Domain.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace UserInterface.Controllers;

[ApiController]
[Route("api/users")]
public class AccountController : Controller
{
    private readonly ICrudRepository<UserDTO> _userCrudRepository;
    private readonly IEnergyFlowSummation _energyFlowSummation;
    private readonly UserValidator _userValidator; //вопрос тут ли происходит валидация

    public AccountController(ICrudRepository<UserDTO> userCrudRepository, IEnergyFlowSummation energyFlowSummation, UserValidator userValidator)
    {
        _userCrudRepository = userCrudRepository;
        _energyFlowSummation = energyFlowSummation;
        _userValidator = userValidator;
    }

    [HttpGet]
    public async Task<IActionResult> Get(int id)
    {
        UserDTO? dtoModel = await _userCrudRepository.GetByIdAsync(id);
        return Ok(dtoModel);
    }
}