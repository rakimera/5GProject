using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.ContrAgents;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace Application.Interfaces;

public interface IContrAgentService : ICrudService<ContrAgentDto>
{
    Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions);
    Task<BaseResponse<ContrAgentDto>> UpdateContrAgent(UpdateContrAgentDto model, string modifier);
}