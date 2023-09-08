using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models;
using Application.Models.Antennae;
using Application.Models.ContrAgents;
using Application.Models.ExecutiveCompany;
using Application.Models.TranslatorSpecs;
using Application.Models.Users;
using Domain.Entities;
using Domain.Enums;

namespace Infrastructure.Persistence.DataSeeding;

public class DataSeed
{
    private readonly IServiceWrapper _serviceWrapper;
    private readonly IRepositoryWrapper _repositoryWrapper;

    public DataSeed(IServiceWrapper serviceWrapper, IRepositoryWrapper repositoryWrapper)
    {
        _serviceWrapper = serviceWrapper;
        _repositoryWrapper = repositoryWrapper;
    }
    public async Task SeedExecutiveCompany()
    {
        var executiveCompany = _repositoryWrapper.ExecutiveCompanyRepository.GetAll();
        if (executiveCompany is null)
        {
            ExecutiveCompanyDto company = new ExecutiveCompanyDto
            {
                BIN = "123456789111",
                Address = "Алматинская область, Алматы, 1 микр, 43",
                CompanyName = "AlcaponeLTD",
                LicenseNumber = "№123123132ФЫВ123123АВЫ",
                LicenseDateOfIssue = DateTime.Today.AddDays(-10),
                TownName = "Алматы"
            };
            await _serviceWrapper.ExecutiveCompanyService.CreateAsync(company, "Admin");
            await _repositoryWrapper.Save();
        }
        
    }
    
    public async Task SeedAdmin()
    {
        User? user = await _repositoryWrapper.UserRepository.GetByCondition(u => u.Login.Equals("admin@gmail.com"));
        if (user is null)
        {
            var executiveCompany = _serviceWrapper.ExecutiveCompanyService.GetAll().Result.First().Id;
            UserDto admin = new UserDto()
            {
                Name = "Admin",
                Surname = "Admin",
                Login = "admin@gmail.com",
                Password = "Qwerty@123",
                Roles = new List<string>()
                {
                    "Admin"
                },
                ExecutiveCompanyId = executiveCompany
            };
            await _serviceWrapper.UserService.CreateAsync(admin, "Admin");
            await _repositoryWrapper.Save();
        }
    }
    
    public async Task SeedAntenna()
    {
        List<Antenna>? antenna = _repositoryWrapper.AntennaRepository.GetAll().ToList();
        if (antenna.Count == 0)
        {
            AntennaDto baseAntenna = new AntennaDto()
            {
                Model = "TBXLHA-6565B-VTM",
                VerticalSizeDiameter = 9.2M,
            };
            await _serviceWrapper.AntennaService.CreateAsync(baseAntenna, "Admin");
            await _repositoryWrapper.Save();
        }
    }
    
    public async Task SeedTranslator()
    {
        List<TranslatorSpecs>? translatorSpecs = _repositoryWrapper.TranslatorSpecsRepository.GetAll().ToList();
        Antenna? antenna = await _repositoryWrapper.AntennaRepository.GetByCondition(x => x.Model.Equals("TBXLHA-6565B-VTM"));
        if (translatorSpecs.Count == 0)
        {
            TranslatorSpecsDto baseTranslator = new TranslatorSpecsDto()
            {
                Frequency = 900,
                Gain = 16.5M,
                AntennaId = antenna.Id
            };
            await _serviceWrapper.TranslatorSpecsService.CreateAsync(baseTranslator, "Admin");
            await _repositoryWrapper.Save();
        }
    }

    public async Task SeedRoles()
    {
        List<Role>? allRoles = _repositoryWrapper.RoleRepository.GetAll().ToList();
        if (allRoles.Count == 0)
        {
            List<Role> roles = new List<Role>
            {
                new Role()
                {
                    RoleName = "Admin",
                },
                new Role()
                {
                    RoleName = "Manager",
                },
                new Role()
                {
                    RoleName = "Analyst",
                }
            };
            foreach (var role in roles)
            {
                await _repositoryWrapper.RoleRepository.CreateAsync(role);
            }

            await _repositoryWrapper.Save();
        }
    }
    
