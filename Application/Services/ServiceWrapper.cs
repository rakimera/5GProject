using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Validation;
using AutoMapper;

namespace Application.Services;

public class ServiceWrapper : IServiceWrapper
{
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<IProjectService> _projectService;

    public ServiceWrapper(
        IRepositoryWrapper repository,
        IMapper mapper,
        UserValidator userValidator,
        ProjectValidator projectValidator)
    {
        _userService = new Lazy<IUserService>(() => new UserService(repository, mapper, userValidator));
        _projectService = new Lazy<IProjectService>(()=> new ProjectService(repository, mapper, projectValidator));
    }

    public IUserService UserService => _userService.Value;
    public IProjectService ProjectService => _projectService.Value;
}