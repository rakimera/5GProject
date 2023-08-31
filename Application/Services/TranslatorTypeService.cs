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

public class TranslatorTypeService : ITranslatorTypeService
{
    private readonly IRepositoryWrapper _repository;
    private readonly IMapper _mapper;
    private readonly TranslatorTypeValidator _translatorTypeValidator;

    public TranslatorTypeService(IRepositoryWrapper repository, IMapper mapper, TranslatorTypeValidator translatorTypeValidator)
    {
        _repository = repository;
        _mapper = mapper;
        _translatorTypeValidator = translatorTypeValidator;
    }

    public BaseResponse<IEnumerable<TranslatorTypeDto>> GetAll()
    {
        IQueryable<TranslatorType> translatorTypes = _repository.TranslatorTypeRepository.GetAll();
        List<TranslatorTypeDto> models = _mapper.Map<List<TranslatorTypeDto>>(translatorTypes);
        
        if (models.Count > 0)
        {
            return new BaseResponse<IEnumerable<TranslatorTypeDto>>(
                Result: models,
                Success: true,
                Messages: new List<string> { "Типы передатчиков успешно получены" });
        }

        return new BaseResponse<IEnumerable<TranslatorTypeDto>>(
            Result: models,
            Success: true,
            Messages: new List<string> { "Типы передатчиков не найдены, возможно они еще не созданы или удалены" });
    }

    public async Task<BaseResponse<string>> CreateAsync(TranslatorTypeDto model, string creator)
    {
        TranslatorType translatorType = _mapper.Map<TranslatorType>(model);
        translatorType.CreatedBy = creator;
        await _repository.TranslatorTypeRepository.CreateAsync(translatorType);
        await _repository.Save();

        return new BaseResponse<string>(
            Result: translatorType.Id.ToString(),
            Success: true,
            Messages: new List<string> { "Тип передатчика успешно создан" });
    }

    public async Task<BaseResponse<TranslatorTypeDto>> GetByOid(string id)
    {
        TranslatorType? translatorType = await _repository.TranslatorTypeRepository.GetByCondition(x => x.Id.ToString() == id);
        TranslatorTypeDto model = _mapper.Map<TranslatorTypeDto>(translatorType);
        if (translatorType is null)
            return new BaseResponse<TranslatorTypeDto>(
                Result: null,
                Messages: new List<string> { "Тип передатчика не найден" },
                Success: true);

        return new BaseResponse<TranslatorTypeDto>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Тип передатчика успешно найден" });
    }

    public async Task<BaseResponse<bool>> Delete(string id)
    {
        TranslatorType? translatorType = await _repository.TranslatorTypeRepository.GetByCondition(x => x.Id.ToString() == id);
        if (translatorType is not null)
        {
            translatorType.IsDelete = true;
            _repository.TranslatorTypeRepository.Update(translatorType);
            await _repository.Save();

            return new BaseResponse<bool>(
                Result: true,
                Success: true,
                Messages: new List<string> { "Тип передатчика успешно удален" });
        }
        return new BaseResponse<bool>(
            Result: false,
            Messages: new List<string> { "Тип передатчика не существует" },
            Success: false);
    }

    public async Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions)
    {
        var queryableUsers = _repository.UserRepository.GetAll();
        return await DataSourceLoader.LoadAsync(queryableUsers, loadOptions);
    }

    public async Task<BaseResponse<string>> Update(TranslatorTypeDto model, string author)
    {
        var result = await _translatorTypeValidator.ValidateAsync(model);
        if (!result.IsValid)
        {
            List<string> messages = _mapper.Map<List<string>>(result.Errors);

            return new BaseResponse<string>(
                Result: null,
                Messages: messages,
                Success: false);
        }

        TranslatorType? translatorType = await _repository.TranslatorTypeRepository.GetByCondition(x => x.Id.Equals(model.Id));
        if (translatorType == null)
        {
            return new BaseResponse<string>(
                Result: null,
                Messages: new List<string> { "Тип транслятора не найден" },
                Success: false);
        }

        _mapper.Map(model, translatorType);
        translatorType.LastModifiedBy = author;

        _repository.TranslatorTypeRepository.Update(translatorType);
        await _repository.Save();

        return new BaseResponse<string>(
            Result: translatorType.Id.ToString(),
            Success: true,
            Messages: new List<string> { "Тип транслятора изменен" });

    }
}