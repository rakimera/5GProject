using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.Projects;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace Application.Interfaces;

public interface IProjectService : ICrudService<ProjectDto>
{
    Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions);
    Task<BaseResponse<string>> Update(UpdateProjectDto model, string modifier);
}