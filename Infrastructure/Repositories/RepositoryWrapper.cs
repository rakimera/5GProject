using Application.Interfaces.RepositoryContract;
using Application.Interfaces.RepositoryContract.Common;
using Infrastructure.Persistence.DataContext;
using Infrastructure.Services;

namespace Infrastructure.Repositories;

public class RepositoryWrapper : IRepositoryWrapper
{
    private Project5GDbContext _db;
    private IUserRepository _userRepository;
    private IProjectRepository _projectRepository;
    private ITokenRepository _tokenRepository;
    private IContrAgentRepository _contrAgentRepository;

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
    
    public IContrAgentRepository ContrAgentRepository
    {
        get
        {
            if (_contrAgentRepository == null)
            {
                _contrAgentRepository = new ContrAgentRepository(_db);
            }

            return _contrAgentRepository;
        }
    }

    public IProjectRepository ProjectRepository
    {
        get
        {
            if (_projectRepository == null)
            {
                _projectRepository = new ProjectRepository(_db);
            }

            return _projectRepository;
        }
    }
    
    public ITokenRepository TokenRepository
    {
        get
        {
            if (_tokenRepository == null)
            {
                _tokenRepository = new TokenRepository(_db);
            }

            return _tokenRepository;
        }
    }


    public async Task Save()
    {
        await _db.SaveChangesAsync();
    }
}