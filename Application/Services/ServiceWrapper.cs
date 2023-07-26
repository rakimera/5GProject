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

    public ServiceWrapper(
        IRepositoryWrapper repository,
        IMapper mapper,
        UserValidator userValidator,
        ProjectValidator projectValidator,
        ITokenService tokenService,
        ILoggerService logger)
    {
        _userService = new Lazy<IUserService>(() => new UserService(repository, mapper, userValidator, logger));
        _tokenService = new Lazy<ITokenService>(() => new TokenService(repository));
        _projectService = new Lazy<IProjectService>(()=> new ProjectService(repository, mapper, projectValidator));
        _authorizationService = new Lazy<IAuthorizationService>(()=> new AuthorizationService(repository,tokenService));
    }

    public IUserService UserService => _userService.Value;
    public IProjectService ProjectService => _projectService.Value;
    public ITokenService TokenService => _tokenService.Value;
    public IAuthorizationService AuthorizationService => _authorizationService.Value;
}