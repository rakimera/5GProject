using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.ContrAgents;
using Application.Validation;
using AutoMapper;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Domain.Entities;

namespace Application.Services;

public class ContrAgentService : IContrAgentService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;
    private readonly ContrAgentValidator _contrAgentValidator;

    public ContrAgentService(IRepositoryWrapper repositoryWrapper, IMapper mapper, ContrAgentValidator contrAgentValidator)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
        _contrAgentValidator = contrAgentValidator;
    }

    public BaseResponse<IEnumerable<ContrAgentDto>> GetAll()
    {
        IQueryable<ContrAgent> contrAgents = _repositoryWrapper.ContrAgentRepository.GetAll();
        List<ContrAgentDto> models = _mapper.Map<List<ContrAgentDto>>(contrAgents);
        if (models.Count > 0)
        {
            return new BaseResponse<IEnumerable<ContrAgentDto>>(
                Result: models,
                Success: true,
                Messages: new List<string>{"Контрагенты успешно получены"});
        }
        return new BaseResponse<IEnumerable<ContrAgentDto>>(
            Result: models,
            Success: true,
            Messages: new List<string>{"Данные не были получены, возможно контрагенты еще не созданы или удалены"});
    }

    public async Task<BaseResponse<string>> CreateAsync(ContrAgentDto model)
    {
        ContrAgent contrAgent = _mapper.Map<ContrAgent>(model);
        var result = await _contrAgentValidator.ValidateAsync(contrAgent);
        if (result.IsValid)
        {
            contrAgent.Created = DateTime.Now;
            contrAgent.CreatedBy = "Admin";
            await _repositoryWrapper.ContrAgentRepository.CreateAsync(contrAgent);
            await _repositoryWrapper.Save();

            return new BaseResponse<string>(
                Result: contrAgent.Id.ToString(),
                Success: true,
                Messages: new List<string>{"Контрагент успешно создан"});
        }
        List<string> messages = _mapper.Map<List<string>>(result.Errors);
        return new BaseResponse<string>(
            Result: "", 
            Messages: messages,
            Success: false);
    }

    public async Task<BaseResponse<ContrAgentDto>> GetByOid(string oid)
    {
        ContrAgent? contrAgent = await _repositoryWrapper.ContrAgentRepository.GetByCondition(x => x.Id.ToString() == oid);
        ContrAgentDto model = _mapper.Map<ContrAgentDto>(contrAgent);
        if (contrAgent is null)
            return new BaseResponse<ContrAgentDto>(
                Result: null,
                Messages: new List<string>{"Контрагент не найден"},
                Success: true);
        return new BaseResponse<ContrAgentDto>(
            Result: model,
            Success: true,
            Messages: new List<string>{"Контрагент успешно найден"});
    }

    public async Task<BaseResponse<ContrAgentDto>> UpdateContrAgent(UpdateContrAgentDto model)
    {
        BaseResponse<ContrAgentDto> getContrAgentResponse = await GetByOid(model.Id);
        if (!getContrAgentResponse.Success || getContrAgentResponse.Result == null)
        {
            return new BaseResponse<ContrAgentDto>(
                Result: null,
                Messages: new List<string> { "Контрагент не найден" },
                Success: false);
        }
        ContrAgentDto existingContrAgentDto = getContrAgentResponse.Result;
        _mapper.Map(model, existingContrAgentDto);
        ContrAgent contrAgent =
            await _repositoryWrapper.ContrAgentRepository.GetByCondition(x => x.Id.Equals(existingContrAgentDto.Id));
        if (contrAgent == null)
        {
            return new BaseResponse<ContrAgentDto>(
                Result: null,
                Messages: new List<string> { "Контрагент не найден" },
                Success: false);
        }

        existingContrAgentDto.LastModified = DateTime.Now;
        existingContrAgentDto.LastModifiedBy = "Admin";
        _mapper.Map(existingContrAgentDto, contrAgent);
        var result = await _contrAgentValidator.ValidateAsync(contrAgent);
        if (!result.IsValid)
            return new BaseResponse<ContrAgentDto>(
                Result: null,
                Messages: _mapper.Map<List<string>>(result.Errors),
                Success: false);
        
        _repositoryWrapper.ContrAgentRepository.Update(contrAgent);
        await _repositoryWrapper.Save();
        
        return new BaseResponse<ContrAgentDto>(
            Result: existingContrAgentDto,
            Success: true,
            Messages: new List<string> { "Пользователь успешно изменен" });
    }
    
    public async Task<LoadResult> GetLoadResult(DataSourceLoadOptionsBase loadOptions)
    {
        var queryableContrAgents = _repositoryWrapper.ContrAgentRepository.GetAll();
        return await DataSourceLoader.LoadAsync(queryableContrAgents, loadOptions);
    }

    public async Task<BaseResponse<bool>> Delete(string oid)
    {
        ContrAgent? contrAgent = await _repositoryWrapper.ContrAgentRepository.GetByCondition(x => x.Id.ToString() == oid);
        if (contrAgent is not null)
        {
            contrAgent.IsDelete = true;
            _repositoryWrapper.ContrAgentRepository.Update(contrAgent);
            await _repositoryWrapper.Save();

            return new BaseResponse<bool>(
                Result: true,
                Success: true,
                Messages: new List<string>{"Контрагент успешно удален"});
        }
        return new BaseResponse<bool>(
            Result: false, 
            Messages: new List<string>{"Контрагент не существует"},
            Success: false);
    }
}