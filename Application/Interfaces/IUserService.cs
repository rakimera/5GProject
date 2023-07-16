using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models;

namespace Application.Interfaces;

public interface IUserService : ICrudService<UserDto>
{
    public BaseResponse<UserDTO> GetAuthorizedUser(string login, string password);
    public Task<BaseResponse<UserDTO>> GetByLogin(string login);
}
