using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.AntennaTranslator;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace Application.Interfaces;

public interface IAntennaTranslatorService : ICrudService<AntennaTranslatorDto> 
{
    Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions);
    Task<BaseResponse<AntennaTranslatorDto>> Update(UpdateAntennaTranslatorDto model);
    BaseResponse<List<AntennaTranslatorDto>> GetAllByProjectId(string id);
}