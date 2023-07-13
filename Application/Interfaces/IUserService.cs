using Application.Interfaces.Common;
using Application.Models;
using Application.Models.Users;

namespace Application.Interfaces;

public interface IUserService : ICrudService<UserDTO>
{
}
