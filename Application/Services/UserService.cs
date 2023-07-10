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
    public BaseResponse<IEnumerable<UserDTO>> GetAll()
    {
        try
        {
            IQueryable<User> users = _repositoryWrapper.UserRepository.GetAll();
            List<UserDTO> models = _mapper.Map<List<UserDTO>>(users);

            if (models.Count > 0)
            {
                return new BaseResponse<IEnumerable<UserDTO>>(
                    Result: models,
                    Success: true,
                    StatusCode: 200);
            } 
            return new BaseResponse<IEnumerable<UserDTO>>(
                    Result: models,
                    Success: true,
                    StatusCode: 200,
                    Message: "Данные не были получены, возможно пользователи еще не созданы");
        }
        catch (Exception e)
        {
            return new BaseResponse<IEnumerable<UserDTO>>(
                Result: null, 
                Message: e.Message,
                Success: false,
                StatusCode: 500);
        }
    }

    public async Task<BaseResponse<Guid?>> CreateAsync(UserDTO model)
    {
        try
        {
            var result = await _userValidator.ValidateAsync(model);
            if (result.IsValid)
            {
                model.Created = DateTime.Now;
                model.Oid = new Guid();
                model.CreatedBy = "Admin"; // реализация зависит от методики работы авторизацией и регистрацией.
                User user = _mapper.Map<User>(model);
                
                await _repositoryWrapper.UserRepository.CreateAsync(user);

                return new BaseResponse<Guid?>(
                    Result: user.Oid,
                    Success: true,
                    StatusCode: 200);
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

    public async Task<BaseResponse<UserDTO>> GetByOid(Guid oid)
    {
        try
        {
            User? user = await _repositoryWrapper.UserRepository.GetByCondition(x => x.Oid == oid);
            UserDTO model = _mapper.Map<UserDTO>(user);

            if (user is null)
                return new BaseResponse<UserDTO>(
                    Result: null,
                    Message: "Пользователь не найден",
                    Success: true,
                    StatusCode: 404);
            return new BaseResponse<UserDTO>(
                Result: model,
                Success: true,
                StatusCode: 200);

        }
        catch (Exception e)
        {
            return new BaseResponse<UserDTO>(
                Result: null,
                Success: false,
                Message: e.Message,
                StatusCode: 500);
        }
    }

    public async Task<BaseResponse<Guid?>> Update(UserDTO model)
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

                return new BaseResponse<Guid?>(
                    Result: user.Oid,
                    Success: true,
                    StatusCode: 200);
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
            BaseResponse<UserDTO> response = await GetByOid(oid);
            if (response.Success)
            {
                User user = _mapper.Map<User>(response.Result);
                user.IsDelete = true;
                _repositoryWrapper.UserRepository.Update(user);

                return new BaseResponse<bool>(
                    Result: true,
                    Success: true,
                    StatusCode: 200);
            }
            throw new InvalidDataException(string.Join('\n', response.Message));
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