    public async Task ProjectStatus()
    {
        List<ProjectStatus>? projectStatuses = _repositoryWrapper.ProjectStatusRepository.GetAll().ToList();
        if (projectStatuses.Count == 0)
        {
            projectStatuses = new List<ProjectStatus>
            {
                new ProjectStatus()
                {
                    Status = "Новый проект"
                },
                new ProjectStatus()
                {
                    Status = "Требуются данные"
                },
                new ProjectStatus()
                {
                    Status = "Ждет решения"
                },
                new ProjectStatus()
                {
                    Status = "Проект завршен успешно"
                },
                new ProjectStatus()
                {
                    Status = "Проект завершен отказом"
                }
            };
            foreach (var projectStatus in projectStatuses)
            {
                await _repositoryWrapper.ProjectStatusRepository.CreateAsync(projectStatus);
            }
            await _repositoryWrapper.Save();
        }
    }
    
    public async Task SeedContrAgents()
    {
        List<ContrAgent>? contrs = _repositoryWrapper.ContrAgentRepository.GetAll().ToList();
        if (contrs.Count == 0)
        {
            List<ContrAgentDto> contrAgents = new List<ContrAgentDto>
            {
                new ContrAgentDto
                {
                    CompanyName = "Tele2",
                    BIN = "523456789531",
                    DirectorName = "Алексей",
                    DirectorSurname = "Пронин",
                    DirectorPatronymic = "Викторович",
                    Address = "Address_Tele2",
                    Email = "tele2@info.com",
                    PhoneNumber = "+77477477477"
                },
                new ContrAgentDto
                {
                    CompanyName = "Activ",
                    BIN = "923400089531",
                    DirectorName = "Евгений",
                    DirectorSurname = "Иванов",
                    DirectorPatronymic = "Павлович",
                    Address = "Address_Activ",
                    Email = "activ@info.com",
                    PhoneNumber = "+77027027022"
                },
            };
            foreach (var contrAgent in contrAgents)
            {
                await _serviceWrapper.ContrAgentService.CreateAsync(contrAgent, "Admin");
            }

            await _repositoryWrapper.Save();
        }
    }

    public async Task SeedDistricts()
    {
        List<District>? allDistricts = _repositoryWrapper.DistrictRepository.GetAll().ToList();
        if (allDistricts.Count == 0)
        {
            List<DistrictDto> districts = new List<DistrictDto>
            {
                new DistrictDto
                {
                    DistrictName = "Абайская область",
                },
                new DistrictDto
                {
                    DistrictName = "Акмолинская область",
                },
                new DistrictDto
                {
                    DistrictName = "Актюбинская область",
                },
                new DistrictDto
                {
                    DistrictName = "Алматинская область",
                },
                new DistrictDto
                {
                    DistrictName = "Атырауская область",
                },
                new DistrictDto
                {
                    DistrictName = "Восточно-Казахстанская область",
                },
                new DistrictDto
                {
                    DistrictName = "Жамбылская область",
                },
                new DistrictDto
                {
                    DistrictName = "Жетысуская область",
                },
                new DistrictDto
                {
                    DistrictName = "Западно-Казахстанская область",
                },
                new DistrictDto
                {
                    DistrictName = "Карагандинская область",
                },
                new DistrictDto
                {
                    DistrictName = "Костанайская область",
                },
                new DistrictDto
                {
                    DistrictName = "Кызылординская область",
                },
                new DistrictDto
                {
                    DistrictName = "Мангистауская область",
                },
                new DistrictDto
                {
                    DistrictName = "Павлодарская область",
                },
                new DistrictDto
                {
                    DistrictName = "Северо-Казахстанская область",
                },
                new DistrictDto
                {
                    DistrictName = "Туркестанская область",
                },
                new DistrictDto
                {
                    DistrictName = "Улытауская область",
                },
                new DistrictDto
                {
                    DistrictName = "Города Республиканского значения",
                }
            };

            foreach (var district in districts)
            {
                await _serviceWrapper.DistrictService.CreateAsync(district, "Admin");
            }

            await _repositoryWrapper.Save();
        }
    }

