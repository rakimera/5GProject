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
    private readonly Lazy<IContrAgentService> _contrAgentService;
    private readonly Lazy<IDistrictService> _districtService;
    private readonly Lazy<ITownService> _townService;
    private readonly Lazy<IAntennaService> _antennaService;
    private readonly Lazy<IEnergyFlowService> _energyFlowService;
    private readonly Lazy<IRoleService> _roleService;
    private readonly Lazy<ICompanyLicenseService> _companyLicenseService;
    private readonly Lazy<IExecutiveCompanyService> _executiveCompanyService;
    private readonly Lazy<ITranslatorSpecsService> _translatorSpecsService;
    private readonly Lazy<IProjectAntennaService> _projectAntennaService;

    public ServiceWrapper(
        IRepositoryWrapper repository,
        IMapper mapper,
        UserValidator userValidator,
        ContrAgentValidator contrAgentValidator,
        ProjectValidator projectValidator,
        UpdateProjectValidator updateProjectValidator,
        ITokenService tokenService,
        TranslatorSpecsValidator translatorSpecsValidator,
        AntennaValidator antennaValidator,
        EnergyResultValidator energyResultValidator,
        RoleValidator roleValidator,
        ProjectAntennaValidator projectAntennaValidator,
        CompanyLicenseValidator licenseValidator,
        ExecutiveCompanyValidator executiveCompanyValidator)
    {
        _projectAntennaService = new Lazy<IProjectAntennaService>(() => new ProjectAntennaService(repository, mapper, projectAntennaValidator));
        _roleService = new Lazy<IRoleService>(() => new RoleService(repository, mapper, roleValidator));
        _companyLicenseService = new Lazy<ICompanyLicenseService>(() => new CompanyLicenseService(repository, mapper, licenseValidator));
        _executiveCompanyService = new Lazy<IExecutiveCompanyService>(() => new ExecutiveCompanyService(repository, mapper, executiveCompanyValidator));
        _districtService = new Lazy<IDistrictService>(() => new DistrictService(repository, mapper));
        _userService = new Lazy<IUserService>(() => new UserService(repository, mapper, userValidator));
        _tokenService = new Lazy<ITokenService>(() => new TokenService(repository));
        _projectService = new Lazy<IProjectService>(()=> new ProjectService(repository, mapper, projectValidator, updateProjectValidator));
        _authorizationService = new Lazy<IAuthorizationService>(()=> new AuthorizationService(repository,tokenService));
        _contrAgentService = new Lazy<IContrAgentService>(() => new ContrAgentService(repository, mapper, contrAgentValidator));
        _townService = new Lazy<ITownService>(() => new TownService(repository,mapper));
        _antennaService = new Lazy<IAntennaService>(() => new AntennaService(repository, mapper, antennaValidator));
        _translatorSpecsService = new Lazy<ITranslatorSpecsService>(() => new TranslatorSpecsService(repository, mapper, translatorSpecsValidator));
        _energyFlowService = new Lazy<IEnergyFlowService>(() => new EnergyFlowService(energyResultValidator, mapper, repository));
        _energyFlowService = new Lazy<IEnergyFlowService>(() => new EnergyFlowService(energyResultValidator, mapper, repository));
    }
        

    public IUserService UserService => _userService.Value;
    public IProjectAntennaService ProjectAntennaService => _projectAntennaService.Value;
    public IProjectService ProjectService => _projectService.Value;
    public ITokenService TokenService => _tokenService.Value;
    public IAuthorizationService AuthorizationService => _authorizationService.Value;
    public IContrAgentService ContrAgentService => _contrAgentService.Value;
    public IDistrictService DistrictService => _districtService.Value;
    public ITownService TownService => _townService.Value;
    public IAntennaService AntennaService => _antennaService.Value;
    public ITranslatorSpecsService TranslatorSpecsService => _translatorSpecsService.Value;
    public IEnergyFlowService EnergyFlowService => _energyFlowService.Value;
    public IRoleService RoleService => _roleService.Value;
    public ICompanyLicenseService CompanyLicenseService => _companyLicenseService.Value;
    public IExecutiveCompanyService ExecutiveCompanyService => _executiveCompanyService.Value;
}