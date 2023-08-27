namespace Application.Interfaces.RepositoryContract.Common;

public interface IRepositoryWrapper
{
    IUserRepository UserRepository { get; }
    IProjectRepository ProjectRepository { get; }
    ITokenRepository TokenRepository { get; }
    IContrAgentRepository ContrAgentRepository { get; }
    IRoleRepository RoleRepository { get; }
    IUserRoleRepository UserRoleRepository { get; }
    IDistrictRepository DistrictRepository { get; }
    ITownRepository TownRepository { get; }
    IAntennaRepository AntennaRepository { get; }
    ITranslatorSpecsRepository TranslatorSpecsRepository { get; }
    IEnergyFlowRepository EnergyFlowRepository { get; }
    IRadiationZoneRepository RadiationZoneRepository { get; }
    IAntennaTranslatorRepository AntennaTranslatorRepository { get; }
    ICompanyLicenseRepository CompanyLicenseRepository { get; }
    IExecutiveCompanyRepository ExecutiveCompanyRepository { get; }
    Task Save();
}