    public async Task SeedTowns()
    {
        List<Town> allTowns = _repositoryWrapper.TownRepository.GetAll().ToList();
        if (allTowns.Count == 0)
        {
            List<Town> towns = new List<Town>
            {
                new Town
                {
                    TownName = "Атырау",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictId("Атырауская область"),
                },
                new Town
                {
                    TownName = "Рудный",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictId("Костанайская область"),
                },
                new Town
                {
                    TownName = "Экибастуз",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictId("Павлодарская область"),
                },
                new Town
                {
                    TownName = "Талдыкорган",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictId("Жетысуская область"),
                },
                new Town
                {
                    TownName = "Темиртау",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictId("Карагандинская область"),
                },
                new Town
                {
                    TownName = "Кокшетау",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictId("Акмолинская область"),
                },
                new Town
                {
                    TownName = "Туркестан",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictId("Туркестанская область"),
                },
                new Town
                {
                    TownName = "Петропавловск",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictId("Северо-Казахстанская область"),
                },
                new Town
                {
                    TownName = "Уральск",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictId("Западно-Казахстанская область"),
                },
                new Town
                {
                    TownName = "Костанай",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictId("Костанайская область"),
                },
                new Town
                {
                    TownName = "Актау",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictId("Мангистауская область"),
                },
                new Town
                {
                    TownName = "Кызылорда",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictId("Кызылординская область"),
                },
                new Town
                {
                    TownName = "Семей",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictId("Абайская область"),
                },
                new Town
                {
                    TownName = "Павлодар",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictId("Павлодарская область"),
                },
                new Town
                {
                    TownName = "Усть-Каменогорск",
                    DistrictId =
                        await _serviceWrapper.DistrictService.GetByDistrictId("Восточно-Казахстанская область"),
                },
                new Town
                {
                    TownName = "Тараз",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictId("Жамбылская область"),
                },
                new Town
                {
                    TownName = "Караганда",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictId("Карагандинская область"),
                },
                new Town
                {
                    TownName = "Актобе",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictId("Актюбинская область"),
                },
                new Town
                {
                    TownName = "Алматы",
                    DistrictId =
                        await _serviceWrapper.DistrictService.GetByDistrictId("Города Республиканского значения"),
                },
                new Town
                {
                    TownName = "Шымкент",
                    DistrictId =
                        await _serviceWrapper.DistrictService.GetByDistrictId("Города Республиканского значения"),
                },
                new Town
                {
                    TownName = "Астана",
                    DistrictId =
                        await _serviceWrapper.DistrictService.GetByDistrictId("Города Республиканского значения"),
                }
            };
            foreach (var town in towns)
            {
                await _serviceWrapper.TownService.CreateTownAsync(town);
            }

            await _repositoryWrapper.Save();
        }
    }
    
