namespace Application.Interfaces.RepositoryContract.Common;

public interface IRepositoryWrapper
{
    IUserRepository UserRepository { get; }
    IProjectRepository ProjectRepository { get; }
    ITokenRepository TokenRepository { get; }
    Task Save();
}