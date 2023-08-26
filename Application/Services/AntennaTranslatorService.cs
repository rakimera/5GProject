using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.AntennaTranslator;
using Application.Validation;
using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;

namespace Application.Services;

public class AntennaTranslatorService : IAntennaTranslatorService
{
    private readonly IMapper _mapper;
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly AntennaTranslatorValidator _antennaTranslatorValidator;

    public AntennaTranslatorService(IMapper mapper, IRepositoryWrapper repositoryWrapper, AntennaTranslatorValidator antennaTranslatorValidator)
    {
        _mapper = mapper;
        _repositoryWrapper = repositoryWrapper;
        _antennaTranslatorValidator = antennaTranslatorValidator;
    }

    public BaseResponse<IEnumerable<AntennaTranslatorDto>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<string>> CreateAsync(AntennaTranslatorDto model, string creator)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<AntennaTranslatorDto>> GetByOid(string oid)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<bool>> Delete(string oid)
    {
        throw new NotImplementedException();
    }

    public Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions)
    {
        throw new NotImplementedException();
    }

    public Task<BaseResponse<AntennaTranslatorDto>> Update(UpdateAntennaTranslatorDto model)
    {
        throw new NotImplementedException();
    }

    public BaseResponse<List<AntennaTranslatorDto>> GetAllByProjectId(string id)
    {
        throw new NotImplementedException();
    }
}