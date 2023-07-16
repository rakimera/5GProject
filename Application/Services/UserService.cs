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
    public BaseResponse<IEnumerable<UserDto>> GetAll()
    {
        try
        {
            IQueryable<User> users = _repositoryWrapper.UserRepository.GetAll();
            // users = users.AsEnumerable().Where(u => u.IsDelete == false).AsQueryable(); //Для отображения только не удаленных пользователей
            List<UserDto> models = _mapper.Map<List<UserDto>>(users);

            if (models.Count > 0)
            {
                return new BaseResponse<IEnumerable<UserDto>>(
                    Result: models,
                    Success: true,
                    StatusCode: 200,
                    Messages: new List<string>{"Пользователи успешно получены"});
            } 
            return new BaseResponse<IEnumerable<UserDto>>(
                    Result: models,
                    Success: true,
                    StatusCode: 200,
                    Messages: new List<string>{"Данные не были получены, возможно пользователи еще не созданы или удалены"});
        }
        catch (Exception e)
        {
            return new BaseResponse<IEnumerable<UserDto>>(
                Result: null, 
                Messages: new List<string>{e.Message},
                Success: false,
                StatusCode: 500);
        }
    }

    public async Task<BaseResponse<string>> CreateAsync(UserDto model)
    {
        try
        {
            var result = await _userValidator.ValidateAsync(model);
            if (result.IsValid)
            {
                model.Created = DateTime.Now;
                model.CreatedBy = "Admin"; // реализация зависит от методики работы авторизацией и регистрацией.
                User user = _mapper.Map<User>(model);
                await _repositoryWrapper.UserRepository.CreateAsync(user);
                await _repositoryWrapper.Save();

                return new BaseResponse<string>(
                    Result: user.Id.ToString(),
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

    public async Task<BaseResponse<UserDto>> GetByOid(string oid)
    {
        try
        {
            User? user = await _repositoryWrapper.UserRepository.GetByCondition(x => x.Id.ToString() == oid);
            UserDto model = _mapper.Map<UserDto>(user);

            if (user is null)
                return new BaseResponse<UserDto>(
                    Result: null,
                    Messages: new List<string>{"Пользователь не найден"},
                    Success: true,
                    StatusCode: 404);
            return new BaseResponse<UserDto>(
                Result: model,
                Success: true,
                StatusCode: 200,
                Messages: new List<string>{"Пользователь успешно найден"});

        }
        catch (Exception e)
        {
            return new BaseResponse<UserDto>(
                Result: null,
                Success: false,
                Messages: new List<string>{e.Message},
                StatusCode: 500);
        }
    }

    public async Task<BaseResponse<string>> Update(UserDto model)
    {
        try
        {
            var result = await _userValidator.ValidateAsync(model);
            if (result.IsValid)
            {
                User? user = await _repositoryWrapper.UserRepository.GetByCondition(x => x.Id.Equals(model.Id));
                /*User user = _mapper.Map<User>(model);*/

                user.Name = model.Name;
                user.Surname = model.Surname;
                user.Role = model.Role;
                user.LastModified = DateTime.Now;
                user.LastModifiedBy = "Admin";
                
                _repositoryWrapper.UserRepository.Update(user);
                await _repositoryWrapper.Save();

                return new BaseResponse<string>(
                    Result: user.Id.ToString(),
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
            User? user = await _repositoryWrapper.UserRepository.GetByCondition(x => x.Id.ToString() == oid);
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