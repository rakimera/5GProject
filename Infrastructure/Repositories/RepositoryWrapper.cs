using Application.Interfaces.RepositoryContract;
using Application.Interfaces.RepositoryContract.Common;
using Domain.Entities;
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
    private IRoleRepository _roleRepository;
    private IUserRoleRepository _userRoleRepository;
    private IDistrictRepository _districtRepository;
    private ITownRepository _townRepository;
    private IAntennaRepository _antennaRepository;
    private IEnergyFlowRepository _energyFlowRepository;
    private IRadiationZoneRepository _radiationZoneRepository;
    private IAntennaTranslatorRepository _antennaTranslatorRepository;
    private ITranslatorSpecsRepository _translatorSpecsRepository;

    public RepositoryWrapper(Project5GDbContext db)
    {
        _db = db;
    }
    
    public IAntennaTranslatorRepository AntennaTranslatorRepository
    {
        get
        {
            if (_antennaTranslatorRepository == null)
            {
                _antennaTranslatorRepository = new AntennaTranslatorRepository(_db);
            }

            return _antennaTranslatorRepository;
        }
    }
    
    public IRadiationZoneRepository RadiationZoneRepository
    {
        get
        {
            if (_radiationZoneRepository == null)
            {
                _radiationZoneRepository = new RadiationZoneRepository(_db);
            }

            return _radiationZoneRepository;
        }
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

    public IRoleRepository RoleRepository
    {
        get
        {
            if (_roleRepository == null)
            {
                _roleRepository = new RoleRepository(_db);
            }

            return _roleRepository;
        }
    }

    public IDistrictRepository DistrictRepository
    {
        get
        {
            if (_districtRepository == null)
            {
                _districtRepository = new DistrictRepository(_db);
            }

            return _districtRepository;
        }
    }

    public ITownRepository TownRepository
    {
        get
        {
            if (_townRepository == null)
            {
                _townRepository = new TownRepository(_db);
            }

            return _townRepository;
        }
    }

    public IAntennaRepository AntennaRepository
    {
        get
        {
            if (_antennaRepository == null)
            {
                _antennaRepository = new AntennaRepository(_db);
            }

            return _antennaRepository;
        }
    }
    
    public ITranslatorSpecsRepository TranslatorSpecsRepository
    {
        get
        {
            if (_translatorSpecsRepository == null)
            {
                _translatorSpecsRepository = new TranslatorSpecsRepository(_db);
            }

            return _translatorSpecsRepository;
        }
    }

    public IUserRoleRepository UserRoleRepository
    {
        get
        {
            if (_userRoleRepository == null)
            {
                _userRoleRepository = new UserRoleRepository(_db);
            }

            return _userRoleRepository;
        }
    }

    public IEnergyFlowRepository EnergyFlowRepository
    {
        get
        {
            if (_energyFlowRepository == null)
            {
                _energyFlowRepository = new EnergyFlowRepository(_db);
            }

            return _energyFlowRepository;
        }
    }
    
    public async Task Save()
    {
        await _db.SaveChangesAsync();
    }
}