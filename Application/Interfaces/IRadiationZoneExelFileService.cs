using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.RadiationZone.RadiationZoneExelFile;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IRadiationZoneExelFileService : ICrudService<RadiationZoneExelFileDto>
{
    Task<BaseResponse<RadiationZoneExelFileDto>> ConvertExel(RadiationZoneExelFileDto model, IFormFile uploadedFile);
    BaseResponse<List<RadiationZoneExelFileDto>> GetAllById(string id);
}