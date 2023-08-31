namespace Application.Interfaces;

public interface IServiceWrapper
{
    IUserService UserService { get; }
    ITokenService TokenService { get; }
    IProjectService ProjectService { get; }
    IAuthorizationService AuthorizationService { get; }
    IContrAgentService ContrAgentService { get; }
    IDistrictService DistrictService { get; }
    ITownService TownService { get; }
    IAntennaService AntennaService { get; }
    ITranslatorSpecsService TranslatorSpecsService { get; }
    IProjectAntennaService ProjectAntennaService { get; }
    IEnergyFlowService EnergyFlowService { get; }
    IRoleService RoleService { get; }
    IExecutiveCompanyService ExecutiveCompanyService { get; }
    IAntennaTranslatorService AntennaTranslatorService { get; }
}