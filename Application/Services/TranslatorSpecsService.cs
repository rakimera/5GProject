using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.TranslatorSpecs;
using Application.Validation;
using AutoMapper;
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
        await _validator.ValidateAsync(model);
        model.CreatedBy = creator;
        TranslatorSpecs translatorSpecs = _mapper.Map<TranslatorSpecs>(model);
        await _repository.TranslatorSpecsRepository.CreateAsync(translatorSpecs);
        await _repository.Save();

        return new BaseResponse<string>(
            Result: translatorSpecs.Id.ToString(),
            Success: true,
            Messages: new List<string> { "Передатчик успешно создан" });
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

    public async Task<BaseResponse<string>> Update(UpdateTranslatorSpecsDto model)
    {
        var translatorSpecsDto = _mapper.Map<TranslatorSpecsDto>(model);
        var result = await _validator.ValidateAsync(translatorSpecsDto);
        if (!result.IsValid)
        {
            List<string> messages = _mapper.Map<List<string>>(result.Errors);
        
            return new BaseResponse<string>(
                Result: "",
                Messages: messages,
                Success: false);
        }

        TranslatorSpecs? translatorSpecs = await _repository.TranslatorSpecsRepository.GetByCondition(x => x.Id.ToString() == model.Id);
        if (translatorSpecs == null)
        {
            return new BaseResponse<string>(
                Result: null,
                Messages: new List<string> { "Передатчик не найден" },
                Success: false);
        }

        _mapper.Map(model, translatorSpecs);
        translatorSpecs.LastModifiedBy = "Admin";

        _repository.TranslatorSpecsRepository.Update(translatorSpecs);
        await _repository.Save();

        return new BaseResponse<string>(
            Result: translatorSpecs.Id.ToString(),
            Success: true,
            Messages: new List<string> { "Передатчик успешно изменен" });
    }
}