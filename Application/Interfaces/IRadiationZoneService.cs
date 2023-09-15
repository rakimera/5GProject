using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.RadiationZone;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace Application.Interfaces;

public interface IRadiationZoneService : ICrudService<RadiationZoneDto>
{
    Task<BaseResponse<string>> Update(UpdateRadiationZoneDto model, string modifier);
    Task<LoadResult> GetLoadResultById(string id, DataSourceLoadOptionsBase loadOptions);
}