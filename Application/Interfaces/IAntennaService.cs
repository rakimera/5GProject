using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.Antennae;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;


namespace Application.Interfaces;

public interface IAntennaService : ICrudService<AntennaDto>
{
    Task<BaseResponse<string>> Update(UpdateAntennaDto model, string modifier);
    Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions);
}