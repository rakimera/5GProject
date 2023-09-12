using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.TranslatorSpecs;
using Application.Validation;
using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Domain.Entities;

namespace Application.Services;

public class TranslatorSpecsService : ITranslatorSpecsService
{
    private readonly IRepositoryWrapper _repository;
    private readonly IMapper _mapper;
    private readonly TranslatorSpecsValidator _validator;

    public TranslatorSpecsService(
        IRepositoryWrapper repository, 
        IMapper mapper, 
        TranslatorSpecsValidator validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }

    public BaseResponse<IEnumerable<TranslatorSpecsDto>> GetAll()
    {
        IQueryable<TranslatorSpecs> translatorSpecs = _repository.TranslatorSpecsRepository.GetAll();
        List<TranslatorSpecsDto> models = _mapper.Map<List<TranslatorSpecsDto>>(translatorSpecs);
        
        if (models.Count > 0)
        {
            return new BaseResponse<IEnumerable<TranslatorSpecsDto>>(
                Result: models,
                Success: true,
                Messages: new List<string> { "Передатчики успешно получены" });
        }

        return new BaseResponse<IEnumerable<TranslatorSpecsDto>>(
            Result: models,
            Success: true,
            Messages: new List<string> { "Данные не были получены, возможно Передатчики еще не созданы или удалены" });
    }

    public async Task<BaseResponse<string>> CreateAsync(TranslatorSpecsDto model, string creator)
    {
        var result = await _validator.ValidateAsync(model);
        if (result.IsValid)
        {
            model.CreatedBy = creator;
            TranslatorSpecs translatorSpecs = _mapper.Map<TranslatorSpecs>(model);
            await _repository.TranslatorSpecsRepository.CreateAsync(translatorSpecs);
            await _repository.Save();

            return new BaseResponse<string>(
                Result: translatorSpecs.Id.ToString(),
                Success: true,
                Messages: new List<string> { "Передатчик успешно создан" });
        }

        List<string>? errors = _mapper.Map<List<string>>(result.Errors);
        return new BaseResponse<string>(
            Result: null,
            Success: false,
            Messages: errors);
    }

    public async Task<BaseResponse<TranslatorSpecsDto>> GetByOid(string id)
    {
        TranslatorSpecs? translatorSpecs = await _repository.TranslatorSpecsRepository.GetByCondition(x => x.Id.ToString() == id);
        TranslatorSpecsDto model = _mapper.Map<TranslatorSpecsDto>(translatorSpecs);
        if (translatorSpecs is null)
            return new BaseResponse<TranslatorSpecsDto>(
                Result: null,
                Messages: new List<string> { "Передатчик не найден" },
                Success: true);

        return new BaseResponse<TranslatorSpecsDto>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Передатчик успешно найден" });
    }

    public async Task<BaseResponse<bool>> Delete(string id)
    {
        TranslatorSpecs? translatorSpecs = await _repository.TranslatorSpecsRepository.GetByCondition(x => x.Id.ToString() == id);
        if (translatorSpecs is not null)
        {
            translatorSpecs.IsDelete = true;
            _repository.TranslatorSpecsRepository.Update(translatorSpecs);
            await _repository.Save();

            return new BaseResponse<bool>(
                Result: true,
                Success: true,
                Messages: new List<string> { "Передатчик успешно удален" });
        }
        return new BaseResponse<bool>(
            Result: false,
            Messages: new List<string> { "Передатчик не существует" },
            Success: false);
    }

    public async Task<BaseResponse<string>> Update(TranslatorSpecsDto model, string author)
    {
        var result = await _validator.ValidateAsync(model);
        if (!result.IsValid)
        {
            List<string> messages = _mapper.Map<List<string>>(result.Errors);
        
            return new BaseResponse<string>(
                Result: "",
                Messages: messages,
                Success: false);
        }

        TranslatorSpecs? translatorSpecs = await _repository.TranslatorSpecsRepository.GetByCondition(x => x.Id.Equals(model.Id));
        if (translatorSpecs == null)
        {
            return new BaseResponse<string>(
                Result: null,
                Messages: new List<string> { "Передатчик не найден" },
                Success: false);
        }

        _mapper.Map(model, translatorSpecs);
        translatorSpecs.LastModifiedBy = author;

        _repository.TranslatorSpecsRepository.Update(translatorSpecs);
        await _repository.Save();

        return new BaseResponse<string>(
            Result: translatorSpecs.Id.ToString(),
            Success: true,
            Messages: new List<string> { "Передатчик успешно изменен" });
    }

    public async Task<LoadResult> GetLoadResult(string id, DataSourceLoadOptionsBase loadOptions)
    {
        var queryableTranslatorSpecs = _repository.TranslatorSpecsRepository.GetAllByCondition(x => x.AntennaId.ToString() == id);
        return await DataSourceLoader.LoadAsync(queryableTranslatorSpecs, loadOptions);
    }

    public BaseResponse<List<TranslatorSpecsDto>> GetAllByAntennaId(string id)
    {
        IQueryable<TranslatorSpecs>? translatorSpecs = _repository.TranslatorSpecsRepository.GetAllByCondition(x => x.AntennaId.ToString() == id);
        List<TranslatorSpecsDto> model = _mapper.Map<List<TranslatorSpecsDto>>(translatorSpecs);

        if (translatorSpecs is null)
            return new BaseResponse<List<TranslatorSpecsDto>>(
                Result: null,
                Messages: new List<string> { "Свойства антенны не найдены" },
                Success: true);
        return new BaseResponse<List<TranslatorSpecsDto>>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Свойства антенны успешно найдены" });
    }

    public BaseResponse<List<TranslatorSpecsDto>> GetAllByProjectId(string id)
    {
        IQueryable<TranslatorSpecs>? projectAntennas = _repository.TranslatorSpecsRepository.GetAllByCondition(x => x.AntennaId.ToString() == id);
        List<TranslatorSpecsDto> model = _mapper.Map<List<TranslatorSpecsDto>>(projectAntennas);

        if (projectAntennas is null)
            return new BaseResponse<List<TranslatorSpecsDto>>(
                Result: null,
                Messages: new List<string> { "Трансляторы антенны не найдены" },
                Success: true);
        return new BaseResponse<List<TranslatorSpecsDto>>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Трансляторы антенны успешно найдены" });
    }
}