    public async Task SeedRadiationZone()
    {
        List<RadiationZone>? allRadiationZones = _repositoryWrapper.RadiationZoneRepository.GetAll().ToList();
        Antenna? antenna = await _repositoryWrapper.AntennaRepository.GetByCondition(x => x.Model.Equals("TBXLHA-6565B-VTM"));
        TranslatorSpecs? translatorSpecs = await _repositoryWrapper.TranslatorSpecsRepository
            .GetByCondition(x => x.AntennaId == antenna.Id && x.Frequency == 900);
        if (allRadiationZones.Count == 0)
        {
            List<RadiationZone> radiationZones = new List<RadiationZone>()
            {
                new RadiationZone
                {
                    Degree = 0,
                    Value = 0.000M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                }, new RadiationZone
                {
                    Degree = 1,
                    Value = -0.210M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                }, new RadiationZone
                {
                    Degree = 2,
                    Value = -0.790M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                }, new RadiationZone
                {
                    Degree = 3,
                    Value = -1.740M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                }, new RadiationZone
                {
                    Degree = 4,
                    Value = -3.140M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                }, new RadiationZone
                {
                    Degree = 5,
                    Value = -5.110M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 6,
                    Value = -7.680M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 7,
                    Value = -11.510M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 8,
                    Value = -17.740M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 9,
                    Value = -28.130M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 10,
                    Value = -20.420M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 11,
                    Value = -15.490M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 12,
                    Value = -13.190M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 13,
                    Value = -12.110M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 14,
                    Value = -11.770M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 15,
                    Value = -11.920M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 16,
                    Value = -12.440M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 17,
                    Value = -13.190M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 18,
                    Value = -14.110M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 19,
                    Value = -15.080M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 20,
                    Value = -15.940M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 21,
                    Value = -16.570M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 22,
                    Value = -16.850M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 23,
                    Value = -16.850M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 24,
                    Value = -16.680M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 25,
                    Value = -16.500M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 26,
                    Value = -16.450M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 27,
                    Value = -16.630M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 28,
                    Value = -17.120M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 29,
                    Value = -18.040M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 30,
                    Value = -19.460M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 31,
                    Value = -21.550M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 32,
                    Value = -24.640M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 33,
                    Value = -28.480M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 34,
                    Value = -29.410M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 35,
                    Value = -26.120M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 36,
                    Value = -23.120M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 37,
                    Value = -21.080M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 38,
                    Value = -19.880M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 39,
                    Value = -19.200M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 40,
                    Value = -18.970M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 41,
                    Value = -19.150M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 42,
                    Value = -19.720M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 43,
                    Value = -20.600M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 44,
                    Value = -21.880M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 45,
                    Value = -23.530M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 46,
                    Value = -25.440M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 47,
                    Value = -27.490M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 48,
                    Value = -29.230M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 49,
                    Value = -29.870M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 50,
                    Value = -29.390M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 51,
                    Value = -28.430M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 52,
                    Value = -27.610M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 53,
                    Value = -27.080M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 54,
                    Value = -26.920M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 55,
                    Value = -27.070M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 56,
                    Value = -27.530M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 57,
                    Value = -28.300M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 58,
                    Value = -29.310M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 59,
                    Value = -30.630M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 60,
                    Value = -32.200M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 61,
                    Value = -34.090M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 62,
                    Value = -36.150M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 63,
                    Value = -37.940M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 64,
                    Value = -38.920M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 65,
                    Value = -29.420M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 66,
                    Value = -37.260M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 67,
                    Value = -36.070M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 68,
                    Value = -35.000M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 69,
                    Value = -34.160M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 70,
                    Value = -33.550M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 71,
                    Value = -33.060M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 72,
                    Value = -32.670M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 73,
                    Value = -32.420M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 74,
                    Value = -32.250M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 75,
                    Value = -32.200M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 76,
                    Value = -32.180M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 77,
                    Value = -32.250M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 78,
                    Value = -32.320M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 79,
                    Value = -32.510M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 80,
                    Value = -32.760M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 81,
                    Value = -33.110M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 82,
                    Value = -33.530M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 83,
                    Value = -33.990M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 84,
                    Value = -34.420M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 85,
                    Value = -34.830M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 86,
                    Value = -35.180M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 87,
                    Value = -35.420M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 88,
                    Value = -35.680M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 89,
                    Value = -35.920M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 90,
                    Value = -36.160M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 91,
                    Value = -36.300M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 92,
                    Value = -36.470M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 93,
                    Value = -36.550M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 94,
                    Value = -36.610M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 95,
                    Value = -36.840M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 96,
                    Value = -37.210M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 97,
                    Value = -37.670M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 98,
                    Value = -38.310M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 99,
                    Value = -39.110M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 100,
                    Value = -39.910M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 101,
                    Value = -40.570M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 102,
                    Value = -41.020M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 103,
                    Value = -41.050M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 104,
                    Value = -40.560M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 105,
                    Value = -39.770M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 106,
                    Value = -38.790M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 107,
                    Value = -37.840M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 108,
                    Value = -36.910M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 109,
                    Value = -36.140M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 110,
                    Value = -35.420M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 111,
                    Value = -34.900M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 112,
                    Value = -34.540M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 113,
                    Value = -34.270M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 114,
                    Value = -34.130M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 115,
                    Value = -34.080M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 116,
                    Value = -34.150M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 117,
                    Value = -34.250M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 118,
                    Value = -34.470M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 119,
                    Value = -34.730M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 120,
                    Value = -35.150M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 121,
                    Value = -35.740M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 122,
                    Value = -36.490M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 123,
                    Value = -37.500M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 124,
                    Value = -38.840M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 125,
                    Value = -40.520M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 126,
                    Value = -42.670M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 127,
                    Value = -45.420M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 128,
                    Value = -48.260M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 129,
                    Value = -49.860M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 130,
                    Value = -48.850M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 131,
                    Value = -47.060M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 132,
                    Value = -45.770M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 133,
                    Value = -44.580M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 134,
                    Value = -43.480M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 135,
                    Value = -42.300M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 136,
                    Value = -40.750M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 137,
                    Value = -39.390M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 138,
                    Value = -38.190M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 139,
                    Value = -37.320M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 140,
                    Value = -36.870M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 141,
                    Value = -36.800M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 142,
                    Value = -36.940M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 143,
                    Value = -36.950M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 144,
                    Value = -36.610M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 145,
                    Value = -35.770M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 146,
                    Value = -34.500M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 147,
                    Value = -33.340M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 148,
                    Value = -32.420M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 149,
                    Value = -31.890M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 150,
                    Value = -31.800M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 151,
                    Value = -32.120M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 152,
                    Value = -32.900M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 153,
                    Value = -34.120M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 154,
                    Value = -35.800M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 155,
                    Value = -37.930M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 156,
                    Value = -40.380M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 157,
                    Value = -42.640M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 158,
                    Value = -43.890M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 159,
                    Value = -43.860M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 160,
                    Value = -43.210M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 161,
                    Value = -42.840M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 162,
                    Value = -42.770M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 163,
                    Value = -43.150M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 164,
                    Value = -44.290M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 165,
                    Value = -46.200M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 166,
                    Value = -49.550M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 167,
                    Value = -57.070M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 168,
                    Value = -81.800M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 169,
                    Value = -55.700M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 170,
                    Value = -53.310M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 171,
                    Value = -56.350M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 172,
                    Value = -58.240M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 173,
                    Value = -45.780M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 174,
                    Value = -39.180M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 175,
                    Value = -34.960M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 176,
                    Value = -31.820M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 177,
                    Value = -29.590M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 178,
                    Value = -28.030M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 179,
                    Value = -27.050M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 180,
                    Value = -26.600M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 181,
                    Value = -26.560M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 182,
                    Value = -27.220M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 183,
                    Value = -28.460M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 184,
                    Value = -30.560M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 185,
                    Value = -33.440M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 186,
                    Value = -37.480M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 187,
                    Value = -41.420M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 188,
                    Value = -41.310M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 189,
                    Value = -39.210M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 190,
                    Value = -38.100M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 191,
                    Value = -37.830M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 192,
                    Value = -37.770M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 193,
                    Value = -37.020M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 194,
                    Value = -35.490M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 195,
                    Value = -33.650M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 196,
                    Value = -32.320M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 197,
                    Value = -31.530M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 198,
                    Value = -31.260M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 199,
                    Value = -31.650M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 200,
                    Value = -32.650M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 201,
                    Value = -34.330M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 202,
                    Value = -36.460M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 203,
                    Value = -38.470M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 204,
                    Value = -38.930M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 205,
                    Value = -37.570M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 206,
                    Value = -35.840M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 207,
                    Value = -34.300M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 208,
                    Value = -33.010M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 209,
                    Value = -31.880M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 210,
                    Value = -30.980M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 211,
                    Value = -30.370M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 212,
                    Value = -30.080M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 213,
                    Value = -30.180M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 214,
                    Value = -30.760M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 215,
                    Value = -31.920M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 216,
                    Value = -33.800M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 217,
                    Value = -36.590M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 218,
                    Value = -41.080M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 219,
                    Value = -47.160M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 220,
                    Value = -46.520M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 221,
                    Value = -41.930M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 222,
                    Value = -39.460M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 223,
                    Value = -38.180M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 224,
                    Value = -37.770M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 225,
                    Value = -38.060M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 226,
                    Value = -38.570M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 227,
                    Value = -39.480M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 228,
                    Value = -40.610M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 229,
                    Value = -41.540M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 230,
                    Value = -42.230M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 231,
                    Value = -42.440M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 232,
                    Value = -42.010M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 233,
                    Value = -40.820M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 234,
                    Value = -39.270M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 235,
                    Value = -37.700M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 236,
                    Value = -36.240M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 237,
                    Value = -34.970M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 238,
                    Value = -33.980M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 239,
                    Value = -33.170M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 240,
                    Value = -32.550M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 241,
                    Value = -32.150M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 242,
                    Value = -31.900M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 243,
                    Value = -31.790M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 244,
                    Value = -31.790M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 245,
                    Value = -31.980M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 246,
                    Value = -32.190M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 247,
                    Value = -32.550M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 248,
                    Value = -33.050M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 249,
                    Value = -33.600M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 250,
                    Value = -34.300M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 251,
                    Value = -35.040M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 252,
                    Value = -35.890M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 253,
                    Value = -36.830M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 254,
                    Value = -37.830M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 255,
                    Value = -38.800M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 256,
                    Value = -39.750M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 257,
                    Value = -40.420M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 258,
                    Value = -41.090M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 259,
                    Value = -42.100M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 260,
                    Value = -42.880M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 261,
                    Value = -43.260M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 262,
                    Value = -43.900M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 263,
                    Value = -44.040M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 264,
                    Value = -44.170M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 265,
                    Value = -44.400M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 266,
                    Value = -44.530M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 267,
                    Value = -44.290M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 268,
                    Value = -43.890M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 269,
                    Value = -43.350M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 270,
                    Value = -42.770M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 271,
                    Value = -41.890M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 272,
                    Value = -40.930M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 273,
                    Value = -39.940M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 274,
                    Value = -39.000M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 275,
                    Value = -38.190M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 276,
                    Value = -37.420M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 277,
                    Value = -36.730M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 278,
                    Value = -36.150M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 279,
                    Value = -35.580M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 280,
                    Value = -35.060M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 281,
                    Value = -34.620M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 282,
                    Value = -34.260M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 283,
                    Value = -33.970M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 284,
                    Value = -33.760M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 285,
                    Value = -33.670M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 286,
                    Value = -33.610M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 287,
                    Value = -33.650M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 288,
                    Value = -33.760M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 289,
                    Value = -33.880M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 290,
                    Value = -34.080M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 291,
                    Value = -34.300M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 292,
                    Value = -34.520M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 293,
                    Value = -34.740M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 294,
                    Value = -35.050M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 295,
                    Value = -35.430M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 296,
                    Value = -35.980M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 297,
                    Value = -37.000M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 298,
                    Value = -38.480M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 299,
                    Value = -40.580M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 300,
                    Value = -42.010M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 301,
                    Value = -39.820M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 302,
                    Value = -35.810M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 303,
                    Value = -32.400M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 304,
                    Value = -29.660M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 305,
                    Value = -27.440M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 306,
                    Value = -25.720M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 307,
                    Value = -24.460M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },
                new RadiationZone
                {
                    Degree = 308,
                    Value = -23.490M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 309,
                    Value = -22.910M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 310,
                    Value = -22.660M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 311,
                    Value = -22.750M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 312,
                    Value = -23.250M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 313,
                    Value = -24.160M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 314,
                    Value = -25.490M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 315,
                    Value = -27.120M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 316,
                    Value = -28.310M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 317,
                    Value = -28.000M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 318,
                    Value = -26.290M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 319,
                    Value = -24.400M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 320,
                    Value = -22.860M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 321,
                    Value = -21.780M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 322,
                    Value = -21.170M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 323,
                    Value = -21.010M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 324,
                    Value = -21.330M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 325,
                    Value = -22.070M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 326,
                    Value = -23.320M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 327,
                    Value = -24.750M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 328,
                    Value = -25.810M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 329,
                    Value = -25.770M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 330,
                    Value = -24.740M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 331,
                    Value = -23.550M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 332,
                    Value = -22.800M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 333,
                    Value = -22.620M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 334,
                    Value = -23.140M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 335,
                    Value = -24.490M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 336,
                    Value = -26.850M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 337,
                    Value = -30.690M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 338,
                    Value = -33.860M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 339,
                    Value = -31.900M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 340,
                    Value = -28.790M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 341,
                    Value = -27.130M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 342,
                    Value = -26.680M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 343,
                    Value = -27.210M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 344,
                    Value = -28.350M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 345,
                    Value = -29.500M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 346,
                    Value = -30.000M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 347,
                    Value = -29.870M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 348,
                    Value = -28.320M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 349,
                    Value = -24.260M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 350,
                    Value = -19.410M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 351,
                    Value = -15.030M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 352,
                    Value = -11.320M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 353,
                    Value = -8.430M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 354,
                    Value = -5.940M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 355,
                    Value = -4.040M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 356,
                    Value = -2.520M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 357,
                    Value = -1.370M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 358,
                    Value = -0.580M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 359,
                    Value = -0.120M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                },new RadiationZone
                {
                    Degree = 360,
                    Value = 0.000M,
                    DirectionType = DirectionType.Vertical,
                    TranslatorSpecsId = translatorSpecs.Id
                }
            };
            foreach (var radiationZone in radiationZones)
            {
                await _repositoryWrapper.RadiationZoneRepository.CreateAsync(radiationZone);
            }
    
            await _repositoryWrapper.Save();
        }
    }
}