using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.Projects;
using Application.Validation;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class ProjectService : IProjectService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ProjectValidator _projectValidator;

    public ProjectService(IRepositoryWrapper repositoryWrapper, IMapper mapper, ProjectValidator projectValidator)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _projectValidator = projectValidator;
    }

    public BaseResponse<IEnumerable<ProjectDto>> GetAll()
    {
        IQueryable<Project> projects = _repositoryWrapper.ProjectRepository.GetAll();
        List<ProjectDto> models = _mapper.Map<List<ProjectDto>>(projects);

        if (models.Count > 0)
        {
            return new BaseResponse<IEnumerable<ProjectDto>>(
                Result: models,
                Success: true,
                Messages: new List<string> { "Проекты успешно получены" });
        }

        return new BaseResponse<IEnumerable<ProjectDto>>(
            Result: models,
            Success: true,
            Messages: new List<string> { "Данные не были получены, возможно проекты еще не созданы или удалены" });
    }

    public async Task<BaseResponse<string>> CreateAsync(ProjectDto model)
    {
        var result = await _projectValidator.ValidateAsync(model);
        if (result.IsValid)
        {
            model.Created = DateTime.Now;
            model.CreatedBy = "Admin"; // реализация зависит от методики работы авторизацией.
            Project project = _mapper.Map<Project>(model);
            await _repositoryWrapper.ProjectRepository.CreateAsync(project);
            await _repositoryWrapper.Save();

            return new BaseResponse<string>(
                Result: project.Id.ToString(),
                Success: true,
                Messages: new List<string> { "Проект успешно создан" });
        }

        List<string> messages = _mapper.Map<List<string>>(result.Errors);

        return new BaseResponse<string>(
            Result: "",
            Messages: messages,
            Success: false);
    }

    public async Task<BaseResponse<ProjectDto>> GetByOid(string oid)
    {
        Project? project = await _repositoryWrapper.ProjectRepository.GetByCondition(x => x.Id.ToString() == oid);
        ProjectDto model = _mapper.Map<ProjectDto>(project);

        if (project is null)
            return new BaseResponse<ProjectDto>(
                Result: null,
                Messages: new List<string> { "Проект не найден" },
                Success: true);
        return new BaseResponse<ProjectDto>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Проект успешно найден" });
    }

    public async Task<BaseResponse<string>> Update(ProjectDto model)
    {
        var result = await _projectValidator.ValidateAsync(model);
        if (result.IsValid)
        {
            model.LastModified = DateTime.Now;
            model.LastModifiedBy = "Admin"; // реализация зависит от методики работы авторизацией.
            Project project = _mapper.Map<Project>(model);

            _repositoryWrapper.ProjectRepository.Update(project);
            await _repositoryWrapper.Save();

            return new BaseResponse<string>(
                Result: project.Id.ToString(),
                Success: true,
                Messages: new List<string> { "Проект успешно изменен" });
        }

        List<string> messages = _mapper.Map<List<string>>(result.Errors);

        return new BaseResponse<string>(
            Result: "",
            Messages: messages,
            Success: false);
    }

    public async Task<BaseResponse<bool>> Delete(string oid)
    {
        Project? project = await _repositoryWrapper.ProjectRepository.GetByCondition(x => x.Id.ToString() == oid);
        if (project is not null)
        {
            project.IsDelete = true;
            _repositoryWrapper.ProjectRepository.Update(project);
            await _repositoryWrapper.Save();

            return new BaseResponse<bool>(
                Result: true,
                Success: true,
                Messages: new List<string> { "Проект успешно удален" });
        }

        return new BaseResponse<bool>(
            Result: false,
            Messages: new List<string> { "Проекта не существует" },
            Success: false);
    }
}