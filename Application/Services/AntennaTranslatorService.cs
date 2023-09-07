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
        IQueryable<AntennaTranslator> antennaTranslators = _repositoryWrapper.AntennaTranslatorRepository.GetAll();
        List<AntennaTranslatorDto> models = _mapper.Map<List<AntennaTranslatorDto>>(antennaTranslators);
        
        if (models.Count > 0)
        {
            return new BaseResponse<IEnumerable<AntennaTranslatorDto>>(
                Result: models,
                Success: true,
                Messages: new List<string> { "Передатчики антенны успешно получены" });
        }

        return new BaseResponse<IEnumerable<AntennaTranslatorDto>>(
            Result: models,
            Success: true,
            Messages: new List<string> { "Передатчики антенны не найдены, возможно они еще не созданы или удалены" });
    }

    public async Task<BaseResponse<string>> CreateAsync(AntennaTranslatorDto model, string creator)
    {
        AntennaTranslator antennaTranslator = _mapper.Map<AntennaTranslator>(model);
        antennaTranslator.CreatedBy = creator;
        await _repositoryWrapper.AntennaTranslatorRepository.CreateAsync(antennaTranslator);
        await _repositoryWrapper.Save();

        return new BaseResponse<string>(
            Result: antennaTranslator.Id.ToString(),
            Success: true,
            Messages: new List<string> { "Передатчик антенны успешно создан" });
    }

    public async Task<BaseResponse<AntennaTranslatorDto>> GetByOid(string id)
    {
        AntennaTranslator? antennaTranslator = await _repositoryWrapper.AntennaTranslatorRepository.GetByCondition(x => x.Id.ToString() == id);
        AntennaTranslatorDto model = _mapper.Map<AntennaTranslatorDto>(antennaTranslator);
        if (antennaTranslator is null)
            return new BaseResponse<AntennaTranslatorDto>(
                Result: null,
                Messages: new List<string> { "Передатчик анетнны не найден" },
                Success: true);

        return new BaseResponse<AntennaTranslatorDto>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Передатчик антенны успешно найден" });
    }

    public async Task<BaseResponse<bool>> Delete(string oid)
    {
        AntennaTranslator? antennaTranslator = await _repositoryWrapper.AntennaTranslatorRepository.GetByCondition(x => x.Id.ToString() == oid);
        if (antennaTranslator is not null)
        {
            antennaTranslator.IsDelete = true;
            _repositoryWrapper.AntennaTranslatorRepository.Update(antennaTranslator);
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

    public async Task<LoadResult> GetLoadResult(string id, DataSourceLoadOptionsBase loadOptions)
    {
        var queryableUsers = _repositoryWrapper.AntennaTranslatorRepository.GetAllByCondition(x => x.ProjectAntennaId.ToString() == id);
        return await DataSourceLoader.LoadAsync(queryableUsers, loadOptions);

    }

    public async Task<BaseResponse<string>> Update(AntennaTranslatorDto model, string author)
    {
        {
            var result = await _antennaTranslatorValidator.ValidateAsync(model);
            if (!result.IsValid)
            {
                List<string> messages = _mapper.Map<List<string>>(result.Errors);

                return new BaseResponse<string>(
                    Result: null,
                    Messages: messages,
                    Success: false);
            }

            AntennaTranslator? antennaTranslator =
                await _repositoryWrapper.AntennaTranslatorRepository.GetByCondition(x => x.Id.Equals(model.Id));
            if (antennaTranslator == null)
            {
                return new BaseResponse<string>(
                    Result: null,
                    Messages: new List<string> { "Транслятор антенны не найден" },
                    Success: false);
            }

            _mapper.Map(model, antennaTranslator);
            antennaTranslator.LastModifiedBy = author;

            _repositoryWrapper.AntennaTranslatorRepository.Update(antennaTranslator);
            await _repositoryWrapper.Save();

            return new BaseResponse<string>(
                Result: antennaTranslator.Id.ToString(),
                Success: true,
                Messages: new List<string> { "Транслятор антенны изменен" });
        }
    }

    public BaseResponse<List<AntennaTranslatorDto>> GetAllByProjectAntennaId(string id)
    {
        IQueryable<AntennaTranslator>? projectAntennas = _repositoryWrapper.AntennaTranslatorRepository.GetAllByCondition(x => x.ProjectAntennaId.ToString() == id);
        List<AntennaTranslatorDto> model = _mapper.Map<List<AntennaTranslatorDto>>(projectAntennas);

        if (projectAntennas is null)
            return new BaseResponse<List<AntennaTranslatorDto>>(
                Result: null,
                Messages: new List<string> { "Свойства антенны не найдены" },
                Success: true);
        return new BaseResponse<List<AntennaTranslatorDto>>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Свойства антенны успешно найдены" });
    }
}