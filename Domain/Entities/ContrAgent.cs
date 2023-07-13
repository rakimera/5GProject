using Domain.Common;

namespace Domain.Entities;

public class ContrAgent : BaseEntity
{
    public string CompanyName { get; set; }
    public int BIN { get; set; }
    public string DirectorName { get; set; }
    public string DirectorSurname { get; set; }
    public string DirectorPatronymic { get; set; }
    public decimal AmplificationFactor { get; set; }
}