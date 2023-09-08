using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.TranslatorSpecs;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace Application.Interfaces;

public interface ITranslatorSpecsService : ICrudService<TranslatorSpecsDto>
{
    public Task<BaseResponse<string>> Update(TranslatorSpecsDto model, string author);
    Task<LoadResult> GetLoadResult(string id, DataSourceLoadOptionsBase loadOptions);
    BaseResponse<List<TranslatorSpecsDto>> GetAllByAntennaId(string id);
    BaseResponse<List<TranslatorSpecsDto>> GetAllByProjectId(string id);
}