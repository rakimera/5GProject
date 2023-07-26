namespace Application.Models.ContrAgents;

public class CreateContrAgentDto
{
    public string CompanyName { get; set; }
    public int BIN { get; set; }
    public string DirectorName { get; set; }
    public string DirectorSurname { get; set; }
    public string DirectorPatronymic { get; set; }
    public decimal AmplificationFactor { get; set; }
}