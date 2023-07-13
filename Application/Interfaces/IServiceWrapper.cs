namespace Application.Interfaces;

public interface IServiceWrapper
{
    IUserService UserService { get; }
    IProjectService ProjectService { get; }
}