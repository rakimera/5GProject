using Application.Interfaces.RepositoryContract.Common;
using Domain.Entities;

namespace Application.Interfaces.RepositoryContract;

public interface IRadiationZoneExelFileRepository : IBaseRepository<RadiationZoneExelFile>
{
    void Delete(RadiationZoneExelFile exelFile);
}