using Application.Interfaces.RepositoryContract.Common;
using Domain.Entities;

namespace Application.Interfaces.RepositoryContract;

public interface ITokenRepository : IBaseRepository<RefreshToken>
{
    void Delete(RefreshToken refreshToken);
}