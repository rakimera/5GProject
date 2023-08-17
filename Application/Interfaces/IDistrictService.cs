using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models;
using Domain.Entities;

namespace Application.Interfaces;

public interface IDistrictService : ICrudService<DistrictDto>
{
    public Task<Guid> GetByDistrictId(string name);
}