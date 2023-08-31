using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.TranslatorSpecs;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace Application.Interfaces;

public interface ITranslatorSpecsService : ICrudService<TranslatorSpecsDto>
{
    Task<BaseResponse<string>> Update(UpdateTranslatorSpecsDto model);
    Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions);
}