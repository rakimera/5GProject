using Application.Interfaces;
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

    public ServiceWrapper(
        IRepositoryWrapper repository,
        IMapper mapper,
        UserValidator userValidator,
        ContrAgentValidator contrAgentValidator,
        ProjectValidator projectValidator,
        ITokenService tokenService)
    {
        _userService = new Lazy<IUserService>(() => new UserService(repository, mapper, userValidator));
        _tokenService = new Lazy<ITokenService>(() => new TokenService(repository));
        _projectService = new Lazy<IProjectService>(()=> new ProjectService(repository, mapper, projectValidator));
        _authorizationService = new Lazy<IAuthorizationService>(()=> new AuthorizationService(repository,tokenService));
        _contrAgentService = new Lazy<IContrAgentService>(() => new ContrAgentService(repository, mapper, contrAgentValidator));
    }

    public IUserService UserService => _userService.Value;
    public IProjectService ProjectService => _projectService.Value;
    public ITokenService TokenService => _tokenService.Value;
    public IAuthorizationService AuthorizationService => _authorizationService.Value;
    public IContrAgentService ContrAgentService => _contrAgentService.Value;
}