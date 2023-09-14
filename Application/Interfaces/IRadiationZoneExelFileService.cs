using Application.DataObjects;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IRadiationZoneExelFileService
{
    Task<BaseResponse<string>> CreateAsync(string translatorId, IFormFile verticalFile,
        IFormFile horizontalFile, string creator);
}