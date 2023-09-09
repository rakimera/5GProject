using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.Users;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Domain.Entities;

namespace Application.Interfaces;

public interface IUserService : ICrudService<UserDto>
{
    Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions);
    Task<BaseResponse<UserDto>> UpdateUser(UpdateUserDto model, string lastModifiedBy);
    string CreatePassword(string password, byte[] salt);
    BaseResponse<List<Role>> GetRoles();
    Task<BaseResponse<UserDto>> GetCurrentUser(string? login);
}