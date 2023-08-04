using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models;
using Domain.Entities;

namespace Application.Interfaces;

public interface IDistrictService : ICrudService<DistrictDto>
{
    public Task<string?> GetByDistrictOid(string name);
    public Task<District> GetDistrictByName(string name);
}