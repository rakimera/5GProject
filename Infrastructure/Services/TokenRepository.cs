using Application.Interfaces.RepositoryContract;
using Domain.Entities;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class TokenRepository : BaseRepository<RefreshToken>, ITokenRepository
{
    public TokenRepository(Project5GDbContext dbContext) : base(dbContext)
    {
    }

    public void Delete(RefreshToken refreshToken)
    {
        DbContext.RefreshTokens.Remove(refreshToken);
    }
}