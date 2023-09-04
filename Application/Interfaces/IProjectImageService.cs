using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.Projects.ProjectImages;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IProjectImageService : ICrudService<ProjectImageDto>
{
    Task<BaseResponse<ProjectImageDto>> SaveFile(ProjectImageDto model, IFormFile uploadedFile);
}