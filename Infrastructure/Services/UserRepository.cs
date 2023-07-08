using Application.Interfaces.RepositoryContract;
using Domain.Entities;
using Infrastructure.Persistance.DataContext;
using Infrastructure.Repositories;

namespace Infrastructure.Services;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(Project5GDbContext dbContext) : base(dbContext)
    {
    }
}