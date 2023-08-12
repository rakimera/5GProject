using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models.Antennae;


namespace Application.Interfaces;

public interface IAntennaService : ICrudService<AntennaDto>
{
    Task<BaseResponse<string>> Update(UpdateAntennaDto model);
}