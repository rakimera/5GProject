using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models;
using Domain.Entities;

namespace Application.Interfaces;

public interface IDistrictService : ICrudService<DistrictDto>
{
    public Task<BaseResponse<string>> GetByDistrictOid(string name);
    public Task<BaseResponse<District>> GetDistrictByName(string name);
}