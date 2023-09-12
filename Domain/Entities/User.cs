using Domain.Common;

namespace Domain.Entities;

public class User : BaseEntity
{
    public required string Login { get; set; }
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public string? Patronymic { get; set; }
    public required string PhoneNumber { get; set; }
    public required string PasswordHash { get; set; }
    public byte[] Salt { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; }
    public Guid? ExecutiveCompanyId { get; set; }
    public ExecutiveCompany ExecutiveCompany { get; set; }
}