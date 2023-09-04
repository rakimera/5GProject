using Application.Interfaces.RepositoryContract.Common;
using Domain.Entities;

namespace Application.Interfaces.RepositoryContract;

public interface IProjectImageRepository : IBaseRepository<ProjectImage>
{
    void Delete(ProjectImage projectImage);
}