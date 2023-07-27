using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.Users;
using Domain.Entities;

namespace Application.Interfaces;

public interface IUserService : ICrudService<UserDto>
{
    BaseResponse<IQueryable<User>?> GetAllQueryable();
    public BaseResponse<UserDto> GetAuthorizedUser(string login, string password);
    public Task<BaseResponse<UserDto>> GetByLogin(string login);
}
