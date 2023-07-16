namespace Application.Models;

public class UpdateUserDto
{
    public Guid Oid { get; set; }
    
    public string Login { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Role { get; set; }
    public string Oid { get; set; }
}