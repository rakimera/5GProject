using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Validation;
using AutoMapper;

namespace Application.Services;

public class ServiceWrapper : IServiceWrapper
{
    private readonly Lazy<IUserService> _userService;
    private readonly Lazy<ITokenService> _tokenService;

    public ServiceWrapper(
        IRepositoryWrapper repository,
        IMapper mapper,
        UserValidator userValidator)
    {
        _userService = new Lazy<IUserService>(() => new UserService(repository, mapper, userValidator));
        _tokenService = new Lazy<ITokenService>(() => new TokenService());
    }

    public IUserService UserService => _userService.Value;
    public ITokenService TokenService => _tokenService.Value;
}