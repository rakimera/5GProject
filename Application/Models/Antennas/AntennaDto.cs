using Application.Models.TranslatorSpecs;

namespace Application.Models.Antennas;

public class AntennaDto
{
    public required string Model { get; set; }
    public decimal VerticalSizeDiameter { get; set; }
    public List<TranslatorSpecsDto> TranslatorSpecsList { get; set; }
}