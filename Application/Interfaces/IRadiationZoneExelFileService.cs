using Application.DataObjects;
using Application.Models.RadiationZone.RadiationZoneExelFile;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IRadiationZoneExelFileService 
{
    Task<BaseResponse<RadiationZoneExelFileDto>> ConvertExel(RadiationZoneExelFileDto model, IFormFile uploadedFile);
    BaseResponse<List<RadiationZoneExelFileDto>> GetAllById(string id);
    BaseResponse<IEnumerable<RadiationZoneExelFileDto>> GetAll();
    Task<BaseResponse<RadiationZoneExelFileDto>> GetByOid(string oid);
    Task<BaseResponse<bool>> Delete(string oid);
    Task<BaseResponse<string>> CreateAsync( string id, IFormFile vertical, IFormFile horizontal, string creator);
}