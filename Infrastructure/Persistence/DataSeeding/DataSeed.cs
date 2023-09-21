using System.Security.Cryptography;
using Domain.Entities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DataSeeding;

public static class DataSeed
{
    public static void CreateExecutiveCompany(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ExecutiveCompany>().HasData(
            new ExecutiveCompany()
            {
                Id = new Guid("8cdf45cf-caa2-4258-b2c7-b0dd788a9898"), 
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                BIN = "160540025718",
                Address = "Ауэзовский район, мкр. Таугуль-1, д. 46, кв. 35",
                CompanyName = "ТОО «Центр технических решений»",
                LicenseNumber = "№02303P от 26.08.2021",
                DirectorName = "Мурат",
                DirectorSurname = "Жайлауов",
                DirectorPatronymic = "",
                LicenseDateOfIssue = DateTime.Today.AddDays(-10),
                TownName = "Алматы"
            }
        );
    }

    public static void CreateTranslatorType(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TranslatorType>().HasData(
            new TranslatorType()
            {
                Id = new Guid("ab576a8d-1e90-4fb3-9f36-852ac548ac6d"), 
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                Type = "GSM/UMTS"
            }
        );
        
        modelBuilder.Entity<TranslatorType>().HasData(
            new TranslatorType()
            {
                Id = new Guid("b0aa2ea4-c7d1-4dbb-84d2-3eb593a88f3f"), 
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                Type = "GSM/LTE"
            }
        );
        
        modelBuilder.Entity<TranslatorType>().HasData(
            new TranslatorType()
            {
                Id = new Guid("f2315247-17ef-4c99-94a4-c6ac54124a6e"), 
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                Type = "UMTS"
            }
        );
    }

