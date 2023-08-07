using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.Antennas;
using Domain.Entities;

namespace Application.Interfaces;

public interface IAntennaService : ICrudService<AntennaDto>
{
    Task<BaseResponse<string>> Update(UpdateAntennaDto model);
    public Task<Guid?> GetByAntennaOid(string name);
}