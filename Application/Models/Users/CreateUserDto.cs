namespace Application.Models.Users;

public class CreateUserDto
{
    public string Login { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Password { get; set; }
    public List<string> Roles { get; set; }
    public Guid? ExecutiveCompanyId { get; set; }
}