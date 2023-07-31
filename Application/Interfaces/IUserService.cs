using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.Users;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace Application.Interfaces;

public interface IUserService : ICrudService<UserDto>
{
    Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions);
    public BaseResponse<UserDto> GetAuthorizedUser(string login, string password);
    public Task<BaseResponse<UserDto>> GetByLogin(string login);
    Task<BaseResponse<UserDto>> UpdateUser(UpdateUserDto model);
}
