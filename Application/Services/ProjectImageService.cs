using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.Projects.ProjectImages;
using AutoMapper;

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
        throw new NotImplementedException();
    }

    public Task<BaseResponse<string>> CreateAsync(ProjectImageDto model, string creator)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<ProjectImageDto>> GetByOid(string oid)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<bool>> Delete(string oid)
    {
        throw new NotImplementedException();
    }
}