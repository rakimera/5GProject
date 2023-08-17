using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Domain.Entities;

namespace Application.Interfaces;

public interface ITownService : ICrudService<TownDto>
{
    Task<BaseResponse<string>> CreateTownAsync(Town town);
    Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions);
}