using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.AntennaTranslator;
using Application.Validation;
using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Domain.Entities;

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
        IQueryable<AntennaTranslator>? projectAntennas = _repositoryWrapper.AntennaTranslatorRepository.GetAllByCondition(x => x.Id.ToString() == id);
        List<AntennaTranslatorDto> model = _mapper.Map<List<AntennaTranslatorDto>>(projectAntennas);

        if (projectAntennas is null)
            return new BaseResponse<List<AntennaTranslatorDto>>(
                Result: null,
                Messages: new List<string> { "Антенны проекта не найдены" },
                Success: true);
        return new BaseResponse<List<AntennaTranslatorDto>>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Антенны проекта успешно найдены" });
    }
}