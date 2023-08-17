using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.Roles;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace Application.Interfaces;

public interface IRoleService: ICrudService<RoleDto>
{
    Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions);
    Task<BaseResponse<RoleDto>> UpdateRole(UpdateRoleDto model);
}