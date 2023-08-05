using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models;
using Application.Models.ContrAgents;
using Application.Models.Users;
using Domain.Entities;

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
    

    public async Task SeedAdmin()
    {
        User? user = await _repositoryWrapper.UserRepository.GetByCondition(u => u.Login.Equals("admin@gmail.com"));
        if (user is null)
        {
            UserDto admin = new UserDto()
            {
                Name = "Admin",
                Surname = "Admin",
                Login = "admin@gmail.com",
                Password = "Qwerty@123",
                Role = "Admin"
            };
            await _serviceWrapper.UserService.CreateAsync(admin,"Admin");
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
                    AmplificationFactor = 0.71M
                },
                new ContrAgentDto
                {
                    CompanyName = "Activ",
                    BIN = "923400089531",
                    DirectorName = "Евгений",
                    DirectorSurname = "Иванов",
                    DirectorPatronymic = "Павлович",
                    AmplificationFactor = 0.45M
                },
            };
            foreach (var contrAgent in contrAgents)
            {
                await _serviceWrapper.ContrAgentService.CreateAsync(contrAgent,"Admin");
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
                await _serviceWrapper.DistrictService.CreateAsync(district,"Admin");
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
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Атырауская область"),
                },
                new Town
                {
                    TownName = "Рудный",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Костанайская область"),
                },
                new Town
                {
                    TownName = "Экибастуз",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Павлодарская область"),
                },
                new Town
                {
                    TownName = "Талдыкорган",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Жетысуская область"),
                },
                new Town
                {
                    TownName = "Темиртау",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Карагандинская область"),
                },
                new Town
                {
                    TownName = "Кокшетау",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Акмолинская область"),
                },
                new Town
                {
                    TownName = "Туркестан",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Туркестанская область"),
                },
                new Town
                {
                    TownName = "Петропавловск",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Северо-Казахстанская область"),
                },
                new Town
                {
                    TownName = "Уральск",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Западно-Казахстанская область"),
                },
                new Town
                {
                    TownName = "Костанай",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Костанайская область"),
                },
                new Town
                {
                    TownName = "Актау",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Мангистауская область"),
                },
                new Town
                {
                    TownName = "Кызылорда",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Кызылординская область"),
                },
                new Town
                {
                    TownName = "Семей",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Абайская область"),
                },
                new Town
                {
                    TownName = "Павлодар",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Павлодарская область"),
                },
                new Town
                {
                    TownName = "Усть-Каменогорск",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Восточно-Казахстанская область"),
                },
                new Town
                {
                    TownName = "Тараз",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Жамбылская область"),
                },
                new Town
                {
                    TownName = "Караганда",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Карагандинская область"),
                },
                new Town
                {
                    TownName = "Актобе",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Актюбинская область"),
                },
                new Town
                {
                    TownName = "Алматы",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Города Республиканского значения"),
                },
                new Town
                {
                    TownName = "Шымкент",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Города Республиканского значения"),
                },
                new Town
                {
                    TownName = "Астана",
                    DistrictId = await _serviceWrapper.DistrictService.GetByDistrictOid("Города Республиканского значения"),
                }
            };
            foreach (var town in towns)
            {
                await _serviceWrapper.TownService.CreateTownAsync(town);
            }
            await _repositoryWrapper.Save();
        }
    }
}