    public static void CreateAdmin(this ModelBuilder modelBuilder)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        string password = GetPassword(salt, "Qwerty@123");
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = new Guid("a2b8f1c4-3d6e-4dca-9b7f-82763139a8e7"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                Name = "Admin",
                Surname = "Admin",
                Login = "admin@gmail.com",
                ExecutiveCompanyId = new Guid("8cdf45cf-caa2-4258-b2c7-b0dd788a9898"),
                PhoneNumber = "7012224562",
                Salt = salt,
                PasswordHash = password
            }
        );
    }

    private static string GetPassword(byte[] salt, string password)
    {
        string passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));
        return passwordHash;
    }
    
    public static void CreateRoles(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role()
            {
                Id = new Guid("cfd8fbf2-16ea-48ad-8c10-8ed144543d61"), 
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                RoleName = "Admin"
            }
        );
        
        modelBuilder.Entity<Role>().HasData(
            new Role()
            {
                Id = new Guid("7fda8b12-98c0-48bb-92c2-4fb3eb6b82c1"), 
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                RoleName = "Manager"
            }
        );
        
        modelBuilder.Entity<Role>().HasData(
            new Role()
            {
                Id = new Guid("3b7aae7e-87ae-4f02-9b01-9d9fc881af53"), 
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                RoleName = "Analyst"
            }
        );
    }

    public static void CreateUserRole(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRole>().HasData(
            new UserRole()
            {
                Id = new Guid("a78f374a-e084-4629-8f24-135dc0a0ab3d"), 
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                UserId = new Guid("a2b8f1c4-3d6e-4dca-9b7f-82763139a8e7"),
                RoleId = new Guid("cfd8fbf2-16ea-48ad-8c10-8ed144543d61")
            }
        );
    }
    
    public static void CreateAntenna(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Antenna>().HasData(
            new Antenna()
            {
                Id = new Guid("6a4e94d1-0d8f-4926-86d1-4f75b5c6a02a"), 
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                Model = "TBXLHA-6565B-VTM",
                VerticalSizeDiameter = 9.2M
            }
        );
    }
    
    public static void CreateTranslator(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TranslatorSpecs>().HasData(
            new TranslatorSpecs()
            {
                Id = Guid.NewGuid(), 
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                Frequency = 900,
                AntennaId = new Guid("6a4e94d1-0d8f-4926-86d1-4f75b5c6a02a")
            }
        );
    }
    
    public static void CreateProjectStatuses(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProjectStatus>().HasData(
            new ProjectStatus()
            {
                Id = new Guid("e4e5b8f6-40a5-4e7c-8d6b-3e3e9e2a1815"), 
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                Status = "Новый проект"
            }
        );
        
        modelBuilder.Entity<ProjectStatus>().HasData(
            new ProjectStatus()
            {
                Id = new Guid("7d3f1bd0-cb19-4a27-bd8c-8c2f5bdaa6fc"), 
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                Status = "Ждет решения"
            }
        );
        
        modelBuilder.Entity<ProjectStatus>().HasData(
            new ProjectStatus()
            {
                Id = new Guid("93e1d9e0-33b0-4f3c-b7d7-9fe1df0e2d51"), 
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                Status = "Проект завршен успешно"
            }
        );
        
        modelBuilder.Entity<ProjectStatus>().HasData(
            new ProjectStatus()
            {
                Id = new Guid("b8c1f7d6-2b45-47c9-8f13-4e17c9600be8"), 
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                Status = "Проект завершен отказом"
            }
        );
    }

    public static void CreateContrAgents(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ContrAgent>().HasData(
            new ContrAgent()
            {
                Id = new Guid("fc3e1eb3-928a-495a-9e90-6d6d1c6287a9"), 
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                CompanyName = "ТОО ЗАКАЗЧИК",
                BIN = "523456789531",
                DirectorName = "Алексей",
                DirectorSurname = "Пронин",
                DirectorPatronymic = "Викторович",
                Address = "Алматы, Сейфуллина 498, 2 этаж, офис 207",
                Email = "tele2@info.com",
                PhoneNumber = "7472020222"
            }
        );
    }

    public static void CreateDistricts(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<District>().HasData(
            new District()
            {
                Id = new Guid("1c3e25a9-4c43-4e0d-bc48-7d07b5f29f61"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                DistrictName = "Абайская область",
            }
        );
        
        modelBuilder.Entity<District>().HasData(
            new District()
            {
                Id = new Guid("2c3e25a9-4c43-4e0d-bc48-7d07b5f29f62"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                DistrictName = "Акмолинская область",
            }
        );    
        
        modelBuilder.Entity<District>().HasData(
            new District()
            {
                Id = new Guid("3c3e25a9-4c43-4e0d-bc48-7d07b5f29f63"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                DistrictName = "Актюбинская область",
            }
        );
        
        modelBuilder.Entity<District>().HasData(
            new District()
            {
                Id = new Guid("4c3e25a9-4c43-4e0d-bc48-7d07b5f29f64"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                DistrictName = "Алматинская область",
            }
        );
        
        modelBuilder.Entity<District>().HasData(
            new District()
            {
                Id = new Guid("5c3e25a9-4c43-4e0d-bc48-7d07b5f29f65"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                DistrictName = "Атырауская область",
            }
        );
        
        modelBuilder.Entity<District>().HasData(
            new District()
            {
                Id = new Guid("6c3e25a9-4c43-4e0d-bc48-7d07b5f29f66"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                DistrictName = "Восточно-Казахстанская область",
            }
        );
        
        modelBuilder.Entity<District>().HasData(
            new District()
            {
                Id = new Guid("7c3e25a9-4c43-4e0d-bc48-7d07b5f29f67"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                DistrictName = "Жамбылская область",
            }
        );
        
        modelBuilder.Entity<District>().HasData(
            new District()
            {
                Id = new Guid("8c3e25a9-4c43-4e0d-bc48-7d07b5f29f68"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                DistrictName = "Жетысуская область",
            }
        );
        
        modelBuilder.Entity<District>().HasData(
            new District()
            {
                Id = new Guid("9c3e25a9-4c43-4e0d-bc48-7d07b5f29f69"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                DistrictName = "Костанайская область",
            }
        );
        
        modelBuilder.Entity<District>().HasData(
            new District()
            {
                Id = new Guid("aab187ad-4eb8-4606-83f8-f39f3853ff99"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                DistrictName = "Кызылординская область",
            }
        );
        
        modelBuilder.Entity<District>().HasData(
            new District()
            {
                Id = new Guid("c77c452c-e96c-4a0b-b6ae-0366ce319a01"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                DistrictName = "Мангистауская область",
            }
        );
        
        modelBuilder.Entity<District>().HasData(
            new District()
            {
                Id = new Guid("a8ae3a20-76ec-4d4f-b4c7-c5bb3640b033"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                DistrictName = "Павлодарская область",
            }
        );
        
        modelBuilder.Entity<District>().HasData(
            new District()
            {
                Id = new Guid("0c254c2b-8ccf-46bf-bee7-6714181c1195"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                DistrictName = "Северо-Казахстанская область",
            }
        );
        
        modelBuilder.Entity<District>().HasData(
            new District()
            {
                Id = new Guid("26a7bd03-1133-40ec-a7e6-f541c80321af"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                DistrictName = "Туркестанская область",
            }
        );
        
        modelBuilder.Entity<District>().HasData(
            new District()
            {
                Id = new Guid("29135555-ac63-46ac-ae81-b6523ef86c53"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                DistrictName = "Улытауская область",
            }
        );
        
        modelBuilder.Entity<District>().HasData(
            new District()
            {
                Id = new Guid("1b7be14e-5c69-4521-ac39-2b9c331ee772"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                DistrictName = "Города Республиканского значения",
            }
        );
        
        modelBuilder.Entity<District>().HasData(
            new District()
            {
                Id = new Guid("99fd7f1f-f56f-423d-808b-1ece46362337"), 
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                DistrictName = "Западно-Казахстанская область"
            }
        );
        
        modelBuilder.Entity<District>().HasData(
            new District()
            {
                Id = new Guid("79aff963-9019-4925-8b7f-86029d3b2162"), 
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                DistrictName = "Карагандинская область"
            }
        );
    }
    
    public static void CreateTowns(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("1d6e60b7-0481-40d3-a98c-cb8ef2dc0b33"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Атырау",
                DistrictId = new Guid("5c3e25a9-4c43-4e0d-bc48-7d07b5f29f65"), // Привязка к Атырауской области
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("1f7e8363-6752-4186-846b-a2f71b5517df"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Рудный",
                DistrictId = new Guid("9c3e25a9-4c43-4e0d-bc48-7d07b5f29f69"), // Привязка к Костанайской области
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("23887382-1484-4617-81c3-8d64d92a9352"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Экибастуз",
                DistrictId = new Guid("a8ae3a20-76ec-4d4f-b4c7-c5bb3640b033"), // Привязка к Павлодарской области
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("e2f904f5-ff0f-41b9-811b-75a52bcf2bfd"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Талдыкорган",
                DistrictId = new Guid("8c3e25a9-4c43-4e0d-bc48-7d07b5f29f68"), // Привязка к Жетысуйской области
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("c4da5efa-23a6-4428-9e50-51ee6a492d8f"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Темиртау",
                DistrictId = new Guid("79aff963-9019-4925-8b7f-86029d3b2162"), // Привязка к Карагандинской области
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("072347d8-515a-48ce-b8ae-0342c317b436"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Кокшетау",
                DistrictId = new Guid("2c3e25a9-4c43-4e0d-bc48-7d07b5f29f62"), // Привязка к Акмолинской области
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("931a1bc0-8eb6-471b-a1e5-5510948e2e49"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Туркестан",
                DistrictId = new Guid("26a7bd03-1133-40ec-a7e6-f541c80321af"), // Привязка к Туркестанской области
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("a9cc0638-b62b-4086-8744-261f510833b1"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Петропавловск",
                DistrictId = new Guid("0c254c2b-8ccf-46bf-bee7-6714181c1195"), // Привязка к Северо-Казахстанской области
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("d25b8f36-e2ca-45c3-a44f-2aef3ff72c11"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Уральск",
                DistrictId = new Guid("99fd7f1f-f56f-423d-808b-1ece46362337"), // Привязка к Западно-Казахстанской области
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("eea97cf7-3577-4b56-a28b-df0f849f7c7c"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Костанай",
                DistrictId = new Guid("9c3e25a9-4c43-4e0d-bc48-7d07b5f29f69"), // Привязка к Костанайской области
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("32a8adf3-3086-4c0e-a822-ec6ee2f73f24"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Актау",
                DistrictId = new Guid("c77c452c-e96c-4a0b-b6ae-0366ce319a01"), // Привязка к Мангистауской области
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("be7cd4d4-1670-459a-9047-4f9f42fbfc49"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Кызылорда",
                DistrictId = new Guid("aab187ad-4eb8-4606-83f8-f39f3853ff99"), // Привязка к Кызылординской области
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("df697ace-2f5e-40ba-916d-4d3398744d01"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Семей",
                DistrictId = new Guid("1c3e25a9-4c43-4e0d-bc48-7d07b5f29f61") // Привязка к Абайской области
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("7dfc7928-700c-4cee-844a-4cee2d4a2d11"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Павлодар",
                DistrictId = new Guid("a8ae3a20-76ec-4d4f-b4c7-c5bb3640b033"), // Привязка к Павлодарской области
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("7aab7653-da53-4039-a589-e0c8225665a7"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Усть-Каменогорск",
                DistrictId = new Guid("6c3e25a9-4c43-4e0d-bc48-7d07b5f29f66") // Привязка к Восточно-Казахстанской области
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("d29e4a73-3aa4-4e6d-a946-02d3d03786b3"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Тараз",
                DistrictId = new Guid("7c3e25a9-4c43-4e0d-bc48-7d07b5f29f67"), // Привязка к Жамбыльской области
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("f55e9590-4010-4b79-9b08-f6d741460725"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Караганда",
                DistrictId = new Guid("79aff963-9019-4925-8b7f-86029d3b2162"), // Привязка к Карагандинской области
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("973d98d4-9c28-449f-b8d9-836119c9c097"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Актобе",
                DistrictId = new Guid("3c3e25a9-4c43-4e0d-bc48-7d07b5f29f63") // Привязка к Актюбинской области
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("db65ba2b-493a-48c7-a10f-f049490f683c"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Алматы",
                DistrictId = new Guid("1b7be14e-5c69-4521-ac39-2b9c331ee772"), // Привязка к Городам Республиканского значения
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("5b35bce6-4151-4abe-93b5-a654fdf85c08"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Шымкент",
                DistrictId = new Guid("1b7be14e-5c69-4521-ac39-2b9c331ee772"), // Привязка к Городам Республиканского значения
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("850c2f51-b391-4c82-930d-e695fe6b92ef"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Астана",
                DistrictId = new Guid("1b7be14e-5c69-4521-ac39-2b9c331ee772"), // Привязка к Городам Республиканского значения
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("7b6a2949-b31a-4cdf-a983-15fcf584fc0b"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Жезказган",
                DistrictId = new Guid("29135555-ac63-46ac-ae81-b6523ef86c53"), // Привязка к Улытауской области
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("f8a5dcfd-8c7d-4c1f-a149-4271151efa3a"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Каражал",
                DistrictId = new Guid("29135555-ac63-46ac-ae81-b6523ef86c53"), // Привязка к Улытауской области
            }
        );
        
        modelBuilder.Entity<Town>().HasData(
            new Town
            {
                Id = new Guid("ef239531-fb37-41b0-8e7a-32c650e4945a"),
                IsDelete = false,
                Created = DateTime.Now,
                CreatedBy = "SuperSystem",
                TownName = "Сатпаев",
                DistrictId = new Guid("29135555-ac63-46ac-ae81-b6523ef86c53"), // Привязка к Улытауской области
            }
        );
    }
}