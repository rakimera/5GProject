using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.AntennaTranslator;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace Application.Interfaces;

public interface ITranslatorTypeService : ICrudService<TranslatorTypeDto>
{
    Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions);
    Task<BaseResponse<string>> Update(TranslatorTypeDto model, string author);
}