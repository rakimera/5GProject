using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.Users;
using Application.Validation;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly UserValidator _userValidator;
    private readonly ILoggerService _logger;

    public UserService(IRepositoryWrapper repositoryWrapper, IMapper mapper, UserValidator userValidator,
        ILoggerService logger)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _userValidator = userValidator;
        _logger = logger;
    }

    public BaseResponse<IEnumerable<UserDto>> GetAll()
    {
        IQueryable<User> users = _repositoryWrapper.UserRepository.GetAll();
        List<UserDto> models = _mapper.Map<List<UserDto>>(users);

        if (models.Count > 0)
        {
            return new BaseResponse<IEnumerable<UserDto>>(
                Result: models,
                Success: true,
                Messages: new List<string> { "Пользователи успешно получены" });
        }

        return new BaseResponse<IEnumerable<UserDto>>(
            Result: models,
            Success: true,
            Messages: new List<string>
                { "Данные не были получены, возможно пользователи еще не созданы или удалены" });
    }

    public async Task<BaseResponse<string>> CreateAsync(UserDto model)
    {
        var result = await _userValidator.ValidateAsync(model);
        if (result.IsValid)
        {
            model.Created = DateTime.Now;
            model.CreatedBy = "Admin"; // реализация зависит от методики работы авторизацией и регистрацией.
            User user = _mapper.Map<User>(model);
            await _repositoryWrapper.UserRepository.CreateAsync(user);
            await _repositoryWrapper.Save();

            _logger.LogInformation($"Пользователь успешно создан. Id: {user.Id}");
            return new BaseResponse<string>(
                Result: user.Id.ToString(),
                Success: true,
                Messages: new List<string> { "Пользователь успешно создан" });
        }

        List<string> messages = _mapper.Map<List<string>>(result.Errors);
        _logger.LogError("Не удалось создать пользователя");
        return new BaseResponse<string>(
            Result: "",
            Messages: messages,
            Success: false);
    }

    public async Task<BaseResponse<UserDto>> GetByOid(string oid)
    {
        User? user = await _repositoryWrapper.UserRepository.GetByCondition(x => x.Id.ToString() == oid);
        UserDto model = _mapper.Map<UserDto>(user);

        if (user is null)
        {
            _logger.LogError("Пользователь не найден");
            return new BaseResponse<UserDto>(
                Result: null,
                Messages: new List<string> { "Пользователь не найден" },
                Success: true);
        }

        _logger.LogInformation($"Пользователь {model.Name} успешно найден");
        return new BaseResponse<UserDto>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Пользователь успешно найден" });
    }

    public async Task<BaseResponse<UserDto>> GetByLogin(string login)
    {
        User? user = await _repositoryWrapper.UserRepository.GetByCondition(x => x.Login == login);
        UserDto model = _mapper.Map<UserDto>(user);

        if (user is null)
        {
            _logger.LogError($"Пользователь не найден");
            return new BaseResponse<UserDto>(
                Result: null,
                Messages: new List<string> { "Пользователь не найден" },
                Success: true);
        }

        _logger.LogInformation($"Пользователь {model.Name} успешно найден");
        return new BaseResponse<UserDto>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Пользователь успешно найден" });
    }

    public async Task<BaseResponse<string>> Update(UserDto model)
    {
        var result = await _userValidator.ValidateAsync(model);
        if (result.IsValid)
        {
            User? user = await _repositoryWrapper.UserRepository.GetByCondition(x => x.Id.Equals(model.Id));

            user.Name = model.Name;
            user.Surname = model.Surname;
            user.Role = model.Role;
            user.LastModified = DateTime.Now;
            user.LastModifiedBy = "Admin";

            _repositoryWrapper.UserRepository.Update(user);
            await _repositoryWrapper.Save();

            _logger.LogInformation($"Пользователь {model.Name} успешно изменен");
            return new BaseResponse<string>(
                Result: user.Id.ToString(),
                Success: true,
                Messages: new List<string> { "Пользователь успешно изменен" });
        }
        
        _logger.LogError($"Пользователя не удалось обновить");
        return new BaseResponse<string>(
            Result: "",
            Messages: _mapper.Map<List<string>>(result.Errors),
            Success: false);
    }

    public async Task<BaseResponse<bool>> Delete(string oid)
    {
        User? user = await _repositoryWrapper.UserRepository.GetByCondition(x => x.Id.ToString() == oid);
        if (user is not null)
        {
            user.IsDelete = true;
            _repositoryWrapper.UserRepository.Update(user);
            await _repositoryWrapper.Save();

            _logger.LogInformation($"Пользователь {user.Name} успешно удален");
            return new BaseResponse<bool>(
                Result: true,
                Success: true,
                Messages: new List<string> { "Пользователь успешно удален" });
        }

        _logger.LogError($"Пользователя не существует");
        return new BaseResponse<bool>(
            Result: false,
            Messages: new List<string> { "Пользователя не существует" },
            Success: false);
    }
}