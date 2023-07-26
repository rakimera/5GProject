using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.ContrAgents;
using Application.Validation;
using AutoMapper;
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
        try
        {
            IQueryable<ContrAgent> contrAgents = _repositoryWrapper.ContrAgentRepository.GetAll();
            List<ContrAgentDto> models = _mapper.Map<List<ContrAgentDto>>(contrAgents); 

            if (models.Count > 0)
            {
                return new BaseResponse<IEnumerable<ContrAgentDto>>(
                    Result: models,
                    Success: true,
                    StatusCode: 200,
                    Messages: new List<string>{"Контрагенты успешно получены"});
            }
            return new BaseResponse<IEnumerable<ContrAgentDto>>(
                Result: models,
                Success: true,
                StatusCode: 200,
                Messages: new List<string>{"Данные не были получены, возможно контрагенты еще не созданы или удалены"});
        }
        catch (Exception e)
        {
            return new BaseResponse<IEnumerable<ContrAgentDto>>(
                Result: null, 
                Messages: new List<string>{e.Message},
                Success: false,
                StatusCode: 500);
        }
    }

    public async Task<BaseResponse<string>> CreateAsync(ContrAgentDto model)
    {
        try
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
                    StatusCode: 200,
                    Messages: new List<string>{"Контрагент успешно создан"});
            }
            List<string> messages = _mapper.Map<List<string>>(result.Errors);
            
            return new BaseResponse<string>(
                Result: "", 
                Messages: messages,
                Success: false,
                StatusCode: 400);
        }
        catch (Exception e)
        {
            return new BaseResponse<string>(
                Result: "",
                Messages: new List<string>{e.Message},
                Success: false,
                StatusCode: 500);
        }
    }

    public async Task<BaseResponse<ContrAgentDto>> GetByOid(string oid)
    {
        try
        {
            ContrAgent? contrAgent = await _repositoryWrapper.ContrAgentRepository.GetByCondition(x => x.Id.ToString() == oid);
            ContrAgentDto model = _mapper.Map<ContrAgentDto>(contrAgent);

            if (contrAgent is null)
                return new BaseResponse<ContrAgentDto>(
                    Result: null,
                    Messages: new List<string>{"Контрагент не найден"},
                    Success: true,
                    StatusCode: 404);
            return new BaseResponse<ContrAgentDto>(
                Result: model,
                Success: true,
                StatusCode: 200,
                Messages: new List<string>{"Контрагент успешно найден"});

        }
        catch (Exception e)
        {
            return new BaseResponse<ContrAgentDto>(
                Result: null,
                Success: false,
                Messages: new List<string>{e.Message},
                StatusCode: 500);
        }
    }

    public async Task<BaseResponse<string>> Update(ContrAgentDto model)
    {
        try
        {
            ContrAgent contrAgent = _mapper.Map<ContrAgent>(model);
            var result = await _contrAgentValidator.ValidateAsync(contrAgent);
            if (result.IsValid)
            {
                contrAgent.LastModified = DateTime.Now;
                contrAgent.LastModifiedBy = "Admin";
                _repositoryWrapper.ContrAgentRepository.Update(contrAgent);
                await _repositoryWrapper.Save();

                return new BaseResponse<string>(
                    Result: contrAgent.Id.ToString(),
                    Success: true,
                    StatusCode: 200,
                    Messages: new List<string>{"Контрагент успешно изменен"});
            }
            return new BaseResponse<string>(
                Result: "", 
                Messages: _mapper.Map<List<string>>(result.Errors),
                Success: false,
                StatusCode: 400);
        }
        catch (Exception e)
        {
            return new BaseResponse<string>(
                Result: "",
                Messages: new List<string>{e.Message},
                Success: false,
                StatusCode: 500);
        }
    }

    public async Task<BaseResponse<bool>> Delete(string oid)
    {
        try
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
                    StatusCode: 200,
                    Messages: new List<string>{"Контрагент успешно удален"});
            }
            
            return new BaseResponse<bool>(
                Result: false, 
                Messages: new List<string>{"Контрагент не существует"},
                Success: false,
                StatusCode: 400);
        }
        catch (Exception e)
        {
            return new BaseResponse<bool>(
                Result: false,
                Messages: new List<string>{e.Message},
                Success: false,
                StatusCode: 500);
        }
    }
}