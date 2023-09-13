using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.RadiationZone.RadiationZoneExelFile;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

namespace Application.Services;

public class RadiationZoneExelFileService : IRadiationZoneExelFileService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;

    public RadiationZoneExelFileService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public BaseResponse<IEnumerable<RadiationZoneExelFileDto>> GetAll()
    {
        IQueryable<RadiationZoneExelFile> exelFiles = _repositoryWrapper.RadiationZoneExelFileRepository.GetAll();
        List<RadiationZoneExelFileDto> models = _mapper.Map<List<RadiationZoneExelFileDto>>(exelFiles);

        if (models.Count > 0)
        {
            return new BaseResponse<IEnumerable<RadiationZoneExelFileDto>>(
                Result: models,
                Success: true,
                Messages: new List<string> { "Exel файлы успешно получены" });
        }

        return new BaseResponse<IEnumerable<RadiationZoneExelFileDto>>(
            Result: models,
            Success: true,
            Messages: new List<string> { "Exel файлы не были получены, возможно оно еще не создано или удалено" });
    }

    public async Task<BaseResponse<string>> CreateAsync( string id, IFormFile vertical, IFormFile horizontal, string creator)
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
                if (list.Count != 361)
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

    public async Task<BaseResponse<RadiationZoneExelFileDto>> GetByOid(string oid)
    {
        RadiationZoneExelFile? exelFile = await _repositoryWrapper.RadiationZoneExelFileRepository.GetByCondition(x => x.Id.ToString() == oid);
        RadiationZoneExelFileDto model = _mapper.Map<RadiationZoneExelFileDto>(exelFile);

        if (exelFile is null)
            return new BaseResponse<RadiationZoneExelFileDto>(
                Result: null,
                Messages: new List<string> { "Exel файл не найден" },
                Success: true);
        return new BaseResponse<RadiationZoneExelFileDto>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Exel файл успешно найден" });
    }

    public async Task<BaseResponse<bool>> Delete(string oid)
    {
        RadiationZoneExelFile? exelFile = await _repositoryWrapper.RadiationZoneExelFileRepository.GetByCondition(x => x.Id.ToString() == oid);
        if (exelFile != null) 
            _repositoryWrapper.RadiationZoneExelFileRepository.Delete(exelFile);
        else
            return new BaseResponse<bool>(
                Result: false,
                Messages: new List<string> { "Exel файл не найден" },
                Success: false);
        
        await _repositoryWrapper.Save();
        
        return new BaseResponse<bool>(
            Result: true,
            Messages: new List<string> { "Exel файл успешно удален" },
            Success: true);
    }

    public async Task<BaseResponse<RadiationZoneExelFileDto>> ConvertExel(RadiationZoneExelFileDto model, IFormFile uploadedFile)
    {
        string[] allowedExtension = new[] { ".csv", ".xlsx", ".xlsm" };
        if (uploadedFile.Length != 0)
        {
            var fileExtension = Path.GetExtension(uploadedFile.FileName).ToLower();
            if (allowedExtension.Any(extension => extension == fileExtension))
            {
                using (var memoryStream = new MemoryStream())
                {                    await uploadedFile.CopyToAsync(memoryStream);

                    if (memoryStream.Length < 4097152)
                    {
                        model.ExelFile = memoryStream.ToArray();
                    }
                    else
                    {
                        return new BaseResponse<RadiationZoneExelFileDto>(
                            Result: model,
                            Messages: new List<string> { "Размер файла больше допустимого (4Mb)" },
                            Success: false);
                    }
                }    
                return new BaseResponse<RadiationZoneExelFileDto>(
                    Result: model,
                    Messages: new List<string> { "Файл успешно сохранен" },
                    Success: true);
            }
            return new BaseResponse<RadiationZoneExelFileDto>(
                Result: model,
                Messages: new List<string> { "Расширение файла не допустимо" },
                Success: false);
        }
        return new BaseResponse<RadiationZoneExelFileDto>(
            Result: model,
            Messages: new List<string> { "Ошибка получения файла на сервере" },
            Success: false);
    }

    public BaseResponse<List<RadiationZoneExelFileDto>> GetAllById(string id)
    {
        IQueryable<RadiationZoneExelFile>? projects = _repositoryWrapper.RadiationZoneExelFileRepository.GetAllByCondition(x => x.TranslatorSpecId.ToString() == id);
        List<RadiationZoneExelFileDto> model = _mapper.Map<List<RadiationZoneExelFileDto>>(projects);

        if (projects is null)
            return new BaseResponse<List<RadiationZoneExelFileDto>>(
                Result: null,
                Messages: new List<string> { "Exel файл не найден" },
                Success: true);
        return new BaseResponse<List<RadiationZoneExelFileDto>>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Exel файл успешно найден" });
    }
}