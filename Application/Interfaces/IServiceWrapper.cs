namespace Application.Interfaces;

public interface IServiceWrapper
{
    IUserService UserService { get; }
    ITokenService TokenService { get; }
}