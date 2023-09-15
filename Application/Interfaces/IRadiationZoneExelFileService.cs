using Application.DataObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Application.Interfaces;

public interface IRadiationZoneExelFileService
{
    Task<BaseResponse<string>> CreateAsync(string translatorId, IFormFile verticalFile,
        IFormFile horizontalFile, string creator);

    Task<BaseResponse<FileContentResult>> GetTemplate();
}