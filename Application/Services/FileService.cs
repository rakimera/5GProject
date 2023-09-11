using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Domain.Entities;
using Domain.Enums;
using OfficeOpenXml;

namespace Application.Services;

public class FileService : IFileService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IEnergyFlowService _energyFlowService;
    private readonly IBiohazardRadiusService _biohazardRadiusService;


    public FileService(IRepositoryWrapper repositoryWrapper, IEnergyFlowService energyFlowService, IBiohazardRadiusService biohazardRadiusService)
    {
        _repositoryWrapper = repositoryWrapper;
        _energyFlowService = energyFlowService;
        _biohazardRadiusService = biohazardRadiusService;
    }

    public Task<BaseResponse<bool>> GetLoadXlsx()
    {
        throw new NotImplementedException();
    }

    public async Task<BaseResponse<bool>> ReadExcel(string filePath,TranslatorSpecs translatorSpecs,DirectionType type)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
        {
            using (ExcelPackage package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                List<RadiationZone> list = new List<RadiationZone>();
                for (int row = 1; row <= worksheet.Dimension.Rows; row++)
                {
                    var degreeCellValue = worksheet.Cells[row, 1].Value;
                    var valueCellValue = worksheet.Cells[row, 2].Value;
                    if (int.TryParse(degreeCellValue?.ToString(), out int degree) && decimal.TryParse(valueCellValue?.ToString(), out decimal value))
                    {
                        RadiationZone radiationZone = new RadiationZone()
                        {
                            Degree = degree,
                            Value = value,
                            DirectionType = type,
                            TranslatorSpecsId = translatorSpecs.Id
                        };
                        await _repositoryWrapper.RadiationZoneRepository.CreateAsync(radiationZone);
                        list.Add(radiationZone);
                    }
                }
                if (list.Count != 360)
                {
                    return new BaseResponse<bool>(
                        Result: false,
                        Messages: new List<string> { "Файл не корректный" },
                        Success: false);
                }
                await _repositoryWrapper.Save();
            }
        }
        return new BaseResponse<bool>(
            Result: true,
            Messages: new List<string> { "Файл успешно считан" },
            Success: true);
    }

    public Task<BaseResponse<bool>> ProjectWord(string oid)
    {
        throw new NotImplementedException();
    }


    private int CheckCountTable(List<BiohazardRadius> biohazardRadii, int maxRadiusDegree, int degreeZ,DirectionType type)
    {
        var y = 0;
        var maxRadius = biohazardRadii.Max(radiation => Math.Abs(radiation.MaximumBiohazardRadius));
        var radiationMaxRadius = biohazardRadii
            .First(radiation => Math.Abs(radiation.MaximumBiohazardRadius) == maxRadius);
        var minRadiusValueZ = biohazardRadii.Min(radiation => radiation.BiohazardRadiusZ);
        var radiationZ = biohazardRadii
            .First(radiation => radiation.BiohazardRadiusZ == minRadiusValueZ);
        if (type == DirectionType.Horizontal)
        {
            var maxRadiusValueZ = biohazardRadii.Max(radiation => Math.Abs(radiation.BiohazardRadiusZ) );
            radiationZ = biohazardRadii
                .First(radiation => Math.Abs(radiation.BiohazardRadiusZ) == maxRadiusValueZ);
        }
        
        for (int i = 0; i < biohazardRadii.Count; i++) 
        {
            var x = i % 10;
            if (x == 0 || biohazardRadii[i] == radiationMaxRadius || biohazardRadii[i] == radiationZ
                || biohazardRadii[i].Degree == maxRadiusDegree || biohazardRadii[i].Degree == degreeZ)
            {
                y++;
            }
            
        }

        return y;
    }
}