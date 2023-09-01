using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.AntennaTranslator;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace Application.Interfaces;

public interface IAntennaTranslatorService : ICrudService<AntennaTranslatorDto> 
{
    Task<LoadResult> GetLoadResult(string id, DataSourceLoadOptionsBase loadOptions);
    Task<BaseResponse<string>> Update(AntennaTranslatorDto model, string author);
    BaseResponse<List<AntennaTranslatorDto>> GetAllByProjectAntennaId(string id);
}