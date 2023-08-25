using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.RadiationZone;

namespace Application.Interfaces;

public interface IRadiationZoneService : ICrudService<RadiationZoneDto>
{
    Task<BaseResponse<string>> Update(UpdateRadiationZoneDto model);
}