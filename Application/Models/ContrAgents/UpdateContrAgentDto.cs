namespace Application.Models.ContrAgents;

public class UpdateContrAgentDto
{
    public string Id { get; set; }
    public string CompanyName { get; set; }
    public string BIN { get; set; }
    public string DirectorName { get; set; }
    public string DirectorSurname { get; set; }
    public string DirectorPatronymic { get; set; }
    public decimal AmplificationFactor { get; set; }
}