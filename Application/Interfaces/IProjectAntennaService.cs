using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.Antennae;
using Application.Models.Projects.ProjectAntennas;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace Application.Interfaces;

public interface IProjectAntennaService : ICrudService<ProjectAntennaDto>
{
    Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions);
    Task<BaseResponse<ProjectAntennaDto>> Update(UpdateProjectAntennaDto model);
    BaseResponse<List<ProjectAntennaDto>> GetAllByProjectId(string id);
}