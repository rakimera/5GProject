using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models;

namespace Application.Interfaces;

public interface IUserService : ICrudService<UserDTO>
{
    public BaseResponse<UserDTO> GetAuthorizedUser(string login, string password);
}
