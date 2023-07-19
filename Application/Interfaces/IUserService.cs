using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.Users;

namespace Application.Interfaces;

public interface IUserService : ICrudService<UserDto>
{
    public Task<BaseResponse<UserDto>> GetByLogin(string login);
}
