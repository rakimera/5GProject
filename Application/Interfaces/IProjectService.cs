using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.Projects;

namespace Application.Interfaces;

public interface IProjectService : ICrudService<ProjectDto>
{
    Task<BaseResponse<string>> Update(UpdateProjectDto model);
}