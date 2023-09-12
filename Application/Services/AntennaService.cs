using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.Antennae;
using Application.Validation;
using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Domain.Entities;

namespace Application.Services;

public class AntennaService : IAntennaService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly AntennaValidator _antennaValidator;

    public AntennaService(IRepositoryWrapper repositoryWrapper, IMapper mapper, AntennaValidator antennaValidator)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _antennaValidator = antennaValidator;
    }

    public BaseResponse<IEnumerable<AntennaDto>> GetAll()
    {
        IQueryable<Antenna> antennas = _repositoryWrapper.AntennaRepository.GetAll();
        List<AntennaDto> models = _mapper.Map<List<AntennaDto>>(antennas);

        if (models.Count > 0)
        {
            return new BaseResponse<IEnumerable<AntennaDto>>(
                Result: models,
                Success: true,
                Messages: new List<string> { "Антенны успешно получены" });
        }

        return new BaseResponse<IEnumerable<AntennaDto>>(
            Result: models,
            Success: true,
            Messages: new List<string> { "Данные не были получены, возможно антенны еще не созданы или удалены" });
    }

    public async Task<BaseResponse<string>> CreateAsync(AntennaDto model, string creator)
    {
        await _antennaValidator.ValidateAsync(model);
        model.CreatedBy = creator;
        Antenna antenna = _mapper.Map<Antenna>(model);
        await _repositoryWrapper.AntennaRepository.CreateAsync(antenna);
        await _repositoryWrapper.Save();

        return new BaseResponse<string>(
            Result: antenna.Id.ToString(),
            Success: true,
            Messages: new List<string> { "Антенна успешна создана" });
    }

    public async Task<BaseResponse<AntennaDto>> GetByOid(string oid)
    {
        Antenna? antenna = await _repositoryWrapper.AntennaRepository.GetByCondition(x => x.Id.ToString() == oid);
        AntennaDto model = _mapper.Map<AntennaDto>(antenna);
        if (antenna is null)
            return new BaseResponse<AntennaDto>(
                Result: null,
                Messages: new List<string> { "Антенна не найдена" },
                Success: true);

        return new BaseResponse<AntennaDto>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Антенна успешно найдена" });
    }
    
    public async Task<BaseResponse<bool>> Delete(string id)
    {
        Antenna? antenna = await _repositoryWrapper.AntennaRepository.GetByCondition(x => x.Id.ToString() == id);
        if (antenna is not null)
        {
            antenna.IsDelete = true;
            _repositoryWrapper.AntennaRepository.Update(antenna);
            await _repositoryWrapper.Save();

            return new BaseResponse<bool>(
                Result: true,
                Success: true,
                Messages: new List<string> { "Антенна успешно удалена" });
        }
        return new BaseResponse<bool>(
            Result: false,
            Messages: new List<string> { "Антенна не существует" },
            Success: false);
    }

    public async Task<BaseResponse<string>> Update(UpdateAntennaDto model, string modifier)
    {
        var antennaDto = _mapper.Map<AntennaDto>(model);
        var result = await _antennaValidator.ValidateAsync(antennaDto);
        if (!result.IsValid)
        {
            List<string> messages = _mapper.Map<List<string>>(result.Errors);
        
            return new BaseResponse<string>(
                Result: "",
                Messages: messages,
                Success: false);
        }

        Antenna? antenna = await _repositoryWrapper.AntennaRepository.GetByCondition(x => x.Id.ToString() == model.Id);
        if (antenna == null)
        {
            return new BaseResponse<string>(
                Result: null,
                Messages: new List<string> { "Антенна не найдена" },
                Success: false);
        }

        _mapper.Map(model, antenna);
        antenna.LastModifiedBy = modifier;

        _repositoryWrapper.AntennaRepository.Update(antenna);
        await _repositoryWrapper.Save();

        return new BaseResponse<string>(
            Result: antenna.Id.ToString(),
            Success: true,
            Messages: new List<string> { "Антенна успешно изменена" });
    }

    public async Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions)
    {
        var queryableAntennae = _repositoryWrapper.AntennaRepository.GetAll();
        return await DataSourceLoader.LoadAsync(queryableAntennae, loadOptions);
    }
}