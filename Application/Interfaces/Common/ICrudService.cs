using Application.DataObjects;

namespace Application.Interfaces.Common;

public interface ICrudService <T>
{
    BaseResponse<IEnumerable<T>> GetAll();
    Task<BaseResponse<Guid?>> CreateAsync(T model);
    Task<BaseResponse<T>> GetByOid(Guid oid);
    Task<BaseResponse<Guid?>> Update(T model);
    Task<BaseResponse<bool>> Delete(Guid oid);
}