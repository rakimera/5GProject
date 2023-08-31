namespace Application.Models.Users;

public class UpdateUserDto
{
    public string Id { get; set; }
    public string Login { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public List<string> Roles { get; set; }
    public Guid? ExecutiveCompanyId { get; set; }
}