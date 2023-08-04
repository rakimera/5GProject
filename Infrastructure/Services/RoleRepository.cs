using Application.Interfaces.RepositoryContract;
using Domain.Entities;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    public RoleRepository(Project5GDbContext dbContext) : base(dbContext)
    {
    }
}