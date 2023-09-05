using Application.Interfaces.RepositoryContract.Common;
using Domain.Entities;

namespace Application.Interfaces.RepositoryContract;

public interface IAntennaTranslatorRepository : IBaseRepository<AntennaTranslator>
{
    void Delete(AntennaTranslator antennaTranslator);
}