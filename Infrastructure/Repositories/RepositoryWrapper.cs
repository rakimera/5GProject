using Application.Interfaces.RepositoryContract;
using Application.Interfaces.RepositoryContract.Common;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Services;

namespace Infrastructure.Repositories;

public class RepositoryWrapper : IRepositoryWrapper
{
    private Project5GDbContext _db;
    private IUserRepository _userRepository;

    public RepositoryWrapper(Project5GDbContext db)
    {
        _db = db;
    }

    public IUserRepository UserRepository
    {
        get
        {
            if (_userRepository == null)
            {
                _userRepository = new UserRepository(_db);
            }

            return _userRepository;
        }
    }


    public async Task Save()
    {
        await _db.SaveChangesAsync();
    }
}