using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models;
using Application.Models.ContrAgents;
using Application.Models.Users;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Persistence.DataSeeding;

public class DataSeed
{
    private readonly IServiceWrapper _serviceWrapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;

    public DataSeed(IServiceWrapper serviceWrapper, IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _serviceWrapper = serviceWrapper;
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
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
            await _serviceWrapper.UserService.CreateAsync(admin);
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
                await _serviceWrapper.ContrAgentService.CreateAsync(contrAgent);
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
                await _serviceWrapper.DistrictService.CreateAsync(district);
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
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Атырауская область"),
                    District = await _serviceWrapper.DistrictService.GetDistrictByName("Атырауская область")
                },
                new Town
                {
                    TownName = "Рудный",
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Костанайская область"),
                    District = await _serviceWrapper.DistrictService.GetDistrictByName("Костанайская область")
                },
                new Town
                {
                    TownName = "Экибастуз",
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Павлодарская область"),
                    District = await _serviceWrapper.DistrictService.GetDistrictByName("Павлодарская область")
                },
                new Town
                {
                    TownName = "Талдыкорган",
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Жетысуская область"),
                    District = await _serviceWrapper.DistrictService.GetDistrictByName("Жетысуская область")
                },
                new Town
                {
                    TownName = "Темиртау",
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Карагандинская область"),
                    District = await _serviceWrapper.DistrictService.GetDistrictByName("Карагандинская область")
                },
                new Town
                {
                    TownName = "Кокшетау",
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Акмолинская область"),
                    District = await _serviceWrapper.DistrictService.GetDistrictByName("Акмолинская область")
                },
                new Town
                {
                    TownName = "Туркестан",
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Туркестанская область"),
                    District = await _serviceWrapper.DistrictService.GetDistrictByName("Туркестанская область")
                },
                new Town
                {
                    TownName = "Петропавловск",
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Северо-Казахстанская область"),
                    District = await _serviceWrapper.DistrictService.GetDistrictByName("Северо-Казахстанская область")
                },
                new Town
                {
                    TownName = "Уральск",
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Западно-Казахстанская область"),
                    District = await _serviceWrapper.DistrictService.GetDistrictByName("Западно-Казахстанская область")
                },
                new Town
                {
                    TownName = "Костанай",
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Костанайская область"),
                    District = await _serviceWrapper.DistrictService.GetDistrictByName("Костанайская область")
                },
                new Town
                {
                    TownName = "Актау",
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Мангистауская область"),
                    District = await _serviceWrapper.DistrictService.GetDistrictByName("Мангистауская область")
                },
                new Town
                {
                    TownName = "Кызылорда",
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Кызылординская область"),
                    District = await _serviceWrapper.DistrictService.GetDistrictByName("Кызылординская область")
                },
                new Town
                {
                    TownName = "Семей",
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Абайская область"),
                    District = await _serviceWrapper.DistrictService.GetDistrictByName("Абайская область")
                },
                new Town
                {
                    TownName = "Павлодар",
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Павлодарская область"),
                    District = await _serviceWrapper.DistrictService.GetDistrictByName("Павлодарская область")
                },
                new Town
                {
                    TownName = "Усть-Каменогорск",
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Восточно-Казахстанская область"),
                    District = await  _serviceWrapper.DistrictService.GetDistrictByName("Восточно-Казахстанская область")
                },
                new Town
                {
                    TownName = "Тараз",
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Жамбылская область"),
                    District = await _serviceWrapper.DistrictService.GetDistrictByName("Жамбылская область")
                },
                new Town
                {
                    TownName = "Караганда",
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Карагандинская область"),
                    District = await _serviceWrapper.DistrictService.GetDistrictByName("Карагандинская область")
                },
                new Town
                {
                    TownName = "Актобе",
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Актюбинская область"),
                    District = await _serviceWrapper.DistrictService.GetDistrictByName("Актюбинская область")
                },
                new Town
                {
                    TownName = "Алматы",
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Города Республиканского значения"),
                    District = await _serviceWrapper.DistrictService.GetDistrictByName("Города Республиканского значения")
                },
                new Town
                {
                    TownName = "Шымкент",
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Города Республиканского значения"),
                    District = await _serviceWrapper.DistrictService.GetDistrictByName("Города Республиканского значения")
                },
                new Town
                {
                    TownName = "Астана",
                    DistrictOid = await _serviceWrapper.DistrictService.GetByDistrictOid("Города Республиканского значения"),
                    District = await _serviceWrapper.DistrictService.GetDistrictByName("Города Республиканского значения")
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