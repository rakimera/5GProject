using Application.Interfaces;

namespace Application.Services;

public class ServiceWrapper : IServiceWrapper
{
    public IUserService UserService { get; }
}