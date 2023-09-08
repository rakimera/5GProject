using Domain.Common;

namespace Domain.Entities;

public class User : BaseEntity
{
    public string Login { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string? Patronymic { get; set; }
    public string? PhoneNumber { get; set; }
    public string PasswordHash { get; set; }
    public byte[] Salt { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }
    public Guid? ExecutiveCompanyId { get; set; }
    public ExecutiveCompany ExecutiveCompany { get; set; }
}