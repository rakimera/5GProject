using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models;
using Application.Validation;
using AutoMapper;
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
        try
        {
            IQueryable<User> users = _repositoryWrapper.UserRepository.GetAll();
            users = users.AsEnumerable().Where(u => u.IsDelete == false).AsQueryable(); //Для отображения только не удаленных пользователей
            List<UserDto> models = _mapper.Map<List<UserDto>>(users);

            if (models.Count > 0)
            {
                return new BaseResponse<IEnumerable<UserDto>>(
                    Result: models,
                    Success: true,
                    StatusCode: 200,
                    Message:"Пользователи успешно получены");
            } 
            return new BaseResponse<IEnumerable<UserDto>>(
                    Result: models,
                    Success: true,
                    StatusCode: 200,
                    Message: "Данные не были получены, возможно пользователи еще не созданы");
        }
        catch (Exception e)
        {
            return new BaseResponse<IEnumerable<UserDto>>(
                Result: null, 
                Message: e.Message,
                Success: false,
                StatusCode: 500);
        }
    }

    public async Task<BaseResponse<Guid?>> CreateAsync(UserDto model)
    {
        try
        {
            var result = await _userValidator.ValidateAsync(model);
            if (result.IsValid)
            {
                model.Oid = Guid.NewGuid();
                model.Created = DateTime.Now;
                model.CreatedBy = "Admin"; // реализация зависит от методики работы авторизацией и регистрацией.
                User user = _mapper.Map<User>(model);
                await _repositoryWrapper.UserRepository.CreateAsync(user);
                await _repositoryWrapper.Save();

                return new BaseResponse<Guid?>(
                    Result: user.Oid,
                    Success: true,
                    StatusCode: 200,
                    Message:"Пользователь успешно создан");
            }

            throw new InvalidDataException(string.Join('\n', result.Errors));
        }
        catch (Exception e)
        {
            if (e is InvalidDataException ex)
            {
                return new BaseResponse<Guid?>(Result: null, 
                    Message: ex.Message,
                    Success: false,
                    StatusCode: 400);
            }
            return new BaseResponse<Guid?>(Result: null,
                Message: e.Message,
                Success: false,
                StatusCode: 500);
        }
    }

    public async Task<BaseResponse<UserDto>> GetByOid(Guid oid)
    {
        try
        {
            User? user = await _repositoryWrapper.UserRepository.GetByCondition(x => x.Oid == oid);
            UserDto model = _mapper.Map<UserDto>(user);

            if (user is null)
                return new BaseResponse<UserDto>(
                    Result: null,
                    Message: "Пользователь не найден",
                    Success: true,
                    StatusCode: 404);
            return new BaseResponse<UserDto>(
                Result: model,
                Success: true,
                StatusCode: 200,
                Message:"Пользователь успешно найден");

        }
        catch (Exception e)
        {
            return new BaseResponse<UserDto>(
                Result: null,
                Success: false,
                Message: e.Message,
                StatusCode: 500);
        }
    }
    
    public BaseResponse<UserDto> GetAuthorizedUser(string login,string password)
    {
        try
        {
            User? user = _repositoryWrapper.UserRepository.GetByCondition(x => x.Login == login && x.Password == password).Result;
            UserDto model = _mapper.Map<UserDto>(user);

            if (user is null)
                return new BaseResponse<UserDto>(
                    Result: null,
                    Message: "Пользователь не найден",
                    Success: true,
                    StatusCode: 404);
            return new BaseResponse<UserDto>(
                Result: model,
                Success: true,
                StatusCode: 200,
                Message:"Пользователь успешно найден");

        }
        catch (Exception e)
        {
            return new BaseResponse<UserDto>(
                Result: null,
                Success: false,
                Message: e.Message,
                StatusCode: 500);
        }
    }

    public async Task<BaseResponse<Guid?>> Update(UserDto model)
    {
        try
        {
            var result = await _userValidator.ValidateAsync(model);
            if (result.IsValid)
            {
                model.LastModified = DateTime.Now;
                model.LastModifiedBy = "Admin"; // реализация зависит от методики работы авторизацией и регистрацией.
                User user = _mapper.Map<User>(model);
                
                _repositoryWrapper.UserRepository.Update(user);
                await _repositoryWrapper.Save();

                return new BaseResponse<Guid?>(
                    Result: user.Oid,
                    Success: true,
                    StatusCode: 200,
                    Message:"Пользователь успешно изменен");
            }

            throw new InvalidDataException(string.Join('\n', result.Errors));
        }
        catch (Exception e)
        {
            if (e is InvalidDataException ex)
            {
                return new BaseResponse<Guid?>(Result: null, 
                    Message: ex.Message,
                    Success: false,
                    StatusCode: 400);
            }
            return new BaseResponse<Guid?>(Result: null,
                Message: e.Message,
                Success: false,
                StatusCode: 500);
        }
    }

    public async Task<BaseResponse<bool>> Delete(Guid oid)
    {
        try
        {
            User? user = await _repositoryWrapper.UserRepository.GetByCondition(x => x.Oid == oid);
            if (user is not null)
            {
                user.IsDelete = true;
                _repositoryWrapper.UserRepository.Update(user);
                await _repositoryWrapper.Save();

                return new BaseResponse<bool>(
                    Result: true,
                    Success: true,
                    StatusCode: 200,
                    Message: "Пользователь успешно удален");
            }
            throw new InvalidDataException("Пользователя не существует");
        }
        catch (Exception e)
        {
            if (e is InvalidDataException ex)
            {
                return new BaseResponse<bool>(
                    Result: false, 
                    Message: ex.Message,
                    Success: false,
                    StatusCode: 400);
            }
            return new BaseResponse<bool>(
                Result: false,
                Message: e.Message,
                Success: false,
                StatusCode: 500);
        }
    }
}