using Application.Interfaces.RepositoryContract.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces.RepositoryContract;

public interface IProjectImageRepository : IBaseRepository<ProjectImage>
{
    void Delete(ProjectImage projectImage);
    Task<string> SaveImage(string projectId, IFormFile uploadFormFile);
}