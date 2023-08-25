using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models.RadiationZone;
using Application.Validation;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class RadiationZoneService : IRadiationZoneService
{
    private readonly IRepositoryWrapper _repository;
    private readonly IMapper _mapper;
    private readonly RadiationZoneValidator _validator;

    public RadiationZoneService(
        IRepositoryWrapper repository, 
        IMapper mapper, 
        RadiationZoneValidator validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }

    public BaseResponse<IEnumerable<RadiationZoneDto>> GetAll()
    {
        IQueryable<RadiationZone> radiationZones = _repository.RadiationZoneRepository.GetAll();
        List<RadiationZoneDto> models = _mapper.Map<List<RadiationZoneDto>>(radiationZones);
        
        if (models.Count > 0)
        {
            return new BaseResponse<IEnumerable<RadiationZoneDto>>(
                Result: models,
                Success: true,
                Messages: new List<string> { "Зона излучения радиоволн успешно получена" });
        }

        return new BaseResponse<IEnumerable<RadiationZoneDto>>(
            Result: models,
            Success: true,
            Messages: new List<string> { "Данные не были получены, возможно зона излучения радиоволн еще не создана или удалена" });
    }

    public async Task<BaseResponse<string>> CreateAsync(RadiationZoneDto model, string creator)
    {
        await _validator.ValidateAsync(model);
        model.CreatedBy = creator;
        RadiationZone radiationZone = _mapper.Map<RadiationZone>(model);
        await _repository.RadiationZoneRepository.CreateAsync(radiationZone);
        await _repository.Save();

        return new BaseResponse<string>(
            Result: radiationZone.Id.ToString(),
            Success: true,
            Messages: new List<string> { "Радиоволна успешно создана" });
    }

    public async Task<BaseResponse<RadiationZoneDto>> GetByOid(string oid)
    {
        RadiationZone? radiationZone = await _repository.RadiationZoneRepository.GetByCondition(x => x.Id.ToString() == oid);
        RadiationZoneDto model = _mapper.Map<RadiationZoneDto>(radiationZone);
        if (radiationZone is null)
            return new BaseResponse<RadiationZoneDto>(
                Result: null,
                Messages: new List<string> { "Радиоволна не найдена" },
                Success: true);

        return new BaseResponse<RadiationZoneDto>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Радиоволна успешно найдена" });
    }

    public async Task<BaseResponse<bool>> Delete(string oid)
    {
        RadiationZone? radiationZone = await _repository.RadiationZoneRepository.GetByCondition(x => x.Id.ToString() == oid);
        if (radiationZone is not null)
        {
            radiationZone.IsDelete = true;
            _repository.RadiationZoneRepository.Update(radiationZone);
            await _repository.Save();

            return new BaseResponse<bool>(
                Result: true,
                Success: true,
                Messages: new List<string> { "Радиоволна успешно удалена" });
        }
        return new BaseResponse<bool>(
            Result: false,
            Messages: new List<string> { "Радиоволна не существует" },
            Success: false);
    }

    public async Task<BaseResponse<string>> Update(UpdateRadiationZoneDto model)
    {
        var radiationZoneDto = _mapper.Map<RadiationZoneDto>(model);
        var result = await _validator.ValidateAsync(radiationZoneDto);
        if (!result.IsValid)
        {
            List<string> messages = _mapper.Map<List<string>>(result.Errors);
        
            return new BaseResponse<string>(
                Result: "",
                Messages: messages,
                Success: false);
        }

        RadiationZone? radiationZone = await _repository.RadiationZoneRepository.GetByCondition(x => x.Id.ToString() == model.Id);
        if (radiationZone == null)
        {
            return new BaseResponse<string>(
                Result: null,
                Messages: new List<string> { "Радиоволна не найдена" },
                Success: false);
        }

        _mapper.Map(model, radiationZone);
        radiationZone.LastModifiedBy = "Admin";

        _repository.RadiationZoneRepository.Update(radiationZone);
        await _repository.Save();

        return new BaseResponse<string>(
            Result: radiationZone.Id.ToString(),
            Success: true,
            Messages: new List<string> { "Радиоволна успешна изменена" });
    }
}