using Application.DataObjects;
using Domain.Entities;

namespace Application.Interfaces;

public interface IBiohazardRadiusService
{
    // decimal Multiplier(decimal value);
    //
    // decimal GetRB(decimal power, decimal height, decimal lost, decimal multiplier);
    // decimal GetRZ(decimal degree, decimal rB);
    // decimal GetRX(decimal degree, decimal rB);
    public Task<BaseResponse<bool>> Create(Project project);
}