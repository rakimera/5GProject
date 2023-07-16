using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models;

namespace Application.Interfaces;

public interface IUserService : ICrudService<UserDto>
{
    public BaseResponse<UserDto> GetAuthorizedUser(string login, string password);
    public Task<BaseResponse<UserDto>> GetByLogin(string login);
}
