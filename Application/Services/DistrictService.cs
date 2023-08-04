using Application.DataObjects;
using Application.Interfaces;
using Application.Interfaces.RepositoryContract.Common;
using Application.Models;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class DistrictService : IDistrictService
{
    private readonly IRepositoryWrapper _repositoryWrapper;
    private readonly IMapper _mapper;

    public DistrictService(IRepositoryWrapper repositoryWrapper, IMapper mapper)
    {
        _repositoryWrapper = repositoryWrapper;
        _mapper = mapper;
    }

    public BaseResponse<IEnumerable<DistrictDto>> GetAll()
    {
        IQueryable<District> districts = _repositoryWrapper.DistrictRepository.GetAll();
        List<DistrictDto> models = _mapper.Map<List<DistrictDto>>(districts);
        if (models.Count > 0)
        {
            return new BaseResponse<IEnumerable<DistrictDto>>(
                Result: models,
                Success: true,
                Messages: new List<string> { "Регионы успешно получены" });
        }

        return new BaseResponse<IEnumerable<DistrictDto>>(
            Result: models,
            Success: true,
            Messages: new List<string>
                { "Данные не были получены, возможно регионы еще не созданы или удалены" });
    }

    public async Task<BaseResponse<string>> CreateAsync(DistrictDto model,string creator)
    {
        model.CreatedBy = creator;
        District district = _mapper.Map<District>(model);
        await _repositoryWrapper.DistrictRepository.CreateAsync(district);
        await _repositoryWrapper.Save();

        return new BaseResponse<string>(
            Result: district.Id.ToString(),
            Success: true,
            Messages: new List<string> { "Регион успешно создан" });
    }

    public async Task<BaseResponse<DistrictDto>> GetByOid(string oid)
    {
        District? district = await _repositoryWrapper.DistrictRepository.GetByCondition(x => x.Id.ToString() == oid);
        DistrictDto model = _mapper.Map<DistrictDto>(district);
        if (district is null)
            return new BaseResponse<DistrictDto>(
                Result: null,
                Messages: new List<string> { "Регион не найден" },
                Success: true);

        return new BaseResponse<DistrictDto>(
            Result: model,
            Success: true,
            Messages: new List<string> { "Регион успешно найден" });
    }

    public async Task<BaseResponse<bool>> Delete(string oid)
    {
        District? district = await _repositoryWrapper.DistrictRepository.GetByCondition(x => x.Id.ToString() == oid);
        if (district is not null)
        {
            district.IsDelete = true;
            _repositoryWrapper.DistrictRepository.Update(district);
            await _repositoryWrapper.Save();

            return new BaseResponse<bool>(
                Result: true,
                Success: true,
                Messages: new List<string> { "Регион успешно удален" });
        }
        return new BaseResponse<bool>(
            Result: false,
            Messages: new List<string> { "Регион не существует" },
            Success: false);
    }

    public async Task<Guid?> GetByDistrictOid(string name)
    {
        var district = await _repositoryWrapper.DistrictRepository
            .GetByCondition(x => x.DistrictName.Equals(name));
        if (district != null)
            return district.Id;
        return null;
    }

}