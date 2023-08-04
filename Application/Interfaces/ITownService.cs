using Application.DataObjects;
using Application.Interfaces.Common;
using Application.Models;
using Domain.Entities;

namespace Application.Interfaces;

public interface ITownService : ICrudService<TownDto>
{
    public Task<BaseResponse<string>> CreateTownAsync(Town town);
}