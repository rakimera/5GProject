using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class TownService : ITownService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;

    public TownService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public BaseResponse<IEnumerable<TownDto>> GetAll()
    {
        List<Town> towns = _repositoryWrapper.TownRepository.GetAll().ToList();
        List<TownDto> models = _mapper.Map<List<TownDto>>(towns);
        if (models.Count > 0)
        {
            return new BaseResponse<IEnumerable<TownDto>>(
                Result: models,
                Success: true,
                Messages: new List<string> { "Города успешно получены" });
        }

        return new BaseResponse<IEnumerable<TownDto>>(
            Result: models,
            Success: true,
            Messages: new List<string>
                { "Данные не были получены, возможно города еще не созданы или удалены" });
    }

    public async Task<BaseResponse<string>> CreateAsync(TownDto model,string creator)
    {
        model.CreatedBy = creator;
        Town town = _mapper.Map<Town>(model);
        await _repositoryWrapper.TownRepository.CreateAsync(town);
        await _repositoryWrapper.Save();

        return new BaseResponse<string>(
            Result: town.Id.ToString(),
            Success: true,
            Messages: new List<string> { "Город успешно создан" });
    }
    
    public async Task<BaseResponse<string>> CreateTownAsync(Town town)
    {
        town.CreatedBy = "Admin";
        await _repositoryWrapper.TownRepository.CreateAsync(town);
        await _repositoryWrapper.Save();

        return new BaseResponse<string>(
            Result: town.Id.ToString(),
            Success: true,
            Messages: new List<string> { "Город успешно создан" });
    }

    public async Task<BaseResponse<TownDto>> GetByOid(string oid)
    {
        Town? town = await _repositoryWrapper.TownRepository.GetByCondition(x => x.Id.ToString() == oid);
        TownDto model = _mapper.Map<TownDto>(town);
        if (town is null)
            return new BaseResponse<TownDto>(
                Result: null,
                Messages: new List<string> { "Город не найден" },
                Success: true);

        return new BaseResponse<TownDto>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Город успешно найден" });
    }

    public async Task<BaseResponse<bool>> Delete(string oid)
    {
        Town? town = await _repositoryWrapper.TownRepository.GetByCondition(x => x.Id.ToString() == oid);
        if (town is not null)
        {
            town.IsDelete = true;
            _repositoryWrapper.TownRepository.Update(town);
            await _repositoryWrapper.Save();

            return new BaseResponse<bool>(
                Result: true,
                Success: true,
                Messages: new List<string> { "Город успешно удален" });
        }
        return new BaseResponse<bool>(
            Result: false,
            Messages: new List<string> { "Город не существует" },
            Success: false);
    }
}