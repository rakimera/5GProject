using System.Security.Cryptography;
using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.Users;
using Application.Validation;
using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Domain.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly UserValidator _userValidator;
    private readonly UpdateUserValidator _updateUserValidator;

    public UserService(IRepositoryWrapper repositoryWrapper, IMapper mapper, UserValidator userValidator, UpdateUserValidator updateUserValidator)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _userValidator = userValidator;
        _updateUserValidator = updateUserValidator;
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

    public async Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions)
    {
        var queryableUsers = _repositoryWrapper.UserRepository.GetAll();
        return await DataSourceLoader.LoadAsync(queryableUsers, loadOptions);
    }

    public string CreatePassword(string password, byte[] salt)
    {
        string passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
        return passwordHash;
    }

    public async Task<BaseResponse<string>> CreateAsync(UserDto model, string creator)
    {
        var result = await _userValidator.ValidateAsync(model);

        if (result.IsValid)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            string passwordHash = CreatePassword(model.Password, salt);

            model.CreatedBy = creator;
            User user = _mapper.Map<User>(model);
            user.Salt = salt;
            user.PasswordHash = passwordHash;

            await _repositoryWrapper.UserRepository.CreateAsync(user);

            await AssignRolesToUser(model, user);
            await _repositoryWrapper.Save();

            return new BaseResponse<string>(
                Result: user.Id.ToString(),
                Success: true,
                Messages: new List<string> { "Пользователь успешно создан" });
        }

        List<string> messages = _mapper.Map<List<string>>(result.Errors);
        return new BaseResponse<string>(
            Result: "",
            Messages: messages,
            Success: false);
    }

    public async Task<BaseResponse<UserDto>> GetByOid(string oid)
    {
        User? user = await _repositoryWrapper.UserRepository.GetByCondition(x => x.Id.ToString() == oid);
        if (user is null)
        {
            return new BaseResponse<UserDto>(
                Result: null,
                Messages: new List<string> { "Пользователь не найден" },
                Success: false);
        }

        UserDto model = _mapper.Map<UserDto>(user);

        model.Roles = new List<string>();
        model.Roles = _repositoryWrapper.UserRoleRepository
            .GetAll()
            .Where(userRole => userRole.UserId == user.Id)
            .Select(userRole => userRole.Role.RoleName)
            .ToList();


        return new BaseResponse<UserDto>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Пользователь успешно найден" });
    }

    public async Task<BaseResponse<UserDto>> UpdateUser(UpdateUserDto model, string lastModifiedBy)
    {
        var result = await _updateUserValidator.ValidateAsync(model);
        if (!result.IsValid)
        {
            return new BaseResponse<UserDto>(
                Result: null,
                Messages: _mapper.Map<List<string>>(result.Errors),
                Success: false);
        }

        User? user = await _repositoryWrapper.UserRepository.GetByCondition(x => x.Id.ToString() == model.Id);
        if (user == null)
        {
            return new BaseResponse<UserDto>(
                Result: null,
                Messages: new List<string> { "Пользователь не найден" },
                Success: false);
        }

        _mapper.Map(model, user);
        
        if (!string.IsNullOrEmpty(model.Password))
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
            user.PasswordHash = CreatePassword(user.PasswordHash, salt);
            user.Salt = salt;
        }

        user.LastModifiedBy = lastModifiedBy;

        _repositoryWrapper.UserRepository.Update(user);
        UserDto userDto = _mapper.Map<UserDto>(model);
        await AssignRolesToUser(userDto, user);
        await _repositoryWrapper.Save();

        return new BaseResponse<UserDto>(
            Result: userDto,
            Success: true,
            Messages: new List<string> { "Пользователь успешно изменен" });
    }

    public BaseResponse<List<Role>> GetRoles()
    {
        var roles = _repositoryWrapper.RoleRepository.GetAll().ToList();
        if (roles.Count > 0)
        {
            return new BaseResponse<List<Role>>(
                Result: roles,
                Success: true,
                Messages: new List<string> { "Роли успешно получены" });
        }

        return new BaseResponse<List<Role>>(
            Result: roles,
            Success: true,
            Messages: new List<string>
                { "Данные не были получены, возможно роли еще не созданы или удалены" });
    }

    public async Task<BaseResponse<bool>> Delete(string oid)
    {
        User? user = await _repositoryWrapper.UserRepository.GetByCondition(x => x.Id.ToString() == oid);
        if (user is not null)
        {
            user.IsDelete = true;
            _repositoryWrapper.UserRepository.Update(user);
            await _repositoryWrapper.Save();

            return new BaseResponse<bool>(
                Result: true,
                Success: true,
                Messages: new List<string> { "Пользователь успешно удален" });
        }

        return new BaseResponse<bool>(
            Result: false,
            Messages: new List<string> { "Пользователя не существует" },
            Success: false);
    }

    private async Task AssignRolesToUser(UserDto model, User user)
    {
        var userRoles = _repositoryWrapper.UserRoleRepository.GetAll();
        foreach (var userRole in userRoles)
        {
            if (userRole.UserId == user.Id)
            {
                _repositoryWrapper.UserRoleRepository.Delete(userRole);
            }
        }

        foreach (var role in model.Roles
                     .Select(roleName => GetRoles()
                         .Result!
                         .FirstOrDefault(r => r.RoleName == roleName))
                     .Where(role => role is not null))
        {
            await _repositoryWrapper.UserRoleRepository.CreateAsync(new UserRole
            {
                Role = role,
                RoleId = role.Id,
                User = user,
                UserId = user.Id
            });
        }
    }

    public async Task<BaseResponse<UserDto>> GetCurrentUser(string? login)
    {
        if (login == null)
        {
            return new BaseResponse<UserDto>(
                Result: null,
                Success: false,
                Messages: new List<string> { "Пользователь не найден" });
        }

        var user = await _repositoryWrapper.UserRepository.GetByCondition(x => Equals(x.Login, login));
        if (user == null)
        {
            return new BaseResponse<UserDto>(
                Result: null,
                Success: false,
                Messages: new List<string> { "Пользователь не найден" });
        }
        
        var userDto = _mapper.Map<UserDto>(user);
        
        userDto.Roles = new List<string>();
        userDto.Roles = _repositoryWrapper.UserRoleRepository
            .GetAll()
            .Where(userRole => userRole.UserId == user.Id)
            .Select(userRole => userRole.Role.RoleName)
            .ToList();

        var executiveCompany = _repositoryWrapper.ExecutiveCompanyRepository
            .GetByCondition(x => x.Id == user.ExecutiveCompanyId).Result;
        if (executiveCompany != null)
            userDto.ExecutiveCompanyName = executiveCompany.CompanyName;

        if (user == null)
            return new BaseResponse<UserDto>(
                Result: null,
                Success: true,
                Messages: new List<string> { "Пользователь не найден" });

        return new BaseResponse<UserDto>(
            Result: userDto,
            Success: true,
            Messages: new List<string> { "Пользователь успешно найден" });
    }
}