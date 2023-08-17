namespace Application.Models.EnergyResult;

public class CreateEnergyResultDto
{
    public decimal PowerSignal { get; set; }
    public decimal Gain { get; set; }
    public decimal TransmitLossFactor { get; set; }
    public decimal HeightInstall { get; set; }
    public Guid AntennaTranslatorId { get; set; }
}