using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.Projects.ProjectAntennas;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace Application.Interfaces;

public interface IProjectAntennaService : ICrudService<ProjectAntennaDto>
{
    Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions, string id);
    Task<BaseResponse<string>> Update(ProjectAntennaDto model, string modifare);
    BaseResponse<List<ProjectAntennaDto>> GetAllByProjectId(string id);
}