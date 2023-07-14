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
            // users = users.AsEnumerable().Where(u => u.IsDelete == false).AsQueryable(); //Для отображения только не удаленных пользователей
            List<UserDTO> models = _mapper.Map<List<UserDTO>>(users);

            if (models.Count > 0)
            {
                return new BaseResponse<IEnumerable<UserDTO>>(
                    Result: models,
                    Success: true,
                    StatusCode: 200,
                    Messages: new List<string>{"Пользователи успешно получены"});
            } 
            return new BaseResponse<IEnumerable<UserDTO>>(
                    Result: models,
                    Success: true,
                    StatusCode: 200,
                    Messages: new List<string>{"Данные не были получены, возможно пользователи еще не созданы или удалены"});
        }
        catch (Exception e)
        {
            return new BaseResponse<IEnumerable<UserDTO>>(
                Result: null, 
                Messages: new List<string>{e.Message},
                Success: false,
                StatusCode: 500);
        }
    }

    public async Task<BaseResponse<string>> CreateAsync(UserDTO model)
    {
        try
        {
            var result = await _userValidator.ValidateAsync(model);
            if (result.IsValid)
            {
                model.Oid = Guid.NewGuid().ToString();
                model.Created = DateTime.Now;
                model.CreatedBy = "Admin"; // реализация зависит от методики работы авторизацией и регистрацией.
                User user = _mapper.Map<User>(model);
                await _repositoryWrapper.UserRepository.CreateAsync(user);
                await _repositoryWrapper.Save();

                return new BaseResponse<string>(
                    Result: user.Oid,
                    Success: true,
                    StatusCode: 200,
                    Messages: new List<string>{"Пользователь успешно создан"});
            }
            List<string> messages = _mapper.Map<List<string>>(result.Errors);
            
            return new BaseResponse<string>(
                Result: "", 
                Messages: messages,
                Success: false,
                StatusCode: 400);
        }
        catch (Exception e)
        {
            return new BaseResponse<string>(
                Result: "",
                Messages: new List<string>{e.Message},
                Success: false,
                StatusCode: 500);
        }
    }

    public async Task<BaseResponse<UserDTO>> GetByOid(string oid)
    {
        try
        {
            User? user = await _repositoryWrapper.UserRepository.GetByCondition(x => x.Oid == oid);
            UserDTO model = _mapper.Map<UserDTO>(user);

            if (user is null)
                return new BaseResponse<UserDTO>(
                    Result: null,
                    Messages: new List<string>{"Пользователь не найден"},
                    Success: true,
                    StatusCode: 404);
            return new BaseResponse<UserDTO>(
                Result: model,
                Success: true,
                StatusCode: 200,
                Messages: new List<string>{"Пользователь успешно найден"});

        }
        catch (Exception e)
        {
            return new BaseResponse<UserDTO>(
                Result: null,
                Success: false,
                Messages: new List<string>{e.Message},
                StatusCode: 500);
        }
    }

    public async Task<BaseResponse<string>> Update(UserDTO model)
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

                return new BaseResponse<string>(
                    Result: user.Oid,
                    Success: true,
                    StatusCode: 200,
                    Messages: new List<string>{"Пользователь успешно изменен"});
            }
            List<string> messages = _mapper.Map<List<string>>(result.Errors);

            return new BaseResponse<string>(
                Result: "", 
                Messages: messages,
                Success: false,
                StatusCode: 400);
        }
        catch (Exception e)
        {
            return new BaseResponse<string>(
                Result: "",
                Messages: new List<string>{e.Message},
                Success: false,
                StatusCode: 500);
        }
    }

    public async Task<BaseResponse<bool>> Delete(string oid)
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
                    Messages: new List<string>{"Пользователь успешно удален"});
            }
            
            return new BaseResponse<bool>(
                Result: false, 
                Messages: new List<string>{"Пользователя не существует"},
                Success: false,
                StatusCode: 400);
        }
        catch (Exception e)
        {
            return new BaseResponse<bool>(
                Result: false,
                Messages: new List<string>{e.Message},
                Success: false,
                StatusCode: 500);
        }
    }
}