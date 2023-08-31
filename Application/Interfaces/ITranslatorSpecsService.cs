using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.TranslatorSpecs;

namespace Application.Interfaces;

public interface ITranslatorSpecsService : ICrudService<TranslatorSpecsDto>
{
    Task<BaseResponse<string>> Update(UpdateTranslatorSpecsDto model);
    BaseResponse<List<TranslatorSpecsDto>> GetAllByProjectId(string id);
}