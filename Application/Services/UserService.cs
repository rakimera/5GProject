using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.Users;
using Application.Validation;
using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Domain.Entities;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly UserValidator _userValidator;

    public UserService(IRepositoryWrapper repositoryWrapper, IMapper mapper, UserValidator userValidator)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _userValidator = userValidator;
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

    public async Task<BaseResponse<string>> CreateAsync(UserDto model, string creator)
    {
        var mapUser = _mapper.Map<User>(model);
        var result = await _userValidator.ValidateAsync(mapUser);
        if (result.IsValid)
        {
            model.Created = DateTime.Now;
            model.CreatedBy = creator;
            User user = _mapper.Map<User>(model);
            await _repositoryWrapper.UserRepository.CreateAsync(user);
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
        UserDto model = _mapper.Map<UserDto>(user);

        if (user is null)
        {
            return new BaseResponse<UserDto>(
                Result: null,
                Messages: new List<string> { "Пользователь не найден" },
                Success: true);
        }

        return new BaseResponse<UserDto>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Пользователь успешно найден" });
    }

    public async Task<BaseResponse<UserDto>> UpdateUser(UpdateUserDto model)
    {
        BaseResponse<UserDto> getUserResponse = await GetByOid(model.Id);
        if (!getUserResponse.Success || getUserResponse.Result == null)
        {
            return new BaseResponse<UserDto>(
                Result: null,
                Messages: new List<string> { "Пользователь не найден" },
                Success: false);
        }

        UserDto existingUserDto = getUserResponse.Result;
        _mapper.Map(model, existingUserDto);

        User? user = await _repositoryWrapper.UserRepository.GetByCondition(x => x.Id.Equals(existingUserDto.Id));
        if (user == null)
        {
            return new BaseResponse<UserDto>(
                Result: null,
                Messages: new List<string> { "Пользователь не найден" },
                Success: false);
        }

        _mapper.Map(existingUserDto, user);
        user.LastModified = DateTime.Now;
        user.LastModifiedBy = "Admin";

        var result = await _userValidator.ValidateAsync(user);
        if (!result.IsValid)
        {
            return new BaseResponse<UserDto>(
                Result: null,
                Messages: _mapper.Map<List<string>>(result.Errors),
                Success: false);
        }

        _repositoryWrapper.UserRepository.Update(user);
        await _repositoryWrapper.Save();

        return new BaseResponse<UserDto>(
            Result: existingUserDto,
            Success: true,
            Messages: new List<string> { "Пользователь успешно изменен" });
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
}