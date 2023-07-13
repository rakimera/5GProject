using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.Projects;
using Application.Validation;
using AutoMapper;

namespace Application.Services;

public class ProjectService : IProjectService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ProjectValidator _validator;

    public ProjectService(IRepositoryWrapper repositoryWrapper, IMapper mapper, ProjectValidator validator)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _validator = validator;
    }

    public BaseResponse<IEnumerable<ProjectDTO>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<Guid?>> CreateAsync(ProjectDTO model)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<ProjectDTO>> GetByOid(Guid oid)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<Guid?>> Update(ProjectDTO model)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<bool>> Delete(Guid oid)
    {
        throw new NotImplementedException();
    }
}