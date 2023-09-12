using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.AntennaTranslator;
using Application.Models.Projects.ProjectAntennas;
using Application.Validation;
using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Domain.Entities;

namespace Application.Services;

public class ProjectAntennaService : IProjectAntennaService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly ProjectAntennaValidator _projectAntennaValidator;

    public ProjectAntennaService(IRepositoryWrapper repositoryWrapper, IMapper mapper, ProjectAntennaValidator projectAntennaValidator)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _projectAntennaValidator = projectAntennaValidator;
    }

    public BaseResponse<IEnumerable<ProjectAntennaDto>> GetAll()
    {
        IQueryable<ProjectAntenna> projects = _repositoryWrapper.ProjectAntennaRepository.GetAll();
        List<ProjectAntennaDto> models = _mapper.Map<List<ProjectAntennaDto>>(projects);

        if (models.Count > 0)
        {
            return new BaseResponse<IEnumerable<ProjectAntennaDto>>(
                Result: models,
                Success: true,
                Messages: new List<string> { "Антенны проекта успешно получены" });
        }

        return new BaseResponse<IEnumerable<ProjectAntennaDto>>(
            Result: models,
            Success: true,
            Messages: new List<string> { "Данные не были получены, возможно антенны проекта еще не созданы или удалены" });
    }

    public async Task<BaseResponse<string>> CreateAsync(ProjectAntennaDto model, string creator)
    {
        model.CreatedBy = creator;

        var result = await _projectAntennaValidator.ValidateAsync(model);
        if (result.IsValid)
        {
            ProjectAntenna project = _mapper.Map<ProjectAntenna>(model);
            await _repositoryWrapper.ProjectAntennaRepository.CreateAsync(project);
            await _repositoryWrapper.Save();

            return new BaseResponse<string>(
                Result: project.Id.ToString(),
                Success: true,
                Messages: new List<string> { "Антенна успешно добавлена" });
        }

        List<string> messages = _mapper.Map<List<string>>(result.Errors);

        return new BaseResponse<string>(
            Result: "",
            Messages: messages,
            Success: false);
    }
    
    public async Task<BaseResponse<string>> Update(ProjectAntennaDto model, string modifare)
    {
        var result = await _projectAntennaValidator.ValidateAsync(model);
        if (!result.IsValid)
        {
            List<string> messages = _mapper.Map<List<string>>(result.Errors);

            return new BaseResponse<string>(
                Result: "",
                Messages: messages,
                Success: false);
        }

        ProjectAntenna? projectAntenna = await _repositoryWrapper.ProjectAntennaRepository.GetByCondition(x => x.Id.Equals(model.Id));
        if (projectAntenna == null)
        {
            return new BaseResponse<string>(
                Result: null,
                Messages: new List<string> { "Антенна проекта не найдена" },
                Success: false);
        }

        if (projectAntenna.HeightFromGroundLevel != model.HeightFromGroundLevel)
        {
            //логика обновления просчетов EnergyResult
        }

        if (projectAntenna.AntennaId != model.AntennaId)
        {
            var response = await Delete(projectAntenna.Id.ToString());
            if (response.Result)
            {
                var createResponse = await CreateAsync(model, modifare);
                return createResponse;
            }
        }
        
        _mapper.Map(model, projectAntenna);
        projectAntenna.LastModifiedBy = modifare;

        _repositoryWrapper.ProjectAntennaRepository.Update(projectAntenna);
        await _repositoryWrapper.Save();

        return new BaseResponse<string>(
            Result: projectAntenna.Id.ToString(),
            Success: true,
            Messages: new List<string> { "Антенна успешно изменена" });
    }

    public async Task<BaseResponse<ProjectAntennaDto>> GetByOid(string oid)
    {
        ProjectAntenna? projectAntenna = await _repositoryWrapper.ProjectAntennaRepository.GetByCondition(x => x.Id.ToString() == oid);
        ProjectAntennaDto model = _mapper.Map<ProjectAntennaDto>(projectAntenna);

        if (projectAntenna is null)
            return new BaseResponse<ProjectAntennaDto>(
                Result: null,
                Messages: new List<string> { "Антенна проекта не найдена" },
                Success: true);
        return new BaseResponse<ProjectAntennaDto>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Антенна проекта успешно найдена" });
    }
    
    public BaseResponse<List<ProjectAntennaDto>> GetAllByProjectId(string id)
    {
        IQueryable<ProjectAntenna>? projectAntennas = _repositoryWrapper.ProjectAntennaRepository.GetAllByCondition(x => x.ProjectId.ToString() == id);
        List<ProjectAntennaDto> model = _mapper.Map<List<ProjectAntennaDto>>(projectAntennas);

        if (projectAntennas is null)
            return new BaseResponse<List<ProjectAntennaDto>>(
                Result: null,
                Messages: new List<string> { "Антенны проекта не найдены" },
                Success: true);
        return new BaseResponse<List<ProjectAntennaDto>>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Антенны проекта успешно найдены" });
    }

    public async Task<BaseResponse<bool>> Delete(string oid)
    {
        ProjectAntenna? projectAntenna = await _repositoryWrapper.ProjectAntennaRepository.GetByCondition(x => x.Id.ToString() == oid);
        if (projectAntenna is not null)
        {
            projectAntenna.IsDelete = true;
            _repositoryWrapper.ProjectAntennaRepository.Update(projectAntenna);
            await _repositoryWrapper.Save();

            return new BaseResponse<bool>(
                Result: true,
                Success: true,
                Messages: new List<string> { "Антенна проекта успешно удалена" });
        }

        return new BaseResponse<bool>(
            Result: false,
            Messages: new List<string> { "Антенна проекта не существует" },
            Success: false);
    }

    public async Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions, string id)
    {
        var queryableUsers = _repositoryWrapper.ProjectAntennaRepository.GetAllByCondition(x => x.ProjectId.ToString() == id);
        LoadResult? r = await DataSourceLoader.LoadAsync(queryableUsers, loadOptions);
        return await DataSourceLoader.LoadAsync(queryableUsers, loadOptions);
    }
}