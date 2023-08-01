using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.Users;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace Application.Interfaces;

public interface IUserService : ICrudService<UserDto>
{
    Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions);
    Task<BaseResponse<UserDto>> UpdateUser(UpdateUserDto model);
}
