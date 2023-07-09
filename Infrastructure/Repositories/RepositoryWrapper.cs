using Application.Interfaces.RepositoryContract;
using Application.Interfaces.RepositoryContract.Common;
using Infrastructure.Persistence.DataContext;

namespace Infrastructure.Repositories;

public class RepositoryWrapper : IRepositoryWrapper
{
    private readonly Project5GDbContext _db;
    public RepositoryWrapper(IUserRepository userRepository, Project5GDbContext db)
    {
        UserRepository = userRepository;
        _db = db;
    }

    public IUserRepository UserRepository { get; }
    public async Task Save()
    {
        await _db.SaveChangesAsync();
    }
}