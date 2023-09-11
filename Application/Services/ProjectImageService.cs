using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.Projects.ProjectImages;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class ProjectImageService : IProjectImageService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;

    public ProjectImageService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public BaseResponse<IEnumerable<ProjectImageDto>> GetAll()
    {
        IQueryable<ProjectImage> projectImages = _repositoryWrapper.ProjectImageRepository.GetAll();
        List<ProjectImageDto> models = _mapper.Map<List<ProjectImageDto>>(projectImages);

        if (models.Count > 0)
        {
            return new BaseResponse<IEnumerable<ProjectImageDto>>(
                Result: models,
                Success: true,
                Messages: new List<string> { "Фото проекта успешно получено" });
        }

        return new BaseResponse<IEnumerable<ProjectImageDto>>(
            Result: models,
            Success: true,
            Messages: new List<string> { "Фото проекта не было получено, возможно оно еще не создано или удалено" });
    }

    public async Task<BaseResponse<string>> CreateAsync(ProjectImageDto model, string creator)
    {
        model.CreatedBy = creator;
        
        ProjectImage projectImage = _mapper.Map<ProjectImage>(model);
        await _repositoryWrapper.ProjectImageRepository.CreateAsync(projectImage);
        await _repositoryWrapper.Save();

        return new BaseResponse<string>(
            Result: projectImage.Id.ToString(),
            Success: true,
            Messages: new List<string> { "Фото проекта успешно создано" });
    }

    public async Task<BaseResponse<ProjectImageDto>> GetByOid(string oid)
    {
        ProjectImage? project = await _repositoryWrapper.ProjectImageRepository.GetByCondition(x => x.Id.ToString() == oid);
        ProjectImageDto model = _mapper.Map<ProjectImageDto>(project);

        if (project is null)
            return new BaseResponse<ProjectImageDto>(
                Result: null,
                Messages: new List<string> { "Фото проекта не найдены" },
                Success: true);
        return new BaseResponse<ProjectImageDto>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Фото проекта успешно найдены" });
    }

    public async Task<BaseResponse<bool>> Delete(string id)
    {
        ProjectImage? projectImage = await _repositoryWrapper.ProjectImageRepository.GetByCondition(x => x.Id.ToString() == id);
        if (projectImage != null) 
            _repositoryWrapper.ProjectImageRepository.Delete(projectImage);
        else
            return new BaseResponse<bool>(
                Result: false,
                Messages: new List<string> { "Фото проекта не найдено" },
                Success: false);
        
        await _repositoryWrapper.Save();
        
        return new BaseResponse<bool>(
            Result: true,
            Messages: new List<string> { "Операция произведена успешно" },
            Success: true);
    }

    public async Task<BaseResponse<ProjectImageDto>> ConvertImage(ProjectImageDto model, IFormFile uploadedFile)
    {
        string[] allowedExtension = new[] { ".jpg", ".png", ".jpeg", ".jpe", ".jfif", ".tiff", ".tif", ".svg" };
        if (uploadedFile.Length != 0)
        {
            var fileExtension = Path.GetExtension(uploadedFile.FileName).ToLower();
            if (allowedExtension.Any(extension => extension == fileExtension))
            {
                using (var memoryStream = new MemoryStream())
                {                    await uploadedFile.CopyToAsync(memoryStream);

                    if (memoryStream.Length < 4097152)
                    {
                        model.Image = memoryStream.ToArray();
                    }
                    else
                    {
                        return new BaseResponse<ProjectImageDto>(
                            Result: model,
                            Messages: new List<string> { "Размер файла больше допустимого (4Mb)" },
                            Success: false);
                    }
                }    
                return new BaseResponse<ProjectImageDto>(
                    Result: model,
                    Messages: new List<string> { "Файл успешно сохранен" },
                    Success: true);
            }
            return new BaseResponse<ProjectImageDto>(
                Result: model,
                Messages: new List<string> { "Расширение файла не допустимо" },
                Success: false);
        }
        return new BaseResponse<ProjectImageDto>(
            Result: model,
            Messages: new List<string> { "Ошибка получения файла на сервере" },
            Success: false);
    }

    public BaseResponse<List<ProjectImageDto>> GetAllById(string id)
    {
        IQueryable<ProjectImage>? projects = _repositoryWrapper.ProjectImageRepository.GetAllByCondition(x => x.ProjectId.ToString() == id);
        List<ProjectImageDto> model = _mapper.Map<List<ProjectImageDto>>(projects);

        if (model.Count == 0)
            return new BaseResponse<List<ProjectImageDto>>(
                Result: model,
                Messages: new List<string> { "Фото проекта не найдены" },
                Success: false);
        return new BaseResponse<List<ProjectImageDto>>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Фото проекта успешно найдены" });
    }
}