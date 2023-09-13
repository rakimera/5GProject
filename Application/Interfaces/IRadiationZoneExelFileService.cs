using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.RadiationZone.RadiationZoneExelFile;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Interfaces;

public interface IRadiationZoneExelFileService : ICrudService<RadiationZoneExelFileDto>
{
    Task<BaseResponse<RadiationZoneExelFileDto>> ConvertExel(RadiationZoneExelFileDto model, IFormFile uploadedFile);
    BaseResponse<List<RadiationZoneExelFileDto>> GetAllById(string id);

    Task<BaseResponse<string>> CreateAsync(
        RadiationZoneExelFileDto model,
        [FromForm] IFormFile vertical,
        [FromForm] IFormFile horizontal,
        string creator);
}