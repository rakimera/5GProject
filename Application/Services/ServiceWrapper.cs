using Application.Interfaces;
using Application.Interfaces.RepositoryContract;
using Application.Interfaces.RepositoryContract.Common;
using Application.Validation;
using AutoMapper;

namespace Application.Services;

public class ServiceWrapper : IServiceWrapper
{
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<ITokenService> _tokenService;
    private readonly Lazy<IAuthorizationService> _authorizationService;
    private readonly Lazy<IProjectService> _projectService;
    private readonly Lazy<IContrAgentService> _contrAgentService;
    private readonly Lazy<IDistrictService> _districtService;
    private readonly Lazy<ITownService> _townService;
    private readonly Lazy<IAntennaService> _antennaService;

    public ServiceWrapper(
        IRepositoryWrapper repository,
        IMapper mapper,
        UserValidator userValidator,
        ContrAgentValidator contrAgentValidator,
        ProjectValidator projectValidator,
        ITokenService tokenService)
    {
        _districtService = new Lazy<IDistrictService>(() => new DistrictService(repository,mapper));
        _userService = new Lazy<IUserService>(() => new UserService(repository, mapper, userValidator));
        _tokenService = new Lazy<ITokenService>(() => new TokenService(repository));
        _projectService = new Lazy<IProjectService>(()=> new ProjectService(repository, mapper, projectValidator));
        _authorizationService = new Lazy<IAuthorizationService>(()=> new AuthorizationService(repository,tokenService));
        _contrAgentService = new Lazy<IContrAgentService>(() => new ContrAgentService(repository, mapper, contrAgentValidator));
        _townService = new Lazy<ITownService>(() => new TownService(repository,mapper));
        _antennaService = new Lazy<IAntennaService>(() => new AntennaService(repository, mapper));
    }

    public IUserService UserService => _userService.Value;
    public IProjectService ProjectService => _projectService.Value;
    public ITokenService TokenService => _tokenService.Value;
    public IAuthorizationService AuthorizationService => _authorizationService.Value;
    public IContrAgentService ContrAgentService => _contrAgentService.Value;
    public IDistrictService DistrictService => _districtService.Value;
    public ITownService TownService => _townService.Value;
    public IAntennaService AntennaService => _antennaService.Value;
}