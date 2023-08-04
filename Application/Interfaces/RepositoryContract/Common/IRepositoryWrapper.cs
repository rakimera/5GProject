namespace Application.Interfaces.RepositoryContract.Common;

public interface IRepositoryWrapper
{
    IUserRepository UserRepository { get; }
    IProjectRepository ProjectRepository { get; }
    ITokenRepository TokenRepository { get; }
    IContrAgentRepository ContrAgentRepository { get; }
    IRoleRepository RoleRepository { get; }
    IDistrictRepository DistrictRepository { get; }
    ITownRepository TownRepository { get; }
    Task Save();
}